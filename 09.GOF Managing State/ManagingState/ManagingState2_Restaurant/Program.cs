var restaurant = new Restaurant();
var cooker = new Cooker();

restaurant.CookMasala(cooker, Country.India, new DateTime(2022, 6, 1));
Console.WriteLine();
restaurant.CookMasala(cooker, Country.India, new DateTime(2022, 1, 1));
