public class BarclaysFilter : IFilter
{
    public IEnumerable<Trade> Match(IEnumerable<Trade> trades)
    {
        return trades.Where(t => t.Type == DealType.Option && t.SubType == DealSubTupe.NYOption && t.Amount > 50);
    }
}
