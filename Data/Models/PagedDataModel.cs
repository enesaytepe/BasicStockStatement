namespace Data.Models
{
    public class PagedDataModel<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public List<T> DataList { get; set; }
    }
}
