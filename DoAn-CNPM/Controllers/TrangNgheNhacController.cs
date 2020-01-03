using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn_CNPM.DAL;
using DoAn_CNPM.Models;
using System.IO;
using DoAn_CNPM.Models;
using System.Net;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace DoAn_CNPM.Controllers
{
    public class TrangNgheNhacController : Controller
    {
        //
        // GET: /TrangNgheNhac/
        public ActionResult Index(int id, int preID=-1)
        {

            //Khai báo
            //SongDAL songdal = new SongDAL();
            var songdal = SongDAL.createSongDAL();
            var playListDAL = PlayListDAL.createPlayListDAL();

            //lấy danh sách bài hát
            Song song = songdal.getSongByID(id);
            ViewBag.song = song;

            //tăng view khi nghe bắt đầu nghe nhạc
            songdal.UpView(id);

            //lấy lyrics
            ViewBag.lyrics = SomeFunction.getFormatLyrics(Server.MapPath("~"), song.Lyrics); ;

            // lấy danh sách nhạc đề cử
            List<Song> listRecommend = songdal.getSongsWithoutID(song.ID, 6);
            ViewBag.listSongRecommend = listRecommend;

            //Lấy playlist và bài hát có trong yêu thích 
            bool exist;
            if (Session["user"] != null)
            {
                string username = ((User)Session["user"]).UserName;
                exist = playListDAL.existSongInFavorite(id, username);

                //Lấy danh sách các playlist của user
                List<Playlist> list = playListDAL.getPlayListByUser(username);
                List<int> listr = playListDAL.getPlayListHasSong(id, username);
                ViewBag.AllPlayList = list;
                ViewBag.ExistSongPlayList = listr;
                ViewBag.inFavorite = exist;

            }

            //set previous song
            if (Response.Cookies["previousSong"]!=null)
            {
                Response.Cookies["previousSong"].Value = preID.ToString();
            }
            else
            {
                HttpCookie cookie = new HttpCookie("previousSong");
                cookie.Expires = DateTime.Now.AddHours(1);
                Response.Cookies.Add(cookie);
            }

            return View(ViewBag);
        }

        [HttpPost]
        public ActionResult LikeSong()
        {
            if (Session["user"] == null)
            {
                return Json(new { result = -1, message = "Vui lòng đăng nhập để thực hiện chức năng này" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                User username = (User)Session["user"];
                int Song = Convert.ToInt32(Request.Form["song"]);
                var playListDAL = PlayListDAL.createPlayListDAL();
                //kiểm tra bài hát tồn tại trong danh sách yêu thích ko
                if (playListDAL.existSongInFavorite(Song,username.UserName))
                {
                    playListDAL.deleteFavoriteSong(username.UserName, Song);
                    return Json(new { result = 1, imgsrc = "/Assets/image/icon/chuaThich.png", message="Xóa bài hát khỏi danh sách yêu thích thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    playListDAL.addFavoriteSong(username.UserName,Song);
                    //check user đã từng like bài này chưa, nếu chưa tăng like bài hát lên 1
                    //SongDAL songdal = new SongDAL();
                    var songdal = SongDAL.createSongDAL();
                    if (!songdal.checkUserLikedSong(username.UserName, Song))
                    {
                        songdal.insertUserLikedSong(Song, username.UserName);
                        songdal.UpLike(Song);
                        return Json(new { result = 1,like=songdal.getSongByID(Song).Likes, imgsrc = "/Assets/image/icon/daThich.png", message = "Thêm bài hát vào danh sách yêu thích thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { result = 1, imgsrc = "/Assets/image/icon/daThich.png", message = "Thêm bài hát vào danh sách yêu thích thành công" }, JsonRequestBehavior.AllowGet);
                }
            }

        }

        [HttpPost]
        public ActionResult AddPlayList()
        {
            if (Session["user"] != null)
            {
                string username = ((User)Session["user"]).UserName;
                int playList = Convert.ToInt32(Request.Form["playlist"]);
                int song = Convert.ToInt32(Request.Form["song"]);

                var playListDAL = PlayListDAL.createPlayListDAL();
                if (playListDAL.checkExistSongInPlayList(song, playList, username))
                {
                    if (playListDAL.deleteSongInPlayList(song, playList, username))
                    {

                        return Json(new { result = 1, message = "Xóa bài hát khỏi play list thành công", imgsrc = "/Assets/image/icon/PlayList.png" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = 0, message = "Xảy ra lỗi, vui lòng thử lại sau" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    Song_PlayList tam = new Models.Song_PlayList();
                    tam.Status=true;
                    tam.Song= song;
                    tam.PlayList = playList;
                    if (playListDAL.insertSongtoPlayList(tam))
                    {
                        return Json(new { result = 1, message = "Thêm bài hát vào play list thành công", imgsrc = "/Assets/image/icon/PlayListLike.png" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = 0, message = "Xảy ra lỗi, vui lòng thử lại sau" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                return Json(new { result = 0,message="Vui lòng đăng nhập để thực hiện chức năng này" }, JsonRequestBehavior.AllowGet);
            }

        }

        //tạo playlist mới và trả về partial view với playlist mới
        [HttpPost]
        public ActionResult AddNewPlayList()
        {
            if (Session["user"] != null)
            {
                string playlistName = Request.Form["playlistName"];
                int currentSong = Convert.ToInt32(Request.Form["currentSong"]);
                User user = (User)Session["user"];
                var playListDAL = PlayListDAL.createPlayListDAL();
                if (playListDAL.addNewPlayList(playlistName, user.UserName))
                {
                    //Lấy danh sách các playlist của user
                    List<Playlist> list = playListDAL.getPlayListByUser(user.UserName);
                    List<int> listr = playListDAL.getPlayListHasSong(currentSong, user.UserName);
                    ViewBag.AllPlayList = list;
                    ViewBag.ExistSongPlayList = listr;
                    ViewBag.songID = currentSong;
                    return PartialView(ViewBag);
                }
                else{
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult PlaySongList(List<int> listSongID=null)
        {
          
            if(listSongID==null)
                listSongID = JsonConvert.DeserializeObject<List<int>>(Request.Form["listSong"]);

            //SongDAL songdal = new SongDAL();
            var songdal = SongDAL.createSongDAL();
            List<Song> listSong = new List<Song>();
            var playListDAL = PlayListDAL.createPlayListDAL();

            bool inFavorite;

            //danh sách các id bài hát
            ViewBag.listID = listSongID;  //Request.Form["listSong"];
            //lấy danh sách bài hát trong list
            foreach (var item in listSongID)
            {
                listSong.Add(songdal.getSongByID(item));
            }
            ViewBag.listSong = listSong;


            //check bài hát có trong danh sách yêu thích hay ko
            if (Session["user"] != null)
            {
                User user = (User)Session["user"];
                if (playListDAL.checkSongExistsInFavorite(listSong.First().ID, user.UserName))
                    inFavorite = true;
                else
                    inFavorite = false;

                //tăng view
                songdal.UpView(listSong.FirstOrDefault().ID);
            }
            else
            {
                inFavorite = false;
            }
            ViewBag.inFavorite = inFavorite;

            //Lấy lời bài hát
            ViewBag.lyrics = SomeFunction.getFormatLyrics(Server.MapPath("~"), listSong.First().Lyrics);

            

            return View("PlaySongList");
        }

        public ActionResult PlayAlbum(int id)
        {
            var albumDAL = AlbumDAL.createAlbumDAL();

            List<int> listSongID = albumDAL.getListSongIDByAlbum(id);

            if (listSongID !=null && listSongID.Count > 0)
            {
                return PlaySongList(listSongID);
            }
            else
            {
                ViewBag.details = "Album này không có bài hát. Vui lòng thử lại sau";
                return View("Error");
            }
            
            
        }
        
        //nghe tất cả bài hát trong bxh tuần
        public ActionResult PlayRankForWeek()
        {
            //SongDAL songdal = new SongDAL();
            var songdal = SongDAL.createSongDAL();
            List<Song> listRank = songdal.getRankForWeek(200);
            List<int> listSongID = new List<int>();
            foreach (var item in listRank)
            {
                if (item.Status == true)
                    listSongID.Add(item.ID);
            }
            return PlaySongList(listSongID);
        }

        // nghe tất cả bài hát trong bxh tháng
        public ActionResult PlayRankForMonth()
        {
            //SongDAL songdal = new SongDAL();
            var songdal = SongDAL.createSongDAL();
            List<Song> listRank = songdal.getRankForMonth(200);
            List<int> listSongID = new List<int>();
            foreach (var item in listRank)
            {
                if (item.Status == true)
                    listSongID.Add(item.ID);
            }
            return PlaySongList(listSongID);
        }

        //nghe tất cả bài hát trong bxh views cao nhất
        public ActionResult PlayRankForViews()
        {
            //SongDAL songdal = new SongDAL();
            var songdal = SongDAL.createSongDAL();
            List<Song> listRank = songdal.getTopViewSong(200);
            List<int> listSongID = new List<int>();
            foreach (var item in listRank)
            {
                if (item.Status == true)
                    listSongID.Add(item.ID);
            }
            return PlaySongList(listSongID);
        }

        //nghe bài hát trong danh sách yêu thích
        public ActionResult PlayFavoriteListSong()
        {
            if (Session["user"] != null)
            {
                User user = (User)Session["user"];
                var playListDAL = PlayListDAL.createPlayListDAL();
                //SongDAL songdal = new SongDAL();
                var songdal = SongDAL.createSongDAL();
                List<FavoriteSong> listFS = playListDAL.getFavoriteSongByUser(user.UserName);
                List<int> listSongID = new List<int>();
                foreach (var item in listFS)
                {
                    Song tempSong = songdal.getSongByID(item.Song);
                    if (tempSong != null && tempSong.Status==true)
                    {
                        listSongID.Add(tempSong.ID);
                    }
                }
                if (listSongID.Count > 0)
                {
                    return PlaySongList(listSongID);
                }
                else
                {
                    ViewBag.details = "Danh sách bài hát yêu thích rỗng";
                    return View("Error");
                }
            }
            else
            {
                ViewBag.details = "Vui lòng đăng nhập để thực hiện chức năng này";
                return View("Error");
            }
            

        }
        
        //nghe bài hát trong playlist
        public ActionResult PlaySongsInPlayList(int id)
        {
            if (Session["user"] != null)
            {
                User user = (User)Session["user"];
                var playListDAL = PlayListDAL.createPlayListDAL();
                List<int> listSongID = playListDAL.getAllSongInPlayList(id);
                if (listSongID.Count > 0)
                {
                    return PlaySongList(listSongID);
                }
                else
                {
                    ViewBag.details = "Danh sách bài hát yêu thích rỗng";
                    return View("Error");
                }
            }
            else
            {
                ViewBag.details = "Vui lòng đăng nhập để thực hiện chức năng này";
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult GetSongPartial()
        {
            int songID = Convert.ToInt32(Request.Form["songID"]);
            //SongDAL songdal = new SongDAL();
            var songdal = SongDAL.createSongDAL();
            Song song = songdal.getSongByID(songID);
            if (song != null)
            {

                ViewBag.lyrics = SomeFunction.getFormatLyrics(Server.MapPath("~"), song.Lyrics);
                ViewBag.song = song;

                if (Session["user"] != null)
                {
                    User user = (User)Session["user"];
                    var playListDAL = PlayListDAL.createPlayListDAL();
                    if (playListDAL.checkSongExistsInFavorite(song.ID, user.UserName))
                        ViewBag.inFavorite = true;
                }
                //tăng view
                songdal.UpView(song.ID);

                return PartialView("PlayingSong");
            }
            else
            {
                return Json(new { result = -1, message = "Bài hát không tồn tại" });
            }
        }

        //mobile
        public ActionResult GetAllSongJson()
        {
            //SongDAL songdal = new SongDAL();
            var songdal = SongDAL.createSongDAL();
            List<Song> listSong = songdal.getAllSongs();

            List<MobileModel.MobileSong> listMobileSong = new List<MobileModel.MobileSong>();
            foreach (var item in listSong)
            {
                string songImage = Request.Url.Host + ":" + Request.Url.Port + "/Assets/image/avartarArtist/" + item.Artists.FirstOrDefault().Image+".jpg";
                string file = Request.Url.Host + ":" + Request.Url.Port + "/Assets/singer/" + item.FileName;
                listMobileSong.Add(new MobileModel.MobileSong(item.ID, item.Name, item.Artists.First().Name, songImage, file));
            }
            
            
            

            return Json(new {
                domain = Request.Url.Host,
                port = Request.Url.Port,
                Songs = listMobileSong
            },JsonRequestBehavior.AllowGet);
        }
    }
}