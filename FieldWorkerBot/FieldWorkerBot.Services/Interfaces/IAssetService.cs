using System.Threading.Tasks;
using FieldWorkerBot.Domain;

namespace FieldWorkerBot.Services.Interfaces
{
    public interface IAssetService
    {
        Task<Asset> GetByName(string name);
    }
}