public class ProductCatalog : IProductCatalog
{
    public Product GetProductDetails(string productId)
    {
        return new Product("111", "Name", 200.22);
    }
}

public interface IProductCatalog
{
    Product GetProductDetails(string productId);
}

public record Product(string id, string name, double price);