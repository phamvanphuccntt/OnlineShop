using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class UserDao
    {
        OnlineShopDbContext db = null;
        public UserDao()
        {
            db = new OnlineShopDbContext();
        }
        public long Insert(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return user.ID;
        }

        public int Login(string userName, string passWord) {

            var result = db.Users.SingleOrDefault(x=>x.UserName == userName);

            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.Status == false)
                {
                    return -1;
                }
                else {
                    if (result.Password == passWord)
                    {
                        return 1;
                    }
                    else {
                        return -2;
                    }
                }
                
            }
        }

        public User getUserByUserName(string userName) {
            return db.Users.SingleOrDefault(x=>x.UserName == userName);
        }
    }
}
