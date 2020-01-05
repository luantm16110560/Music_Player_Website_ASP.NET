using System;
using System.Collections.Generic;
using DoAn_CNPM.Controllers;
using DoAn_CNPM.Models;
using DoAn_CNPM.DAL;
using System.Linq;
using System.Web;

namespace DoAn_CNPM.Controllers
{
    public class PlayRankForMonth : absPlaySongbyList
    {
        public override List<int> PlaySongList()
        {
            var songdal = SongDAL.createSongDAL();
            List<Song> listRank = songdal.getRankForMonth(200);
            List<int> listSongID = new List<int>();
            foreach (var item in listRank)
            {
                if (item.Status == true)
                    listSongID.Add(item.ID);
            }
            return listSongID;
        }
        public override List<int> PlaySongListbyString(string username) { return null; }
    }
}