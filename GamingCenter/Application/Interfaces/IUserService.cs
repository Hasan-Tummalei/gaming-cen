using GamingCenter.Application.DTOs;
using GamingCenter.Domain.Entities;

namespace GamingCenter.Application.Interfaces
{
    public interface IUserService
    {

        Task<UserResponseDTO> RegisterUser(UserRegistrationDTO userRegistrationDto);
        Task<string> LoginUser(UserLoginDto userLoginDto);
        Task<UserResponseDTO> UpdateUser(Guid id, UserUpdateDto updateDto);
        Task<string> UpdateUserMemberShipAsync(Guid id, UserUpdateMembershipDto userUpdateMembershipDto);

    }
}
