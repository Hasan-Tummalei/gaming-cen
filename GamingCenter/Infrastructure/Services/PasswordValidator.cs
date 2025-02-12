using System.Text.RegularExpressions;
using GamingCenter.Application.Interfaces;

namespace GamingCenter.Infrastructure.Services
{
    public class PasswordValidator : IPasswordValidator
    {
        public bool isValid( string password)
        {

            string pattern =  @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,}$";
            return Regex.IsMatch(password, pattern); 
        }
    }
}
