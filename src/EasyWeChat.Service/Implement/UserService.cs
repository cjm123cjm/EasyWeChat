using EasyWeChat.Common.Captcha;
using EasyWeChat.Common.RedisUtil;
using EasyWeChat.Domain;
using EasyWeChat.Domain.Entities;
using EasyWeChat.Domain.IRepository;
using EasyWeChat.IService.Consts;
using EasyWeChat.IService.Dtos;
using EasyWeChat.IService.Dtos.Inputs;
using EasyWeChat.IService.Dtos.Outputs;
using EasyWeChat.IService.Enums;
using EasyWeChat.IService.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Mime;

namespace EasyWeChat.Service.Implement
{
    public class UserService : ServiceBase, IUserService
    {
        private readonly EasyWeChatDbContext _chatDbContext;
        private readonly IJwtTokenGenerator _tokenGenerator;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IConfiguration _configuration;
        private readonly IUserContactRepository _userContactRepository;
        private ResponseDto responseDto;

        public UserService(
            IJwtTokenGenerator tokenGenerator,
            IUserInfoRepository userInfoRepository,
            IConfiguration configuration,
            IUserContactRepository userContactRepository,
            EasyWeChatDbContext chatDbContext)
        {
            _tokenGenerator = tokenGenerator;
            _userInfoRepository = userInfoRepository;
            responseDto = new ResponseDto();
            _configuration = configuration;
            _userContactRepository = userContactRepository;
            _chatDbContext = chatDbContext;
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto> GetCurrentUserInfoAsync()
        {
            var userInfo = await _userInfoRepository.FindByIdAsync(LoginUserId);
            if (userInfo == null)
            {
                responseDto.Code = 400;
                responseDto.Message = "用户不存在";

                return responseDto;
            }

            var userinfodto = ObjectMapper.Map<UserInfoDto>(userInfo);

            userinfodto.Thumbnail = ServerUrl + "/upload/" + EasyWeChatConst.UserThumbnail + userInfo.UserId + "." + userInfo.PicExtension;

            responseDto.Result = userinfodto;

            return responseDto;
        }

        /// <summary>
        /// 获取好友详情
        /// </summary>
        /// <param name="contactId">好友id</param>
        /// <returns></returns>
        public async Task<ResponseDto> GetFirentContactAsync(long contactId)
        {
            var contact = await _userContactRepository.All().Where(t => t.UserId == LoginUserId && t.ContactId == contactId).FirstOrDefaultAsync();
            if (contact == null)
            {
                responseDto.Code = 400;
                responseDto.Message = "此用户不是您的好友";

                return responseDto;
            }

            var user = await _userInfoRepository.FindByIdAsync(contactId);
            if (user == null)
            {
                responseDto.Code = 400;
                responseDto.Message = "用户不存在";

                return responseDto;
            }

            var userDto = ObjectMapper.Map<UserInfoDto>(user);

            userDto.Thumbnail = ServerUrl + "/upload/" + EasyWeChatConst.UserThumbnail + user.UserId + "." + user.PicExtension;

            responseDto.Result = userDto;

            return responseDto;
        }

        /// <summary>
        /// 我的好友
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto> GetMyFirentsAsync()
        {
            var userContact = await _userContactRepository.All().Where(t => t.UserId == LoginUserId && (t.Status == 1 || t.Status == 3 || t.Status == 5)).ToListAsync();

            var ids = userContact.Select(t => t.ContactId).Distinct().ToList();

            var users = await _userInfoRepository.All().Where(t => ids.Contains(t.UserId)).ToListAsync();

            var userContactDtos = ObjectMapper.Map<List<UserContactDto>>(userContact);

            foreach (var item in userContactDtos)
            {
                var first = users.FirstOrDefault(t => t.UserId == item.ContactId);
                if (first != null)
                {
                    item.AreaName = first.AreaName;
                    item.ContanctName = first.NickName;
                    item.Thumbnail = ServerUrl + "/upload/" + EasyWeChatConst.UserThumbnail + first.UserId + "." + first.PicExtension;
                }
            }

            responseDto.Result = userContactDtos;

            return responseDto;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginInput"></param>
        /// <returns></returns>
        public async Task<ResponseDto> LoginAsync(LoginInput loginInput)
        {
            //判断验证码是否正确
            var verifyCode = CacheManager.Get<VerifyCode>(RedisKeyPrefix.VerifyCode + loginInput.codeKey);
            if (verifyCode == null || verifyCode.Code != loginInput.VerifyCode)
            {
                responseDto.Code = 400;
                responseDto.Message = "验证码错误";

                return responseDto;
            }

            var userInfo = await _userInfoRepository.GetAllWhere(t => t.Email == loginInput.Email && t.Password == loginInput.Password)
                                                    .FirstOrDefaultAsync();

            if (userInfo == null)
            {
                responseDto.Code = 400;
                responseDto.Message = "账户或密码错误";

                return responseDto;
            }

            //判断是否在线,如果在线提示此账号已在其他地方登录
            if (CacheManager.Exist(RedisKeyPrefix.Heart + userInfo.UserId))
            {
                int any = CacheManager.Get<int>(RedisKeyPrefix.Online + loginInput.Email);
                if (any == 1)
                {
                    responseDto.Code = 400;
                    responseDto.Message = "提示此账号已在其他地方登录";

                    return responseDto;
                }
            }

            var userDto = ObjectMapper.Map<UserInfoDto>(userInfo);

            //判断是否是管理员
            string adminEmails = _configuration.GetSection("AdminEmail").Value;
            var adminEmailSplits = adminEmails.Split(',');
            if (adminEmailSplits.Contains(userDto.Email))
            {
                userDto.IsAdmin = true;
            }

            //查询我的群组、我的联系人
            var userContact = await _userContactRepository.All().Where(t => t.UserId == userInfo.UserId && (t.Status == 1) && t.ContanctType == 1).ToListAsync();
            var contactIds = userContact.Select(t => t.ContactId).ToList();
            CacheManager.Remove(RedisKeyPrefix.User_Contact_Ids + userDto.UserId);
            if (contactIds.Count > 0)
            {
                CacheManager.Set(RedisKeyPrefix.User_Contact_Ids + userDto.UserId, contactIds);
            }

            var token = _tokenGenerator.GenerateToken(userDto);

            //移除验证码
            CacheManager.Remove(RedisKeyPrefix.VerifyCode + loginInput.codeKey);
            //保存用户信息到redis
            CacheManager.Set(RedisKeyPrefix.Online + token, userDto);

            responseDto.Token = token;
            responseDto.Result = userDto;

            return responseDto;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto> LoginOutAsync()
        {
            var userInfo = await _userInfoRepository.FindByIdAsync(LoginUserId);
            if (userInfo == null)
            {
                responseDto.Message = "用户不存在";
                responseDto.Code = 400;
                return responseDto;
            }

            userInfo.LastOffTime = DateTime.Now;

            await _userInfoRepository.UpdateAsync(userInfo);

            //redis 下线
            CacheManager.Set(RedisKeyPrefix.Online + LoginUserEmail, 0);

            return responseDto;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registInput"></param>
        /// <returns></returns>
        public async Task<ResponseDto> RegistAsync(RegistInput registInput)
        {
            //判断验证码是否正确
            var verifyCode = CacheManager.Get<VerifyCode>(RedisKeyPrefix.VerifyCode + registInput.CodeKey);
            if (verifyCode == null || verifyCode.Code != registInput.VerifyCode)
            {
                responseDto.Code = 400;
                responseDto.Message = "验证码错误";
                return responseDto;
            }

            var user = await _userInfoRepository.GetByEmailAsync(registInput.Email);
            if (user != null)
            {
                responseDto.Message = "邮箱已存在";
                responseDto.Code = 400;
                return responseDto;
            }

            var entity = ObjectMapper.Map<UserInfo>(registInput);

            entity.UserId = SnowIdWorker.NextId();

            await _chatDbContext.UserInfos.AddAsync(entity);

            //添加机器人好友
            SystemSettingDto dto = CacheManager.Get<SystemSettingDto>(RedisKeyPrefix.SystemSeting);
            await _chatDbContext.UserContacts.AddAsync(new UserContact
            {
                UserId = entity.UserId,
                ContactId = dto.RobotUid,
                ContanctType = 0,
                Status = 1,
                LastUpdatedTime = DateTime.Now
            });

            //增加会话id
            string sessionId = GetChatSessionIdByUserIds(new long[] { entity.UserId, dto.RobotUid });
            ChatSession chatSession = new ChatSession
            {
                SessionId = sessionId,
                LastMessage = dto.RobotWelcome,
                LastReceviceTime = DateTime.Now
            };
            await _chatDbContext.ChatSessions.AddAsync(chatSession);
            //增加会话人信息
            ChatSessionUser chatSessionUser = new ChatSessionUser
            {
                UserId = entity.UserId,
                ContactId = dto.RobotUid,
                ContactType = 0,
                SessionId = sessionId,
                ContactName = dto.RobotNickName,
                LastReceiveTime = DateTime.Now
            };
            await _chatDbContext.ChatSessionUsers.AddAsync(chatSessionUser);
            ChatMessage chatMessage = new ChatMessage();
            chatMessage.SessionId = sessionId;
            chatMessage.MessageType = (int)MessageTypeEnum.CHAT;
            chatMessage.MessageContent = dto.RobotWelcome;
            chatMessage.SendUserId = dto.RobotUid;
            chatMessage.SendUserNickName = dto.RobotNickName;
            chatMessage.SendTime = DateTime.Now;
            chatMessage.ContactId = entity.UserId;
            chatMessage.ContactName = entity.NickName;
            chatMessage.ContactType = 0;
            chatMessage.Status = 1;
            await _chatDbContext.ChatMessages.AddAsync(chatMessage);

            await _chatDbContext.SaveChangesAsync();

            CacheManager.Remove(RedisKeyPrefix.VerifyCode + registInput.Email);

            return responseDto;
        }

        /// <summary>
        /// 修改当前用户信息
        /// </summary>
        /// <param name="userInfoInput"></param>
        /// <returns></returns>
        public async Task<ResponseDto> UpdateCurrentUserInfoAsync(UserInfoInput userInfoInput)
        {
            var userInfo = await _userInfoRepository.FindByIdAsync(LoginUserId);
            if (userInfo == null)
            {
                responseDto.Message = "用户不存在";
                responseDto.Code = 400;
                return responseDto;
            }

            ObjectMapper.Map(userInfoInput, userInfo);

            if (userInfoInput.avatarFile != null)
            {
                string extension = Path.GetExtension(userInfoInput.avatarFile.FileName);

                userInfo.PicExtension = extension;
            }

            await _userInfoRepository.UpdateAsync(userInfo);

            return responseDto;
        }

        /// <summary>
        /// 更新用户最后连接时间
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto> UpdateLastLoginTimeAsync(long userId)
        {
            var userInfo = await _userInfoRepository.FindByIdAsync(LoginUserId);
            if (userInfo != null)
            {
                userInfo.LastLoginTime = DateTime.Now;

                await _userInfoRepository.UpdateAsync(userInfo);
            }

            return responseDto;
        }

        /// <summary>
        /// 更新用户最后离线时间
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto> UpdateLastOffTimeAsync(long userId)
        {
            var userInfo = await _userInfoRepository.FindByIdAsync(LoginUserId);
            if (userInfo != null)
            {
                userInfo.LastOffTime = DateTime.Now;

                await _userInfoRepository.UpdateAsync(userInfo);
            }

            return responseDto;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto> UpdatePasswordAsync(UpdatePasswordInput updatePassword)
        {
            var userInfo = await _userInfoRepository.FindByIdAsync(LoginUserId);
            if (userInfo == null)
            {
                responseDto.Message = "用户不存在";
                responseDto.Code = 400;
                return responseDto;
            }

            if (userInfo.Password != updatePassword.OldPassword)
            {
                responseDto.Message = "旧密码错误";
                responseDto.Code = 400;
                return responseDto;
            }

            userInfo.Password = updatePassword.NewPassword;

            await _userInfoRepository.UpdateAsync(userInfo);

            return responseDto;
        }
    }
}
