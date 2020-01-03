using DoAn_CNPM.DAL;
using DoAn_CNPM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn_CNPM.Controllers
{
    public class TrangAdminController : Controller
    {

        public ActionResult Index(string message = null, int tabIndex = 0)
        {
            User user = (User)Session["adminUser"];

            if (user!=null && user.Role==true)
            {
                if (message != null)
                    ViewBag.message = message;
                ViewBag.tabIndex = tabIndex;
                return View();
            }
            else
            {
                return RedirectToAction("LogInAdmin");
            }
                
        }
        
        //Trang bài hát
        public ActionResult GetSong()
        {
            var songdal = SongDAL.createSongDAL();
            ViewBag.listSong = songdal.getAllSongsAdmin();
            
            return PartialView();
        }
        
        [HttpPost]
        public ActionResult GetSongDetails(int id)
        {
            //SongDAL songdal = new SongDAL();
            var songdal = SongDAL.createSongDAL();
            var albumDAL = AlbumDAL.createAlbumDAL();
            var artistdal = ArtistDAL.createArtistDAL();
            var genredal = GenreDAL.createGenreDAL();
            ViewBag.song = songdal.getSongByIDAdmin(id);
            ViewBag.listAlbum = albumDAL.getAllAlbum();
            ViewBag.listArtist = artistdal.getAllArtists();
            ViewBag.listGenre = genredal.getAllGenre();
            return PartialView();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddOrUpdateSong(Song song, HttpPostedFileBase file, int[] artist,int[] genre, string Status)
        {
            //SongDAL songdal = new SongDAL();
            var songdal = SongDAL.createSongDAL();
            song.Views = song.Views == null ? 0 : song.Views;
            song.ViewsPerMonth = song.ViewsPerMonth == null ? 0 : song.ViewsPerMonth;
            song.ViewsPerWeek = song.ViewsPerWeek == null ? 0 : song.ViewsPerWeek;
            song.Likes = song.Likes == null ? 0 : song.Likes;
            //thêm
            if (song.ID == 0)
            {
                
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".mp3";
                song.Status = Status == null ? false : true;
                song.FileName = fileName;
                
                
                var path = Path.Combine(Server.MapPath("~/Assets/singer"), fileName);
                file.SaveAs(path);
                if (songdal.insertSong(song) 
                    && songdal.AddArtistToSong(song,artist.ToList()) 
                    && songdal.AddGenreToSong(song,genre.ToList()))
                {
                    return RedirectToAction("Index", new { message = "Thêm thành công", tabIndex = 0 });
                }
                    
                else
                    return RedirectToAction("Index", new { message = "Thêm thất bại", tabIndex = 0 });
            }
            //sửa
            else
            {
                Song songConf = songdal.getSongByIDAdmin(song.ID);
                if (songConf!=null)
                {
                    bool changeWeek = songConf.ViewsPerWeek == song.ViewsPerWeek;
                    bool changeMonth = songConf.ViewsPerMonth == song.ViewsPerMonth;
                    string fileName = string.IsNullOrEmpty(songConf.FileName)? DateTime.Now.ToString("yyyyMMddHHmmss") + ".mp3":songConf.FileName;
                    songConf = song;
                    songConf.FileName = fileName;
                    songConf.Status = Status == null ? false : true;
                    if (file != null)
                    {
                        var path = Path.Combine(Server.MapPath("~/Assets/singer"), fileName);
                        file.SaveAs(path);
                    }

                    songdal.RemoveArtistToSong(songConf);
                    songdal.RemoveGenreToSong(songConf);
                    songdal.UpdateSong(songConf);
                    songdal.AddArtistToSong(songConf, artist.ToList());
                    songdal.AddGenreToSong(songConf, genre.ToList());
                    if (!changeWeek)
                        songdal.updateWeekRank();
                    if (!changeMonth)
                        songdal.updateMonthRank();

                    return RedirectToAction("Index", new { message = "Cập nhật bài hát thành công", tabIndex = 0 });
                }
                else
                {
                    return RedirectToAction("Index", new { message = "Không tìm thấy bài hát", tabIndex = 0 });
                }
            }
        }


        //Trang album
        public ActionResult GetAlbum()
        {
            var albumDAL = AlbumDAL.createAlbumDAL();
            ViewBag.listAlbum = albumDAL.getAllAlbumAdmin();
            return PartialView();
        }

        [HttpPost]
        public ActionResult GetAlbumDetails(int id)
        {
            var albumDAL = AlbumDAL.createAlbumDAL();
            var artistdal = ArtistDAL.createArtistDAL();
            ViewBag.album = albumDAL.getAlbumByIDAdmin(id);
            ViewBag.listArtist = artistdal.getAllArtists();
            return PartialView();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddOrUpdateAlbum(Album album, int[] artist, string Status)
        {
            var albumDAL = AlbumDAL.createAlbumDAL();
            album.Likes = album.Likes == null ? 0 : album.Likes;
            album.Views = album.Views == null ? 0 : album.Views;
            HttpPostedFileBase albumImageFile = Request.Files["albumImageFile"];
            //thêm
            if (album.ID == 0)
            {

                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                album.Status = Status == null ? false : true;
                album.Image = fileName;
                var path = Path.Combine(Server.MapPath("~/Assets/image/imageAlbum"), fileName);
                albumImageFile.SaveAs(path);
                if (albumDAL.insertAlbum(album) && albumDAL.AddArtistToAlbum(album,artist.ToList()))
                    return RedirectToAction("Index", new { message = "Thêm thành công", tabIndex = 1 });
                else
                    return RedirectToAction("Index", new { message = "Thêm thất bại", tabIndex = 1 });
            }
            //sửa
            else
            {
                Album albumConf = albumDAL.getAlbumByIDAdmin(album.ID);
               
                if (albumConf != null)
                {
                    string fileName = string.IsNullOrEmpty(albumConf.Image) ? DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg" : albumConf.Image;
                    albumConf = album;
                    albumConf.Image = fileName;

                    albumConf.Status = Status == null ? false : true;
                    if (albumImageFile.ContentLength>0)
                    {
                        var path = Path.Combine(Server.MapPath("~/Assets/image/imageAlbum"), fileName);
                        albumImageFile.SaveAs(path);
                    }

                    albumDAL.RemoveArtistToAlbum(albumConf);
                    albumDAL.UpdateAlbum(albumConf);
                    albumDAL.AddArtistToAlbum(albumConf, artist.ToList());

                    return RedirectToAction("Index", new { message = "Cập nhật thành công", tabIndex = 1 });
                }
                else
                {
                    return RedirectToAction("Index", new { message = "Không tìm thấy album", tabIndex = 1 });
                }
            }
        }

        //Trang Artist
        public ActionResult GetArtist()
        {
            var artistdal = ArtistDAL.createArtistDAL();
            ViewBag.listArtist = artistdal.getAllArtistsAdmin();
            return PartialView();
        }

        [HttpPost]
        public ActionResult GetArtistDetails(int id)
        {
            var artistdal = ArtistDAL.createArtistDAL();
            Artist artist = artistdal.getArtistByIDAdmin(id);
            ViewBag.artist = artist;
            return PartialView();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddOrUpdateArtist(Artist artist, string Status)
        {
            var artistdal = ArtistDAL.createArtistDAL();
            artist.Follows = artist.Follows == null ? 0 : artist.Follows;
            HttpPostedFileBase artistImageFile = Request.Files["artistImageFile"];
            HttpPostedFileBase backGroundImageFile = Request.Files["backGroundImageFile"];
            artist.Follows = artist.Follows==null ? 0 : artist.Follows;
            //thêm
            if (artist.ID == 0)
            {
                string gen = DateTime.Now.ToString("yyyyMMddHHmmss");
                string fileImageName = gen + "image.jpg";
                string fileBackgroundImage = gen + "background.jpg";
                artist.Status = Status == null ? false : true;
                artist.Image = fileImageName;
                artist.Background = fileBackgroundImage;
                var pathImage = Path.Combine(Server.MapPath("~/Assets/image/avartarArtist"), fileImageName);
                var pathBackground = Path.Combine(Server.MapPath("~/Assets/image/backgroundArtist"), fileBackgroundImage);
                artistImageFile.SaveAs(pathImage);
                backGroundImageFile.SaveAs(pathBackground);
                if (artistdal.insertArtist(artist))
                    return RedirectToAction("Index", new { message = "Thêm thành công", tabIndex = 2});
                else
                    return RedirectToAction("Index", new { message = "Thêm thất bại", tabIndex = 2 });
            }
            //sửa
            else
            {
                Artist artistConf = artistdal.getArtistByIDAdmin(artist.ID);
                if (artistConf != null)
                {
                    string imageName = string.IsNullOrEmpty(artistConf.Image) ? DateTime.Now.ToString("yyyyMMddHHmmss") + "image.jpg" : artistConf.Image;
                    string backGroundImageName = string.IsNullOrEmpty(artistConf.Background) ? DateTime.Now.ToString("yyyyMMddHHmmss") + "background.jpg" : artistConf.Background;
                    artistConf = artist;
                    artistConf.Image = imageName;
                    artistConf.Background = backGroundImageName;

                    artistConf.Status = Status == null ? false : true;
                    if (artistImageFile.ContentLength >0)
                    {
                        var path = Path.Combine(Server.MapPath("~/Assets/image/avartarArtist"), imageName);
                        artistImageFile.SaveAs(path);
                    }
                    if (backGroundImageFile.ContentLength > 0)
                    {
                        var path = Path.Combine(Server.MapPath("~/Assets/image/backgroundArtist"), backGroundImageName);
                        backGroundImageFile.SaveAs(path);
                    }

                    artistdal.UpdateArtist(artistConf);

                    return RedirectToAction("Index", new { message = "Cập nhật thành công", tabIndex = 2 });
                }
                else
                {
                    return RedirectToAction("Index", new { message = "Không tìm thấy nghệ sĩ", tabIndex = 2 });
                }
            }
        }

        //Trang thể loại
        public ActionResult GetGenre()
        {
            var genredal = GenreDAL.createGenreDAL();
            ViewBag.listGenre = genredal.getAllGenreAdmin();
            return PartialView();
        }

        [HttpPost]
        public ActionResult GetGenreDetails(int id)
        {
            var genredal = GenreDAL.createGenreDAL();
            ViewBag.genre = genredal.getGenreByIDAdmin(id);
            return PartialView();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddOrUpdateGenre(Genre genre, string Status)
        {
            HttpPostedFileBase file = Request.Files["GenreImage"];
            var genredal = GenreDAL.createGenreDAL();
            if (genre.ID == 0)
            {
                genre.Status = Status == null ? false : true;
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "genreImage.jpg";
                genre.Image = fileName;
                var path = Path.Combine(Server.MapPath("~/Assets/image/imageGenre"), fileName);
                file.SaveAs(path);
                if (genredal.InsertGenre(genre))
                {
                    return RedirectToAction("Index", new { message = "Cập nhật thành công", tabIndex = 3 });
                }
                else
                {
                    return RedirectToAction("Index", new { message = "Thêm thất bại", tabIndex = 3 });
                }
            }
            else
            {
                Genre genreConf = genredal.getGenreByIDAdmin(genre.ID);
                if (genreConf != null)
                {
                    string imageName = string.IsNullOrEmpty(genreConf.Image) ? DateTime.Now.ToString("yyyyMMddHHmmss") + "genreImage.jpg" : genreConf.Image;
                    genreConf = genre;
                    genreConf.Image = imageName;

                    genreConf.Status = Status == null ? false : true;
                    if (file.ContentLength > 0 )
                    {
                       
                        var path = Path.Combine(Server.MapPath("~/Assets/image/imageGenre"), imageName);
                        file.SaveAs(path);
                    }
                   

                    genredal.UpdateGenre(genreConf);

                    return RedirectToAction("Index", new { message = "Cập nhật thành công", tabIndex = 3});

                }
                else
                {
                    return RedirectToAction("Index", new { message = "Không tìm thấy thể loại", tabIndex = 3 });
                }
            }
        }

        //trang người dùng
        public ActionResult GetUser()
        {
            var userdal = UserDAL.createUserDAL();
            ViewBag.listUser = userdal.getAllUserAdmin();
            return PartialView();
        }

        [HttpPost]
        public ActionResult GetUserDetails(string id)
        {
            var userdal = UserDAL.createUserDAL();
            ViewBag.user = userdal.getUserByUserNameAdmin(id);
            return PartialView();
        }

        public ActionResult LockOrUnlockUser(string username, bool Status)
        {
            //UserDAL userdal = new UserDAL();
            var userdal = UserDAL.createUserDAL();
            User user = userdal.getUserByUserNameAdmin(username);
            if (user != null)
            {
                user.Status = Status;
                if (userdal.LockOrUnlockUserAdmin(user))
                {
                    return RedirectToAction("Index", new { message = "Cập nhật thành công", tabIndex = 4 });
                }
                else
                {
                    return RedirectToAction("Index", new { message = "Cập nhật thất bại", tabIndex = 4 });
                }
            }
            else
            {
                return RedirectToAction("Index", new { message = "Không tìm thấy người dùng", tabIndex = 4 });
            }
        }

        public ActionResult LogInAdmin()
        {
            return View();
        }

        public ActionResult CheckLogin(string username, string password)
        {
            //UserDAL userdal = new UserDAL();
            var userdal = UserDAL.createUserDAL();
            User user = userdal.getUserByUserNameAdmin(username);
            if (user != null)
            {
                if(password == user.Password && user.Role==true)
                {
                    Session["adminUser"] = user;
                    return Json(new { result = 1 });
                }
                else
                {
                    return Json(new { result = 0 });
                }
            }
            else
            {
                return Json(new { result = 0 });
            }
        }

        public ActionResult LogOutAdmin()
        {
            Session.Remove("adminUser");
            return RedirectToAction("LogInAdmin");

        }

        [HttpPost]
        public ActionResult SignUpAdmin(string username, string password)
        {
            User curUser = (User)Session["adminUser"];
            if (curUser != null)
            {
                //UserDAL userdal = new UserDAL();
                var userdal = UserDAL.createUserDAL();
                User newUser = new User();
                newUser.UserName = username;
                newUser.Password = password;
                newUser.Role = true;
                userdal.insertUser(newUser);
                return Json(new { result = 1 });
            }
            else
            {
                return Json(new { result = 0 });
            }
        }
    }
}