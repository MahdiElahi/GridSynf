
namespace Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext db;
        protected IQueryable<Book>? query = null;

        public BookRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<FilterResult<Book>> GetAllAsync(FilterViewModel filterViewModel)
        {
            try
            {

                query = db.Set<Book>();

                if (filterViewModel.Where is not null) Filtering(filterViewModel);
                if (filterViewModel.SortBy is not null) Sorting(filterViewModel);

                var result = await query.Skip(filterViewModel.Skip).Take(filterViewModel.Take).Include(s => s.Country).AsNoTracking().Select(x => new Book
                {
                    RegisterNumber = x.RegisterNumber,
                    Country = x.Country,
                    Status = x.Status,
                    Id = x.Id,
                    Author = x.Author
                }).ToListAsync();


                return new FilterResult<Book>
                {
                    Result = result,
                    Count = await query.CountAsync()
                };
            }
            catch (Exception ex)
            {

                throw;
            }

        }
     
        public async Task<Book> UpdateAsync(Book Book)
        {
            try
            {
                Book.Country = null;
                var result = db.Books.Update(Book);
                await db.SaveChangesAsync();
                return result.Entity;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    

        void Filtering(FilterViewModel filterViewModel)
        {
            if (filterViewModel.Where is not null)
            {
                foreach (var item in filterViewModel.Where)
                {
                    switch (item.Field)
                    {
                        case nameof(Book.RegisterNumber):
                            query = query.Where(x => x.RegisterNumber.Contains(Convert.ToString(item.value)));
                            break;

                        case nameof(Book.Author):
                            query = query.Where(x => x.Author.Contains(Convert.ToString(item.value)));
                            break;

                        case nameof(Book.Date):
                            query = query.Where(x => x.Date.Contains(Convert.ToString(item.value)));
                            break;

                        case nameof(Book.Status):
                            query = query.Where(x => x.Status.Contains(Convert.ToString(item.value)));
                            break;

                        case nameof(Book.Country.Title):
                            query = query.Where(x => x.Country.Title.Contains(Convert.ToString(item.value)) || x.Country.ThreeWord.Contains(Convert.ToString(item.value)));
                            break;

                        default:
                            break;
                    }
                }
            }
        }
        void Sorting(FilterViewModel filterViewModel)
        {
            if (filterViewModel.SortBy is not null)
            {
                switch (filterViewModel.SortBy)
                {
                    case nameof(Book.RegisterNumber):
                        query = filterViewModel.SortDirection is "ascending" ? query = query.OrderBy(s => s.RegisterNumber) : query.OrderByDescending(s => s.RegisterNumber);
                        break;

                    case nameof(Book.Author):
                        query = filterViewModel.SortDirection is "ascending" ? query = query.OrderBy(s => s.Author) : query.OrderByDescending(s => s.Author);
                        break;

                    case nameof(Book.Date):
                        query = filterViewModel.SortDirection is "ascending" ? query = query.OrderBy(s => s.Date) : query.OrderByDescending(s => s.Date);
                        break;

                    case nameof(Book.Status):
                        query = filterViewModel.SortDirection is "ascending" ? query = query.OrderBy(s => s.Status) : query.OrderByDescending(s => s.Status);

                        break;

                    case nameof(Book.Country.Title):
                        query = filterViewModel.SortDirection is "ascending" ? query = query.OrderBy(s => s.Country.Title) : query.OrderByDescending(s => s.Country.Title);
                        break;

                    default:
                        break;
                }
            }
        }

     
        public async Task<List<string>> GetStatus()
        {
            return await db.Books.Select(x => x.Status).Distinct().AsNoTracking().ToListAsync();
        }
    }
}
