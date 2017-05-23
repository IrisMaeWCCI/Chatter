﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Chatter.Models;
using Newtonsoft.Json;

namespace Chatter.Controllers
{
    public class ChatsController : Controller
    {
        private ChatterEntities db = new ChatterEntities();

        // GET: Chats
        public ActionResult Index()
        {
            var chats = db.Chats.Include(c => c.AspNetUser);
            return View(chats.ToList());
        }

        // GET: Chats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chat chat = db.Chats.Find(id);
            if (chat == null)
            {
                return HttpNotFound();
            }
            return View(chat);
        }

        // GET: Chats/Create
        public ActionResult Create()
        {
            ViewBag.UserName = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Chats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ChatID,UserName,Message,Timestamp")] Chat chat)
        {
            if (ModelState.IsValid)
            {
                db.Chats.Add(chat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserName = new SelectList(db.AspNetUsers, "Id", "Email", chat.UserName);
            return View(chat);
        }

        // GET: Chats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chat chat = db.Chats.Find(id);
            if (chat == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserName = new SelectList(db.AspNetUsers, "Id", "Email", chat.UserName);
            return View(chat);
        }

        // POST: Chats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ChatID,UserName,Message,Timestamp")] Chat chat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserName = new SelectList(db.AspNetUsers, "Id", "Email", chat.UserName);
            return View(chat);
        }

        // GET: Chats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chat chat = db.Chats.Find(id);
            if (chat == null)
            {
                return HttpNotFound();
            }
            return View(chat);
        }

        // POST: Chats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Chat chat = db.Chats.Find(id);
            db.Chats.Remove(chat);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
       
       public JsonResult TestJson()
        {
            //SELECT Chat.UserName, AspNetUsers.UserName
            //FROM Chat
            //INNER JOIN AspNetUsers ON Chat.UserName = AspNetUsers.UserName
            //ORDER BY chat.TimeStamp DESC;


            //Now, the LINQ equivalent is declared as a variable (below).
            //var chats = from Chats in db.Chats
            //            orderby
            //Chats.Timestamp descending
            //select new {

            //    Chats.UserName,
            //    UserName= AspNetUser.UserName  
            // };

            var output = JsonConvert.SerializeObject(chats.ToList());


            //Finally, we return Json to the view
            return Json(output, JsonRequestBehavior.AllowGet);
        }

        string jsonTest = "{ \"firstName\": \"Melanie\", \"lastName\": \"McGee\", \"children\": [{ \"firstName\": \"Mira\", \"age\": 13 }, {\"firstName\": \"Ethan\", \"age\": null }] }";

         }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
}
