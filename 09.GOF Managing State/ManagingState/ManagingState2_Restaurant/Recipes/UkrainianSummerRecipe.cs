public class UkrainianSummerRecipe : IRecipe
{
    public void Cook(ICooker cooker)
    {
        cooker.FryRice(150, Level.Medium);
        cooker.SaltRice(Level.Low);

        cooker.FryChicken(200, Level.Medium);
        cooker.SaltChicken(Level.Low);    }
}
