
namespace Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository BookRepository;

        public BookService(IBookRepository BookRepository)
        {
            this.BookRepository = BookRepository;
        }
  
        public async Task<FilterResult<BookViewModel>> GetAllAsync(FilterViewModel filterViewModel)
        {
            var mapped = ObjectMapper.Mapper.Map<FilterResult<BookViewModel>>(await BookRepository.GetAllAsync(filterViewModel));
            return  mapped;
        }

        public async Task<List<string>> GetStatus()
        {
            return await BookRepository.GetStatus();
        }

        public async Task UpdateAsync(BookViewModel viewModel)
        {
            var mapped = ObjectMapper.Mapper.Map<Book>(viewModel);
            await BookRepository.UpdateAsync(mapped);
        }
      
    }
}
