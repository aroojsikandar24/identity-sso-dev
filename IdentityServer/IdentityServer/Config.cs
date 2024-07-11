using Duende.IdentityServer.Models;
using IdentityModel;

public static class Config
{
    public static IEnumerable<Client> GetClients()
    {
        return new List<Client>
        {
            new Client
            {
                ClientId = "app1",
                ClientSecrets = { new Secret("app1-secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = { "https://localhost:5001/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:5001"},
                AllowedScopes = { "openid", "profile", "app1", "roles"},
                RequirePkce = true,
                AllowOfflineAccess = true,
                AllowAccessTokensViaBrowser = true,
            },
            new Client
            {
                ClientId = "app2",
                ClientSecrets = { new Secret("app2-secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = { "https://localhost:5002/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:5002" },
                AllowedScopes = { "openid", "profile", "app2" },
                RequirePkce = true,
                AllowOfflineAccess = true,
                AllowAccessTokensViaBrowser = true
            }
        };
    }

    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new List<ApiScope>
        {
            new ApiScope("app1", "My API 1", new[] { JwtClaimTypes.Role }),
            new ApiScope("app2", "My API 2", new[] { JwtClaimTypes.Role }),
        };
    }

    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource("roles", new[] { JwtClaimTypes.Role })
        };
    }

}
