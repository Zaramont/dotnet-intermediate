internal class MasalaRecipeFactory : IRecipeFactory
{

    public IRecipe GetEnglishRecipe() => new EnglishRecipe();

    public IRecipe GetIndianRecipe() => new IndianRecipe();

    public IRecipe GetUkrainianRecipe() => new UkrainianRecipe();

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