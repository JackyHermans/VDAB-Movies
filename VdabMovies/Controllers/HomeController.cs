using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VdabMovies.Models;
using VdabMovies.Services;

namespace VdabMovies.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string Naam = Request["naam"];
            string Postcode = Request["postcode"];
            int pc;
            if (Naam == null || !(int.TryParse(Postcode, out pc)))
            {
                Session.Clear();
            }
            else
            {
                var klant = VerhuurService.FindKlant(Naam, pc);
                if (klant != null)
                {
                    Session["klant"] = klant;
                }
                else
                {
                    ViewBag.error = true;
                }
            }
            return View();
        }
        
        public ActionResult Verhuren (int? id)
        {
            if (Session["klant"] == null)
            {
                return RedirectToAction("Index");
            }
            if (id != null)
            {
                ViewBag.genre = (VerhuurService.GetGenre((int)id)).GenreSoort;
                ViewBag.genreid = id;
            }
            return View();
        }

        public PartialViewResult GenreLijst()
        {
            return PartialView(VerhuurService.GetGenres());
        }

        public PartialViewResult FilmsPerGenre(int GenreId)
        {
            return PartialView(VerhuurService.GetFilmsPerGenre(GenreId));
        }

        public ActionResult WinkelMandje (int? id)
        {
            if (Session["klant"] == null)
            {
                return RedirectToAction("Index");
            }
            List<Film> films = new List<Film>();
            if (Session["films"] != null)
            {
                films = (List<Film>)Session["films"];
            }
            if (id != null)
            {
                var film = VerhuurService.GetFilm((int)id);
                if (!(films.Contains(film)))
                {
                    films.Add(film);
                }
            }
            Session["films"] = films;
            return View(films);
        }

        public ActionResult Verwijder(int id)
        {
            if (Session["klant"] == null)
            {
                return RedirectToAction("Index");
            }
            var film = VerhuurService.GetFilm(id);
            ViewBag.filmId = film.BandNr;
            ViewBag.filmTitel = film.Titel;
            return View();
        }

        public ActionResult Verwijderen (int id)
        {
            if (Session["klant"] == null)
            {
                return RedirectToAction("Index");
            }
            List<Film> films = new List<Film>();
            if (Session["films"] != null)
            {
                films = (List<Film>)Session["films"];
                var film = films.FirstOrDefault(f => f.BandNr == id);
                films.Remove(film);
            }
            Session["films"] = films;
            return RedirectToAction("Winkelmandje");
        }

        public ActionResult Afrekenen()
        {
            if (Session ["klant"] == null)
            {
                return RedirectToAction("Index");
            }
            List<Film> films = new List<Film>();
            Klant klant = (Klant)Session["klant"];
            if (Session["films"] != null)
            {
                films = (List<Film>)Session["films"];
            }
            List<Verhuur> verhuringen = new List<Verhuur>();
            decimal Totaal = 0m;
            foreach (var film in films)
            {
                Verhuur verhuur = new Models.Verhuur();
                verhuur.BandNr = film.BandNr;
                verhuur.KlantNr = klant.KlantNr;
                verhuur.VerhuurDatum = DateTime.Now;
                Totaal += film.Prijs;
                verhuringen.Add(verhuur);
            }
            List<Verhuur> mislukt = VerhuurService.HuurFilms(verhuringen);
            if (mislukt.Count != 0)
            {
                foreach (var verhuur in mislukt)
                {
                    verhuringen.Remove(verhuur);
                    Totaal -= verhuur.Films.Prijs;
                }
            }

            Afrekening kassa = new Models.Afrekening();
            kassa.verhuringen = verhuringen;
            kassa.Totaal = Totaal;
            kassa.klant = klant;
            Session.Clear();
            return View(kassa);
        }
    }
}