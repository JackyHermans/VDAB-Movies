using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VdabMovies.Models
{
    [MetadataType(typeof(FilmProperties))]
    public partial class Film
    {
        public void VerhuurFilm()
        {
            if (this.InVoorraad > 0)
            {
                this.InVoorraad--;
                this.UitVoorraad++;
            }
            else
            {
                throw new Exception("Deze film is momenteel niet voorradig");
            }
        }
    }
}