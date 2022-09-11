internal class SummerMasalaRecipeFactory : IRecipeFactory
{

    public IRecipe GetEnglishRecipe() => new EnglishSummerRecipe();

    public IRecipe GetIndianRecipe() => new IndianSummerRecipe();

    public IRecipe GetUkrainianRecipe() => new UkrainianSummerRecipe();

    public IRecipe CreateRecipe(Country country)
    {
        var recipe = country switch
        {
            Country.Ukraine => this.GetUkrainianRecipe(),
            Country.England => this.GetEnglishRecipe(),
            Country.India => this.GetIndianRecipe(),
            _ => throw new ArgumentException("Recipe for this country isn't supported", nameof(country))
        };

        return recipe;
    }
}