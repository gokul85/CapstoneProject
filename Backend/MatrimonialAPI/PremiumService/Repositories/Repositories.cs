using PremiumService.Data;
using PremiumService.Models;

namespace PremiumService.Repositories
{
    public class ContactViewsRepository : BaseRepository<int, ContactViews>
    {
        public ContactViewsRepository(PremiumServiceDBContext context) : base(context)
        {
        }
    }

    public class PaymentsRepository : BaseRepository<int, Payments>
    {
        public PaymentsRepository(PremiumServiceDBContext context) : base(context)
        {
        }
    }
}
