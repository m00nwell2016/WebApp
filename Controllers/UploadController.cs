using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.ComponentModel.DataAnnotations;
using WebApp.Models;
using WebApp.Data;

namespace WebApp.Controllers
{
    public class UploadController : Controller
    {
        private IHostingEnvironment hostingEnv;

        public UploadController(IHostingEnvironment env)
        {
            this.hostingEnv = env;
        }

        public IActionResult UploadFiles()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadFiles(IList<IFormFile> files)
        {
            foreach(var file in files)
            {
                var name = ContentDispositionHeaderValue
                            .Parse(file.ContentDisposition)
                            .FileName
                            .Replace("\"", "");

                var destDir = hostingEnv.WebRootPath + "\\Upload\\";

                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }

                using (FileStream fs = System.IO.File.Create(destDir + name))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }

            return View();
        }

        public IActionResult FileShow()
        {
            return View();
        }

        public FileResult Download(String fileName)
        {
            var destDir = hostingEnv.WebRootPath + "\\Upload\\";
            var filepath = destDir + fileName;

            byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);

            return File(fileBytes, "application/x-msdownload",  fileName);
        }
    }
}