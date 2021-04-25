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
        /// This message recieves request from chat controller containing a chat id
        /// </summary>
        /// <param name="id"</param> This is a chat Id
        /// <returns>A list of messages for the chat id passed to the method</returns>
        public ActionResult ChatMessages(int id)
        {
            Debug.WriteLine("MESSAGE/CHATMESSAGES/" + id);
            //Request data from API controller via http request 
            string request = "MessageData/GetMessagesByChatId/" + id;
            HttpResponseMessage response = client.GetAsync(request).Result;
            //The IHTTPActionResult should send an OK response as well as a MessageDto object list 
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<MessageDto> MessageDtos = response.Content.ReadAsAsync<IEnumerable<MessageDto>>().Result;

                ListMessages MessageList = new ListMessages();

                MessageList.Messages = MessageDtos;
                ChatDto thisChat = new ChatDto();
                thisChat.ChatId = id;
                MessageList.Chat = thisChat;

                return View(MessageList);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Message/Create
        /// <summary>
        /// This method will take a chat Id, and add to a new message object. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The new message is passed to the create view to create a new message</returns>
        public ActionResult Create(int id)
        {
            MessageDto Message = new MessageDto();
            Message.ChatId = id;
            //This will also need to set Message.SenderId from the browser cookie and send along to the create view
            return View(Message);
        }

        // POST: Message/Create 
        /// <summary>
        /// Method takes a New Message of object type messageDto and sends to api createmessage controller which will add it to the database
        /// </summary>
        /// <param name="NewMessage"></param>
        /// <returns>If successfully added to database, redirects user to the show method to display the new message</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MessageDto NewMessage)
        {
            //Create today's date and store as Message.Datesent **
            NewMessage.DateSent = DateTime.Now;
            //Create A sender for now. After the next stage of development, this userId will come from the logged in user's userId cookie
            NewMessage.SenderId = User.Identity.GetUserId();

            //Serialize method returns the object as a Json object - otherwise no way to see contents
            Debug.WriteLine("NEWMESSAGE OBJECT: " + JsSerializer.Serialize(NewMessage));
            //string to send request to - add the chat Id in url
            string requestAddress = "MessageData/CreateMessage";
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

        // GET: Message/Show/5
        /// <summary>
        /// The show method takes an ID parameter of type MessageId sends the id to the messagedata/getMessageById method 
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Show view, sending a messageDto object corresponding to that Id </returns>
        [HttpGet]
        public ActionResult Show(int id)
        {
            Debug.WriteLine("YOU ARE IN THE SHOW CONTROLLER");
            //resquest from the getMessageById controller the team with associated id
            string requestAddress = "MessageData/GetMessageById/" + id;
            HttpResponseMessage response = client.GetAsync(requestAddress).Result;

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("THE MESSAGE WAS FOUND");
                MessageDto MessageDto = response.Content.ReadAsAsync<MessageDto>().Result;

                /*Get chat for messageId - messageDto.ChadId*/
                id = MessageDto.ChatId;
                requestAddress = "ChatData/GetChatById/" + id;
                response = client.GetAsync(requestAddress).Result;
                ChatDto ThisChat = response.Content.ReadAsAsync<ChatDto>().Result;

                /*Get user for the messageId (sender)*/
                string senderId = MessageDto.SenderId;
                Debug.WriteLine("THE USER ID IS : " + MessageDto.SenderId);
                requestAddress = "UserData/GetUserById/" + id;
                response = client.GetAsync(requestAddress).Result;
                ApplicationUserDto ThisUser = response.Content.ReadAsAsync<ApplicationUserDto>().Result;

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

        // GET: Message/Edit/5
        /// <summary>
        /// Method takes parameter of messageId and sends request to GetMessageById to retrieve the message associated with the given Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>If a message is found in the database, the message will be sent to the edit view where a user can update the object properties</returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            //logic follows 
            string requestAddress = "MessageData/GetMessageById/" + id;
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

        // POST: Message/Edit
        /// <summary>
        /// Method accepts post request with a message object parameter called UpdateMessage and calls MessageData/UpdateMessage API method which updates the database record
        /// </summary>
        /// <param name="UpdatedMessage"></param>
        /// <returns>The updated database record to the show view where the user can review the edits they have made</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Message UpdatedMessage)
        {
            string requestAddress = "MessageData/UpdateMessage";
            Debug.WriteLine("NEW MESSAGE DATA: " + JsSerializer.Serialize(UpdatedMessage));
            HttpContent content = new StringContent(JsSerializer.Serialize(UpdatedMessage));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(requestAddress, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Show", new { id = UpdatedMessage.MessageId });
            }

            else
            {
                return RedirectToAction("Error");
            }
        }

        //GET: Message/ConfirmDelete/3
        /// <summary>
        /// Method accepts get request containing message Id and sends request to API method messagedata/getmessagebyid to get the corresponding message record in database 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns view where user can confirm they want to delete the record</returns>
        [HttpGet]
        public ActionResult ConfirmDelete(int id)
        {
            //logic follows 
            string requestAddress = "MessageData/GetMessageById/" + id;
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

        //POST: Message/Delete
        /// <summary>
        /// Takes post request containing message Id to be created. Request is validated to come from the confirmdelete page. Sends request to API method messagedata/deletemessage
        /// </summary>
        /// <param name="id"></param>
        /// <returns>If deletion successful, redirects user to list page where they will no longer see the message</returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int chatId, int messageId)
        {
/*            Debug.WriteLine("MESSAGE TO DELETE OBJECT: " + JsSerializer.Serialize(MessageToDelete));
*/          string requestAddress = "MessageData/DeleteMessage/" + messageId;
            
            Debug.WriteLine("GOING TO DELETE MESSAGE: " + messageId);
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(requestAddress, content).Result;
            Debug.WriteLine(response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                /*Return to the list of messages for the chat - should no longer see this message*/
                return RedirectToAction("ChatMessages", new { id = chatId });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //Error view 
        public ActionResult Error()
        {
            return View();
        }
    }
}