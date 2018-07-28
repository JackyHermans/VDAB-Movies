using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VdabMovies.Models
{
    public class FilmProperties
    {
        //[DisplayFormat(DataFormatString = "{0:€ #,##0.00")]
        public decimal Prijs { get; set; }
    }
}