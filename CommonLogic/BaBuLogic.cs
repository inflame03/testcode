using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using BaBu.Models;
using BaBu.DataAccess;

namespace BaBu.Logic
{
    public class BaBuLogic
    {
        Random ran = new Random(DateTime.Now.Minute + DateTime.Now.Millisecond);

        public BaBuLogic()
        {

        }

        public FoodComponent[] GetItemList(ItemType type)
        {
            FoodComponent[] result = null;

            BaBuDataAccess bda = new BaBuDataAccess();

            result = bda.GetAllItems(type).ToArray<FoodComponent>();

            return result;
        }

        public FoodComponent GetItem(int id, ItemType type)
        {
            FoodComponent result = null;

            BaBuDataAccess bda = new BaBuDataAccess();

            result = bda.GetItem(id, type);

            return result;
        }

        public FoodComponent SaveItem(FoodComponent item, ItemType type)
        {
            FoodComponent result = null;

            BaBuDataAccess bda = new BaBuDataAccess();

            result = bda.SaveItem(item, type);

            return result;
        }

        public bool DeleteUser(int id, ItemType type)
        {

            BaBuDataAccess uda = new BaBuDataAccess();

            return uda.DeleteItem(id, type);

        }

        public Recipe GetRandomRecipe()
        {
            Recipe result = new Recipe();

            BaBuDataAccess bda = new BaBuDataAccess();

            IList tempList = null;

            tempList = this.GetRandomContents(bda.GetAllItemsByType(ItemType.Condiment));
            result.CondimentList = tempList.OfType<Condiments>().ToList<Condiments>();
            this.RandomizeCondiments(result.CondimentList);

            tempList = this.GetRandomContents(bda.GetAllItemsByType(ItemType.Spices));
            result.SpicesList = tempList.OfType<Spices>().ToList<Spices>();

            tempList = this.GetRandomContents(bda.GetAllItemsByType(ItemType.Garnish));
            result.GarnishList = tempList.OfType<Garnish>().ToList<Garnish>();
            this.RandomizeGarnish(result.GarnishList);

            tempList = this.GetRandomContents(bda.GetAllItemsByType(ItemType.Ingredients));
            result.IngredientsList = tempList.OfType<Ingredients>().ToList<Ingredients>();
            this.RandomizeIngredients(result.IngredientsList);

            tempList = this.GetRandomContents(bda.GetAllItemsByType(ItemType.CookingMethod));
            result.CookingMethodList = tempList.OfType<CookingMethod>().ToList<CookingMethod>();




            return result;

        }

        private void RandomizeGarnish(List<Garnish> list)
        {
            Array serveMethod = Enum.GetValues(typeof(ServeMethod));
            Array addTime = Enum.GetValues(typeof(AddingTime));

            foreach (Garnish item in list)
            {
                item.ServingStyle = (ServeMethod)serveMethod.GetValue(this.ran.Next(serveMethod.Length));
                item.AddTime = (AddingTime)addTime.GetValue(this.ran.Next(addTime.Length));
            }

        }

        private void RandomizeCondiments(List<Condiments> list)
        {
            Array values = Enum.GetValues(typeof(ServeMethod));

            foreach (Condiments item in list)
            {
                item.ServingStyle = (ServeMethod)values.GetValue(this.ran.Next(values.Length));
            }

        }

        private void RandomizeIngredients(List<Ingredients> list)
        {
            Array values = Enum.GetValues(typeof(PreparationType));

            foreach ( Ingredients item in list )
            {
                item.preparationType = (PreparationType)values.GetValue(this.ran.Next(values.Length));
            }
        }

        private IList GetRandomContents( IList foodList )
        {
            int end;
            

            IList result = new ArrayList();


            foreach( object food in foodList )
            {
                end = this.ran.Next() % 2;

                // always at least return 1 item
                result.Add(food);

                if (end == 0)
                    break;
            }

            return result;
        }
    }
}
