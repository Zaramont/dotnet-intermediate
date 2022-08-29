var orderService = new OrderService(
    new ProductCatalog(), new PaymentSystem(), new InvoiceSystem());

orderService.PlaceOrder("111", 100, "test@gmail.com");


public interface IOrderService
{
    void PlaceOrder(string productId, int quantity, string email);
}
public class OrderService : IOrderService
{
    private readonly IProductCatalog productCatalog;
    private readonly IPaymentSystem paymentSystem;
    private readonly IInvoiceSystem invoiceSystem;

    public OrderService(IProductCatalog productCatalog, IPaymentSystem paymentSystem, IInvoiceSystem invoiceSystem)
    {
        this.productCatalog = productCatalog ?? throw new ArgumentNullException(nameof(productCatalog));
        this.paymentSystem = paymentSystem ?? throw new ArgumentNullException(nameof(paymentSystem));
        this.invoiceSystem = invoiceSystem ?? throw new ArgumentNullException(nameof(invoiceSystem));
    }

    public void PlaceOrder(string productId, int quantity, string email)
    {
        var product = this.productCatalog.GetProductDetails(productId);
        double totalPrice = product.price * quantity;
        string orderId = "333";
        
        bool paymentResult = this.paymentSystem.MakePayment(new Payment(orderId, totalPrice));
        if (paymentResult)
        {
            this.invoiceSystem.SendInvoice(new Invoice(product.name, quantity, email, totalPrice, orderId));
        }
    }
}
