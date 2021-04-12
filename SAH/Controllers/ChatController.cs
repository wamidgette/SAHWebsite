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

        // GET: Chat/ListForChat
        /// <summary>
        ///Full lis of chats in the database - this is for an admin user and will be restricted for admin usage at a later time
        /// </summary>
        /// <returns>The full list of chats in the database</returns>
        [HttpGet]
        public ActionResult List()
        {
            //Request data from API controller via http request 
            string request = "ChatData/getChats/";
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

        // GET: Chat/Create
        /// <returns>The create view, where a user can create a new chat</returns>
        public ActionResult Create()
        {
            //The view needs to be sent a list of all the users so the client can select a user as the recipient in the view
            string requestAddress = "ChatData/getDoctors/";
            HttpResponseMessage response = client.GetAsync(requestAddress).Result;
            List<UserDto> Doctors = response.Content.ReadAsAsync<List<UserDto>>().Result;

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
        public ActionResult Create(CreateChat CreateChat, int RecipientId)
        {
            /*Separate Chat from message. Create new chat, then create new message.*/

            Debug.WriteLine("Recipient:" + RecipientId);
            Debug.WriteLine("CreateChat OBJECT: " + JsSerializer.Serialize(CreateChat));
            ChatDto NewChat = new ChatDto
            {
                Subject = CreateChat.Chat.Subject,
                DateCreated = DateTime.Now,
            };

            //string to send request to 
            string requestAddress = "ChatData/createChat";
            //Create content which sends the Chat info as a Json object
            HttpContent content = new StringContent(JsSerializer.Serialize(NewChat));
            //Headers are Chat headers that preceed the http Chat content (the json object).
            //Set the headervalue in  "content" to json
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //Send the data object to the request address and store Result in HttpResponseMessage 
            HttpResponseMessage response = client.PostAsync(requestAddress, content).Result;
            //if response is success status code, display the details of the Chat in the "Show" view
            if (response.IsSuccessStatusCode)
            {
                //ASSIGN New Chat Id Var FROM RETURNED CHAT ID
                int ChatId = response.Content.ReadAsAsync<int>().Result;

                MessageDto NewMessage = new MessageDto
                {
                    //Leaving this as 7 for now. Later on, this will take the userId cookie from the logged in user who created the chat
                    SenderId = 7,
                    ChatId = ChatId,  
                    DateSent = DateTime.Now,
                    Content = CreateChat.FirstMessage.Content,
                };

                requestAddress = "MessageData/createMessage";
                content = new StringContent(JsSerializer.Serialize(NewMessage));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = client.PostAsync(requestAddress, content).Result;

                //reads messageId from response and sends to show method

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("../Message/chatMessages", new { id = ChatId });
                }

                else
                {
                    return RedirectToAction("Error");
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
            string requestAddress = "Chatdata/getChatById/" + id;
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
            string requestAddress = "Chatdata/updateChat";
            Debug.WriteLine("NEW Chat DATA: " + JsSerializer.Serialize(updatedChat));
            HttpContent content = new StringContent(JsSerializer.Serialize(updatedChat));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(requestAddress, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("chatMessages", new { id = updatedChat.ChatId });
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
            string requestAddress = "Chatdata/getChatById/" + id;
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
            string requestAddress = "ChatData/deleteChat/" + id;
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