namespace RestoranSystem.Model
{
    public class Meal
    {
        public Enum MealType;
        public string? MealName;
        public decimal MealPrice;

        public Meal(Enum mealType, string mealName, decimal mealPrice)
        {
            MealType = mealType;
            MealName = mealName;
            MealPrice = mealPrice;
        }
    }
}
