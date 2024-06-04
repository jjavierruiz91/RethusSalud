namespace rethus_backend.Utilities.Constants.PaginatioConstants
{
    public class PaginationResultDto<TEntity>
    {
        public IEnumerable<TEntity> Data { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }

    public class PaginationRequestDto<TQueryParameters>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public TQueryParameters QueryParameters { get; set; }
    }
}
