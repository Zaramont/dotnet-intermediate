public class BankFilterFactory
{
    public IFilter CreateBankFilter(Bank bank)
    {
        return bank switch
        {
            Bank.Bofa => new BofaFilter(),
            Bank.Barclays => new BarclaysFilter(),
            Bank.Connacord => new ConnacordFilter(),
            _ => throw new ArgumentException($"Bank '{bank}' is not supported")
        };
    }
}