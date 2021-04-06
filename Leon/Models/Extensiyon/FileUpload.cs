using Microsoft.AspNetCore.Http;
using Leon.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Leon.Models.Extensiyon.ImagesHelpers;

namespace Leon.Models.Extensiyon
{
    public class FileUpload : IFileUpload
    {
        private readonly MyContext _context;
        public FileUpload(MyContext myContext)
        {
            _context = myContext;
        }
        public async Task<string> Create(string root, IFormFile file, string mainFolderName, string subFolderName, string Link)
        {
            string Kod = "";
            if (file != null)
            {
                try
                {
                    if (ImageIsValid(file))
                    {
                        string url = await ImageUploadAsync( root, file, mainFolderName, subFolderName);
                        Kod = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
                        _context.Files.Add(new Models.BLL.File
                        {
                            FolderName = mainFolderName,
                            CreateDate = DateTime.Now,
                            LinkFile = Link,
                            Size = file.Length.ToString(),
                            UrlFile = url,
                            UniqKod = Kod,
                            Type = file.ContentType
                        });
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception)
                {
                    return "";
                }
            }
            return Kod;
        }
    }
}
