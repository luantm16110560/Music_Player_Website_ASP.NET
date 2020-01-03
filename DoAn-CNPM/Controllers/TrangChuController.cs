using DoAn_CNPM.DAL;
using DoAn_CNPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DoAn_CNPM.Controllers
{
    public class TrangChuController : Controller
    {
        //
        // GET: /TrangChu/
        public ActionResult Index()
        {
            //chú ý trong bảng song
            //artist là ca sĩ hát, artist1 là người sáng tác
            //SongDAL songdal = new SongDAL();
            var songdal = SongDAL.createSongDAL();
            var albumDAL = AlbumDAL.createAlbumDAL();
            var artistdal = ArtistDAL.createArtistDAL();
            ViewBag.album = albumDAL.getRandomAlbum(6);
            ViewBag.listSong = songdal.getFeaturedSong(4);
            ViewBag.artist = artistdal.getRandomArtist(8);
            ViewBag.rank = songdal.getRankForMonth(10);
            ViewBag.listAlBum = albumDAL.getRandomAlbum(6);



            return View(ViewBag);

            
        }

        public ActionResult Search(string textSearch)
        {
            //SongDAL songdal = new SongDAL();
            var songdal = SongDAL.createSongDAL();
            var artistdal = ArtistDAL.createArtistDAL();
            var albumDAL = AlbumDAL.createAlbumDAL();

            List<Song> listSong = songdal.searchSong(textSearch.Trim());
            ViewBag.listSong = listSong;

            List<Artist> listArtist = artistdal.searchArtist(textSearch.Trim());
            ViewBag.listArtist = listArtist;

            List<Album> listAlbum = albumDAL.searchAlbum(textSearch.Trim());
            ViewBag.listAlbum = listAlbum;

            ViewBag.textSearch = textSearch;

            return View();
            
        }

        
    }
}