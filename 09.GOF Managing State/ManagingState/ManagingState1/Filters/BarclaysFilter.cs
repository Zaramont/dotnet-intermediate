public class BarclaysFilter : IFilter
{
    private IFilter regionFilter;

    public BarclaysFilter(Country country)
    {
        regionFilter = country switch {
        Country.England => new BarclaysEnglandFilter(),
        Country.USA => new BarclaysUSAFilter(),
        _ => throw new ArgumentException($"There is no such bank in the '{country}'"),
        };
    }

    public virtual IEnumerable<Trade> Match(IEnumerable<Trade> trades)
    {
        return regionFilter.Match(trades);
    }
}
