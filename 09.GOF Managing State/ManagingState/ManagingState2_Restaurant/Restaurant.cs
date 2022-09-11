class Restaurant

{
    private readonly IRecipeFactory _factory;

    public Restaurant(IRecipeFactory factory)
    {
        this._factory = factory;
    }

    public void CookMasala(ICooker cooker, Country country)

    {
        var recipe = this._factory.CreateRecipe(country);
        recipe.Cook(cooker);
    }

}
