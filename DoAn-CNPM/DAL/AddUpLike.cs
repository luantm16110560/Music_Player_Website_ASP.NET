using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn_CNPM.DAL
{
    public class AddUpLike : absAddFavor
    {
        public override void Favor(string username, int songID)
        {
            SongDAL.songDAL.insertUserLikedSong(songID, username);
            SongDAL.songDAL.UpLike(songID);
        }
    }
}