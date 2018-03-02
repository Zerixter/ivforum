using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IVForum.API.Classes
{
    public class Upload
    {
        public static string UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            if (file.ContentType != "image/jpeg" && file.ContentType != "image/png" && file.ContentType != "image/jpg")
            {
                return null;
            }

            var Name = Guid.NewGuid().ToString();

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), @"Resources/Images",
                        Name);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyToAsync(stream).GetAwaiter();
            }
            return path;
        }
    }
}
