using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using VdabMovies.Models;

namespace VdabMovies.Services
{
    public static class VerhuurService
    {
        public static List<Klant> GetKlanten()
        {
            using (var db = new VideoVerhuurEntities1())
            {
                return db.Klanten.ToList();
            }
        }

        public static Klant FindKlant (string naam, int postcode)
        {
            using (var db = new VideoVerhuurEntities1())
            {
                var klant = db.Klanten.FirstOrDefault(k => k.Naam == naam && k.PostCode == postcode);
                return klant;
            }
        }

        public static List<Genre> GetGenres()
        {
            using (var db = new VideoVerhuurEntities1())
            {
                return db.Genres.ToList();
            }
        }

        public static Genre GetGenre (int id)
        {
            using (var db = new VideoVerhuurEntities1())
            {
                var genre = db.Genres.FirstOrDefault(g => g.GenreNr == id);
                return genre;
            }
        }

        public static Film GetFilm(int id)
        {
            using (var db = new VideoVerhuurEntities1())
            {
                var film = db.Films.FirstOrDefault(f => f.BandNr == id);
                return film;
            }
        }

        public static List<Film> GetFilmsPerGenre (int genreId)
        {
            using (var db = new VideoVerhuurEntities1())
            {
                var films = (from film in db.Films
                             where film.GenreNr == genreId
                             select film).ToList();
                return films;
            }
        }

        public static List<Verhuur> HuurFilms(List<Verhuur> verhuringen)
        {
            List<Verhuur> Mislukt = new List<Verhuur>();
            using (var db = new VideoVerhuurEntities1())
            {
                foreach (var verhuur in verhuringen)
                {
                    var film = db.Films.FirstOrDefault(f => f.BandNr == verhuur.BandNr);
                    if (film.InVoorraad > 0)
                    {
                        db.Verhuur.Add(verhuur);
                        film.VerhuurFilm();
                    }
                    else
                    {
                        Mislukt.Add(verhuur);
                    }
                }
                db.SaveChanges();
            }
            return Mislukt;
        }
    }
}