public interface IBankFilterFactory
{
    public IFilter CreateBankFilter(Bank bank, Country country);
}