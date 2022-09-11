internal class BarclaysEnglandFilter : IFilter
{
    public IEnumerable<Trade> Match(IEnumerable<Trade> trades)
    {
        return trades.Where(t => t.Type == DealType.Future && t.Amount > 100);
    }
}