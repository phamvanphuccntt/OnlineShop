using Model.Dao;
using Model.EF;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new UserDao();
            // Search String
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;

            return View(model);
        }

        [HttpGet]
        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var encryptor = Encryptor.MD5Hash(user.Password);
                user.Password = encryptor;

                long id = dao.Insert(user);
                if (id > 0)
                {
                    SetAlert("Thêm user thành công","success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    SetAlert("Thêm user không thành công", "error");
                }
            }
            return View("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var user = new UserDao().ViewDetail(id);
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                
                var result = dao.Update(user);
                if (result)
                {
                    SetAlert("Sửa user thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    SetAlert("Cập nhật user không thành công", "error");
                }
            }
            return View("Index");
        }

        public ActionResult Delete(int id) {
            var check = new UserDao().Delete(id);
            if (check == true)
            {
                SetAlert("Xóa user thành công", "success");
                return RedirectToAction("Index","User");
            }
            else
            {
                SetAlert("Xóa user không thành công", "error");
            }
            return View("Index");
        }

        public JsonResult ChangeStatus(long id)
        {
            var result = new UserDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}