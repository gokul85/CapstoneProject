using ProfileService.Data;
using ProfileService.Models;

namespace ProfileService.Repositories
{
    public class UserProfileRepository : BaseRepository<int, UserProfile>
    {
        public UserProfileRepository(ProfileServiceDBContext context) : base(context)
        {
        }
    }

    public class BasicInfoRepository : BaseRepository<int, BasicInfo>
    {
        public BasicInfoRepository(ProfileServiceDBContext context) : base(context)
        {
        }
    }

    public class AddressRepository : BaseRepository<int, Address>
    {
        public AddressRepository(ProfileServiceDBContext context) : base(context)
        {
        }
    }

    public class CareersRepository : BaseRepository<int, Careers>
    {
        public CareersRepository(ProfileServiceDBContext context) : base(context)
        {
        }
    }

    public class EducationsRepository : BaseRepository<int, Educations>
    {
        public EducationsRepository(ProfileServiceDBContext context) : base(context)
        {
        }
    }

    public class FamilyInfoRepository : BaseRepository<int, FamilyInfo>
    {
        public FamilyInfoRepository(ProfileServiceDBContext context) : base(context)
        {
        }
    }

    public class LifeStyleRepository : BaseRepository<int, Lifestyle>
    {
        public LifeStyleRepository(ProfileServiceDBContext context) : base(context)
        {
        }
    }

    public class PhysicalAttributesRepository : BaseRepository<int, PhysicalAttributes>
    {
        public PhysicalAttributesRepository(ProfileServiceDBContext context) : base(context)
        {
        }
    }

    public class PartnerPreferenceRepository : BaseRepository<int, PartnerPreference>
    {
        public PartnerPreferenceRepository(ProfileServiceDBContext context) : base(context)
        {
        }
    }

    public class ProfileImagesRepository : BaseRepository<int, ProfileImages>
    {
        public ProfileImagesRepository(ProfileServiceDBContext context) : base(context)
        {
        }
    }
}
