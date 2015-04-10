using Needletail.Mvc;
using Needletail.Mvc.Communications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VideoChat.Controllers
{
    public class HomeController : Controller
    {

        static Dictionary<string,MemoryStream> audios = new  Dictionary<string, MemoryStream>();

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(string userName)
        {
            Response.Cookies.Add(new HttpCookie("videoChatUser",userName));
            ViewBag.UserName = userName;
            return View("Chat");
        }

        [HttpPost]
        public JsonResult PushAudio()
        {
            MemoryStream ms = new MemoryStream();
            Request.Files[0].InputStream.CopyTo(ms);
            ms.Position = 0;
            string id = Guid.NewGuid().ToString().Replace("-", "");
            audios.Add(id,ms);
            
            //get the other users
            var receivers = RemoteController.Users.Where(u => u != Request.Cookies["videoChatUser"].Value);
            //send the audio to everyone
            foreach (var u in receivers)
            {
                dynamic call = new ClientCall { CallerId = Request.Cookies["videoChatUser"].Value, ClientId = u };
                call.updateAudio(id);
                RemoteExecution.ExecuteOnClient(call, false);
            }
            return Json(new { success = true });
        }

        public FileResult GetAudio(string id)
        {

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = string.Format("{0}.wav", id),

                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                //Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());

            return new FileStreamResult(audios[id], "audio/vnd.wave");
        }

        [HttpPost]
        public ActionResult PushVideo(string video)
        {
            //get the other users
            var receivers = RemoteController.Users.Where(u => u != Request.Cookies["videoChatUser"].Value);
            //send the audio to everyone
            foreach (var u in receivers)
            {
                dynamic call = new ClientCall { CallerId = Request.Cookies["videoChatUser"].Value, ClientId = u };
                call.updateVideo(video);
                try
                {
                    RemoteExecution.ExecuteOnClient(call, true);
                }
                catch (Exception e)
                { 
                }
            }
            return Json(new { success = true });
        }

        
    }
}