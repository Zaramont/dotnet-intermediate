public class UkrainianRecipe : IRecipe
{
    public void Cook(ICooker cooker)
    {
        cooker.FryRice(500, Level.Strong);
        cooker.SaltRice(Level.Strong);
        cooker.PepperRice(Level.Low);

        cooker.FryChicken(300, Level.Medium);
        cooker.SaltChicken(Level.Medium);
        cooker.PepperChicken(Level.Low);
    }
}
