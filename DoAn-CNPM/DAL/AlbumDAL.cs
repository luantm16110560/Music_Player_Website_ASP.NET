using DoAn_CNPM.Controllers;
using DoAn_CNPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn_CNPM.DAL
{
    public class AlbumDAL
    {
        public AlbumDAL()
        {
            db = new QuanLyNhac();
        }

        private static AlbumDAL albumDAL;
        private QuanLyNhac db;

        public static AlbumDAL createAlbumDAL()
        {
            if (albumDAL == null)
            {
                albumDAL = new AlbumDAL();
            }
            return albumDAL;
        }


        public Album getAlbumByIDAdmin(int id)
        {
            return db.Albums.Find(id);
        }

        public List<Album> getAllAlbum()
        {
            return db.Albums.Where(al =>  al.Status == true).ToList();
        }

        public List<Album> getAllAlbumAdmin()
        {
            return db.Albums.ToList();
        }

        public bool insertAlbum(Album album)
        {
            try
            {
                album.Status = true;
                db.Albums.Add(album);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateAlbum(Album album)
        {
            try
            {
                Album ct = db.Albums.Find(album.ID);
                db.Entry(ct).CurrentValues.SetValues(album);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Album getAlbumByID(int id)
        {
            return db.Albums.Where(al => al.ID == id && al.Status == true).FirstOrDefault();
        }

        public void DeleteAlbum(int ID)
        {
            Album ct = db.Albums.Find(ID);
            ct.Status = false;
            db.SaveChanges();
        }
        public void closeConnect()
        {
            db.Dispose();
        }

        public List<Album> getRandomAlbum(int count)
        {
            return db.Albums.Where(al=>al.Status==true && al.Songs.Count>0).OrderBy(q => Guid.NewGuid()).Take(count).ToList();

        }

        //search album

        public List<Album> searchAlbum(string name)
        {
            name = SomeFunction.ConvertToUnSign(name);
            return db.Albums.Where(delegate (Album album)
            {
                if (album.Status == true && album.Songs.Count>0)
                {
                    if (SomeFunction.ConvertToUnSign(album.Name).Contains(name))
                        return true;
                    foreach (var item in album.Artists)
                    {
                        if (SomeFunction.ConvertToUnSign(item.Name).Contains(name))
                            return true;
                    }
                }
                
                return false;
            }).ToList();
        }

        public List<int> getListSongIDByAlbum(int id)
        {
            List<int> listResult = new List<int>();
            Album album = db.Albums.Where(abl => abl.ID == id && abl.Status == true).FirstOrDefault();
            if (album == null)
                return null;
            foreach (var item in album.Songs)
            {
                if(item.Status==true)
                    listResult.Add(item.ID);
            }

            return listResult;
        }

        public bool AddArtistToAlbum(Album album, List<int> listArtistID)
        {
            try
            {
                Album al = db.Albums.Find(album.ID);
                db.Albums.Attach(al);

                foreach (var item in listArtistID)
                {
                    //artist
                    Artist ar = db.Artists.Find(item);
                    db.Artists.Attach(ar);


                    //gắn relation ship giữa song và artist
                    al.Artists.Add(ar);
                }

                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool RemoveArtistToAlbum(Album album)
        {
            try
            {
                Album al = db.Albums.Find(album.ID);
                al.Artists.Clear();
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
            
        }
    }
}