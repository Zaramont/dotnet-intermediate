public class BofaFilter : IFilter
{
    public IEnumerable<Trade> Match(IEnumerable<Trade> trades)
    {
        return trades.Where(t => t.Amount > 70);
    }
}
