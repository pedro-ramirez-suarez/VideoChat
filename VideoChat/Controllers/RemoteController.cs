using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
//using System.Web.Http;
using Needletail.Mvc;
using Needletail.Mvc.Communications;
using System.Web;

namespace VideoChat.Controllers
{
    public class RemoteController : RemoteExecutionController
    {
        public static List<string> Users = new List<string>();
        public RemoteController()
        {
            this.IncommingConnectionIdAssigned += new IncommingConnectionIdAssignedDelegate(ApiTestController_IncommingConnectionIdAssigned);
            this.IncommingConnectionAssigningId += new IncommingConnectionAssigningIdDelegate(ApiTestController_IncommingConnectionAssigningId);
            RemoteExecutionController.ConnectionLost += new ConnectionLostDelegate(TwoWayController_ConnectionLost);
            
        }


        void TwoWayController_ConnectionLost(ClientCall call)
        {
            //Implement code that needs to be executed when a connection is lost
        }

        string ApiTestController_IncommingConnectionAssigningId()
        {
            //return the user name that they entered
            string user = HttpContext.Current.Request.Cookies["videoChatUser"].Value;
            if (!Users.Contains(user))
                Users.Add(user);
            return user;
        }

        void ApiTestController_IncommingConnectionIdAssigned(string newId)
        {
            //code that needs to run after a connection has been succesfully configured
        }

		       
        
    }
}
