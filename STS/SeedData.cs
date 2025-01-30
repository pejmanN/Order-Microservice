using System.Security.Claims;
using IdentityModel;
using STS.Data;
using STS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;

namespace STS;

public class SeedData
{
    public static void PersistedGrantSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            scope.ServiceProvider
                .GetService<PersistedGrantDbContext>()
                .Database
                .Migrate();

            var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();
            context.Database.Migrate();

            EnsureSeedData(context);
        }
    }
    public static void ApplicationDefaultUserSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();

            SeedDefaultUsers(scope);
        }
    }

    private static void EnsureSeedData(ConfigurationDbContext context)
    {
        SeedClients(context);

        SeedIdentityResource(context);

        SeedApiScopes(context);

        SeedApiResource(context);
    }

    private static void SeedApiResource(ConfigurationDbContext context)
    {
        if (!context.ApiResources.Any())
        {
            Log.Debug("ApiScopes being populated");
            foreach (var resource in Config.ApiResources)
            {
                context.ApiResources.Add(resource.ToEntity());
            }
            context.SaveChanges();
        }
        else
        {
            Log.Debug("ApiScopes already populated");
        }
    }
    private static void SeedApiScopes(ConfigurationDbContext context)
    {
        if (context.ApiScopes.Any())
            return;

        foreach (var resource in Config.ApiScopes)
        {
            context.ApiScopes.Add(resource.ToEntity());
        }
        context.SaveChanges();
    }

    private static void SeedIdentityResource(ConfigurationDbContext context)
    {
        if (context.IdentityResources.Any())
            return;
        
        foreach (var resource in Config.IdentityResources)
        {
            context.IdentityResources.Add(resource.ToEntity());
        }
        context.SaveChanges();
    }

    private static void SeedClients(ConfigurationDbContext context)
    {
        if (context.Clients.Any())
            return;
        
        foreach (var client in Config.Clients)
        {
            context.Clients.Add(client.ToEntity());
        }
        context.SaveChanges();
    }

    private static void SeedDefaultUsers(IServiceScope scope)
    {
        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var bob = userMgr.FindByNameAsync("bob").Result;
        if (bob == null)
        {
            bob = new ApplicationUser
            {
                UserName = "bob",
                Email = "BobSmith@email.com",
                EmailConfirmed = true
            };
            var result = userMgr.CreateAsync(bob, "Pass123$").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(bob, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Bob Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Bob"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                            new Claim("location", "teh")
                        }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
        }
        
    }
}
