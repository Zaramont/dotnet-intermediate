using ManagingState1;

public class BankFilterFactory: IBankFilterFactory
{
    public IFilter CreateBankFilter(Bank bank, Country country)
    {
        return bank switch
        {
            Bank.Bofa => new BofaFilter(),
            Bank.Barclays => new BarclaysFilter(country),
            Bank.Connacord => new ConnacordFilter(),
            Bank.Deutsche => new DeutscheFilter(),
            _ => throw new ArgumentException($"Bank '{bank}' is not supported")
        };
    }
}