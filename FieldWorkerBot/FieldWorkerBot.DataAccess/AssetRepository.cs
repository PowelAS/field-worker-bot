using System.Threading.Tasks;
using FieldWorkerBot.DataAccess.Interfaces;
using FieldWorkerBot.Domain;

namespace FieldWorkerBot.DataAccess
{
    public class AssetRepository : IAssetRepository
    {
        public Task<Asset> GetByName(string name)
        {
            return Task.FromResult(GetAssetMock(name));
        }

        private Asset GetAssetMock(string name)
        {
            var prefix = "X";
            if (name.StartsWith(prefix))
                return new Asset
                {
                    Id = name.Replace(prefix, string.Empty),
                    ObjectId = name.Replace(prefix, string.Empty),
                    Name = name,
                    CurrentStatusCode = 0
                };
            return null;
        }
    }
}
