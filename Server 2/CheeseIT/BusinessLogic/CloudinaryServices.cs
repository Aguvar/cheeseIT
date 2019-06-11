using CheeseIT.BusinessLogic.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace CheeseIT.BusinessLogic
{
    public class CloudinaryServices : ICloudinaryServices
    {
        private readonly string cloud = "cheeseit";
        private readonly string apiKey = "459124453776213";
        private readonly string apiSecret = "45QvH-I2B739JpjEUZTH3XbanKM";

        public string ProcessImage(string base64Image)
        {
            Account account = new Account(
                cloud,
                apiKey,
                apiSecret
            );

            Cloudinary cloudinary = new Cloudinary(account);

            //Save it in Cloudinary
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription($"data:image/jpg;base64,{base64Image}")
            };
            var uploadResult = cloudinary.Upload(uploadParams);

            //Return filepath
            return uploadResult.Uri.ToString();
        }
    }
}
