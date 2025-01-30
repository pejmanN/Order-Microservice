using Duende.IdentityServer.Models;

namespace STS;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };


    public static IEnumerable<ApiResource> ApiResources =>
           new ApiResource[]
           {
                new ApiResource("order-api")
                {
                    ApiSecrets = new List<Secret>()
                    {
                        new Secret("OrderApi-secret".Sha256())
                    },
                    Scopes = new List<string>()
                    {
                        "order"
                    }
                },
           };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("order"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                AllowedScopes = { "scope1" }
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "interactive",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:44300/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "scope2" }
            },

            new Client
                {
                    ClientId = "Order-front",

                    //because the client is public client so it can not hold ClientSecret
                    RequireClientSecret = false,

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = {
                         "http://localhost:4200/auth-callback",
                         "http://localhost:4200/assets/silent-refresh.html" //for silent refresh (iframe uri) callback
                     },

                    //for refresh token
                    AllowOfflineAccess = false,

                    AllowedScopes = { "openid", "profile","order"},

                    AllowAccessTokensViaBrowser = false,

                    RequirePkce = true ,
                    AccessTokenLifetime=70,
                    RequireConsent= true,
                },

                 //for `ToDoApp` client
                new Client
                {
                    ClientId = "todo-app",
                    ClientSecrets ={ new Secret("secret".Sha256())  } ,

                    //because the client is not public 
                    RequireClientSecret = true,

                    AllowedGrantTypes = GrantTypes.Code,

                    // the installed package for OIDC, use default route for redierction  "signin-oidc"
                    RedirectUris = { "https://localhost:5007/signin-oidc" },

                    //for refresh token
                    AllowOfflineAccess = false,

                    AllowedScopes = { "openid", "profile","order"},

                    RequireConsent=true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RequirePkce = true,
                },

                 new Client
                {
                    ClientId = "postman",
                    //ClientSecrets ={ new Secret("secret".Sha256())  } ,

                    //because the client is not public 
                    RequireClientSecret = false,

                    AllowedGrantTypes = GrantTypes.Code,

                    
                    RedirectUris = { "urn:ietf:wg:oauth:2.0:oob" },

                    //for refresh token
                    AllowOfflineAccess = false,

                    AllowedScopes = { "openid", "profile","order"},

                    RequireConsent=false,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RequirePkce = true,
                }
        };
}
