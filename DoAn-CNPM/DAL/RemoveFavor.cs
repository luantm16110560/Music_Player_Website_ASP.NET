using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn_CNPM.DAL
{
    public class RemoveFavor : absAddFavor
    {
        public override void Favor(string username, int songID)
        {
            PlayListDAL.playListDAL.deleteFavoriteSong(username, songID);
        }
    }
}