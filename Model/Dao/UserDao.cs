using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList.Mvc;
using PagedList;

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
        public IEnumerable<User> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<User> model = db.Users;
            // SearchString
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.UserName.Contains(searchString) || x.Name.Contains(searchString));
            }

            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }
        public bool Update(User user) {

            try
            {
                var u = db.Users.Find(user.ID);

                u.Name = user.Name;
                u.Address = user.Address;
                u.Email = user.Email;
                u.Phone = user.Phone;
                u.Status = user.Status;
                u.ModifiedBy = user.ModifiedBy;
                u.ModifiedDate = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch (Exception e) {
                
                return false;
            }
            
        }

        public User ViewDetail(int id) {
            return db.Users.Find(id);
        }

        public bool Delete(int id)
        {
            var user = db.Users.Find(id);
            if (user != null) {
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool ChangeStatus(long id)
        {
            var user = db.Users.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();
            return user.Status;
        }

        public bool checkUserName(string userName)
        {
            return db.Users.Count(x=>x.UserName == userName) > 0;
        }
        public bool checkEmail(string email)
        {
            return db.Users.Count(x => x.Email == email) > 0;
        }

        public long InsertForFacebook(User user)
        {
            var users = db.Users.SingleOrDefault(x => x.UserName == user.UserName);
            if (users == null)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return user.ID;
            }
            else
            {
                return users.ID;
            }
            
        }
    }
}
