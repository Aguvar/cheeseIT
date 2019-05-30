using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;

namespace CheeseIT.BusinessLogic
{
    public class CheeseServices
    {
        internal string ProcessImage(string base64Image)
        {

            Account account = new Account(
                "cheeseit",
                "459124453776213",
                "45QvH-I2B739JpjEUZTH3XbanKM"
                );

            Cloudinary cloudinary = new Cloudinary(account);

            //Decode image


            //Save it to azure blob storage or locally or in Cloudinary

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(base64Image)
            };
            var uploadResult = cloudinary.Upload(uploadParams);

            //Return filepath
            return uploadResult.Uri.ToString();
        }
    }
}
