class Restaurant

{
    public void CookMasala(ICooker cooker, Country country, DateTime currentTime)

    {
        IRecipeFactory factory = (currentTime.Month > 5 && currentTime.Month < 9) ?
            new SummerMasalaRecipeFactory() :
            new MasalaRecipeFactory();
        var recipe = factory.CreateRecipe(country);
        recipe.Cook(cooker);
    }

}
