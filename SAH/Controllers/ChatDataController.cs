﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SAH.Models;
using System.Diagnostics;
using Microsoft.AspNet.Identity;


namespace SAH.Controllers
{
    public class ChatDataController : ApiController
    {
        private SAHDataContext db = new SAHDataContext();

        /// <summary>
        /// GET: api/ChatData/GetChatById/4 takes a ChatId and returns the associated ChatDto object
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ChatDto object</returns>

        [HttpGet]
        [ResponseType(typeof(ChatDto))]
        public IHttpActionResult GetChatById(int id)
        {
            Debug.WriteLine("YOU ARE IN THE GET Chat BY ID TEST API METHOD");
            Chat Chat = db.Chats.Find(id);

            if (Chat == null)
            {
                Debug.WriteLine("COULD NOT FIND Chat IN API");
                return NotFound();
            }

            ChatDto ChatDto = new ChatDto
            {
                ChatId = Chat.ChatId,
                Subject = Chat.Subject,
                DateCreated = Chat.DateCreated,

            };
            Debug.WriteLine("FOUND THE Chat IN API");
            return Ok(ChatDto);
        }
        /*getChats  - this is for admin users and will be implemented fully later on.*/
        /// <summary>
        /// Returns a full list of all chats - no parameters
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ChatDto))]
/*        [Authorize(Roles = "Admin")]
*/        public IHttpActionResult GetChats()
        {
                Debug.WriteLine("YOU ARE IN THE GET Chats TEST API METHOD");
                List<Chat> Chats = db.Chats.ToList();

                List<ChatDto> ChatDtos = new List<ChatDto> { };

                foreach (var Chat in Chats)
                {
                    ChatDto thisChat = new ChatDto
                    {
                        ChatId = Chat.ChatId,
                        Subject = Chat.Subject,
                        DateCreated = Chat.DateCreated,
                    };
                    ChatDtos.Add(thisChat);
                }

                return Ok(ChatDtos);
            
           
        }

        /*When login functionality complete, browser will identify user, and this method can show all of the chats associated with him/her*/
        [HttpGet]
        [ResponseType(typeof(List<ChatDto>))]
/*        [Authorize(Roles ="Admin")]
*/        public IHttpActionResult GetChatsForUser(string id)
        {
            Debug.WriteLine("YOU ARE IN THE GET Chats FOR User TEST API METHOD");
            List<Chat> Chats = db.Chats.Where(c => c.ApplicationUsers.Any(u => u.Id == id)).ToList();
            List<ChatDto> ChatDtos = new List<ChatDto> { };

            foreach (var Chat in Chats)
            {
                ChatDto thisChat = new ChatDto
                {
                    ChatId = Chat.ChatId,
                    Subject = Chat.Subject,
                    DateCreated = Chat.DateCreated,
                };
                ChatDtos.Add(thisChat);
            }

            return Ok(ChatDtos);
        }
    
        /// <summary>
        /// Takes the chat Id and returns the users associated with the chat. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of users (should just be 2) for the chat id given </returns>
        [HttpGet]
        [ResponseType(typeof(ApplicationUserDto))]
/*        [Authorize()]
*/        public IHttpActionResult GetUsersForChat(int id)
        {
            Debug.WriteLine("YOU ARE IN THE GET Sender FOR Chat TEST API METHOD");
            List<ApplicationUser> Users = db.Users.Where(u => u.Chats.Any(c => c.ChatId == id)).ToList();

            List<ApplicationUserDto> UserDtos = new List<ApplicationUserDto> { };

            foreach (var User in Users)
            {
                ApplicationUserDto UserDto = new ApplicationUserDto
                {
                    Id = User.Id,
                    FirstName = User.FirstName,
                    LastName = User.LastName
                };
                UserDtos.Add(UserDto);
            }
            return Ok(UserDtos);
        }

        /// <summary>
        /// Takes a chat Id and Returns a list of messages for that chat.
        /// </summary>
        /// <param name="ChatId"></param>
        /// <returns>List of MesssageDto objects</returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<MessageDto>))]
/*        [Authorize()]
*/        public IHttpActionResult GetMessagesByChatId(int id)
        {
            Debug.WriteLine("YOU ARE IN THE GET MESSAGES BY Chat ID TEST API METHOD");
            List<Message> Messages = db.Messages.Where(m => m.ChatId.Equals(id)).ToList();

            List<MessageDto> MessageDtos = new List<MessageDto> { };

            foreach (var Message in Messages)
            {
                MessageDto MessageDto = new MessageDto
                {
                    MessageId = Message.MessageId,
                    SenderId = Message.SenderId,
                    ChatId = Message.ChatId,
                    DateSent = Message.DateSent,
                    Content = Message.Content
                };
                MessageDtos.Add(MessageDto);
            }

            return Ok(MessageDtos);
        }
        

        //POST: ChatData/CreateChat/RecipientId/SenderId
        /// <summary>
        /// api/messagedata/createchat takes a Chat object and adds it to the database. 
        /// </summary>
        /// <returns>OK if sucessful, badrequest if object passed does not match model</returns>
        
        [HttpPost]
        [ResponseType(typeof(ChatDto))]
/*        [Authorize()]
*/        public IHttpActionResult CreateChat([FromBody] Chat NewChat)
        {
            Debug.WriteLine("IN THE CREATE Chat API CONTROLLER");
                       

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            /*Add the chat to database*/
            db.Chats.Add(NewChat);
            db.SaveChanges();
            return Ok(NewChat.ChatId);
        }

        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("api/ChatData/AddUserForChat/{UserId}/{ChatId}")]
/*        [Authorize()]
*/        public IHttpActionResult AddUserForChat(string UserId, int ChatId)
        {
            Debug.WriteLine("IN THE ADD USER FOR Chat API CONTROLLER");

            ApplicationUser NewUser = db.Users.Find(UserId);

            Chat ThisChat = db.Chats.Include(c=>c.ApplicationUsers).Where(c=>c.ChatId == ChatId).FirstOrDefault();

            /*Add the user to the chat*/

            ThisChat.ApplicationUsers.Add(NewUser);
            db.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [ResponseType(typeof(void))]
/*        [Authorize(Roles="Admin")]*/
          public IHttpActionResult DeleteChat(int id)
        {
            //When the chat is deleted, messages associated with the chat will also be deleted
            Debug.WriteLine("IN THE DELETE DATA CONTROLLER");
            Chat Chat = db.Chats.Find(id);
            if (Chat == null)
            {
                Debug.WriteLine("NO Chat FOUND");
                return NotFound();
            }
            else
            {
                Debug.WriteLine("FOUND Chat");
                db.Chats.Remove(Chat);
                db.SaveChanges();
                return StatusCode(HttpStatusCode.NoContent);
            }
        }
    }
}