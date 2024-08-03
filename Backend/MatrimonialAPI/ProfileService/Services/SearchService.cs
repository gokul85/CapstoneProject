using ProfileService.AsyncDataServices;
using ProfileService.Exceptions;
using ProfileService.Interfaces;
using ProfileService.Models;
using ProfileService.Models.DTOs;
using System.Net.Http;

namespace ProfileService.Services
{
    public class SearchService : ISearchService
    {
        private readonly IRepository<int, BasicInfo> _basicinforepo;
        private readonly IRepository<int, UserProfile> _userprofilerepo;
        private readonly IRepository<int, Address> _addressrepo;
        private readonly IRepository<int, Careers> _careersrepo;
        private readonly IRepository<int, Educations> _educationsrepo;
        private readonly IRepository<int, FamilyInfo> _familyinforepo;
        private readonly IRepository<int, Lifestyle> _lifestylerepo;
        private readonly IRepository<int, PhysicalAttributes> _physicalattrepo;
        private readonly IRepository<int, PartnerPreference> _partnerprefrepo;
        private readonly IRepository<int, ProfileImages> _gallaryimagesrepo;
        public SearchService(
            IRepository<int, BasicInfo> basicinforepo,
            IRepository<int, UserProfile> userprofilerepo,
            IRepository<int, Address> addressrepo,
            IRepository<int, Careers> careersrepo,
            IRepository<int, Educations> educationsrepo,
            IRepository<int, FamilyInfo> familyinforepo,
            IRepository<int, Lifestyle> lifestylerepo,
            IRepository<int, PhysicalAttributes> physicalattrepo,
            IRepository<int, PartnerPreference> partnerprefrepo,
            IRepository<int, ProfileImages> gallaryimagesrepo)
        {
            _basicinforepo = basicinforepo;
            _userprofilerepo = userprofilerepo;
            _addressrepo = addressrepo;
            _careersrepo = careersrepo;
            _educationsrepo = educationsrepo;
            _familyinforepo = familyinforepo;
            _lifestylerepo = lifestylerepo;
            _physicalattrepo = physicalattrepo;
            _partnerprefrepo = partnerprefrepo;
            _gallaryimagesrepo = gallaryimagesrepo;
        }

        int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }

        public async Task<ResponseModel> SearchProfiles(SearchCriteriaDTO searchCriteria, int userid)
        {
            var userprofiles = await _userprofilerepo.FindAllWithIncludes(up => up.UserId == userid, up => up.BasicInfo, up => up.PartnerPref);
            if (userprofiles == null)
            {
                throw new UserProfileNotFoundException("User Profile Not Found");
            }
            var userprofile = userprofiles.FirstOrDefault();
            IEnumerable<UserProfile> res = null;
            if (searchCriteria.PP)
            {
                var partnerPreference = userprofile.PartnerPref;
                if (partnerPreference != null)
                {
                    res = await _userprofilerepo.FindAllWithIncludes(up => up.BasicInfo.Gender != userprofile.BasicInfo.Gender && up.ProfileCompleted == true
                        && (partnerPreference.MinHeight == 0 || up.PhysicalAttribute.Height >= partnerPreference.MinHeight)
                        && (partnerPreference.MaxHeight == 0 || up.PhysicalAttribute.Height <= partnerPreference.MaxHeight)
                        && (partnerPreference.MinWeight == 0 || up.PhysicalAttribute.Weight >= partnerPreference.MinWeight)
                        && (partnerPreference.MaxWeight == 0 || up.PhysicalAttribute.Weight <= partnerPreference.MaxWeight)
                        && (string.IsNullOrEmpty(partnerPreference.MaritalStatus) || up.BasicInfo.MaritalStatus == partnerPreference.MaritalStatus)
                        && (string.IsNullOrEmpty(partnerPreference.Religion) || up.BasicInfo.Religion == partnerPreference.Religion)
                        && (string.IsNullOrEmpty(partnerPreference.Language) || up.BasicInfo.NativeLanguage == partnerPreference.Language)
                        && (string.IsNullOrEmpty(partnerPreference.State) || up.Address.State == partnerPreference.State)
                        && (string.IsNullOrEmpty(partnerPreference.Complexion) || up.PhysicalAttribute.Complextion == partnerPreference.Complexion)
                        && (!partnerPreference.SmokeAcceptable || up.LifeStyle.Smoke == partnerPreference.SmokeAcceptable)
                        && (!partnerPreference.DrinkAcceptable || up.LifeStyle.Drink == partnerPreference.DrinkAcceptable)
                        , up => up.BasicInfo, up => up.PhysicalAttribute, up => up.LifeStyle, up => up.Educations, up => up.Careers, up => up.Address, up => up.FamilyInfo, up => up.Address);
                }
            }
            else
            {
                res = await _userprofilerepo.FindAllWithIncludes(up => up.BasicInfo.Gender != userprofile.BasicInfo.Gender && up.ProfileCompleted == true
                        && (searchCriteria.MinHeight == 0 || up.PhysicalAttribute.Height >= searchCriteria.MinHeight)
                    && (searchCriteria.MaxHeight == 0 || up.PhysicalAttribute.Height <= searchCriteria.MaxHeight)
                    && (searchCriteria.MinWeight == 0 || up.PhysicalAttribute.Weight >= searchCriteria.MinWeight)
                    && (searchCriteria.MaxWeight == 0 || up.PhysicalAttribute.Weight <= searchCriteria.MaxWeight)
                    && (string.IsNullOrEmpty(searchCriteria.MaritalStatus) || up.BasicInfo.MaritalStatus == searchCriteria.MaritalStatus)
                    && (string.IsNullOrEmpty(searchCriteria.Religion) || up.BasicInfo.Religion == searchCriteria.Religion)
                    && (string.IsNullOrEmpty(searchCriteria.Language) || up.BasicInfo.NativeLanguage == searchCriteria.Language)
                    && (string.IsNullOrEmpty(searchCriteria.State) || up.Address.State == searchCriteria.State)
                    && (string.IsNullOrEmpty(searchCriteria.Complexion) || up.PhysicalAttribute.Complextion == searchCriteria.Complexion)
                    && (!searchCriteria.SmokeAcceptable.HasValue || up.LifeStyle.Smoke == searchCriteria.SmokeAcceptable)
                    && (!searchCriteria.DrinkAcceptable.HasValue || up.LifeStyle.Drink == searchCriteria.DrinkAcceptable)
                        , up => up.BasicInfo, up => up.PhysicalAttribute, up => up.LifeStyle, up => up.Educations, up => up.Careers, up => up.Address, up => up.FamilyInfo, up => up.Address);
            }

            if (res == null)
            {
                throw new UserProfileNotFoundException("User Profiles Not Found");
            }

            var searchProfileDTOs = res.Select(up => new SearchProfileDTO
            {
                Id = up.Id,
                Name = up.BasicInfo.FirstName + " " + up.BasicInfo.LastName,
                Profession = up.Careers.LastOrDefault()?.JobTitle ?? "Unknown",
                State = up.Address.State,
                MaritalStatus = up.BasicInfo.MaritalStatus,
                Religion = up.BasicInfo.Religion,
                Height = up.PhysicalAttribute.Height,
                Weight = up.PhysicalAttribute.Weight,
                Age = CalculateAge(up.BasicInfo.DOB),
                Complexion = up.PhysicalAttribute.Complextion,
                Image = up.ProfileImage
            }).ToList();

            return new ResponseModel { result = new SearchResultDTO { SearchProfiles = searchProfileDTOs, PartnerPreference = userprofile.PartnerPref } };
        }


        public async Task<ResponseModel> ViewProfile(int profileid)
        {
            var userprofiles = await _userprofilerepo.FindAllWithIncludes(up => up.Id == profileid, up => up.BasicInfo, up => up.PhysicalAttribute, up => up.LifeStyle, up => up.Educations, up => up.Careers, up => up.Address, up => up.FamilyInfo);

            if (userprofiles == null)
            {
                throw new UserProfileNotFoundException("User Profiles Not Found");
            }

            var userprofile = userprofiles.First();

            var viewprofile = new ViewProfileReturnDTO()
            {
                ProfileImage = userprofile.ProfileImage,
                FirstName = userprofile.BasicInfo.FirstName,
                LastName = userprofile.BasicInfo.LastName,
                DOB = userprofile.BasicInfo.DOB,
                Gender = userprofile.BasicInfo.Gender,
                MaritalStatus = userprofile.BasicInfo.MaritalStatus,
                OnBehalf = userprofile.ProfileFor,
                Intro = userprofile.BasicInfo.Intro,
                Height = userprofile.PhysicalAttribute.Height,
                Weight = userprofile.PhysicalAttribute.Weight,
                BloodGroup = userprofile.PhysicalAttribute.BloodGroup,
                HairColor = userprofile.PhysicalAttribute.HairColor,
                EyeColor = userprofile.PhysicalAttribute.EyeColor,
                Complexion = userprofile.PhysicalAttribute.Complextion,
                Disability = userprofile.PhysicalAttribute.Disability == false ? "No" : "Yes",
                NativeLanguage = userprofile.BasicInfo.NativeLanguage,
                Drink = userprofile.LifeStyle.Drink == false ? "No" : "Yes",
                Smoke = userprofile.LifeStyle.Smoke == false ? "No" : "Yes",
                LivingWith = userprofile.LifeStyle.LivingWith,
                Religion = userprofile.BasicInfo.Religion,
                Caste = userprofile.BasicInfo.Caste,
                State = userprofile.Address.State,
                City = userprofile.Address.City,
                FatherStatus = userprofile.FamilyInfo.Father == true ? "Alive" : "Deceased",
                MotherStatus = userprofile.FamilyInfo.Mother == true ? "Alive" : "Deceased",
                Noofsiblings = userprofile.FamilyInfo.Siblings,
                Educations = userprofile.Educations.ToList(),
                Careers = userprofile.Careers.ToList()
            };

            return new ResponseModel() { result = viewprofile };
        }
    }
}
