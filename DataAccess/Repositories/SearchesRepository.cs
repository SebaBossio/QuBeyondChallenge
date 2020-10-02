using DBEntities;
using DBEntities.Entities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public interface ISearchesRepository : IGenericRepository<Searches>
    {
        Task<List<Searches>> GetTop(int top = 10);
    }

    public class SearchesRepository : GenericRepository<Searches>, ISearchesRepository
    {
        public SearchesRepository(WordFinderContext context) : base(context) { }

        public async Task<List<Searches>> GetTop(int top = 10)
        {
            return await this._context.Searches.OrderBy(x => x.CTS).Take(top).ToListAsync();
        }
    }
}
