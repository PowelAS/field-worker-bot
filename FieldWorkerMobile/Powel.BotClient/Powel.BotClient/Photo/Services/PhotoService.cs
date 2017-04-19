using System;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Powel.BotClient.Infrastructure.Extensions;

namespace Powel.BotClient.Photo.Services
{
    public class PhotoService
    {
        public async Task<byte[]> TakePhotoAsync()
        {
            await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsTakePhotoSupported && CrossMedia.Current.IsCameraAvailable)
            {
                return UnwrapImage(
                    await CrossMedia.Current.TakePhotoAsync(
                        new StoreCameraMediaOptions
                        {
                            Name = Guid.NewGuid() + ".jpg",
                            Directory = "SomeDirectory",
                            CompressionQuality = 30,
                            PhotoSize = PhotoSize.Medium
                        }));
            }

            throw new NotImplementedException();

        }
        private static byte[] UnwrapImage(MediaFile cameraResult)
        {
            if (cameraResult == null)
            {
                return null;
            }

            using (var stream = cameraResult.GetStream())
            {
                return stream.ToByteArray();
            }
        }
    }
}