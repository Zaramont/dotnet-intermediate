public class PaymentSystem : IPaymentSystem
{
    public bool MakePayment(Payment payment)
    {
        return true;
    }
}

public interface IPaymentSystem
{
    bool MakePayment(Payment payment);
}

public record Payment(string orderId, double price);