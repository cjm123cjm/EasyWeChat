using EasyWeChat.Domain;
using EasyWeChat.Domain.Entities;
using EasyWeChat.Domain.IRepository;
using EasyWeChat.IService.Consts;
using EasyWeChat.IService.Dtos;
using EasyWeChat.IService.Dtos.Inputs;
using EasyWeChat.IService.Dtos.Outputs;
using EasyWeChat.IService.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EasyWeChat.Service.Implement
{
    public class ApplyInfoService : ServiceBase, IApplyInfoService
    {
        private readonly EasyWeChatDbContext _context;
        private ResponseDto responseDto;
        private readonly ILogger<ApplyInfoService> _logger;
        private readonly IUserContactRepository _userContactRepository;

        public ApplyInfoService(
            EasyWeChatDbContext context,
            ILogger<ApplyInfoService> logger,
            IUserContactRepository userContactRepository)
        {
            responseDto = new ResponseDto();
            _context = context;
            _logger = logger;
            _userContactRepository = userContactRepository;
        }

        /// <summary>
        /// 申请添加好友或群组
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto> ApplyAddAsync(ApplyInfoInput applyInfoInput)
        {
            if (string.IsNullOrWhiteSpace(applyInfoInput.ApplyMessage))
            {
                applyInfoInput.ApplyMessage = "我是" + LoginUserName;
            }
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    //查询好友是否添加,如果已拉黑则无法添加
                    if (applyInfoInput.ContactType == 0)
                    {
                        var userContact = await _context.UserContacts.FirstOrDefaultAsync(t => t.UserId == LoginUserId && t.ContactId == applyInfoInput.ContanctId && t.ContanctType == applyInfoInput.ContactType);
                        if (userContact != null)
                        {
                            if (userContact.Status == 1)
                            {
                                responseDto.Message = "你们已经是好友了";
                                responseDto.Code = 400;
                                return responseDto;
                            }
                            else if (userContact.Status == 5)
                            {
                                responseDto.Message = "对方已将你拉黑,无法添加";
                                responseDto.Code = 400;
                                return responseDto;
                            }
                        }

                        var receiveUser = await _context.UserInfos.FindAsync(applyInfoInput.ReceiveUserId);
                        if (receiveUser == null)
                        {
                            responseDto.Message = "用户不存在";
                            responseDto.Code = 400;
                            return responseDto;
                        }

                        //直接加好友
                        if (receiveUser.JoinType == 0)
                        {
                            //todo:添加联系人
                            await _userContactRepository.AddContact(LoginUserId, applyInfoInput.ReceiveUserId, applyInfoInput.ReceiveUserId, 0);
                        }
                    }
                    else
                    {
                        var userContact = await _context.UserContacts.FirstOrDefaultAsync(t => t.UserId == LoginUserId && t.ContactId == applyInfoInput.ContanctId && t.ContanctType == applyInfoInput.ContactType);
                        if (userContact != null)
                        {
                            if (userContact.Status == 1)
                            {
                                responseDto.Message = "你已经在群聊里了";
                                responseDto.Code = 400;
                                return responseDto;
                            }
                        }
                        var group = await _context.GroupInfos.FindAsync(applyInfoInput.ContanctId);
                        if (group == null)
                        {
                            responseDto.Message = "群聊不存在";
                            responseDto.Code = 400;
                            return responseDto;
                        }
                        else
                        {
                            if (group.Status == 0)
                            {
                                responseDto.Message = "群聊已解散";
                                responseDto.Code = 400;
                                return responseDto;
                            }
                        }

                        applyInfoInput.ReceiveUserId = group.GroupOwnerId;
                    }

                    var apply = await _context.ApplyInfos.Where(t =>
                                                                t.ApplyUserId == LoginUserId &&
                                                                t.ReceiveUserId == applyInfoInput.ReceiveUserId &&
                                                                t.ContactType == applyInfoInput.ContactType).FirstOrDefaultAsync();
                    if (apply == null)
                    {
                        var applyInfo = ObjectMapper.Map<ApplyInfo>(applyInfoInput);
                        applyInfo.ApplyUserId = LoginUserId;
                        applyInfo.ApplyId = SnowIdWorker.NextId();

                        await _context.ApplyInfos.FindAsync(applyInfo);

                        //todo：发送ws消息给用户
                    }
                    else
                    {
                        if (apply.Status == 3)
                        {
                            responseDto.Message = "对方已将你拉黑,无法申请";
                            responseDto.Code = 400;
                            return responseDto;
                        }
                        ObjectMapper.Map(applyInfoInput, apply);
                        apply.Status = 0;
                        apply.LastApplyTime = DateTime.Now;
                    }

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return responseDto;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    _logger.LogError(ex, "申请添加好友或群组失败");

                    responseDto.Code = 500;
                    responseDto.Message = ex.Message;

                    return responseDto;
                }
            }
        }

        /// <summary>
        /// 申请列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto> ApplyListAsync()
        {
            var applyInfos = await _context.ApplyInfos.AsNoTracking().Where(t => t.ReceiveUserId == LoginUserId && t.Status == 0).ToListAsync();

            var dtos = ObjectMapper.Map<List<ApplyInfoDto>>(applyInfos);

            var userIds = dtos.Where(t => t.ContactType == 0).Select(t => t.ApplyUserId).Distinct().ToList();
            var userDatas = await _context.UserInfos.Where(t => userIds.Contains(t.UserId)).ToListAsync();

            var groupIds = dtos.Where(t => t.ContactType == 1).Select(t => t.ContanctId).Distinct().ToList();
            var groupDatas = await _context.GroupInfos.Where(t => groupIds.Contains(t.GroupId)).ToListAsync();

            dtos.ForEach(t =>
            {
                if (t.ContactType == 0)
                {
                    var first = userDatas.FirstOrDefault(m => m.UserId == t.ContanctId);

                    if (first != null)
                    {
                        t.ContactName = first.NickName;
                        t.Thumbnail = ServerUrl + "/upload/" + EasyWeChatConst.UserThumbnail + first.UserId + "." + first.PicExtension;
                    }
                }
                else
                {
                    var first = groupDatas.FirstOrDefault(m => m.GroupId == t.ContanctId);

                    if (first != null)
                    {
                        t.ContactName = first.GroupName;
                        t.Thumbnail = ServerUrl + "/upload/" + EasyWeChatConst.GroupThumbnail + first.GroupId + "." + first.PicExtension;
                    }
                }
            });

            responseDto.Result = dtos;

            return responseDto;
        }

        /// <summary>
        /// 处理好友申请
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto> DealWithApplyAsync(DealWithApplyDto dealWithApplyDto)
        {
            var apply = await _context.ApplyInfos.FirstOrDefaultAsync(t => t.ApplyId == dealWithApplyDto.ApplyId && t.Status == 0);
            if (apply == null)
            {
                responseDto.Message = "申请不存在";
                responseDto.Code = 400;
                return responseDto;
            }

            if (apply.ContanctId == LoginUserId)
            {
                responseDto.Message = "参数错误";
                responseDto.Code = 400;
                return responseDto;
            }

            apply.LastApplyTime = DateTime.Now;
            apply.Status = dealWithApplyDto.Status;

            if (apply.Status == 1)
            {
                //添加联系人
                await _userContactRepository.AddContact(apply.ApplyUserId, apply.ReceiveUserId, apply.ContanctId, apply.ContactType);

                responseDto.Result = "已同意";
            }
            else if (apply.Status == 3)
            {
                await _userContactRepository.DeleteOrBlockingContact(apply.ContanctId, apply.ApplyUserId, apply.ContactType, 1);
                
                responseDto.Result = "已拉黑";
            }
            else
            {
                responseDto.Result = "已拒绝";
            }

            return responseDto;
        }
    }
}
