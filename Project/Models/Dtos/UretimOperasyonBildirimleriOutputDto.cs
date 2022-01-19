using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.Dtos
{
    public class UretimOperasyonBildirimleriOutputDto
    {
        public int IsNo { get; set; }
        public DateTime Baslangic { get; set; }
        public DateTime Bitis { get; set; }
        public TimeSpan Sure { get; set; }
        public string Status { get; set; }
        public string DurusNedeni { get; set; }
    }
}