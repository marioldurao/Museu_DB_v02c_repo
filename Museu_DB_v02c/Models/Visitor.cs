using System;
using System.ComponentModel.DataAnnotations;
//sadfvdfv
namespace Museu_DB_v02c.Models
{
    public class Visitor
    {

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int ID { get; set; }

        [Required]
        [Range(1, 120)]
        public int Age_Group { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Nationality { get; set; }

        [StringLength(6, MinimumLength = 4)]
        [Required]
        public string Gender { get; set; }
    }
}
