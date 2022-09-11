public interface IRecipeFactory
{
    IRecipe GetEnglishRecipe();
    IRecipe GetIndianRecipe();
    IRecipe GetUkrainianRecipe();
    IRecipe CreateRecipe(Country country);
}