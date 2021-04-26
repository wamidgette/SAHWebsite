using System;
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
    public class MessageDataController : ApiController
    {
        private SAHDataContext db = new SAHDataContext();

        /// <summary>
        /// GET: api/messagedata/getMessageById/4 takes a messageId and returns the associated MessageDto object
        /// </summary>
        /// <param name="id"></param>
        /// <returns>MessageDto object</returns>

        [HttpGet]
        [ResponseType(typeof(MessageDto))]
      /*  [Authorize()]*/

        public IHttpActionResult GetMessageById(int id)
        {
            Debug.WriteLine("YOU ARE IN THE GET MESSAGE TEST API METHOD");
            Message Message = db.Messages.Find(id);

            
            if (Message == null)
            {
                Debug.WriteLine("COULD NOT FIND MESSAGE IN API");
                return NotFound();
            }
            MessageDto MessageDto = new MessageDto
            {
                MessageId = Message.MessageId,
                SenderId = Message.SenderId,
                ChatId = Message.ChatId,
                DateSent = Message.DateSent,
                Content = Message.Content,
            };
            Debug.WriteLine("FOUND THE MESSAGE IN API");
            return Ok(MessageDto);
        }

        [HttpGet]
        [ResponseType(typeof(ChatDto))]
        public IHttpActionResult GetChatForMessage(int id)
        {
            Debug.WriteLine("YOU ARE IN THE GET CHAT FOR MESSAGE TEST API METHOD");
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

        [HttpGet]
        [ResponseType(typeof(ApplicationUserDto))]
        public IHttpActionResult GetSenderForMessage(int id)
        {
            Debug.WriteLine("YOU ARE IN THE GET Sender FOR MESSAGE TEST API METHOD");
            ApplicationUser User = db.Users.Find(id);

            if (User == null)
            {
                Debug.WriteLine("COULD NOT FIND USER IN API");
                return NotFound();
            }

            ApplicationUserDto UserDto = new ApplicationUserDto
            {
                Id = User.Id,
                FirstName = User.FirstName,
                LastName = User.LastName
            };

            Debug.WriteLine("FOUND THE USER IN API");
            return Ok(UserDto);
        }

        [HttpGet]
        [ResponseType(typeof(IEnumerable<MessageDto>))]
        public IHttpActionResult GetMessagesByChatId(int id)
        {
            Debug.WriteLine("YOU ARE IN THE GET MESSAGE BY CHAT ID TEST API METHOD");
            List<Message> Messages = db.Messages
                .Where(m => m.ChatId.Equals(id))
                .ToList();

            List<MessageDto> MessageDtos = new List<MessageDto> { };

            foreach (var Message in Messages)
            {
                MessageDto thisMessage = new MessageDto
                {
                    MessageId = Message.MessageId,
                    SenderId = Message.SenderId,
                    ChatId = Message.ChatId,
                    DateSent = Message.DateSent,
                    Content = Message.Content
                };
                MessageDtos.Add(thisMessage);
            }

            return Ok(MessageDtos);
        }
        /// <summary>
        /// api/messagedata/addmessage takes a message object and adds it to the database. 
        /// </summary>
        /// <returns>OK if sucessful, badrequest if object passed does not match model</returns>

        [HttpPost]
        [ResponseType(typeof(MessageDto))]

        public IHttpActionResult CreateMessage([FromBody] Message NewMessage)
        {
            Debug.WriteLine("IN THE CREATE MESSAGE API CONTROLLER");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Messages.Add(NewMessage);
            db.SaveChanges();
            return Ok(NewMessage.MessageId);
        }

        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateMessage([FromBody] Message UpdatedMessage)
        {
            Debug.WriteLine("YOU ARE IN THE UPDATE MESSAGE API METHOD");

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            //update the entry with the message ID to its new state given
            db.Entry(UpdatedMessage).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            //Return No Content 204 message if row is successfully updated
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteMessage(int id)
        {
            Debug.WriteLine("IN THE DELETE DATA CONTROLLER");
            Message Message = db.Messages.Find(id);
            if (Message == null)
            {
                Debug.WriteLine("NO MESSAGE FOUND");
                return NotFound();
            }
            else
            {
                Debug.WriteLine("FOUND MESSAGE");
                db.Messages.Remove(Message);
                db.SaveChanges();
                return StatusCode(HttpStatusCode.NoContent);
            }
        }
    }
}