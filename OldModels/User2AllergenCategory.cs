using System;
using System.Collections.Generic;

#nullable disable

namespace Karya.Ege.Nutrition.API.Models
{
    public partial class User2AllergenCategory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AllergenCategory { get; set; }
        public string Type { get; set; }

        public virtual User User { get; set; }
    }
}
