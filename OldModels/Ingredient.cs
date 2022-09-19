using System;
using System.Collections.Generic;

#nullable disable

namespace Karya.Ege.Nutrition.API.Models
{
    public partial class Ingredient
    {
        public string FoodName { get; set; }
        public string IngredientName { get; set; }
        public decimal QuantityGr { get; set; }
        public string Unit { get; set; }
        public decimal? GrToUnitConversion { get; set; }
    }
}
