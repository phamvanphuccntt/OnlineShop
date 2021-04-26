using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ProductCategoryDao
    {
        OnlineShopDbContext db = null;
        public ProductCategoryDao()
        {
            db = new OnlineShopDbContext();
        }

        public List<ProductCategory> ListAll()
        {
            return db.ProductCategories.Where(x => x.Status == true).OrderBy(x=>x.DisplayOrder).ToList();
        }

        public ProductCategory viewDetail(long id) {
            var product = db.ProductCategories.Find(id);
            return product;
        }

        public List<Product> relatedProduct(long productId) {
            var product = db.Products.Find(productId);
            return db.Products.Where(x=>x.ID != productId && x.CategoryID == product.CategoryID).ToList();
        }
    }
}
