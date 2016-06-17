using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaBu.Models
{
    public enum ItemType { Condiment, Spices, Garnish, Ingredients, CookingMethod };
    public enum ServeMethod { OnTheSide, OnTop, MixedIn, Platform };
    public enum AddingTime { OnServing, OnTable, OnPrep }
    public enum PreparationType { Chopped, Minced, Sliced, Filleted, Cubed, Julienned, Whole }

    public class FoodComponent
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ItemType ItemType { get; set; }

    }

    public class Condiments : FoodComponent
    {
        public ServeMethod ServingStyle { get; set; }
    }

    public class CookingMethod : FoodComponent
    {
        public int CookingTime { get; set; }
        public int MaxCookingTime { get; set; }
    }

    public class Garnish : FoodComponent
    {
        public ServeMethod ServingStyle { get; set; }
        public AddingTime AddTime { get; set; }
    }

    public class Ingredients : FoodComponent
    {
        public PreparationType preparationType { get; set; }
    }

    public class Spices : FoodComponent
    {
        public bool toTaste { get; set; }
    }

    public class Recipe 
    {
        public Recipe()
        {

        }

        public List<Condiments> CondimentList { get; set; }
        public List<Garnish> GarnishList { get; set; }
        public List<Ingredients> IngredientsList { get; set; }
        public List<Spices> SpicesList { get; set; }
        public List<CookingMethod> CookingMethodList { get; set; }


    }
}
