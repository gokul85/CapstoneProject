using ProfileService.Exceptions;
using ProfileService.Interfaces;
using ProfileService.Models;
using ProfileService.Models.DTOs;

namespace ProfileService.Services
{
    public class UserProfileService : IProfileService
    {
        private readonly IRepository<int, BasicInfo> _basicinforepo;
        private readonly IRepository<int, UserProfile> _userprofilerepo;
        private readonly IRepository<int, Address> _addressrepo;
        private readonly IRepository<int, Careers> _careersrepo;
        private readonly IRepository<int, Educations> _educationsrepo;
        private readonly IRepository<int, FamilyInfo> _familyinforepo;
        private readonly IRepository<int, Lifestyle> _lifestylerepo;
        private readonly IRepository<int, PhysicalAttributes> _physicalattrepo;

        public UserProfileService(
            IRepository<int, BasicInfo> basicinforepo, 
            IRepository<int, UserProfile> userprofilerepo,
            IRepository<int, Address> addressrepo,
            IRepository<int, Careers> careersrepo,
            IRepository<int, Educations> educationsrepo,
            IRepository<int, FamilyInfo> familyinforepo,
            IRepository<int, Lifestyle> lifestylerepo,
            IRepository<int, PhysicalAttributes> physicalattrepo
            )
        {
            _basicinforepo = basicinforepo;
            _userprofilerepo = userprofilerepo;
            _addressrepo = addressrepo;
            _careersrepo = careersrepo;
            _educationsrepo = educationsrepo;
            _familyinforepo = familyinforepo;
            _lifestylerepo = lifestylerepo;
            _physicalattrepo = physicalattrepo;
        }

        public async Task<ResponseModel> AddUserProfile(AddUserProfileDTO addprofile)
        {
            var ups = await _userprofilerepo.FindAll(up => up.UserId == addprofile.UserId);
            if(ups == null)
            {
                throw new UserProfileNotFoundException("User Profile Not Found");
            }
            var userprofile = ups.FirstOrDefault();
            var basicinfo = await _basicinforepo.Get((int)userprofile.BasicInfoId);
            await UpdateBasicInfoDetails(basicinfo,addprofile);
            var addressid = await AddProfileAddress(addprofile);
            var phyattid = await AddProfilePhyAtt(addprofile);
            var familyinfoid = await AddProfileFamilyInfo(addprofile);
            var lifestyleid = await AddProfileLifestyle(addprofile);
            await AddUserProfileEducations(addprofile,userprofile.Id);
            await AddUserProfileCareers(addprofile, userprofile.Id);
            var res = await UpdateUserProfile(userprofile, addressid, phyattid, familyinfoid, lifestyleid);
            return new ResponseModel
            {
                result = res
            };
        }

        private async Task AddUserProfileCareers(AddUserProfileDTO addprofile, int id)
        {
            if(addprofile.Career == null || !addprofile.Career.Any())
            {
                return;
            }

            var careers = addprofile.Career.Select(e => new Careers
            {
                UserProfileId = id,
                JobTitle = e.Designation,
                Company = e.Company,
                StartYear = e.StartYear,
                EndYear = e.EndYear,
            }).ToList();

            foreach(var career in careers)
            {
                await _careersrepo.Add(career);
            }
        }

        private async Task AddUserProfileEducations(AddUserProfileDTO addprofile, int id)
        {
            if (addprofile.Education == null || !addprofile.Education.Any())
            {
                return;
            }

            var educations = addprofile.Education.Select(e => new Educations
            {
                UserProfileId = id,
                Degree = e.Degree,
                Specialization = e.Specialization,
                StartYear = e.StartYear,
                EndYear = e.EndYear,
                Status = e.Status
            }).ToList();

            foreach (var education in educations)
            {
                await _educationsrepo.Add(education);
            }
        }

        private async Task<UserProfile> UpdateUserProfile(UserProfile userprofile, int addressid, int phyattid, int familyinfoid, int lifestyleid)
        {
            userprofile.AddressId = addressid;
            userprofile.PhysicalAttrId = phyattid;
            userprofile.FamilyInfoId = familyinfoid;
            userprofile.LifeStyleId = lifestyleid;
            var res = await _userprofilerepo.Update(userprofile);
            return res;
        }

        private async Task<int> AddProfileLifestyle(AddUserProfileDTO addprofile)
        {
            var lifestyle = new Lifestyle()
            {
                Drink = addprofile.Drink == "Yes" ? true : false,
                Smoke = addprofile.Smoke == "Yes" ? true : false,
                LivingWith = addprofile.LivingWith
            };
            lifestyle = await _lifestylerepo.Add(lifestyle);
            return lifestyle.Id;
        }

        private async Task<int> AddProfileFamilyInfo(AddUserProfileDTO addprofile)
        {
            var familyinfo = new FamilyInfo()
            {
                Father = addprofile.Father == "Alive" ? true : false,
                Mother = addprofile.Mother == "Alive" ? true : false,
                Siblings = addprofile.SiblingsCount
            };

            familyinfo = await _familyinforepo.Add(familyinfo);
            return familyinfo.Id;
        }

        private async Task<int> AddProfilePhyAtt(AddUserProfileDTO addprofile)
        {
            var phyatt = new PhysicalAttributes()
            {
                Height = addprofile.Height,
                Weight = addprofile.Weight,
                EyeColor = addprofile.EyeColor,
                HairColor = addprofile.HairColor,
                Complextion = addprofile.Complexion,
                BloodGroup = addprofile.BloodGroup,
                Disability = addprofile.Disability == "No" ? false : true,
                DisablitiyDetail = addprofile.Disability == "Yes" ? addprofile.DisabilityDetails : "",
            };
            var res = await _physicalattrepo.Add(phyatt);
            return res.Id;
        }

        private async Task<int> AddProfileAddress(AddUserProfileDTO addprofile)
        {
            var address = new Address()
            {
                AddressLine = addprofile.AddressLine,
                City = addprofile.City,
                State = addprofile.State,
                PinCode = addprofile.Pincode
            };
            var res = await _addressrepo.Add(address);
            return res.Id;
        }

        private async Task UpdateBasicInfoDetails(BasicInfo basicinfo, AddUserProfileDTO addprofile)
        {
            basicinfo.Intro = addprofile.Bio;
            basicinfo.NativeLanguage = addprofile.NativeLanguage;
            basicinfo.MaritalStatus = addprofile.MaritalStatus;
            basicinfo.Religion = addprofile.Religion;
            basicinfo.Caste =  addprofile.Caste;
            basicinfo.HighestQualification = addprofile.HighestQualification;

            await _basicinforepo.Update(basicinfo);
        }

        public async Task<ResponseModel> CreateUserProfile(RegisterUserProfileDTO profile)
        {
            var basicinfo = new BasicInfo()
            {
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Gender = profile.Gender,
                DOB = profile.DOB
            };

            basicinfo = await _basicinforepo.Add(basicinfo);

            var userprofile = new UserProfile()
            {
                UserId = profile.UserId,
                ProfileFor = profile.ProfileFor,
                BasicInfoId = basicinfo.Id,
                ProfileCompleted = false,
            };

            userprofile = await _userprofilerepo.Add(userprofile);

            return new ResponseModel() { result = userprofile };
        }
    }
}
