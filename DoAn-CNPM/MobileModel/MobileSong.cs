using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn_CNPM.MobileModel
{
    public class MobileSong
    {
        private int SongID;
        private string SongName;
        private string ArtistName;
        private string SongImage;
        private string File;

        public MobileSong(int id, string name, string artistname, string songimage, string file)
        {
            this.SongID = id;
            this.SongName = name;
            this.ArtistName = artistname;
            this.SongImage = songimage;
            this.File = file;
        }

        public int SongID1
        {
            get
            {
                return SongID;
            }

            set
            {
                SongID = value;
            }
        }

        public string SongName1
        {
            get
            {
                return SongName;
            }

            set
            {
                SongName = value;
            }
        }

        public string ArtistName1
        {
            get
            {
                return ArtistName;
            }

            set
            {
                ArtistName = value;
            }
        }

        public string SongImage1
        {
            get
            {
                return SongImage;
            }

            set
            {
                SongImage = value;
            }
        }

        public string File1
        {
            get
            {
                return File;
            }

            set
            {
                File = value;
            }
        }
    }
}