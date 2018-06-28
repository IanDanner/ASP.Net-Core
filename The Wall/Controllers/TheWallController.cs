using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DbConnection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using The_Wall.Models;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace The_Wall.Controllers
{
    public class TheWallController : Controller
    {
        
        [HttpGet]
        [Route("thewall")]
        public IActionResult The_Wall()
        {   
            if(HttpContext.Session.GetObjectFromJson<List<Dictionary<string, object>>>("Logged")==null){
                return RedirectToAction("Index", "LoginReg");
            }
            TempData["User"] = HttpContext.Session.GetObjectFromJson<List<Dictionary<string, object>>>("Logged");

            TempData["Messages"] = DbConnector.Query("SELECT users.firstName, users.lastName, messages.users_id, messages.id, messages.message, messages.created_at AS message_created, messages.updated_at AS message_updated FROM messages JOIN users ON messages.users_id = users.id ORDER BY message_created;");

            TempData["Comments"] = DbConnector.Query("SELECT users.firstName, users.lastName, messages.message,comments.id, comments.messages_id, comments.users_id, comments.comment, comments.created_at AS comment_created, comments.updated_at AS comment_updated FROM comments JOIN users ON comments.users_id = users.id JOIN messages ON comments.messages_id = messages.id ORDER BY comment_created;");

            return View("TheWall");
        }

        [HttpGet]
        [HttpPost]
        [Route("post_msg")]
        public IActionResult PostMsg(int userId, string message)
        {
            if(message == null){
                TempData["errors"] = "No message input";
                return RedirectToAction("The_Wall", "TheWall");
            }
            if(HttpContext.Session.GetObjectFromJson<List<Dictionary<string, object>>>("Logged")==null){
                return RedirectToAction("Index", "LoginReg");
            }
            DbConnector.Query($"INSERT INTO messages (users_id, message, created_at, updated_at) VALUES ('{userId}', '{message}', NOW(), NOW())");
            return RedirectToAction("The_Wall", "TheWall");
        }

        [HttpGet]
        [HttpPost]
        [Route("delete_msg")]
        public IActionResult DeleteMsg(int msgId)
        {
            if(HttpContext.Session.GetObjectFromJson<List<Dictionary<string, object>>>("Logged")==null){
                return RedirectToAction("Index", "LoginReg");
            }
            DbConnector.Query($"DELETE FROM comments WHERE messages_id = '{msgId}'");
            DbConnector.Query($"DELETE FROM messages WHERE id = '{msgId}'");
            return RedirectToAction("The_Wall", "TheWall");
        }

        [HttpGet]
        [HttpPost]
        [Route("post_cmt")]
        public IActionResult PostCmt(int msgId, int userId, string comment)
        {
            if(comment == null){
                TempData["error"] = "No comment input";
                return RedirectToAction("The_Wall", "TheWall");
            }
            if(HttpContext.Session.GetObjectFromJson<List<Dictionary<string, object>>>("Logged")==null){
                return RedirectToAction("Index", "LoginReg");
            }
            DbConnector.Query($"INSERT INTO comments (messages_id, users_id, comment, created_at, updated_at) VALUES ('{msgId}', '{userId}', '{comment}', NOW(), NOW())");

            return RedirectToAction("The_Wall", "TheWall");
        }

        [HttpGet]
        [HttpPost]
        [Route("delete_cmt")]
        public IActionResult DeleteCmt(int cmtId)
        {
            if(HttpContext.Session.GetObjectFromJson<List<Dictionary<string, object>>>("Logged")==null){
                return RedirectToAction("Index", "LoginReg");
            }
            DbConnector.Query($"DELETE FROM comments WHERE id = '{cmtId}'");
            return RedirectToAction("The_Wall", "TheWall");
        }
    }
}
