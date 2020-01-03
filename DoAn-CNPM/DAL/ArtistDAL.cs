using DoAn_CNPM.Controllers;
using DoAn_CNPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn_CNPM.DAL
{
    public class ArtistDAL
    {
        private QuanLyNhac db;
        public ArtistDAL()
        {
            db = new QuanLyNhac();
        }

        private static ArtistDAL artistDAL;
        public static ArtistDAL createArtistDAL()
        {
            if (artistDAL == null)
            {
                artistDAL = new ArtistDAL();
            }
            return artistDAL;
        }

        public List<Artist> getAllArtists(){
            return db.Artists.Where(a => a.Status == true).ToList();
        }

        public List<Artist> getAllArtistsAdmin()
        {
            return db.Artists.ToList();
        }

        public Artist getArtistByIDAdmin(int id)
        {
            return db.Artists.Find(id);
        }

        public bool insertArtist(Artist Artist)
        {
            try
            {
                Artist.Status = true;
                db.Artists.Add(Artist);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateArtist(Artist Artist)
        {
            try
            {
                Artist ct = db.Artists.Find(Artist.ID);
                db.Entry(ct).CurrentValues.SetValues(Artist);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void DeleteArtist(int ID)
        {
            Artist ct = db.Artists.Find(ID);
            ct.Status = false;
            db.SaveChanges();
        }
        public void closeConnect()
        {
            db.Dispose();
        }
        public List<Artist> getRandomArtist(int count)
        {
            return db.Artists.Where(a => a.Status == true && !string.IsNullOrEmpty(a.Image) && a.Songs1.Count>0).OrderBy(q => Guid.NewGuid()).Take(count).ToList();
        }

        public Artist getArtistByID(int ID)
        {
            return db.Artists.Where(ar => ar.ID == ID && ar.Status == true).FirstOrDefault();
        }

        //kiểm tra user này đã follow nghệ sĩ này chưa
        public bool checkUserFollowArtist(string username, int artistID)
        {
            return db.Artist_User.Any(u => u.User == username && u.Artist == artistID);
        }

        //kiểm tra user này đã từng follow nghệ sĩ này chưa
        public bool checkUserExistInFollowing(string username, int artistID)
        {
            return db.FollowArtists.Any(u => u.Username == username && u.Artist == artistID);
        }

        //thêm vào bảng follow để biết người này đã từng follow nghệ sĩ này
        public void addUserToFollowedArtist(string username, int artistID)
        {
            FollowArtist foA = new FollowArtist();
            foA.Artist = artistID;
            foA.Username = username;
            db.FollowArtists.Add(foA);
            db.SaveChanges();
        }

        //follow artist
        public bool followArtist(Artist_User entity)
        {
            try
            {
                entity.Status = true;
                db.Artist_User.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //del follow artist
        public bool delFollowArtist(string username, int artistID)
        {
            try
            {
                db.Artist_User.Remove(db.Artist_User.Where(u => u.User == username && u.Artist == artistID).First());
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //tăng follow
        public void upFollow(int artistID)
        {
            Artist artist = db.Artists.Find(artistID);
            artist.Follows++;
            db.SaveChanges();
        }

        //search artist
        public List<Artist> searchArtist(string name)
        {
            name = SomeFunction.ConvertToUnSign(name);
            return db.Artists.Where(delegate (Artist artist) {
                return SomeFunction.ConvertToUnSign(artist.Name).Contains(name) && artist.Status == true;
            }).ToList();
        }

        //get all aritst are followed by username

        public List<int> getAllFollowedAristIDByUsername(string username)
        {
            List<Artist_User> listAU = db.Artist_User.Where(au => au.User == username && au.Artist1.Status==true).ToList();
            List<int> listResult = new List<int>();
            foreach (var item in listAU)
            {
                listResult.Add(item.Artist);
            }
            return listResult;
        }

        //lấy danh sách artist đc quan tâm bởi username
        public List<Artist> getArtistCareByUsername(string username)
        {
            List<Artist_User> listAU = db.Artist_User.Where(au => au.User == username && au.Status == true).ToList();
            List<Artist> listResult = new List<Artist>();
            foreach (var item in listAU)
            {
                listResult.Add(item.Artist1);
            }
            return listResult;
        }
    }
}