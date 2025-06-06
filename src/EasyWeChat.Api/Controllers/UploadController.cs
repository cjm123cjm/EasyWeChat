using EasyWeChat.IService.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace EasyWeChat.Api.Controllers
{
    /// <summary>
    /// 文件上传
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly ResponseDto _responseDto;
        private readonly IWebHostEnvironment _hostEnvironment;
        /// <summary>
        /// 注入服务
        /// </summary>
        /// <param name="hostEnvironment"></param>
        public UploadController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            _responseDto = new ResponseDto();
        }

        /// <summary>
        /// 上传文件（支持多文件/大文件500M）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 609715200)]
        [RequestSizeLimit(609715200)]
        public async Task<ResponseDto> UploadFile()
        {
            var request = HttpContext.Request;

            if (!request.HasFormContentType ||
                !MediaTypeHeaderValue.TryParse(request.ContentType, out var mediaTypeHeader) ||
                string.IsNullOrWhiteSpace(mediaTypeHeader.Boundary.Value))
            {
                _responseDto.Message = "文件类型不支持";
                _responseDto.Code = 400;
                return _responseDto;
            }

            var reader = new MultipartReader(mediaTypeHeader.Boundary.Value, request.Body);
            var section = await reader.ReadNextSectionAsync();

            while (section != null)
            {
                var hasContent = ContentDispositionHeaderValue.TryParse(section.ContentDisposition,
                    out var contentDisposition);

                if (hasContent &&
                    contentDisposition!.DispositionType.Equals("form-data") &&
                    !string.IsNullOrEmpty(contentDisposition.FileName.Value))
                {
                    var extension = Path.GetExtension(contentDisposition.FileName.Value);

                    //为文件重命名
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;
                    // 文件保存的文件夹路径
                    var uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "upload");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    var fileFullPath = Path.Combine(uploadPath, fileName);
                    try
                    {
                        using var targetStream = System.IO.File.Create(fileFullPath);
                        await section.Body.CopyToAsync(targetStream);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                section = await reader.ReadNextSectionAsync();
            }

            return _responseDto;
        }
    }
}
