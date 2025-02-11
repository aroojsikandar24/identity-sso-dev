﻿using Duende.IdentityServer.Models;

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
                AllowedScopes = { "openid", "profile", "api1" },
                RequirePkce = true,
                AllowOfflineAccess = true,
                AllowAccessTokensViaBrowser = true
            },
            new Client
            {
                ClientId = "app2",
                ClientSecrets = { new Secret("app2-secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = { "https://localhost:5002/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:5002" },
                AllowedScopes = { "openid", "profile", "api1" },
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
            new ApiScope("api1", "My API")
        };
    }

    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };
    }
}
