public class IndianRecipe : IRecipe
{
    public void Cook(ICooker cooker)
    {
        cooker.FryRice(200, Level.Strong);
        cooker.SaltRice(Level.Strong);
        cooker.PepperRice(Level.Strong);

        cooker.FryChicken(100, Level.Strong);
        cooker.SaltChicken(Level.Strong);
        cooker.PepperChicken(Level.Strong);
    }
}
