using Model.EF;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ProductDao
    {
        OnlineShopDbContext db = null;
        public ProductDao()
        {
            db = new OnlineShopDbContext();
        }
        public List<Product> ListNewProduct(int top) {
            return db.Products.OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }

        public List<Product> ListFeatureProduct(int top)
        {
            return db.Products.Where(x => x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }

        public Product viewDetail(long id)
        {
            return db.Products.Find(id);
        }
        /// <summary>
        /// get list Product by categoryID
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public List<ProductViewModel> listAllCategoryId(long categoryID,ref int totalRecord, int pageIndex, int pageSize)
        {
            totalRecord = db.Products.Where(x => x.CategoryID == categoryID).Count();
            // lấy model sử dụng linQ
            var model = from p in db.Products
                        join pc in db.ProductCategories
                        on p.CategoryID equals pc.ID
                        where p.CategoryID == categoryID
                        select new ProductViewModel() {
                            ID = p.ID,
                            Images = p.Image,
                            Name = p.Name,
                            Price = p.Price,
                            CateName = pc.Name,
                            CateMetaTitle = pc.MetaTitle,
                            MetaTitle = p.MetaTitle,
                            CreateDate = p.CreateDate
                        };
            // var model = db.Products.Where(x=>x.CategoryID == categoryID);
            model.OrderByDescending(x => x.CreateDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return model.ToList();
        }

        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x=>x.Name.Contains(keyword)).Select(x=>x.Name).ToList();
        }
        public List<ProductViewModel> Search(string keyword, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            totalRecord = db.Products.Where(x => x.Name == keyword).Count();
            var model = (from a in db.Products
                         join b in db.ProductCategories
                         on a.CategoryID equals b.ID
                         where a.Name.Contains(keyword)
                         select new
                         {
                             CateMetaTitle = b.MetaTitle,
                             CateName = b.Name,
                             CreatedDate = a.CreateDate,
                             ID = a.ID,
                             Images = a.Image,
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Price = a.Price
                         }).AsEnumerable().Select(x => new ProductViewModel()
                         {
                             CateMetaTitle = x.MetaTitle,
                             CateName = x.Name,
                             CreateDate = x.CreatedDate,
                             ID = x.ID,
                             Images = x.Images,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Price = x.Price
                         });
            model.OrderByDescending(x => x.CreateDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return model.ToList();
        }
    }
}
