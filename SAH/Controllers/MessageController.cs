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
    public class MessageController : Controller
    {
        //Connect to web api using Http client
        private static readonly HttpClient client;
        private JavaScriptSerializer JsSerializer = new JavaScriptSerializer();

        //HTTP Handler 
        static MessageController()
        {
            HttpClientHandler clientHandler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };

            client = new HttpClient(clientHandler);
            client.BaseAddress = new Uri("https://localhost:44378/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Message/ChatMessages
        /// <summary>
        /// This is not a full list of all messages in database - that feature is not really useful. This will be send a request to getMessagesByChatId. Recieves a chat Id parameter.
        /// </summary>
        /// <returns>A list of messages for the chat id passed to the method</returns>
        public ActionResult ChatMessages(int id)
        {
            Debug.WriteLine("MESSAGE/CHATMESSAGES/" + id);
            //Request data from API controller via http request 
            string request = "MessageData/getMessagesByChatId/" + id;
            HttpResponseMessage response = client.GetAsync(request).Result;
            //The IHTTPActionResult should send an OK response as well as a MessageDto object list 
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<MessageDto> MessageDtos = response.Content.ReadAsAsync<IEnumerable<MessageDto>>().Result;
                return View(MessageDtos);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Message/Create
        //This method will take a chat Id on get request. The chat id will be stored as the ChatId for the new message on form submission
        public ActionResult Create(int id)
        {
            MessageDto Message = new MessageDto();
            Message.ChatId = id;
            //This will also need to set Message.SenderId from the browser cookie and send along to the create view
            return View(Message);
        }

        // POST: Message/Create - will need to recieve a chat Id to add the message to that chat
        /// <summary>
        /// Method takes a New Message of object type messageDto and sends to api createmessage controller 
        /// </summary>
        /// <param name="NewMessage"></param>
        /// <returns>adds the message to the database and redirects to the show method to display the new message</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MessageDto NewMessage)
        {
            //Create today's date and store as Message.Datesent **
            NewMessage.DateSent = DateTime.Now;
            //Create A sender for now. After the next stage of development, this userId will come from the logged in user's userId cookie
            NewMessage.SenderId = 7;

            //Serialize method returns the object as a Json object - otherwise no way to see contents
            Debug.WriteLine("NEWMESSAGE OBJECT: " + JsSerializer.Serialize(NewMessage));
            //string to send request to - add the chat Id in url
            string requestAddress = "MessageData/createMessage";
            //Create content which sends the message info as a Json object
            HttpContent content = new StringContent(JsSerializer.Serialize(NewMessage));
            //Headers are message headers that preceed the http message content (the json object).
            //Set the headervalue in  "content" to json
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //Send the data object to the request address and store Result in HttpResponseMessage 
            HttpResponseMessage response = client.PostAsync(requestAddress, content).Result;
            Debug.WriteLine("THIS IS THE SERVER RESPONSE: " + response);
            //if response is success status code, display the details of the message in the "Show" view
            if (response.IsSuccessStatusCode)
            {
                //reads messageId from response and sends to show method
                int MessageId = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Show", new { id = MessageId });
            }

            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// The show method takes an ID parameter of type MessageId sends the id to the messagedata/getMessageById method 
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Show view, sending a messageDto object corresponding to that Id </returns>
        // GET: Message/Show/5
        [HttpGet]
        public ActionResult Show(int id)
        {
            Debug.WriteLine("YOU ARE IN THE SHOW CONTROLLER");
            //resquest from the getMessageById controller the team with associated id
            string requestAddress = "MessageData/getMessageById/" + id;
            HttpResponseMessage response = client.GetAsync(requestAddress).Result;

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("THE MESSAGE WAS FOUND");
                MessageDto MessageDto = response.Content.ReadAsAsync<MessageDto>().Result;

                /*Get chat for messageId - messageDto.ChadId*/
                id = MessageDto.ChatId;
                requestAddress = "MessageData/getChatForMessage/" + id;
                response = client.GetAsync(requestAddress).Result;
                ChatDto ThisChat = response.Content.ReadAsAsync<ChatDto>().Result;

                /*Get user for the messageId (sender)*/
                id = MessageDto.SenderId;
                Debug.WriteLine("THE USER ID IS! : " + MessageDto.SenderId);
                requestAddress = "MessageData/getSenderForMessage/" + id;
                response = client.GetAsync(requestAddress).Result;
                UserDto ThisUser = response.Content.ReadAsAsync<UserDto>().Result;

                /*Make vessel for message information*/
                ShowMessage ShowMessage = new ShowMessage();
                ShowMessage.Message = MessageDto;
                ShowMessage.MessageChat = ThisChat;
                ShowMessage.MessageSender = ThisUser;

                return View(ShowMessage);
            }

            else
            {
                Debug.WriteLine("THERE WAS AN ERROR FINDING THE MESSAGE");
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            //logic follows 
            string requestAddress = "MessageData/getMessageById/" + id;
            HttpResponseMessage response = client.GetAsync(requestAddress).Result;

            if (response.IsSuccessStatusCode)
            {
                MessageDto Message = response.Content.ReadAsAsync<MessageDto>().Result;
                Debug.WriteLine("CURRENT MESSAGE OBJECT" + JsSerializer.Serialize(Message));
                return View(Message);
            }

            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Message updatedMessage)
        {
            string requestAddress = "MessageData/updateMessage";
            Debug.WriteLine("NEW MESSAGE DATA: " + JsSerializer.Serialize(updatedMessage));
            HttpContent content = new StringContent(JsSerializer.Serialize(updatedMessage));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(requestAddress, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Show", new { id = updatedMessage.MessageId });
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
            string requestAddress = "MessageData/getMessageById/" + id;
            HttpResponseMessage response = client.GetAsync(requestAddress).Result;

            if (response.IsSuccessStatusCode)
            {
                MessageDto Message = response.Content.ReadAsAsync<MessageDto>().Result;
                Debug.WriteLine("CURRENT MESSAGE OBJECT" + JsSerializer.Serialize(Message));
                return View(Message);
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
            //Ask Christine: Cannot send an integer as http content? Doesnt it defeat the purpose of a post request to send the Id in a url?
            string requestAddress = "MessageData/deleteMessage/" +  id;
            Debug.WriteLine("GOING TO DELETE MESSAGE: " + id);
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(requestAddress, content).Result;
            Debug.WriteLine(response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                /*Return to the list of message for the chat - should no longer see this message*/
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