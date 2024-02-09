
namespace BookFinder.Adaptors
{
    public class BookAdaptor : DataAdaptor
    {
        private readonly IBookService BookService;
        public BookAdaptor(IBookService BookService)
        {
            this.BookService = BookService;
        }

        public override async Task<object> ReadAsync(DataManagerRequest dataManagerRequest, string additionalParam = null)
        {
            FilterViewModel filteringViewModel = new();

            List<WhereFilter> wheres = dataManagerRequest.Where ?? new List<WhereFilter>();

            if (wheres.Any())
            {
                var perdicates = wheres.FirstOrDefault().predicates;
                filteringViewModel.Where = perdicates;
            }

            if (dataManagerRequest.Sorted is not null)
            {
                filteringViewModel.SortBy = dataManagerRequest.Sorted[0].Name;
                filteringViewModel.SortDirection = dataManagerRequest.Sorted[0].Direction;
            }
            filteringViewModel.Skip = dataManagerRequest.Skip;
            filteringViewModel.Take = dataManagerRequest.Take;

            var result = await BookService.GetAllAsync(filteringViewModel);


            DataResult dataResult = new DataResult
            {
                Count = result.Count,
                Result = result.Result
            };

            return dataResult;
        }
    }
}
