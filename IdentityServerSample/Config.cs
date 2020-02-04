using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServerSample
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("api","My API Application")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "mvc",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    ClientSecrets = new List<Secret>()
                    {
                        new Secret("secret".Sha256())
                    },
                    RequireConsent = false,
                    RedirectUris = {"http://localhost:5001/signin-oidc"},
                    PostLogoutRedirectUris = {"http://localhost:5001/signout-callback-oidc"},
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId
                    }
                },
                //new Client
                //{
                //    ClientId = "client",
                //    AllowedGrantTypes = GrantTypes.ClientCredentials,
                //    ClientSecrets = new List<Secret>()
                //    {
                //        new Secret("secret".Sha256())
                //    },
                //    AllowedScopes = {"api"}
                //},
                //new Client
                //{
                //    ClientId = "PasswordClient",
                //    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                //    ClientSecrets = new List<Secret>()
                //    {
                //        new Secret("secret".Sha256())
                //    },
                //    AllowedScopes = {"api"}
                //}
            };
        }

        public static List<TestUser> GeTestUsers()
        {
            return new List<TestUser>()
            {
                new TestUser()
                {
                    SubjectId = "11",
                    Username = "davy",
                    Password = "123456",
                    Claims = new List<Claim>()
                    {
                        new Claim(JwtClaimTypes.Email,"1042722722@qq.com"),
                        new Claim(ClaimTypes.Role,"user")
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };
        }
    }
}
