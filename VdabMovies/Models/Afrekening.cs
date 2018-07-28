using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VdabMovies.Models
{
    public class Afrekening
    {
        public Klant klant { get; set; }
        //[DisplayFormat(DataFormatString ="{0:€ #,##0.00")]
        public decimal Totaal { get; set; }
        public List<Verhuur> verhuringen { get; set; }
    }
}