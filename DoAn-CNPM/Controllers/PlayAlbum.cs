using System;
using System.Collections.Generic;
using DoAn_CNPM.Controllers;
using DoAn_CNPM.Models;
using DoAn_CNPM.DAL;
using System.Linq;
using System.Web;

namespace DoAn_CNPM.Controllers
{
    public class PlayAlbum : absPlaySongbyList
    {
        public override List<int> PlaySongListbyString(string id)
        {
            var albumDAL = AlbumDAL.createAlbumDAL();
            List<int> listSongID = new List<int>();
            listSongID = albumDAL.getListSongIDByAlbum(System.Convert.ToInt32(id));
            return listSongID;
        }
        public override List<int> PlaySongList() { return null; }
    }
}