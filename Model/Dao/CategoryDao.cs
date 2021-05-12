using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class CategoryDao
    {
        OnlineShopDbContext db = null;
        public CategoryDao()
        {
            db = new OnlineShopDbContext();
        }

        public List<Category> listAll()
        {
            return db.Categories.Where(x => x.Status == true).ToList() ;
        }
        public ProductCategory viewDetail(long id)
        {
            return db.ProductCategories.Find(id);
        }

        public long Insert(Category model)
        {
            db.Categories.Add(model);
            db.SaveChanges();
            return model.ID;
        }
    }
}
