using System.Threading.Tasks;
using FieldWorkerBot.DataAccess.Interfaces;
using FieldWorkerBot.Domain;
using FieldWorkerBot.Services.Interfaces;

namespace FieldWorkerBot.Services
{
    public class AssetService : IAssetService
    {
        private IAssetRepository _assetRepository;

        public AssetService(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public async Task<Asset> GetByName(string name)
        {
            return await _assetRepository.GetByName(name);
        }
    }
}
