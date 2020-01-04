using DoAn_CNPM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DoAn_CNPM.DAL
{
    public class PlayListDAL
    {
        private QuanLyNhac db;

        public PlayListDAL()
        {
            db = new QuanLyNhac();
        }

        public static PlayListDAL playListDAL;
        public static PlayListDAL createPlayListDAL()
        {
            if(playListDAL == null)
            {
                playListDAL = new PlayListDAL();
            }
            return playListDAL;
        }

        public bool deleteFavoriteSong(string username, int Song)
        {
            //try
            //{
                FavoriteSong fal = new FavoriteSong();
                fal.Username = username;
                fal.Song = Song;
                db.FavoriteSongs.Attach(fal);
                db.FavoriteSongs.Remove(fal);
                db.SaveChanges();
                return true;
            //}
            //catch(Exception e)
            //{
            //    return false;
            //}
        }

        public bool addFavoriteSong(string username, int Song)
        {
            try
            {
                FavoriteSong fal = new FavoriteSong();
                fal.Username = username;
                fal.Song = Song;
                
                db.FavoriteSongs.Add(fal);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool existSongInFavorite(int Song, string Username)
        {
            return db.FavoriteSongs.Any(p => p.Song == Song && p.Username==Username);
        }

        public List<Playlist> getPlayListByUser(string username)
        {
            return db.Playlists.Where(p => p.Username == username && p.Status==true).ToList();
         
        }

        public List<int> getPlayListHasSong(int Song, string username)
        { 
            List<int> listResult = new List<int>();

            List<Playlist> listPlayList = db.Playlists.Where(p => p.Status == true && p.Username == username).ToList();
            foreach (var item in listPlayList)
            {
                foreach (var it in item.Song_PlayList)
                {
                    if (it.Song == Song && it.Status == true)
                    {
                        listResult.Add(it.PlayList);
                    }
                }
            }


            return listResult;
        }

        public bool checkExistSongInPlayList(int song, int playlist, string username)
        {
            List<Playlist> listPlayList = db.Playlists.Where(p => p.Status == true && p.Username == username).ToList();
            Playlist playListCT = listPlayList.Find(p => p.ID == playlist);

            foreach (var item in playListCT.Song_PlayList)
            {
                if (item.Song == song && item.Status == true)
                    return true;
            }
            return false;
        }

        public bool insertSongtoPlayList(Song_PlayList song_playlist)
        {
            try
            {
                db.Song_PlayList.Add(song_playlist);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteSongInPlayList(int song, int playlist, string username)
        {
            try
            {
            Playlist playList = db.Playlists.Where(p => p.ID == playlist && p.Status == true).First();
            Song_PlayList song_playList = playList.Song_PlayList.Where(p => p.Song == song && p.Status == true).First();
            db.Song_PlayList.Attach(song_playList);
            db.Song_PlayList.Remove(song_playList);
                db.SaveChanges();
                
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Playlist getPlayListByID(int id)
        {
            return db.Playlists.Where(pl=>pl.ID==id && pl.Status==true).ToList().FirstOrDefault();
        }
        //lấy danh sách id của các bài hát trong playlist

        public List<int> getAllSongInPlayList(int playlist)
        {
            List<Song_PlayList> list = db.Song_PlayList.Where(p => p.PlayList == playlist && p.Status == true).ToList();
            List<int> listResult = new List<int>();
            foreach (var item in list)
            {
                listResult.Add(item.Song);
            }
            return listResult;
        }

        //lấy tất cả các bài hát yêu thích của user
        public List<FavoriteSong> getFavoriteSongByUser(string username)
        {
            return db.FavoriteSongs.Where(p => p.Username == username).ToList();
        }

        //kiểm tra bài hát có tồn tại trong danh sách yêu thích hay ko

        public bool checkSongExistsInFavorite(int songID, string username)
        {
            return db.FavoriteSongs.Any(p => p.Song == songID && p.Username == username);
        }

        //thêm playlist cho user
        public bool addNewPlayList(string playlistName, string username)
        {
            try
            {
                Playlist pl = new Playlist();
                pl.Username = username;
                pl.Name = playlistName;
                pl.Status = true;
                db.Playlists.Add(pl);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            
        }
    }
}