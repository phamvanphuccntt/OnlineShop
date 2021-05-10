using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            var model = new ContactDao().getActiveContact();
            return View(model);
        }

        public JsonResult Send(string name, string mobile, string email, string address, string content)
        {
            var feedBack = new FeedBack();
            feedBack.Name = name;
            feedBack.Phone = mobile;
            feedBack.Email = email;
            feedBack.Address = address;
            feedBack.Content = content;
            feedBack.CreateDate = DateTime.Now;

            var id = new ContactDao().InsertFeedBack(feedBack);
            if (id > 0) {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }
    }
}