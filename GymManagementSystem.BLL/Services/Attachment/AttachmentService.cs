using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Attachment
{
    public class AttachmentService : IAttachmentService
    {

        private readonly long _maxFileSize = 5 * 1024 * 1024; // 5 MB
        private readonly IWebHostEnvironment _env;
        private readonly string[] _allowedExtensions = [ ".jpg", ".jpeg", ".png"];

        public AttachmentService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public bool Delete(string folderName, string fileName)
        {
            try
            {
                var filePath = Path.Combine(_env.ContentRootPath, folderName, fileName);
                if (!File.Exists(filePath))
                
                    File.Delete(filePath);
                    return true;              
            }
            catch (Exception ex)
            {
               return false;
            }
        }

        public (Stream stream, string contentType)? GetFile(string folderName, string fileName)
        {
            if (string.IsNullOrWhiteSpace(folderName) || string.IsNullOrWhiteSpace(fileName)) return null;
            var filePath = Path.Combine(_env.ContentRootPath, folderName, fileName);
            if (!File.Exists(filePath)) return null;
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var extension = Path.GetExtension(filePath).ToLower();
            var contentType = extension switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };
            return (stream, contentType);


        }

        public Task<string?> GetMemberPictureAsync(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<string?> UploadAsync(Stream fileStream, string folderName, string fileName, CancellationToken ct = default)
        {
            if(fileStream is null || !fileStream.CanRead) return null;
            if(fileStream.Length == 0) return null;
            if(fileStream.Length > _maxFileSize) return null;

            var extension = Path.GetExtension(fileName);
            if (string.IsNullOrWhiteSpace(extension) || !_allowedExtensions.Contains(extension)) return null;

            
            var uploadFolder = Path.Combine(_env.WebRootPath, folderName);
            Directory.CreateDirectory(uploadFolder);

            var storageFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadFolder, storageFileName);

            try
                {
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    await fileStream.CopyToAsync(fs, ct);
                
                return storageFileName;
            }
            catch (Exception ex) 
            {
                return null;
            }




        }
    }
}
