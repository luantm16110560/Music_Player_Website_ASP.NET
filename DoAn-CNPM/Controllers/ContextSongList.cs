using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn_CNPM.Controllers
{
    public class ContextSongList
    {
        private absPlaySongbyList _playsonglist;
        public ContextSongList(absPlaySongbyList playsonglist)
        {
            this._playsonglist = playsonglist;
        }
        public List<int> Playsong()
        {
            return _playsonglist.PlaySongList();
        }
        public List<int> Playsongbystring(string s)
        {
            return _playsonglist.PlaySongListbyString(s);
        }
    }
}