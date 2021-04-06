using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Leon.Models.Extensiyon
{
    public class ImagesHelpers
    {
        public static bool ImageIsValid(IFormFile file)
        {
            if (file.Length <= 5 * 1024 * 1024 && (
                           file.ContentType == "image/jpg" ||
                           file.ContentType == "image/jpeg"||
                           file.ContentType == "image/png" 
                           ))
            { return true; }
            else
            {
                return false;
            }
        }
        public static void DeleteImage(string filename, string path)
        {
            try
            {
                string file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", string.Concat(path, filename));
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
            catch
            { }

        }
        public static async Task<string> ImageUploadAsync(string root, IFormFile img, string paths, string sub)
        {
            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", paths);
            string fileID = Guid.NewGuid().ToString().Replace("-", "");
            string filename = sub + "/" + fileID + Path.GetFileName(img.FileName);
            var filePath = Path.Combine(root, paths, filename);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await img.CopyToAsync(fileStream);
            }
            return filename;
        }

      
        public static string GetMimeTypes(string url)
        {
            string ext = "";
            switch (url)
            {
                case "text/plain":
                    ext = ".txt";
                    break;
                case "application/pdf":
                    ext = ".pdf";
                    break;
                case "application/vnd.ms-word":
                    ext = ".doc";
                    break;
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                    ext = ".docx";
                    break;
                case "application/vnd.ms-excel":
                    ext = ".xls";
                    break;
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                    ext = ".xlsx";
                    break;
                case "application/vnd.openxmlformats-officedocument.presentationml.presentation":
                    ext = ".pptx";
                    break;
                case "image/jpeg":
                    ext = ".jpeg";
                    break;
                case "image/jpg":
                    ext = ".jpg";
                    break;
                case "image/png":
                    ext = ".png";
                    break;
                case "image/gif":
                    ext = ".gif";
                    break;
                case "image/svg+xml":
                    ext = ".svg";
                    break;
            }
            return ext;
        }

        public static void ImageUploadResize(string input_Image_Path, string output_Image_Path, int width, int height)
        {
            const long quality = 50L;
            Bitmap image = new Bitmap(input_Image_Path);
            var resized_Bitmap = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(resized_Bitmap))
            {

                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.DrawImage(image, 0, 0, width, height);
                using (var output = File.Open(output_Image_Path, FileMode.Create))
                {
                    var qualityParamId = Encoder.Quality;
                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
                    var codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);
                    resized_Bitmap.Save(output, codec, encoderParameters);
                    output.Close();
                }
                graphics.Dispose();
            }
            image.Dispose();

        }
    }
}
