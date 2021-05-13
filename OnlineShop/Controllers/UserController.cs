using BotDetect.Web.Mvc;
using Facebook;
using Model.Dao;
using Model.EF;
using OnlineShop.Common;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

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
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                else if (dao.checkEmail(model.email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    var user = new User();
                    user.UserName = model.userName;
                    user.Password = Common.Encryptor.MD5Hash(model.password);
                    user.Phone = model.phone;
                    if (!string.IsNullOrEmpty(model.ProvinceID))
                    {
                        user.ProvinceID = int.Parse(model.ProvinceID);
                    }
                    //if (!string.IsNullOrEmpty(model.DistrictID))
                    //{
                    //    user.ProvinceID = int.Parse(model.DistrictID);
                    //}
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

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["fbAppId"],
                client_secret = ConfigurationManager.AppSettings["fbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                respone_type = "code",
                scope = "email",
            });
            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["fbAppId"],
                client_secret = ConfigurationManager.AppSettings["fbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;
            fb.AccessToken = accessToken;

            if (!string.IsNullOrEmpty(accessToken))
            {
                // get the user's info, like email, frist name, middle name etc
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string userName = me.first_name;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;

                var user = new User();
                user.Email = email;
                user.UserName = firstname;
                user.Name = firstname + " " + middlename + " " + lastname;
                user.Status = true;
                user.CreateDate = DateTime.Now;
                var resultInsert = new UserDao().InsertForFacebook(user);

                if(resultInsert > 0)
                {
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    Session.Add(Common.CommonConstants.USER_SESSION, userSession);
                }
            }
            return Redirect("/");
        }

        public JsonResult loadProvince()
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/assets/client/data/Provinces_Data.xml"));

            var xElements = xmlDoc.Element("Root").Elements("Item").Where(x=>x.Attribute("type").Value == "province");

            var list = new List<ProvinceModel>();

            ProvinceModel province = null;
            foreach (var item in xElements)
            {
                province = new ProvinceModel();
                province.ID = int.Parse(item.Attribute("id").Value);
                province.Name = item.Attribute("value").Value;
                list.Add(province);
            }

            return Json(new {
                data = list,
                status = true
            });
        }

        public JsonResult loadDistrict(int provinceID)
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/assets/client/data/Provinces_Data.xml"));
            var xElements = xmlDoc.Element("Root").Elements("Item")
                .Single(x => x.Attribute("type").Value == "province" && int.Parse(x.Attribute("id").Value) == provinceID);

            var list = new List<DistrictModel>();
            DistrictModel district = null;
            foreach (var item in xElements.Elements("Item").Where(x=>x.Attribute("type").Value == "district"))
            {
                district = new DistrictModel();
                district.ID = int.Parse(item.Attribute("id").Value);
                district.Name = item.Attribute("value").Value;
                district.ProvinceID = int.Parse(xElements.Attribute("id").Value);
                list.Add(district);
            }
            return Json(new
            {
                data = list,
                status = true
            });
        }
        //Sai
        public JsonResult loadPrecinct(int districtID)
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/assets/client/data/Provinces_Data.xml"));
            // Sai ở đây, chưa query được
            var xElements = xmlDoc.Element("Root").Elements("Item")
                .Single(x => x.Attribute("type").Value == "district" && int.Parse(x.Attribute("id").Value) == districtID);
            
            var list = new List<PrecinctModel>();
            PrecinctModel precinct = null;
            foreach (var item in xElements.Elements("Item").Where(x => x.Attribute("type").Value == "precinct"))
            {
                precinct = new PrecinctModel();
                precinct.ID = int.Parse(item.Attribute("id").Value);
                precinct.Name = item.Attribute("value").Value;
                precinct.DistrictID = int.Parse(xElements.Attribute("id").Value);
                list.Add(precinct);
            }
            return Json(new
            {
                data = list,
                status = true
            });
        }
    }
}