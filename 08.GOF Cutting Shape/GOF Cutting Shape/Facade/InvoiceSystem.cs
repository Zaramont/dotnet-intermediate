public class InvoiceSystem : IInvoiceSystem
{
    public void SendInvoice(Invoice invoice)
    {
        string message = invoice.ToString();
        Console.WriteLine(message);
    }
}

public interface IInvoiceSystem
{
    void SendInvoice(Invoice invoice);
}

public record Invoice(string productName, int quantity, string email, double price, string orderId);
