using EasyWeChat.IService.Dtos;
using EasyWeChat.IService.Dtos.Inputs;

namespace EasyWeChat.IService.Interfaces
{
    /// <summary>
    /// app发布服务
    /// </summary>
    public interface IAppUpdateService
    {
        /// <summary>
        /// 添加/修改
        /// </summary>
        /// <param name="appUpdateInput"></param>
        /// <returns></returns>
        Task<ResponseDto> AddOrUpdateAppUpdateAsync(AppUpdateInput appUpdateInput);

        /// <summary>
        /// 获取app发布记录
        /// </summary>
        /// <param name="appUpdateQuery"></param>
        /// <returns></returns>
        Task<ResponseDto> GetAppUpdatesAsync(AppUpdateQuery appUpdateQuery);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto> DeleteAddUpdateAsync(long id);

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="publishAppInput"></param>
        /// <returns></returns>
        Task<ResponseDto> PublishAddUpdateAsync(PublishAppInput publishAppInput);

        /// <summary>
        /// 检查更新
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        Task<ResponseDto> CheckVersionAsync(string version);
    }
}
