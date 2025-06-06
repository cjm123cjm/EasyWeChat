using EasyWeChat.Domain.Entities;
using EasyWeChat.Domain.IRepository;
using EasyWeChat.IService.Dtos;
using EasyWeChat.IService.Dtos.Inputs;
using EasyWeChat.IService.Dtos.Outputs;
using EasyWeChat.IService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EasyWeChat.Service.Implement
{
    public class AppUpdateService : ServiceBase, IAppUpdateService
    {
        private readonly IAppUpdateRepository _appUpdateRepository;
        public ResponseDto response;

        public AppUpdateService(IAppUpdateRepository appUpdateRepository)
        {
            _appUpdateRepository = appUpdateRepository;
            response = new ResponseDto();
        }

        /// <summary>
        /// 添加/修改
        /// </summary>
        /// <param name="appUpdateInput"></param>
        /// <returns></returns>
        public async Task<ResponseDto> AddOrUpdateAppUpdateAsync(AppUpdateInput appUpdateInput)
        {
            if (appUpdateInput.Id == 0)
            {
                var appUpdate = ObjectMapper.Map<AppUpdate>(appUpdateInput);
                appUpdate.Id = SnowIdWorker.NextId();

                await _appUpdateRepository.AddAsync(appUpdate);
            }
            else
            {
                var appUpdate = await _appUpdateRepository.FindByIdAsync(appUpdateInput.Id);
                if (appUpdate == null)
                {
                    response.Message = "数据不存在";
                    response.Code = 400;
                    return response;
                }
                if (appUpdate.Status != 0)
                {
                    response.Message = "数据已发布无法修改";
                    response.Code = 400;
                    return response;
                }
                ObjectMapper.Map(appUpdate, appUpdateInput);

                await _appUpdateRepository.UpdateAsync(appUpdate);
            }

            return response;
        }

        /// <summary>
        /// 检查更新
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public async Task<ResponseDto> CheckVersionAsync(string version)
        {
            var first = await _appUpdateRepository.All().Where(t => (t.Status == 2 || (t.Status == 1 && t.GrayscaleUid != null && t.GrayscaleUid.Contains(LoginUserId.ToString())))).FirstOrDefaultAsync();
            if (first != null)
            {
                if (first.Version != version)
                {
                    response.Result = ObjectMapper.Map<AppUpdateDto>(first);
                }
            }
            return response;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto> DeleteAddUpdateAsync(long id)
        {
            var appUpdate = await _appUpdateRepository.FindByIdAsync(id);
            if (appUpdate == null)
            {
                response.Message = "数据不存在";
                response.Code = 400;
                return response;
            }
            if (appUpdate.Status != 0)
            {
                response.Message = "数据已发布无法修改";
                response.Code = 400;
                return response;
            }
            await _appUpdateRepository.DeleteAsync(appUpdate);

            return response;
        }

        /// <summary>
        /// 获取app发布记录
        /// </summary>
        /// <param name="appUpdateQuery"></param>
        /// <returns></returns>
        public async Task<ResponseDto> GetAppUpdatesAsync(AppUpdateQuery appUpdateQuery)
        {
            var query = _appUpdateRepository.All();

            if (!string.IsNullOrWhiteSpace(appUpdateQuery.Version))
            {
                query = query.Where(t => t.Version.Contains(appUpdateQuery.Version));
            }

            var count = await query.CountAsync();

            var apps = await query.OrderByDescending(t => t.CreateTime)
                                  .Skip((appUpdateQuery.PageIndex - 1) * appUpdateQuery.PageSize)
                                  .Take(appUpdateQuery.PageSize).ToListAsync();



            PageDto<AppUpdateDto> pageDto = new PageDto<AppUpdateDto>
            {
                Data = ObjectMapper.Map<List<AppUpdateDto>>(apps),
                Total = count
            };
            response.Result = pageDto;

            return response;
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="publishAppInput"></param>
        /// <returns></returns>
        public async Task<ResponseDto> PublishAddUpdateAsync(PublishAppInput publishAppInput)
        {
            var appUpdate = await _appUpdateRepository.FindByIdAsync(publishAppInput.Id);
            if (appUpdate == null)
            {
                response.Message = "数据不存在";
                response.Code = 400;
                return response;
            }
            if (appUpdate.Status != 0)
            {
                response.Message = "数据已发布无法修改";
                response.Code = 400;
                return response;
            }

            appUpdate.Status = publishAppInput.Status;
            appUpdate.GrayscaleUid = appUpdate.GrayscaleUid;

            await _appUpdateRepository.UpdateAsync(appUpdate);

            return response;
        }
    }
}
