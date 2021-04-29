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
        
        public ActionResult Category(long cateID, int page = 1, int pageSize = 2) {

            var productCategory = new CategoryDao().viewDetail(cateID);
            ViewBag.ProductCategory = productCategory;
            int totalRecord = 0;
            var model = new ProductDao().listAllCategoryId(cateID, ref totalRecord, page, pageSize);

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;

            int maxPage = 3;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(model);
        }

        public ActionResult Detail(long id)
        {
            var product = new ProductDao().viewDetail(id);
            ViewBag.Category = new ProductCategoryDao().viewDetail(product.CategoryID.Value);
            ViewBag.RelatedProduct = new ProductCategoryDao().relatedProduct(product.ID);
            return View(product);
        }
    }
}