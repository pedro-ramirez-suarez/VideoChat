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

        /// <summary>
        /// Receives audio from a user
        /// </summary>
        [HttpPost]
        public JsonResult PushAudio()
        {
            //Get the audio stream and store it on a local collection
            MemoryStream ms = new MemoryStream();
            Request.Files[0].InputStream.CopyTo(ms);
            ms.Position = 0;
            string id = Guid.NewGuid().ToString().Replace("-", "");
            audios.Add(id,ms);

            //send the audio to everyone but me
            var receivers = RemoteController.Users.Where(u => u != Request.Cookies["videoChatUser"].Value);
            foreach (var u in receivers)
            {
                dynamic call = new ClientCall { CallerId = Request.Cookies["videoChatUser"].Value, ClientId = u };
                //since we cannot send the audio, we just send the ID of the audio that we just received
                call.updateAudio(id);
                RemoteExecution.ExecuteOnClient(call, false);
            }
            return Json(new { success = true });
        }

        /// <summary>
        /// Gets the audio stream requested
        /// </summary>
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
            //get the audio
            var audio =audios[id];
            //remove it from the local list
            audios.Remove(id);
            return new FileStreamResult(audio, "audio/vnd.wave");
        }

        /// <summary>
        /// Receives image from the user
        /// </summary>
        /// <param name="video">the image</param>
        [HttpPost]
        public ActionResult PushVideo(string video)
        {
            //send the video to everyone but me
            var receivers = RemoteController.Users.Where(u => u != Request.Cookies["videoChatUser"].Value);
            foreach (var u in receivers)
            {
                //we send the video, and the user who sent it
                dynamic call = new ClientCall { CallerId = Request.Cookies["videoChatUser"].Value, ClientId = u };
                call.updateVideo(video,u);
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