using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Attachment
{
    public interface IAttachmentService
    {
       Task<String?> UploadAsync(Stream fileStream, string folderPath, string fileName, CancellationToken ct = default);
        bool Delete(string folderName, string fileName);
        (Stream stream, string contentType)? GetFile(string folderName, string fileName);
        Task<string?> GetMemberPictureAsync(int id, CancellationToken ct);
    }
}
