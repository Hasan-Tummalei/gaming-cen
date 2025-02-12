using GamingCenter.Application.DTOs;
using GamingCenter.Application.Interfaces;
using GamingCenter.Core.Interfaces;
using GamingCenter.Domain.Entities;
using GamingCenter.Domain.Enums;
using GamingCenter.Domain.Interfaces;
using GamingCenter.Domain.ValueObjects.User;

namespace GamingCenter.Application.Services
{
    /// <summary>
    /// Provides user-related operations such as registration, login, profile updates, and membership management.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRespository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly ILoggerService _loggerService;
        private readonly IMembershipRepository _membershipRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRespository">Repository for user data access.</param>
        /// <param name="passwordHasher">Service for password hashing and verification.</param>
        /// <param name="jwtService">Service for JWT token generation.</param>
        /// <param name="loggerService">Service for logging activities.</param>
        /// <param name="membershipRepository">Repository for membership data access.</param>
        public UserService(IUserRespository userRespository, IPasswordHasher passwordHasher, IJwtService jwtService, ILoggerService loggerService, IMembershipRepository membershipRepository)
        {
            _userRepository = userRespository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _loggerService = loggerService;
            _membershipRepository = membershipRepository;
        }

        /// <summary>
        /// Authenticates a user based on the provided login credentials.
        /// </summary>
        /// <param name="userLoginDto">The user's login details.</param>
        /// <returns>A JWT token if authentication is successful.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when credentials are invalid.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when the user is not found.</exception>
        public async Task<string> LoginUser(UserLoginDto userLoginDto)
        {
            var userFound = await _userRepository.GetUserByEmailAsync(Email.Create(userLoginDto.Email));

            if (userFound != null)
            {
                var isValid = _passwordHasher.VerifyPassword(userLoginDto.Password, userFound.Password.Value);
                if (!isValid)
                    throw new UnauthorizedAccessException("Invalid credentials.");

                return _jwtService.GenerateToken(userFound);
            }
            else
            {
                throw new KeyNotFoundException("User not found.");
            }
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="userRegistrationDto">The user's registration details.</param>
        /// <returns>A <see cref="UserResponseDTO"/> containing the registered user's information.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when a user with the same email already exists.</exception>
        public async Task<UserResponseDTO> RegisterUser(UserRegistrationDTO userRegistrationDto)
        {
            var userFound = await _userRepository.GetUserByEmailAsync(Email.Create(userRegistrationDto.Email));
            if (userFound != null) throw new KeyNotFoundException("A user with same email already exist");

            var dateOfBirthUtc = userRegistrationDto.DateOfBirth.ToUniversalTime();
            var hashedPassword = _passwordHasher.HashPassword(userRegistrationDto.Password);

            var user = new User(
                Email.Create(userRegistrationDto.Email),
                Password.Create(hashedPassword),
                Name.Create(userRegistrationDto.Name),
                DoB.Create(dateOfBirthUtc),
                ProfileImage.Create(userRegistrationDto.ProfileImage),
                UserRole.User
            );

            await _userRepository.AddUserAsync(user);
            _loggerService.LogInformation($"User registered: {user.Email.Value}");

            return new UserResponseDTO(user);
        }

        /// <summary>
        /// Updates the specified user's profile information.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="updateDto">The user's updated details.</param>
        /// <returns>A <see cref="UserResponseDTO"/> containing the updated user's information.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the user is not found.</exception>
        public async Task<UserResponseDTO> UpdateUser(Guid id, UserUpdateDto updateDto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            if (!string.IsNullOrEmpty(updateDto.Name))
            {
                user.UpdateProfile(Name.Create(updateDto.Name), user.Password, user.ProfileImage);
            }
            if (!string.IsNullOrEmpty(updateDto.Password))
            {
                var hashedPassword = _passwordHasher.HashPassword(updateDto.Password);
                user.UpdateProfile(user.Name, Password.Create(hashedPassword), user.ProfileImage);
            }
            if (!string.IsNullOrEmpty(updateDto.ProfileImage))
            {
                user.UpdateProfile(user.Name, user.Password, ProfileImage.Create(updateDto.ProfileImage));
            }

            await _userRepository.UpdateUserAsync(user);
            _loggerService.LogInformation($"User updated: {user.Email.Value}");

            return new UserResponseDTO(user);
        }

        /// <summary>
        /// Updates the membership of a specified user based on their total playtime.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="userUpdateMembershipDto">The membership update details.</param>
        /// <returns>A confirmation message indicating the new membership status.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the user or membership is not found,
        /// or if the user's playtime does not meet the membership requirement.
        /// </exception>
        public async Task<string> UpdateUserMemberShipAsync(Guid id, UserUpdateMembershipDto userUpdateMembershipDto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var memberShip = await _membershipRepository.GetMembershipByIdAsync(userUpdateMembershipDto.MembershipId);
            if (memberShip == null)
                throw new KeyNotFoundException("Membership not found.");

            if (user.TotalPlaytimeHours.Value >= memberShip.HoursThreshold.Value)
            {
                user.UpdateMemberShip(userUpdateMembershipDto.MembershipId, memberShip);
                await _userRepository.UpdateUserAsync(user);
            }
            else
            {
                throw new KeyNotFoundException($"User total played time does not meet the condition of reaching {memberShip.HoursThreshold.Value}.");
            }

            return $"{user.Name.Value} gained the {memberShip.MembershipName.Value} membership, congrats";
        }
    }
}
