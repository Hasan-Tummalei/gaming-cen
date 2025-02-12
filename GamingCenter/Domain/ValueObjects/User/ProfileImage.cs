using System.Text.RegularExpressions;

namespace GamingCenter.Domain.ValueObjects.User
{
    public sealed record ProfileImage
    {

        public string? Value { get; private init; }
        private static string ImageurlPattern = "^(https?:\\/\\/)?([\\da-z\\.-]+)\\.([a-z\\.]{2,6})([\\/\\w \\.-]*)*\\/?(\\?[\\w=&]*)?(#[\\w-]*)?$";


        /* Example Matches:
            http://example.com
            https://www.example.com
            http://sub.example.com/path/to/resource
            https://example.com:8080/path?key=value#section
            www.example.com (protocol is optional)
        */


        private ProfileImage() { }
        private ProfileImage(string profileImage)
        {
            Value = profileImage;
        }


        public static ProfileImage Create(string? profileImage) 
        {
            if (profileImage is not null && !Regex.IsMatch(profileImage, ImageurlPattern)) throw new ArgumentException("Invalid Image URl was passed");
            return new ProfileImage(profileImage);
        }

    }
}
