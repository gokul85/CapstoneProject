using AuthService.Data;
using AuthService.Models;

namespace AuthService.Repositories
{
    public class UserRepository : BaseRepository<int, User>
    {
        public UserRepository(AuthServiceDBContext context) : base(context)
        {
        }
    }

    public class UserDetailRepository : BaseRepository<int, UserDetails>
    {
        public UserDetailRepository(AuthServiceDBContext context) : base(context)
        {
        }
    }
}
