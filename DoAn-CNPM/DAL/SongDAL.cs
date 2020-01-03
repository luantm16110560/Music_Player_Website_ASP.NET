using DoAn_CNPM.Controllers;
using DoAn_CNPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoAn_CNPM.DAL
{
    public class SongDAL
    {
        private QuanLyNhac db;
        public SongDAL()
        {
            db = new QuanLyNhac();
        }

        public static SongDAL songDAL;
        public static SongDAL createSongDAL()
        {
            if(songDAL == null)
            {
                songDAL = new SongDAL();
            }
            return songDAL;
        }

        public Song getSongByIDAdmin(int id)
        {
            return db.Songs.Find(id);
        }

        public List<Song> getAllSongsAdmin()
        {
            return db.Songs.ToList();
        }

        public Song getSongByID(int id)
        {
            return db.Songs.Where(s => s.ID == id && s.Status == true).FirstOrDefault();
        }
        public List<Song> getAllSongs()
        {
            return db.Songs.Where(p=>p.Status==true).ToList();
        }

        public bool checkSongExist(int Song)
        {
            return db.Songs.Any(p => p.ID == Song && p.Status==true);
        }

        public void UpView(int Song)
        {
            Song s = db.Songs.Find(Song);
            s.Views = s.Views==null ? 1: s.Views= s.Views +1;
            s.ViewsPerMonth = s.ViewsPerMonth == null ? 1 : s.ViewsPerMonth = s.ViewsPerMonth + 1;
            s.ViewsPerWeek = s.ViewsPerWeek == null ? 1 : s.ViewsPerWeek = s.ViewsPerWeek + 1;
            db.SaveChanges();
        }

        public List<Song> getSongsWithoutID(int id, int count)
        {
            return db.Songs.Where(q=>q.ID!=id && q.Status==true).OrderBy(q => Guid.NewGuid()).Take(count).ToList();
        }
        
        public bool insertSong(Song song)
        {
            //try
            //{
                db.Songs.Add(song);
                db.SaveChanges();
                return true;
            //}
            //catch{
            //    return false;
            //}
        }

        public bool UpdateSong(Song song)
        {
            //try
            //{
                Song ct = db.Songs.Find(song.ID);
                db.Entry(ct).CurrentValues.SetValues(song);
                db.SaveChanges();
                return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }

        public void DeleteSong(int ID)
        {
            Song ct = db.Songs.Find(ID);
            ct.Status = false;
            db.SaveChanges();
        }

        public void closeConnect()
        {
            db.Dispose();
        }

        public List<Song> getRandomSong(int count){
            return db.Songs.Where(s=>s.Status==true).OrderBy(q => Guid.NewGuid()).Take(count).ToList();
        }

        public List<Song> getFeaturedSong(int count)
        {
            List<RankForWeek> list = db.RankForWeeks.OrderBy(q => Guid.NewGuid()).Take(count).ToList();
            List<Song> listResult = new List<Song>();
            foreach (var item in list)
            {
                Song song = db.Songs.Find(item.Song);
                if(song.Status==true)
                    listResult.Add(song);
            }
            return listResult;
        }

        public List<Song> getRankForMonth(int count)
        {
            List<RankForMonth> list = db.RankForMonths.Take(count).ToList();
            List<Song> listResult = new List<Song>();
            foreach (var item in list)
            {
                Song song = db.Songs.Find(item.Song);
                if(song.Status==true)
                    listResult.Add(song);
            }
            return listResult;
        }

        public List<Song> getRankForWeek(int count)
        {
            List<RankForWeek> list = db.RankForWeeks.Take(count).ToList();
            List<Song> listResult = new List<Song>();
            foreach (var item in list)
            {
                listResult.Add(db.Songs.Find(item.Song));
            }
            return listResult;
        }

        //check user này đã like album chưa
        public bool checkUserLikedSong(string user, int song)
        {
            return db.LikeSongs.Any(p => p.Song == song && p.Username == user);
        }
        
        //tăng like bài hát lên 1
        public void UpLike(int id)
        {
            Song song = db.Songs.Find(id);
            song.Likes++;
            db.SaveChanges();
        }

        //add user like vào bảng LikeSong
        public void insertUserLikedSong(int song, string username)
        {
            LikeSong likesong = new LikeSong();
            likesong.Song=song;
            likesong.Username=username;
            db.LikeSongs.Add(likesong);
            db.SaveChanges();
        }

        //get song by artist
        public List<Song> getSongByArtist(int artist)
        {
            return db.Songs.Where(s => s.Artists.Where(p => p.ID == artist).Count() > 0 && s.Status==true).ToList();
        }



        //search song by name
        public List<Song> searchSong(string name)
        {
            name = SomeFunction.ConvertToUnSign(name);

            List<Song> listResult = db.Songs.Where(delegate(Song s) {


                if (SomeFunction.ConvertToUnSign(s.Name).Contains(name))
                {
                    return true;
                }
                foreach (var item in s.Artists)
                {
                    string artistName = SomeFunction.ConvertToUnSign(item.Name);
                    if (artistName.Contains(name))
                    {
                        return true;
                    }
                }

                return false;
            }).ToList();

            return listResult;
        }

        public List<Song> getTopViewSong(int count)
        {
            return db.Songs.Where(s => s.Status == true).OrderByDescending(s => s.Views).Take(count).ToList();
        }

        //cập nhật lại bxh tuần
        public bool updateWeekRank()
        {
            try
            {
                db.Database.ExecuteSqlCommand("EXEC dbo.GetRankWeekNotFormat");

                return true;
            }
            catch
            {
                return false;
            }
        }

        //cập nhật lại bxh tháng
        public bool updateMonthRank()
        {
            try
            {
                db.Database.ExecuteSqlCommand("EXEC dbo.GetRankMonthNotFormat");

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddArtistToSong(Song song, List<int> listArtistID)
        {
            try
            {
                Song s = db.Songs.Find(song.ID);
                db.Songs.Attach(s);

                foreach (var item in listArtistID)
                {
                    //artist
                    Artist ar = db.Artists.Find(item);
                    db.Artists.Attach(ar);


                    //gắn relation ship giữa song và artist
                    s.Artists.Add(ar);
                }

                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool RemoveArtistToSong(Song song)
        {
            try
            {
                Song s = db.Songs.Find(song.ID);
                s.Artists.Clear();
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public bool RemoveGenreToSong(Song song)
        {
            try
            {
                Song s = db.Songs.Find(song.ID);
                s.Genres.Clear();
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }

        }
        public bool AddGenreToSong(Song song, List<int> listGenreID)
        {
            try
            {
                Song s = db.Songs.Find(song.ID);
                db.Songs.Attach(s);

                foreach (var item in listGenreID)
                {
                    Genre ge = db.Genres.Find(item);
                    db.Genres.Attach(ge);


                    //gắn relation ship giữa song và genre
                    s.Genres.Add(ge);
                }

                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}