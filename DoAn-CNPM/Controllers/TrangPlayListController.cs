using DoAn_CNPM.DAL;
using DoAn_CNPM.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace DoAn_CNPM.Controllers
{
    public class TrangPlayListController : Controller
    {
        
        //xóa bài hát trong playlist
        [HttpPost]
        public ActionResult delSongInPlayList()
        {
            if (Session["user"] != null)
            {
                int songID = Convert.ToInt32(Request.Cookies["playingSongInPlayList"].Value);
                int playList =  Convert.ToInt32(Request.Cookies["currentPlayList"].Value);
                User user = (User)Session["user"];
                //PlayListDAL playlistdal = new PlayListDAL();
                var playlistdal = PlayListDAL.createPlayListDAL();
                if (playlistdal.checkExistSongInPlayList(songID, playList, user.UserName))
                {
                    if (playlistdal.deleteSongInPlayList(songID, playList, user.UserName))
                    {
                        HttpCookie cookie = Request.Cookies["playingSongInPlayList"];
                        cookie.Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies.Add(cookie);
                        return Json(new { result = 1, message = "Xóa bài hát khỏi play list thành công"}, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = 0, message = "Xảy ra lỗi, vui lòng thử lại sau" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { result = 0, message = "Bài hát này không tồn tại trong Play List này" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = 0, message = "Vui lòng đăng nhập để thực hiện chức năng này" }, JsonRequestBehavior.AllowGet);
            }
        }

        //Trả về trang cá nhân 
        public ActionResult AllPlayList()
        {
            if (Session["user"] != null)
            {
                //init
                User user = (User)Session["user"];
                //PlayListDAL playlistdal = new PlayListDAL();
                var playlistdal = PlayListDAL.createPlayListDAL();
                //SongDAL songdal = new SongDAL();
                var songdal = SongDAL.createSongDAL();
                var artistdal = ArtistDAL.createArtistDAL();
                var genredal = GenreDAL.createGenreDAL();

                //lấy danh sách các bài hát yêu thích
                List<FavoriteSong> listFa = playlistdal.getFavoriteSongByUser(user.UserName);
                List<Song> listFaSong = new List<Song>();
                foreach (var item in listFa)
                {
                    //do bảng favorite ko có ràng buộc nên phải kiểm tra những bài hát đã bị xóa khỏi bảng song
                    Song tempSong = songdal.getSongByID(item.Song);
                    if (tempSong != null)
                        listFaSong.Add(tempSong);
                }

                ViewBag.listFaSong = listFaSong;

                //lấy tất cả playlist
                List<Playlist> listPlayList = playlistdal.getPlayListByUser(user.UserName);
                ViewBag.listPLayList = listPlayList;


                //lấy danh sách các artist
                List<Artist> listArtist = artistdal.getAllArtists();
                ViewBag.listArtist = listArtist;

                //Lấy danh sách thể loại
                List<Genre> listGenre = genredal.getAllGenre();
                ViewBag.listGenre = listGenre;

                //lấy danh sách artist quan tâm
                ViewBag.listArtistCare = artistdal.getArtistCareByUsername(user.UserName);

                return View(ViewBag);
            }
            else
            {
                return Content("Vui lòng đăng nhập để thực hiện chức năng này!");
            }

        }

        //Xóa bài hát trong danh sách yêu thích
        [HttpPost]
        public ActionResult delFavoriteSong()
        {
            if (Session["user"] != null)
            {

                User user = (User)Session["user"];
                int songID = Convert.ToInt32(Request.Form["songID"]);
                //PlayListDAL playlistdal = new PlayListDAL();
                var playlistdal = PlayListDAL.createPlayListDAL();

                //kiểm tra bài hát tồn tại trong danh sách yêu thích của user hay ko
                if (playlistdal.checkSongExistsInFavorite(songID, user.UserName))
                {
                    if (playlistdal.deleteFavoriteSong(user.UserName, songID))
                    {
                        return Json(new { result = 1, message = "Xóa bài hát khỏi danh sách yêu thích thành công." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = 0, message = "Đã có lỗi xảy ra. Vui lòng thử lại sau!" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { result = 0, message = "Bài hát không tồn tại trong danh sách yêu thích của bạn." }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json(new { result = 0, message = "Vui lòng đăng nhập để thực hiện chức năng này" }, JsonRequestBehavior.AllowGet);
            }
        }
  
        //lấy danh sách playlist khi mở modal thêm bài hát vào playlist
        [HttpPost]
        public ActionResult GetPlayList()
        {
            if (Session["user"] != null)
            {
                User user = (User)Session["user"];
                int songID = Convert.ToInt32(Request.Form["songID"]);
                //PlayListDAL playlistdal = new PlayListDAL();
                var playlistdal = PlayListDAL.createPlayListDAL();
                //SongDAL songdal = new SongDAL();
                var songdal = SongDAL.createSongDAL();

                ViewBag.song = songdal.getSongByID(songID);
                ViewBag.allPlayList = playlistdal.getPlayListByUser(user.UserName);

                return PartialView("AddPlayList");

            }
            else
            {
                return Json(new { result = 0, message = "Vui lòng đăng nhập để thực hiện chức năng này" }, JsonRequestBehavior.AllowGet);
            }
        }

       
        //upload bài hát
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UploadSong(FormCollection formCollection, HttpPostedFileBase file )
        {
            try
            {
                string fileSongName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".mp3";

                string songName = formCollection["songName"];
                string lyrics = formCollection["lyrics"];
                int artist = Convert.ToInt32(formCollection["artist"]);
                int composer = Convert.ToInt32(formCollection["composer"]);
                int genre = Convert.ToInt32(formCollection["genre"]);

                Song newSong = new Song();
                newSong.Name = songName;
                newSong.Lyrics = lyrics;
                newSong.Likes = 0;
                newSong.Views = 0;
                newSong.ViewsPerMonth = 0;
                newSong.ViewsPerWeek = 0;
                newSong.Status = true;
                newSong.FileName = fileSongName;

                using (QuanLyNhac db = new QuanLyNhac())
                {
                    db.Songs.Add(newSong);
                    db.SaveChanges();
                    int songID = newSong.ID;

                    Song s = db.Songs.Find(songID);
                    db.Songs.Attach(s);

                    //composer
                    Artist a = db.Artists.Find(composer);
                    db.Artists.Attach(a);

                    //artist
                    Artist ar = db.Artists.Find(artist);
                    db.Artists.Attach(ar);

                    //genre
                    Genre g = db.Genres.Find(genre);
                    db.Genres.Attach(g);

                    //gắn relationship giữa song và composer
                    s.Artist = a;

                    //gắn relation ship giữa song và artist
                    s.Artists.Add(ar);

                    //gắn relation ship giữa song và genre
                    s.Genres.Add(g);

                    db.SaveChanges();
                }




                var path = Path.Combine(Server.MapPath("~/Assets/singer"), fileSongName);
                file.SaveAs(path);

                return Json(new { result = true, message = "Upload thành công" });
            }
            catch
            {
                return Json(new { result = false, message = "Đã xảy ra lỗi. Vui lòng thử lại sau" });
            }




        }
	}
}