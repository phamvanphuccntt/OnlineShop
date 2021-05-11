using BotDetect.Web.Mvc;
using Model.Dao;
using Model.EF;
using OnlineShop.Common;
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
                    ModelState.AddModelError("", "Username bị trùng");
                }
                else if (dao.checkEmail(model.email))
                {
                    ModelState.AddModelError("", "Email bị trùng");
                }
                else
                {
                    var user = new User();
                    user.UserName = model.userName;
                    user.Password = Common.Encryptor.MD5Hash(model.password);
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
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.userName, Common.Encryptor.MD5Hash(model.password));

                if (result == 1)
                {
                    var user = dao.getUserByUserName(model.userName);
                    var userSession = new UserLogin();

                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;

                    Session.Add(Common.CommonConstants.USER_SESSION, userSession);
                    return Redirect("/");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khóa.");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập không đúng.");
                }
            }

            return View();
        } 
        public ActionResult Logout()
        {
            Session[Common.CommonConstants.USER_SESSION] = null;
            return RedirectToAction("Login","User");
        }
    }
}