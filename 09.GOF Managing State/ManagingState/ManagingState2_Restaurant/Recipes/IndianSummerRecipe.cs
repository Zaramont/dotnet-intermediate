public class IndianSummerRecipe : IRecipe
{
    public void Cook(ICooker cooker)
    {
        cooker.FryRice(100, Level.Low);
        cooker.PepperRice(Level.Medium);

        cooker.FryChicken(100, Level.Low);
        cooker.PepperChicken(Level.Medium);
    }
}
