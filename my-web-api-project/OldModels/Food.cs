using System;
using System.Collections.Generic;

#nullable disable

namespace Karya.Ege.Nutrition.API.Models
{
    public partial class Food
    {
        public string Name { get; set; }
        public string MainCategory { get; set; }
        public string SubCategory { get; set; }
        public decimal TotalCalories { get; set; }
        public decimal TotalProtein { get; set; }
        public decimal TotalCarb { get; set; }
        public decimal TotalTryptophan { get; set; }
        public decimal TotalVitaminA { get; set; }
        public int TimeMinutes { get; set; }
    }
}
