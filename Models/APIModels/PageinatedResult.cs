namespace XtramileBackend.Models.APIModels
{
    public class PageinatedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
