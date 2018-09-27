using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FunctionalFitness.Models;
using FunctionalFitness.Data;
using FunctionalFitness.Models.VideoViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using YoutubeSearch;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FunctionalFitness.Controllers
{
    public class VideoController : Controller
    {
        private ApplicationDbContext context;

        public VideoController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        public IQueryable<Video> GetVideoList()
        {
            //ApplicationUser thisUser = new ApplicationUser();
            string thisUser = HttpContext.User.Identity.Name;
            int time = new int();
            if (thisUser != null)
            {
                DateTime regDate = context.Users
                .FirstOrDefault(c => c.UserName == thisUser)
                .RegDate;

                time = (DateTime.Now - regDate).Days;
            }
            else time = 0;

            IQueryable<Video> videos = context.Videos
                .Where(c => c.ReleaseDays <= time)
                .OrderByDescending(d => d.ReleaseDays);

            return videos;
        }

        // GET: /<controller>/
        public IActionResult Index(string title)
        {
            VideoViewModel videoViewModel = new VideoViewModel();
            videoViewModel.Title = title;
            videoViewModel.Videos = GetVideoList();


            return View(videoViewModel);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Search()
        {
            VideoSearchViewModel videoSearchViewModel = new VideoSearchViewModel();
            List<Video> list = new List<Video>();
            videoSearchViewModel.List = list;

            return View(videoSearchViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Search(VideoSearchViewModel videoSearchViewModel)
        {
            VideoSearch items = new VideoSearch();
            List<Video> list = new List<Video>();
            foreach (var item in items.SearchQuery(videoSearchViewModel.SearchText, 1))
            {
                Video video = new Video();
                video.Title = item.Title;
                video.Author = item.Author;
                video.Url = item.Url;
                video.Thumbnail = item.Thumbnail;
                list.Add(video);
            }
            videoSearchViewModel.List = list;


            return View(videoSearchViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddVideo(string title, string author, string url, string thumbnail)
        {
            AddVideoViewModel addVideoViewModel = new AddVideoViewModel()
            {
                Title = title,
                Author = author,
                Url = url,
                Thumbnail = thumbnail,

            };
            return View(addVideoViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddVideo(AddVideoViewModel addVideoViewModel)
        {
            if (ModelState.IsValid)
            {
                // Add the new video to existing videos

                Video newVideo = new Video
                {
                    Author = addVideoViewModel.Author,
                    Title = addVideoViewModel.Title,
                    Url = addVideoViewModel.Url,
                    Thumbnail = addVideoViewModel.Thumbnail,
                    ReleaseDays = addVideoViewModel.ReleaseDays
                };

                context.Videos.Add(newVideo);
                context.SaveChanges();

                return Redirect("/Video");
            }
            return View(addVideoViewModel);
        }
    }
}
