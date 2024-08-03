using AuthService.AsyncDataService;
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
        private readonly RabbitMQPublisher _rabbitMQPublisher;
        public UserService(IRepository<int, User> userrepo, IRepository<int, UserDetails> userdetail, ITokenService tokenservice, RabbitMQPublisher rabbitMQPublisher)
        {
            _userrepo = userrepo;
            _userdetailrepo = userdetail;
            _tokenService = tokenservice;
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        // GetAllUsers
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var result = await _userrepo.GetAll();
            if (result == null)
                throw new NoUserFoundException("Users Not Found");
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
            returnDTO.Token = _tokenService.GenerateToken(user, userDB.IsPremium);
            returnDTO.ProfileStatus = userDB.ProfileCompleted;
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
            userdetail.Status = "Active";
            HMACSHA512 hMACSHA = new HMACSHA512();
            userdetail.PasswordHashKey = hMACSHA.Key;
            userdetail.Password = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(userDTO.Password));
            userdetail.IsPremium = false;
            userdetail.ProfileCompleted = false;
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

                var phonefound = await _userrepo.FindAll(u => u.Phone == userDTO.Phone);
                if (phonefound != null)
                {
                    throw new UserAlreadyExistException("Mobile Number is Already Used, Please use different or Please Login!");
                }

                user = MapUserDTOToUser(userDTO);
                userdetails = MapUserDTOToUserDetail(userDTO);
                user = await _userrepo.Add(user);
                userdetails.UserId = user.Id;
                userdetails = await _userdetailrepo.Add(userdetails);

                _rabbitMQPublisher.PublishRegisterUserProfileMessage(new RegisterUserProfileDTO()
                {
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    ProfileFor = userDTO.OnBehalf,
                    UserId = user.Id,
                    DOB = userDTO.DOB,
                    Gender = userDTO.Gender,
                    Email = userDTO.Email,
                    Phone = userDTO.Phone,
                });

                ReturnDTO returnDTO = await Login(new UserLoginDTO() { Email = user.Email, Password = userDTO.Password });
                return returnDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration failed for user: {userDTO.Email}. Exception: {ex}");
                throw ex;
            }
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

        //VerifyUserProfileStatus
        public async Task<bool> VerifyUserProfileStatus(int userId)
        {
            var userdetails = await _userdetailrepo.FindAll(ud => ud.UserId == userId);
            if (userdetails == null)
            {
                throw new NoUserFoundException("No User Found");
            }
            var userdetail = userdetails.FirstOrDefault();
            return userdetail.ProfileCompleted;
        }

        //UpdateUserProfileStatus
        public async Task<string> UpdateUserProfileStatus(int userid)
        {
            var userdetails = await _userdetailrepo.FindAll(ud => ud.UserId == userid);
            if (userdetails == null)
            {
                throw new NoUserFoundException("No User Found");
            }
            var userdetail = userdetails.FirstOrDefault();
            userdetail.ProfileCompleted = true;
            userdetail = await _userdetailrepo.Update(userdetail);
            return "Updated";
        }

        //UpdateUserPremiumStatus
        public async Task<string> UpdateUserPremiumStatus(int userid)
        {
            var userdetails = await _userdetailrepo.FindAll(ud => ud.UserId == userid);
            if(userdetails == null)
            {
                throw new NoUserFoundException("No User Found");
            }
            var userdetail = userdetails.FirstOrDefault();
            userdetail.IsPremium = true;
            userdetail = await _userdetailrepo.Update(userdetail);
            return "Updated";
        }

        // RefreshUserToken
        public async Task<string> RefreshUserToken(int userid)
        {
            var userdetails = await _userdetailrepo.FindAll(ud => ud.UserId == userid);
            if(userdetails == null)
            {
                throw new NoUserFoundException("No User Found");
            }
            var userdetail = userdetails.FirstOrDefault();
            var user = await _userrepo.Get(userid);
            string token = _tokenService.GenerateToken(user, userdetail.IsPremium);
            return token;
        }
    }
}
