using System.Threading.Tasks;
using FieldWorkerBot.Domain;

namespace FieldWorkerBot.DataAccess.Interfaces
{
    public interface IAssetRepository
    {
        Task<Asset> GetByName(string name);
    }
}