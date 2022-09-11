namespace ManagingState1
{
    public class DeutscheFilter : IFilter
    {
        public IEnumerable<Trade> Match(IEnumerable<Trade> trades)
        {
            return trades.Where(t => t.Type == DealType.Option && t.SubType == DealSubTupe.NewOption && t.Amount > 90 && t.Amount < 120);
        }
    }
}