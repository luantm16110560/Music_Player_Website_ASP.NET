using System;
using System.Collections.Generic;
using DoAn_CNPM.Controllers;
using DoAn_CNPM.Models;
using DoAn_CNPM.DAL;
using System.Linq;
using System.Web;

namespace DoAn_CNPM.Controllers
{
    public class PlayRankForView : absPlayRankSong
    {
        public override List<int> PlayRank()
        {
            var songdal = SongDAL.createSongDAL();
            List<Song> listRank = songdal.getTopViewSong(200);
            List<int> listSongID = new List<int>();
            foreach (var item in listRank)
            {
                if (item.Status == true)
                    listSongID.Add(item.ID);
            }
            return listSongID;
        }
    }
}