var trades = new List<Trade>
        {
            new Trade(1,DealType.Future,DealSubTupe.NYOption,10),
            new Trade(2,DealType.Option,DealSubTupe.NYOption,10),
            new Trade(3,DealType.Future,DealSubTupe.NYOption,20),
            new Trade(4,DealType.Option,DealSubTupe.NYOption,20),
            new Trade(5,DealType.Future,DealSubTupe.NYOption,40),
            new Trade(6,DealType.Option,DealSubTupe.Other,40),
            new Trade(7,DealType.Future,DealSubTupe.Other,60),
            new Trade(8,DealType.Option,DealSubTupe.Other,60),
            new Trade(9,DealType.Future,DealSubTupe.Other,100),
            new Trade(10,DealType.Option,DealSubTupe.NewOption,100),
            new Trade(11,DealType.Future,DealSubTupe.NYOption,140),
            new Trade(12,DealType.Option,DealSubTupe.NYOption,140),
        };

var tradeFilter = new TradeFilter();
var bofaTrades = tradeFilter.FilterForBank(trades, Bank.Bofa);
var barclaysTrades = tradeFilter.FilterForBank(trades, Bank.Barclays);
var connacordTrades = tradeFilter.FilterForBank(trades, Bank.Connacord);
PrintTrades(bofaTrades);
PrintTrades(barclaysTrades);
PrintTrades(connacordTrades);

static void PrintTrades(IEnumerable<Trade> trades)
{
    foreach (var item in trades)
    {
        Console.WriteLine(item);
    }
    Console.WriteLine();
}

public interface IFilter

{
    IEnumerable<Trade> Match(IEnumerable<Trade> trades);

}

public class TradeFilter
{
    public IEnumerable<Trade> FilterForBank(IEnumerable<Trade> trades, Bank bank)
    {
        var filter = new BankFilterFactory().CreateBankFilter(bank);
        return filter.Match(trades);
    }
}

public enum Bank
{
    Bofa,
    Connacord,
    Barclays
}

public record Trade(int Id, DealType Type, DealSubTupe SubType, int Amount);

public enum DealType
{
    Option,
    Future
}
public enum DealSubTupe
{
    NYOption,
    NewOption,
    Other
}
