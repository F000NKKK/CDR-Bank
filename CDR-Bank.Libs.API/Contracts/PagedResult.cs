namespace CDR_Bank.Libs.API.Contracts
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
    }

}
