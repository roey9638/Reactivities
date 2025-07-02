using API.Middleware;
using API.SignalR;
using Application.Activities.Queries;
using Application.Core;
using Application.Interfaces;
using Application.Validators;
using Domain;
using FluentValidation;
using Infrastructure.Photos;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors();

builder.Services.AddSignalR();

builder.Services.AddMediatR(x =>
{

    x.RegisterServicesFromAssemblyContaining<GetActivityList.Handler>();
    /* In C#, when you write ValidationBehavior<,>, you're referring to a generic type definition that has two type parameters VVV  
    // but you aren't specifying yet what those types are. It's open — meaning the type parameters are still to be filled in later. 
    // For example, ValidationBehavior<TRequest, TResponse> might be defined like this. */

    /* // HERE I have a [Middleware] that will [Validate] the [Params] that will be [Passed] to the [Diffrent] [Handling] [components] in my [Application].
    // It will [Execute] [Because] the [BaseActivityValidator] VVV
    // And [everything] that will be [Passed] into to it [For Example] [CreateActivity.Command] VVV
    // [Example] -> I Have a [CreateActivityValidator] and it [Inherats] the [BaseActivityValidator] and in there i [Pass] the [CreateActivity.Command]. */
    x.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddScoped<IUserAccessor, UserAccessor>();

builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);

builder.Services.AddValidatorsFromAssemblyContaining<CreateActivityValidator>();

builder.Services.AddTransient<ExceptionMiddleware>();

builder.Services.AddIdentityApiEndpoints<User>(opt =>
{
    opt.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthorization(opt =>
{
    // Here I'm [adding] a [custom] [policy] [named] []"IsActivityHost"].
    opt.AddPolicy("IsActivityHost", policy =>
    {
        /* This says: for [someone] to [meet] the []"IsActivityHost"] [policy], 
            they must [fulfill] the [custom] [requirement] [defined] in [IsHostRequirement] */
        policy.Requirements.Add(new IsHostRequirement());
    });
});

builder.Services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();
builder.Services.AddScoped<IPhotoService, PhotoService>();

/* 
    The [Properties] in the [CloudinarySettings] [Class] will filled from the [CloudinarySettings] [Section]
    That is [inside] the [appsettings.json]
*/
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionMiddleware>();

// Important that it will be [Here] [Before] [app.UseAuthorization()]!!!
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
.AllowCredentials()
.WithOrigins("http://localhost:3000", "https://localhost:3000"));

app.UseAuthentication(); // Must Be Before [app.UseAuthorization()] VVVVVV !!!!!!!!!!

app.UseAuthorization();

app.MapControllers();

// This Making sure that when we [Route] somewhere with the [identity] 
// It will be like the others. For Example ---> "api/login".
app.MapGroup("api").MapIdentityApi<User>();

// That's for when a [client] [browser]. The way he [connects] to this will be like VVV
// "localhost:5001/comments"
app.MapHub<CommentHub>("/comments");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<AppDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    // This will [apply] any [pending migrations] AND if the [dataBase] [does not exist] it will [create] it.
    await context.Database.MigrateAsync();
    await DbInitializer.SeedData(context, userManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An Error Occured During Migration");
}

app.Run();
