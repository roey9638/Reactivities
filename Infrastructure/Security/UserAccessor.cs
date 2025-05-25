using System;
using System.Security.Claims;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Http;
using Persistence;

// This [Infrastructure] [Class] [has] a [dependency] on the [Application] [Project] VVV
// In order to have [Access] to his [Classes]. For Example the [IUserAccessor].

namespace Infrastructure.Security;

// To be able to use the [IHttpContextAccessor]. We Added The VVV
// [<FrameworkReference Include="Microsoft.AspNetCore.App" />]. In the [Infrastructure.csproj]
public class UserAccessor(IHttpContextAccessor httpContextAccessor, AppDbContext dbContext) : IUserAccessor
{
    public async Task<User> GetUserAsync()
    {
        return await dbContext.Users.FindAsync(GetUserId())
            ?? throw new UnauthorizedAccessException("No User is Logged In");
    }

    // This will baiscally [Return] the [Currently] [Loged In] [User].
    public string GetUserId()
    {
        /* // The [httpContextAccessor.HttpContext?.User] -> Gets the current HTTP request context, and from it, the User (the person making the request).
        // The [.FindFirstValue(ClaimTypes.NameIdentifier)] -> This This looks for the user's ID in their list of claims.
        // The [ClaimTypes.NameIdentifier] is a common claim used to store the user’s ID. */
        return httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new Exception("No User Found");
    }
}

/* 
    // And when a [user] [joins] an [activity], 
    // we need to make sure that that [user] is who they say they are. 
    // And the way that we can do that is by getting the [token] and interrogating that token and [getting] the [username] from that [token]. 
    // And then we can use that [username] to go and get the [user] that we want to [add] to that [activity].
*/
