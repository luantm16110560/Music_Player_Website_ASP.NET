using System;
using DoAn_CNPM.Controllers;
using DoAn_CNPM.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn_CNPM.Controllers
{
    public abstract class absPlaySongbyList
    {
        public abstract List<int> PlaySongList();
        public abstract List<int> PlaySongListbyString(string id);
    }
}