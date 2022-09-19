using System;
using System.Collections.Generic;

#nullable disable

namespace Karya.Ege.Nutrition.API.Models
{
    public partial class UserAllergen
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AllergenName { get; set; }
    }
}
