using DoAn_CNPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn_CNPM.DAL
{
    public class UserDAL
    {
        private QuanLyNhac db;
        public UserDAL()
        {
            db = new QuanLyNhac();
        }

        public static UserDAL userDAL;
        public static UserDAL createUserDAL()
        {
            if(userDAL == null)
            {
                userDAL = new UserDAL();
            }
            return userDAL;
        }

        public string getPassByUsername(string username){
            string kq;
            try{
                User user = db.Users.Where(u => u.UserName == username && u.Status == true).FirstOrDefault();
                if (user != null)
                {
                    kq = user.Password;
                }
                else
                {
                    kq = null;
                }
               
            }
            catch{
                kq=null;
            }
            return kq;
        }

        public User getUserByUserName(string username)
        {
                return db.Users.Where(u=>u.UserName == username && u.Status==true).FirstOrDefault(); 
            
        }

        public bool insertUser(User user)
        {
            try
            {
                user.Status = true;
                user.CreatedDate = DateTime.Now;
                db.Users.Add(user);
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool updateUser(User updatedUser)
        {
            try
            {
                User user = db.Users.Where(u => u.Status == true && u.UserName == updatedUser.UserName).FirstOrDefault();
                if (user !=null)
                {
                    
                    user.Name = string.IsNullOrEmpty(updatedUser.Name) ? user.Name : updatedUser.Name;
                    user.Phone = string.IsNullOrEmpty(updatedUser.Phone) ? user.Phone : updatedUser.Phone;
                    user.Email = string.IsNullOrEmpty(updatedUser.Email) ? user.Email : updatedUser.Email;
                    user.Password = string.IsNullOrEmpty(updatedUser.Password) ? user.Password : updatedUser.Password;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public List<User> getAllUserAdmin()
        {
            return db.Users.ToList();
        }

        public User getUserByUserNameAdmin(string username)
        {
            return db.Users.Find(username);
        }
        public bool LockOrUnlockUserAdmin(User user)
        {
            try
            {
                User ct = db.Users.Find(user.UserName);
                ct.Status = user.Status;
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }

        }
    }
}