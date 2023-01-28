using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MiniApp1.API.ClaimsRequirements
{
    public class AgeRequirement : IAuthorizationRequirement
    {
        public int MinimumAge { get; set; }

        public AgeRequirement(int minimumAge)
        {
            MinimumAge = minimumAge;
        }
    }

    /// <summary>
    /// Belirlenen yaş'dan küçük olan kişiler giriş yapamaz.
    /// </summary>
    public class AgeRequirementHandler : AuthorizationHandler<AgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeRequirement requirement)
        {
            var dateOfBirthClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.DateOfBirth);
            if (dateOfBirthClaim is null)
            {
                //Erişim için belirlenen yaş kriterlerini karşılamıyorsunuz.
                context.Fail(new AuthorizationFailureReason(this, "You do not meet the age criteria set for access."));
                return Task.CompletedTask;
            }

            bool isConverted = DateTime.TryParse(dateOfBirthClaim.Value, out DateTime userDateOfBirth);
            if (!isConverted)
            {
                //doğum tarihiniz geçerli değildir.
                context.Fail(new AuthorizationFailureReason(this, "Your date of birth is not valid."));
                return Task.CompletedTask;
            }

            int userAge = DateTime.Now.Year - userDateOfBirth.Year;
            if (userAge < requirement.MinimumAge)
            {
                //Erişim için belirlenen yaş kriterlerini karşılamıyorsunuz.
                context.Fail(new AuthorizationFailureReason(this, "You do not meet the age criteria set for access."));
                return Task.CompletedTask;
            }

            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
