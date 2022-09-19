using System;
using System.Collections.Generic;

#nullable disable

namespace Karya.Ege.Nutrition.API.Models
{
    public partial class Allergen
    {
        public string Category { get; set; }
        public string Type { get; set; }
        public string IngredientName { get; set; }
    }
}
