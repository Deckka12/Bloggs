using Bloggs.Models.Request;
using Bloggs.Models.Response;
using DBContex.Models;
using DBContex.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bloggs.Controllers
{
    public class CommentController : Controller
    {



        public CommentController()
        {
           
        }

        private readonly string _videosRoot = "wwwroot/Videos"; // Путь к вашим видео в проекте

        public IActionResult Index()
        {
            var folderNames = Directory.GetDirectories(_videosRoot)
                                       .Select(Path.GetFileName)
                                       .ToArray();
            return View(folderNames);
        }


        public IActionResult Create(int postId)
        {
            ViewBag.PostId = postId;
            return View();
        }
        public IActionResult ShowVideos(string folderName)
        {
            string folderPath = Path.Combine(_videosRoot, folderName);
            string[] fileNames = Directory.GetFiles(folderPath);

            var videoUrls = fileNames.Select(fileName =>
                Path.Combine("/Videos", folderName, Path.GetFileName(fileName))
            ).ToArray();

            ViewBag.FolderName = folderName;
            ViewBag.VideoUrls = videoUrls;
            return View();
        }


    }
}
