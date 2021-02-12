using System.Threading.Tasks;

namespace AuthServer.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
        void SaveChanges();
    }
}
