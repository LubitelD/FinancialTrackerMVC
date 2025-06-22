using Microsoft.AspNetCore.Identity;


namespace FinancialTrackerInfastructure.Models
{
    public class User: IdentityUser
    {
        public int year {  get; set; }
    }
}
