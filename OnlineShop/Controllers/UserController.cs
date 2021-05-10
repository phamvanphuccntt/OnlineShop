using BotDetect.Web.Mvc;
using Model.Dao;
using Model.EF;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [HttpGet]
        public ActionResult Register()
        {
            var model = new RegisterModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.checkUserName(model.userName))
                {
                    ModelState.AddModelError("","Username bị trùng");
                }
                else if (dao.checkEmail(model.email))
                {
                    ModelState.AddModelError("", "Email bị trùng");
                }
                else
                {
                    var user = new User();
                    user.UserName = model.userName;
                    user.Password = model.password;
                    user.Phone = model.phone;
                    user.Email = model.email;
                    user.Address = model.address;
                    user.CreateDate = DateTime.Now;
                    user.Status = true;
                    var id = dao.Insert(user);
                    if (id > 0)
                    {
                        ViewBag.Success = "Đăng ký thành công";
                    }
                    else
                    {
                        ViewBag.Error = "Đăng ký không thành công";
                    }
                }
            }
            return View(model);
        }

    }
}