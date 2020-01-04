using DoAn_CNPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn_CNPM.DAL
{
    public class GenreDAL
    {
        QuanLyNhac db;

        public GenreDAL()
        {
            db = new QuanLyNhac();
        }

        public static GenreDAL genreDAL;
        public static GenreDAL createGenreDAL()
        {
            if(genreDAL == null)
            {
                genreDAL = new GenreDAL();
            }
            return genreDAL;
        }

        public List<Genre> getAllGenre()
        {
            return db.Genres.Where(g => g.Status == true).ToList();
        }
        public Genre getGenreByID(int id)
        {
            return db.Genres.Where(g => g.Status == true && g.ID == id).FirstOrDefault();
        }

        public List<Genre> getAllGenreAdmin()
        {
            return db.Genres.ToList();
        }

        public Genre getGenreByIDAdmin(int id)
        {
            return db.Genres.Find(id);
        }

        public bool InsertGenre(Genre genre)
        {
            try
            {
                genre.Status = true;
                db.Genres.Add(genre);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateGenre(Genre genre)
        {
            try
            {
                Genre ct = db.Genres.Find(genre.ID);
                db.Entry(ct).CurrentValues.SetValues(genre);
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