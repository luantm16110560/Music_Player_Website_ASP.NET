using DoAn_CNPM.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn_CNPM.Controllers
{
    public class TrangAlbumController : Controller
    {
        // GET: TrangAlbum
        public ActionResult Index()
        {
            var albumDAL = AlbumDAL.createAlbumDAL();
            ViewBag.album = albumDAL.getAllAlbum();
            return View(ViewBag);
        }
    }
}