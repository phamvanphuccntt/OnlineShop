using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult ProductCategory() {
            var model = new ProductCategoryDao().ListAll();
            return PartialView(model);
        }
        
        public ActionResult Category(long cateID) {
            var category = new CategoryDao().viewDetail(cateID);
            return View(category);
        }

        public ActionResult Detail(long id)
        {
            var product = new ProductDao().viewDetail(id);
            return View(product);
        }
    }
}