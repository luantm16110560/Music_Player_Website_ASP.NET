using DoAn_CNPM.DAL;
using DoAn_CNPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn_CNPM.Controllers
{
    public class TrangNgheSiController : Controller
    {
        // GET: TrangNgheSi
        public ActionResult Index(int id)
        {
            var artistdal = ArtistDAL.createArtistDAL();
            //SongDAL songdal = new SongDAL();
            var songdal = SongDAL.createSongDAL();

            //lấy thông tin artist
            Artist artist = artistdal.getArtistByID(id);
            ViewBag.artist = artist;

            //lấy danh sách bài hát của artist
            List<Song> listSong = songdal.getSongByArtist(id);
            ViewBag.listSong = listSong;

            //kiểm tra user có quan tâm artist
            if (Session["user"] != null)
            {
                User user = (User)Session["user"];
                ViewBag.follow = artistdal.checkUserFollowArtist(user.UserName, id);
            }
            else
            {
                ViewBag.follow = false;
            }

            //lấy mô tả về artist
            string info = SomeFunction.getFormatInfoArtist(Server.MapPath("~"), artist.Description);
            ViewBag.info = info;

            return View();
        }

        [HttpPost]
        public ActionResult FollowArtist()
        {
            if (Session["user"] != null)
            {
                User user = (User)Session["user"];
                int artistID = Convert.ToInt32(Request.Form["artistID"]);
                var artistdal = ArtistDAL.createArtistDAL();

                //nếu tồn đã like
                if (artistdal.checkUserFollowArtist(user.UserName, artistID))
                {
                    if (artistdal.delFollowArtist(user.UserName, artistID)){
                        return Json(new { result = 1, message = "Bạn đã hủy quan tâm nghệ sĩ này", src= "/Assets/image/icon/btnFollow.png" });
                    }
                    else
                    {
                        return Json(new { result = -1, message = "Có lỗi xảy ra. Vui lòng thử lại sau" });
                    }
                }
                else
                {
                    Artist_User artistUser = new Artist_User();
                    artistUser.User = user.UserName;
                    artistUser.Artist = artistID;
                    artistUser.Status = true;
                    if (artistdal.followArtist(artistUser))
                    {
                        if (artistdal.checkUserExistInFollowing(user.UserName, artistID))
                        {
                            //thêm người này vào bảng đánh dấu đã từng follow nghệ sĩ này
                            artistdal.addUserToFollowedArtist(user.UserName, artistID);
                            artistdal.upFollow(artistID);
                            return Json(new { result = 3, message = "Bạn đã quan tâm nghệ sĩ này", src= "/Assets/image/icon/btnFollowed.png" });
                        }
                        else
                        {
                            return Json(new { result = 2, message = "Bạn đã quan tâm nghệ sĩ này", src = "/Assets/image/icon/btnFollowed.png" });
                        }    

                    }
                    else
                    {
                        return Json(new { result = -1, message = "Có lỗi xảy ra. Vui lòng thử lại sau" });
                    }
                }
            }
            else
            {
                return Json(new { result = -1, message = "Vui lòng đăng nhập để thực hiện chức năng này" });
            }
        }

        public ActionResult AllArtist()
        {
            var artistdal = ArtistDAL.createArtistDAL();



            ViewBag.listArtist = artistdal.getAllArtists();

            if (Session["user"] != null)
            {
                User user = (User)Session["user"];
                ViewBag.listFollowed = artistdal.getAllFollowedAristIDByUsername(user.UserName);
            }


            return View();
        }

    }
}