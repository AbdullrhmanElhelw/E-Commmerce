namespace E_Commmerce.Helper
{
    public class PageList<T> :List<T> where T : class
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }

        public PageList(List<T>list,int count,int pageIndex,int pageSize )
        {
            this.PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(list);
        }

        public bool HasPrevoiusPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static PageList<T> Create(IEnumerable<T>list,int pageIndex,int pageSize)
        {
            var count = list.Count();
            var items = list.Skip((pageIndex - 1 ) * pageSize ).Take(pageSize).ToList();
            return new PageList<T>(items,count,pageIndex,pageSize);
        }


    }
}
