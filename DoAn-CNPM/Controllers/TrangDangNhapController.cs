using DoAn_CNPM.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn_CNPM.Models;
using System.Net.Mail;
using System.Net;

namespace DoAn_CNPM.Controllers
{
    public class TrangDangNhapController : Controller
    {
        //
        // GET: /TrangDangNhap/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string Login()
        {
            string user = Request.Form["user"];
            string pass = Request.Form["pass"];
            //UserDAL userDAL = new UserDAL();
            var userdal = UserDAL.createUserDAL();
            string truePass = userdal.getPassByUsername(user);
            if (truePass!=null && pass == truePass)
            {
                User currentUser = userdal.getUserByUserName(user);
                if (currentUser != null)
                {
                    Session["user"] = currentUser;
                    return "1";
                }
                return "Tài khoản hoặc mật khẩu không chính xác.";
            }
            return "Tài khoản hoặc mật khẩu không chính xác.";
        }


        [HttpPost]
        public string SignUp()
        {
            User user = new User();
            user.UserName = Request.Form["user"];
            user.Password = Request.Form["pass"];
            user.Phone = Request.Form["phone"];
            user.Email = Request.Form["email"];
            user.Name = Request.Form["name"];
            user.CreatedDate = DateTime.Now;
            user.Status = true;
            user.Role = false;

            //UserDAL userdal = new UserDAL();
            var userdal = UserDAL.createUserDAL();
            if (userdal.insertUser(user))
            {
                Session["user"] = user;
                return "1";
            }
            return "Tạo tài khoản thất bại. Vui lòng kiểm tra lại thông tin";
        }

        public ActionResult Logout()
        {
            Session.Remove("user");

            return RedirectToAction("Index","TrangChu");
        }
        
        //[HttpPost]
        public ActionResult ForgetPassword()
        {
            string fromAddress = "khoatd152@gmail.com";
            string fromPassword = "Dk1521998";
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("khoatd152@gmail.com");
                mail.To.Add("anhdepzai.152@gmail.com");
                //mail.Subject = subject;
                mail.Body = "https://www.facebook.com/";
                mail.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                    smtp.Send(mail);
                }
            }

            return Json(new { message = "Đã gủi mai;" });
        }

        [HttpPost]
        public ActionResult UpdateProfile(User UpdatedUser, string OldPassword)
        {
            if (Session["user"] != null)
            {
                
                User userSession = (User)Session["user"];
                if (userSession.UserName == UpdatedUser.UserName)
                {
                    //UserDAL userdal = new UserDAL();
                    var userdal = UserDAL.createUserDAL();
                    User user = userdal.getUserByUserName(userSession.UserName);
                    if (!string.IsNullOrEmpty(UpdatedUser.Password) && !string.IsNullOrEmpty(OldPassword))
                    {
                        if(user.Password != OldPassword)
                            return Json(new { result = 0, message = "Mật khẩu không chính xác" });
                        else
                        {
                            if (userdal.updateUser(UpdatedUser))
                            {
                                return Json(new { result = 1, message = "Cập nhật thông tin thành công" });
                            }
                            else
                            {
                                return Json(new { result = 0, message = "Xảy ra lỗi vui lòng thử lại sau" });
                            }
                        }
                    }
                    else
                    {
                        if (userdal.updateUser(UpdatedUser))
                        {
                            return Json(new { result = 1, message = "Cập nhật thông tin thành công" });
                        }
                        else
                        {
                            return Json(new { result = 0, message = "Xảy ra lỗi vui lòng thử lại sau" });
                        }
                    }
                }
                else
                {
                    return Json(new { result = 0, message = "Tài khoản hoặc mật khẩu không hợp lệ" });
                }
            }
            else
            {
                return Json(new { result = 0, message = "Vui lòng đăng nhập để thực hiện chức năng này" });
            }
        }
	}
}