using ManagingState1;

public class BankFilterFactory
{
    public IFilter CreateBankFilter(Bank bank)
    {
        return bank switch
        {
            Bank.Bofa => new BofaFilter(),
            Bank.Barclays => new BarclaysFilter(),
            Bank.Connacord => new ConnacordFilter(),
            Bank.Deutsche => new DeutscheFilter(),
            _ => throw new ArgumentException($"Bank '{bank}' is not supported")
        };
    }
}