using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ContactDao
    {
        OnlineShopDbContext db = null;
        public ContactDao()
        {
            db = new OnlineShopDbContext();
        }

        public Contact getActiveContact()
        {
            return db.Contacts.Single(x=>x.Status == true);
        }

        public int InsertFeedBack(FeedBack fb)
        {
            db.FeedBacks.Add(fb);
            db.SaveChanges();
            return fb.ID;
        }
    }
}
