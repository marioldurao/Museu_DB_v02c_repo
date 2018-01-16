using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Museu_DB_v02c.Models
{
    public class VisitorStatsViewModel
    {
        public List<Visitor> Visitors { get; set; }
        public SelectList GendersList { get; set; }
        public SelectList NationalityList { get; set; }
        public SelectList AgeList { get; set; }
        public SelectList DateList { get; set; }
        public string VisitorGender { get; set; }
        public string VisitorNationality { get; set; }
        public string VisitorAge { get; set; }
        public string VisitorDate { get; set; }
        public int MaleNum { get; set; }
        public int FemaleNum { get; set; }
        public int[] GenderInts { get; set; }     //counter for each gender in array
        public int[] NationalityInts { get; set; } //counter for each nationality in array
        public int[] AgeInts { get; set; }         //counter for each Age in array
        public int[] DateInts { get; set; }         //counter for each Age in array
    }
}
