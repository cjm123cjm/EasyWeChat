using EasyWeChat.Common.RedisUtil;
using EasyWeChat.Domain.Entities;
using EasyWeChat.Domain.IRepository;
using EasyWeChat.IService.Consts;
using EasyWeChat.IService.Dtos;
using EasyWeChat.IService.Dtos.Inputs;
using EasyWeChat.IService.Dtos.Outputs;
using EasyWeChat.IService.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EasyWeChat.Service.Implement
{
    public class UserService : ServiceBase, IUserService
    {
        private readonly IJwtTokenGenerator _tokenGenerator;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IConfiguration _configuration;
        private readonly IUserContactRepository _userContactRepository;
        private ResponseDto responseDto;

        public UserService(
            IJwtTokenGenerator tokenGenerator,
            IUserInfoRepository userInfoRepository,
            IConfiguration configuration,
            IUserContactRepository userContactRepository)
        {
            _tokenGenerator = tokenGenerator;
            _userInfoRepository = userInfoRepository;
            responseDto = new ResponseDto();
            _configuration = configuration;
            _userContactRepository = userContactRepository;
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
                item.IsFriends = true;
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
            var userInfo = await _userInfoRepository.GetAllWhere(t => t.Email == loginInput.Email && t.Password == loginInput.Password)
                                                    .FirstOrDefaultAsync();

            if (userInfo == null)
            {
                responseDto.Code = 400;
                responseDto.Message = "账户或密码错误";

                return responseDto;
            }

            //判断是否在线,如果在线提示此账号已在其他地方登录
            if (CacheManager.Exist(RedisKeyPrefix.Online + loginInput.Email))
            {
                int any = CacheManager.Get<int>(RedisKeyPrefix.Online + loginInput.Email);
                if (any == 1)
                {
                    responseDto.Code = 400;
                    responseDto.Message = "提示此账号已在其他地方登录";

                    return responseDto;
                }
            }

            //判断验证码是否正确
            string verifyCode = CacheManager.Get<string>(RedisKeyPrefix.VerifyCode + loginInput.Email);
            if (verifyCode != loginInput.VerifyCode)
            {
                responseDto.Code = 400;
                responseDto.Message = "验证码错误";

                return responseDto;
            }

            var userDto = ObjectMapper.Map<UserInfoDto>(userInfo);

            //判断是否是管理员
            string adminEmails = _configuration.GetSection("AdminEmail").Value;
            var adminEmailSplits = adminEmails.Split(',');
            if (adminEmailSplits.Contains(userDto.Email))
            {
                userDto.IsAdmin = true;
            }

            var token = _tokenGenerator.GenerateToken(userDto);

            //移除验证码
            CacheManager.Remove(RedisKeyPrefix.VerifyCode + userDto.Email);
            //添加到redis 表示上线了
            CacheManager.Set(RedisKeyPrefix.Online + userDto.Email, 1);

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
            var user = await _userInfoRepository.GetByEmailAsync(registInput.Email);
            if (user != null)
            {
                responseDto.Message = "邮箱已存在";
                responseDto.Code = 400;
                return responseDto;
            }

            //判断验证码是否正确
            string verifyCode = CacheManager.Get<string>(RedisKeyPrefix.VerifyCode + registInput.Email);
            if (verifyCode != registInput.VerifyCode)
            {
                responseDto.Code = 400;
                responseDto.Message = "验证码错误";
                return responseDto;
            }

            var entity = ObjectMapper.Map<UserInfo>(registInput);

            entity.UserId = SnowIdWorker.NextId();

            await _userInfoRepository.AddAsync(entity);

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
