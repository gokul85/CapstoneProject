using AuthService.Exceptions;
using AuthService.Interfaces;
using AuthService.Models;
using AuthService.Models.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<int, User> _userrepo;
        private readonly IRepository<int, UserDetails> _userdetailrepo;
        private readonly ITokenService _tokenService;
        public UserService(IRepository<int, User> userrepo, IRepository<int, UserDetails> userdetail, ITokenService tokenservice)
        {
            _userrepo = userrepo;
            _userdetailrepo = userdetail;
            _tokenService = tokenservice;
        }

        // GetAllUsers
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var result = await _userrepo.GetAll();
            if (result == null)
                throw new ObjectsNotFoundException("Users Not Found");
            return result;
        }

        private bool ComparePassword(byte[] encrypterPass, byte[] password)
        {
            for (int i = 0; i < encrypterPass.Length; i++)
            {
                if (encrypterPass[i] != password[i])
                {
                    return false;
                }
            }
            return true;
        }

        private ReturnDTO MapUserToReturn(User user, UserDetails userDB)
        {
            ReturnDTO returnDTO = new ReturnDTO();
            returnDTO.UserID = user.Id;
            returnDTO.Name = user.FirstName;
            returnDTO.Email = user.Email;
            returnDTO.Role = user.Role ?? "User";
            returnDTO.Token = _tokenService.GenerateToken(user);
            return returnDTO;
        }

        // Login
        public async Task<ReturnDTO> Login(UserLoginDTO loginDTO)
        {
            var ub = (await _userrepo.FindAll(u => u.Email == loginDTO.Email));
            if (ub == null)
            {
                throw new UnauthorizedUserException("Invalid Email or password");
            }
            var uDB = ub.FirstOrDefault();
            var userDB = await _userdetailrepo.Get(uDB.Id);
            HMACSHA512 hMACSHA = new HMACSHA512(userDB.PasswordHashKey);
            var encrypterPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
            bool isPasswordSame = ComparePassword(encrypterPass, userDB.Password);
            if (isPasswordSame)
            {
                var user = await _userrepo.Get(userDB.UserId);
                if (userDB.Status == "Active")
                {
                    ReturnDTO loginReturnDTO = MapUserToReturn(user, userDB);
                    return loginReturnDTO;
                }

                throw new UserNotActiveException("Your account is not activated");
            }
            throw new UnauthorizedUserException("Invalid Email or password");
        }

        private User MapUserDTOToUser(UserRegisterDTO userDTO)
        {
            User user = new User
            {
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                Phone = userDTO.Phone,
                Role = "User",
            };
            return user;
        }

        private UserDetails MapUserDTOToUserDetail(UserRegisterDTO userDTO)
        {
            UserDetails userdetail = new UserDetails();
            userdetail.Status = "Disabled";
            HMACSHA512 hMACSHA = new HMACSHA512();
            userdetail.PasswordHashKey = hMACSHA.Key;
            userdetail.Password = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(userDTO.Password));
            userdetail.IsPremium = false;
            return userdetail;
        }

        // Register
        public async Task<ReturnDTO> Register(UserRegisterDTO userDTO)
        {
            User user = null;
            UserDetails userdetails = null;
            try
            {
                var emailfound = await _userrepo.FindAll(u => u.Email == userDTO.Email);
                if (emailfound != null)
                {
                    throw new UserAlreadyExistException("User Account Already Exists, Please Login!");
                }
            }
            catch (Exception ex)
            {
                if (ex is not ObjectsNotFoundException)
                    throw ex;
            }
            user = MapUserDTOToUser(userDTO);
            userdetails = MapUserDTOToUserDetail(userDTO);
            user = await _userrepo.Add(user);
            userdetails.UserId = user.Id;
            userdetails = await _userdetailrepo.Add(userdetails);
            ReturnDTO returnDTO = await Login(new UserLoginDTO() { Email = user.Email, Password = userDTO.Password});
            return returnDTO;
        }

        //UpdateUserRole
        public async Task<string> UpdateUserRole(int userId, string role)
        {
            User user = await _userrepo.Get(userId);
            if (user == null)
            {
                throw new NoUserFoundException("No User Found");
            }
            user.Role = role;
            await _userrepo.Update(user);
            return $"{user.FirstName} is now {role}";
        }

        // UpdateUserStatus
        public async Task<string> UpdateUserStatus(UpdateUserStatusDTO updateuserstatusDTO)
        {
            UserDetails ud = await _userdetailrepo.Get(updateuserstatusDTO.UserId);
            if (ud == null)
            {
                throw new NoUserFoundException("No User Found");
            }
            ud.Status = updateuserstatusDTO.Status;
            await _userdetailrepo.Update(ud);
            return "User Status Successfully Updated";
        }
    }
}
