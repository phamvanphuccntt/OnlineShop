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
    }
}
