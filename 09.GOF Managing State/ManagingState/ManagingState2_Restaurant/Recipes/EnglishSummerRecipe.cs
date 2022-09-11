public class EnglishSummerRecipe : IRecipe
{
    public void Cook(ICooker cooker)
    {
        cooker.FryRice(50, Level.Low);
        cooker.FryChicken(50, Level.Low);
    }
}
