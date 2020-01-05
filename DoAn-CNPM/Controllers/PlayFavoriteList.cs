using System;
using System.Collections.Generic;
using DoAn_CNPM.Controllers;
using DoAn_CNPM.Models;
using DoAn_CNPM.DAL;
using System.Linq;
using System.Web;

namespace DoAn_CNPM.Controllers
{
    public class PlayFavoriteList : absPlaySongbyList
    {
        public override List<int> PlaySongListbyString(string username)
        {
            var playListDAL = PlayListDAL.createPlayListDAL();
            //SongDAL songdal = new SongDAL();
            var songdal = SongDAL.createSongDAL();
            List<FavoriteSong> listFS = playListDAL.getFavoriteSongByUser(username);
            List<int> listSongID = new List<int>();
            foreach (var item in listFS)
            {
                Song tempSong = songdal.getSongByID(item.Song);
                if (tempSong != null && tempSong.Status == true)
                {
                    listSongID.Add(tempSong.ID);
                }
            }
            return listSongID;
        }
        public override List<int> PlaySongList() { return null; }
    }
}