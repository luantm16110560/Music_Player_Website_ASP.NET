using DoAn_CNPM.DAL;
using DoAn_CNPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn_CNPM.Controllers
{
    public class TrangBXHController : Controller
    {
        

        // GET: TrangBXH
        public ActionResult Index()
        {
            //SongDAL songdal = new SongDAL();
            var songdal = SongDAL.createSongDAL();
            ViewBag.listSong = songdal.getRankForWeek(100);

            return View();
        }

        [HttpPost]
        public ActionResult GetRank(int tab)
        {
            //quy định tab 
            //1: tuần, 2: tháng, 3: lượt nghe nhiều nhất
            //SongDAL songdal = new SongDAL();
            var songdal = SongDAL.createSongDAL();
            List<Song> listSong;
            switch (tab)
            {
                case 1:
                    listSong = songdal.getRankForWeek(200);
                    break;
                case 2:
                    listSong = songdal.getRankForMonth(200);
                    break;
                case 3:
                    listSong = songdal.getTopViewSong(200);
                    break;
                default:
                    return Json(new { result = 0, message = "Đã xảy ra lỗi vui lòng thử lại sau." });
            }
            ViewBag.listSong = listSong;
            ViewBag.tab = tab;
            return PartialView("RankPartial");
        }
    }
}