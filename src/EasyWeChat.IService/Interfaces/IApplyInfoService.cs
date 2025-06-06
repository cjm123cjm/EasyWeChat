using EasyWeChat.IService.Dtos;
using EasyWeChat.IService.Dtos.Inputs;

namespace EasyWeChat.IService.Interfaces
{
    /// <summary>
    /// 用户申请
    /// </summary>
    public interface IApplyInfoService
    {
        /// <summary>
        /// 申请添加好友或群组
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto> ApplyAddAsync(ApplyInfoInput applyInfoInput);
        
        /// <summary>
        /// 申请列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto> ApplyListAsync();

        /// <summary>
        /// 处理好友申请
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto> DealWithApplyAsync(DealWithApplyDto dealWithApplyDto);
    }
}
