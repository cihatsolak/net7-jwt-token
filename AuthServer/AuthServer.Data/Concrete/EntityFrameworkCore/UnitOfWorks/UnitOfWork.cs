using AuthServer.Core.UnitOfWorks;
using AuthServer.Data.Concrete.EntityFrameworkCore.Contexts;
using System.Threading.Tasks;

namespace AuthServer.Data.Concrete.EntityFrameworkCore.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
