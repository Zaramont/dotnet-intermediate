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
var barclaysTrades = tradeFilter.FilterForBank(trades, Bank.Barclays, Country.USA);
var connacordTrades = tradeFilter.FilterForBank(trades, Bank.Connacord);
var deutscheTrades = tradeFilter.FilterForBank(trades, Bank.Deutsche);
var barclaysEnglandTrades = tradeFilter.FilterForBank(trades, Bank.Barclays, Country.England);
PrintTrades(bofaTrades);
PrintTrades(barclaysTrades);
PrintTrades(connacordTrades);
PrintTrades(deutscheTrades);
PrintTrades(barclaysEnglandTrades);

static void PrintTrades(IEnumerable<Trade> trades)
{
    foreach (var item in trades)
    {
        Console.WriteLine(item);
    }
    Console.WriteLine();
}

public class TradeFilter
{
    public IEnumerable<Trade> FilterForBank(IEnumerable<Trade> trades, Bank bank, Country country = Country.USA)
    {
        var filter = new BankFilterFactory().CreateBankFilter(bank, country);
        return filter.Match(trades);
    }
}

public record Trade(int Id, DealType Type, DealSubTupe SubType, int Amount);
