using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using System.Web.Http.Results;
using SAH.Models;
using SAH.Models.ModelViews;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Description;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace SAH.Controllers
{
    public class ChatController : Controller
    {
        //Connect to web api using Http client
        private static readonly HttpClient client;
        private JavaScriptSerializer JsSerializer = new JavaScriptSerializer();

        //HTTP Handler 
        static ChatController()
        {
            HttpClientHandler clientHandler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };

            client = new HttpClient(clientHandler);
            client.BaseAddress = new Uri("https://localhost:44378/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        // GET: Chat/List
        /// <summary>
        ///Full list of all chats. This is only available to an admin user. Users should not be able to see the chats of other users
        /// </summary>
        /// <returns>The full list of chats in the database</returns>
        [HttpGet]
        [Authorize]
        public ActionResult List()
        {
            /*If logged in user is Admin, show all chats*/
            if (User.IsInRole("Admin")){
                //Request data from API controller via http request 
                string request = "ChatData/GetChats/";
                HttpResponseMessage response = client.GetAsync(request).Result;
                //The IHTTPActionResult should send an OK response as well as a ChatDto object list 
                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<ChatDto> ChatDtos = response.Content.ReadAsAsync<IEnumerable<ChatDto>>().Result;
                    return View(ChatDtos);
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }

            /*Otherwise, get just the chats for the user logged in*/
            else
            {
                string id = User.Identity.GetUserId();
                //Request data from API controller via http request 
                string request = "ChatData/GetChatsForUser/" + id;
                HttpResponseMessage response = client.GetAsync(request).Result;
                //The IHTTPActionResult should send an OK response as well as a ChatDto object list 
                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<ChatDto> ChatDtos = response.Content.ReadAsAsync<IEnumerable<ChatDto>>().Result;
                    return View(ChatDtos);
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }

            
        }

        // GET: Chat/Create
        /// <returns>The create view, where a user can create a new chat</returns>
        public ActionResult Create()
        {
            /*Search by User ID of Doctor == */
            string id = "4";
            //The view needs to be sent a list of all the users so the client can select a user as the recipient in the view
            string requestAddress = "UserData/GetUsersByRoleId/" + id;
            HttpResponseMessage response = client.GetAsync(requestAddress).Result;
            List<ApplicationUserDto> Doctors = response.Content.ReadAsAsync<List<ApplicationUserDto>>().Result;

            CreateChat CreateChat = new CreateChat();
            CreateChat.Doctors = Doctors;
            Debug.WriteLine("Users object:" + JsSerializer.Serialize(CreateChat.Doctors));
            return View(CreateChat);
        }

        // POST: Chat/Create 
        /// <summary>
        /// Method takes a New Chat of object type ChatDto and sends to api createmessage controller 
        /// </summary>
        /// <param name="NewChat"></param>
        /// <returns>If succesful, the client will be redirected to the ChatMessages view where they will be prompted to create a new message for their new chat </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateChat CreateChat, string RecipientId)
        {
            /*Separate Chat from message. Create new chat, then create new message.*/
            string SenderId = User.Identity.GetUserId(); 
            Debug.WriteLine("Recipient:" + RecipientId);
            Debug.WriteLine("CreateChat OBJECT: " + JsSerializer.Serialize(CreateChat));
            ChatDto NewChat = CreateChat.Chat;
            NewChat.DateCreated = DateTime.Now;

            //string to send request to the createchat API method with the id of the recipient and sender
            string requestAddress = "ChatData/CreateChat";

            //Create content which sends the Chat info as a Json object
            HttpContent content = new StringContent(JsSerializer.Serialize(NewChat));
            Debug.WriteLine("NEW CHAT OBJECT: " + JsSerializer.Serialize(NewChat));
            Debug.WriteLine("Request Address: " + requestAddress);
            //Headers are Chat headers that preceed the http Chat content (the json object).
            //Set the headervalue in  "content" to json
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //Send the data object to the request address and store Result in HttpResponseMessage 
            HttpResponseMessage response = client.PostAsync(requestAddress, content).Result;
            //if response is success status code, display the details of the Chat in the "Show" view
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("SUCCESS ADDING CHAT");
                //ASSIGN New Chat Id Var FROM RETURNED CHAT ID
                int ChatId = response.Content.ReadAsAsync<int>().Result;
                /*Add the users to the chat (sender and recipient) via ChatData/AddUsersForChat/UserId api method */
                //Add sender
                string UserId = SenderId;
                requestAddress = "ChatData/AddUserForChat/"+ UserId + "/" + ChatId;
                Debug.WriteLine("THIS CHAT ID IS: " + ChatId);
                content = new StringContent("");
                response = client.PostAsync(requestAddress, content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Error");
                }

                //Add Recipient
                UserId = RecipientId;
                requestAddress = "ChatData/AddUserForChat/" + UserId + "/" + ChatId;
                content = new StringContent("");
                response = client.PostAsync(requestAddress, content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Error");
                }

                //Add the initial message to the chat

                MessageDto NewMessage = new MessageDto
                {
                    SenderId = SenderId,
                    ChatId = ChatId,  
                    DateSent = DateTime.Now,
                    Content = CreateChat.FirstMessage.Content,
                };

                requestAddress = "MessageData/CreateMessage";
                content = new StringContent(JsSerializer.Serialize(NewMessage));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = client.PostAsync(requestAddress, content).Result;

                //reads messageId from response and sends to show method

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Error");
                }

                /*Add the message sender to the chat */

                else
                {
                    return RedirectToAction("../Message/ChatMessages", new { id = ChatId });
                }

            }

            else
            {
                return RedirectToAction("Error");
            }
        }


        /// <summary>
        /// Method takes a chat id parameter and returns the edit view. Only the chat subject line and users involved can be updated
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            //logic follows 
            string requestAddress = "Chatdata/GetChatById/" + id;
            HttpResponseMessage response = client.GetAsync(requestAddress).Result;

            if (response.IsSuccessStatusCode)
            {
                ChatDto Chat = response.Content.ReadAsAsync<ChatDto>().Result;
                Debug.WriteLine("CURRENT Chat OBJECT" + JsSerializer.Serialize(Chat));
                return View(Chat);
            }

            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ChatDto updatedChat)
        {
            string requestAddress = "Chatdata/UpdateChat";
            Debug.WriteLine("NEW Chat DATA: " + JsSerializer.Serialize(updatedChat));
            HttpContent content = new StringContent(JsSerializer.Serialize(updatedChat));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(requestAddress, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ChatMessages", new { id = updatedChat.ChatId });
            }

            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public ActionResult ConfirmDelete(int id)
        {
            //logic follows 
            string requestAddress = "Chatdata/GetChatById/" + id;
            HttpResponseMessage response = client.GetAsync(requestAddress).Result;

            if (response.IsSuccessStatusCode)
            {
                ChatDto Chat = response.Content.ReadAsAsync<ChatDto>().Result;
                Debug.WriteLine("CURRENT Chat OBJECT" + JsSerializer.Serialize(Chat));
                return View(Chat);
            }

            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string requestAddress = "ChatData/DeleteChat/" + id;
            Debug.WriteLine("GOING TO DELETE Chat: " + id);
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(requestAddress, content).Result;
            Debug.WriteLine(response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                /*Return to the list of Chat for the chat - should no longer see this Chat*/
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}