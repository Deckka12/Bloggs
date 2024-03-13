using DBContex.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DBContex.Models;
using DBContex.Repository;

namespace Bloggs.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;


        public ArticleController( IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

        }

        private readonly string _photosRoot = "wwwroot/Photos"; // Путь к вашим фотографиям в проекте

        public IActionResult Index()
        {
            var folderNames = Directory.GetDirectories(_photosRoot)
                                        .Select(Path.GetFileName)
                                        .ToArray();
            return View(folderNames);
        }
        public IActionResult Contacts()
        {
           
            return View();
        }
        public IActionResult History()
        {

            return View();
        }

        public IActionResult ShowPhotos(string folderName)
        {
            string folderPath = Path.Combine(_photosRoot, folderName);
            string[] fileNames = Directory.GetFiles(folderPath);

            var photoUrls = fileNames.Select(fileName =>
                Path.Combine("/Photos", folderName, Path.GetFileName(fileName))
            ).ToArray();

            ViewBag.FolderName = folderName;
            ViewBag.PhotoUrls = photoUrls;
            return View();
        }
    }
}
