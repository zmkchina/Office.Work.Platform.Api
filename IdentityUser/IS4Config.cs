﻿using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Office.Work.Platform.Api.IdentityUser
{
    /// <summary>
    /// IdentityServer4的配置类，配置IdentityResource，ApiResource以及Client：
    /// </summary>
    public static class IS4Config
    {
        /// <summary>
        /// 获取IS4自身内置的标准API资源(IdentityResource)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }
        /// <summary>
        /// 获取应用需调用的（用户自己编写的）API资源(ApiResource)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new ApiResource[] {
                   new ApiResource("PlatformApis","My Platform Apis")
                // new ApiResource { Name = "PlatformApis", Scopes ={"PlatformApis"} }
            };
        }

        /// <summary>
        /// 定义将访问IS4 的客户端(Client)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new Client[] {
                new Client(){
                    //让客户端支持两种验证模式
                    ClientId="WorkPlatformClient",
                    AllowedGrantTypes={
                        GrantType.ClientCredentials,  //客户端模式
                        GrantType.ResourceOwnerPassword //密码模式
                    },
                    ClientSecrets={new Secret("Work_Platform_ClientPwd".Sha256())},
                    AllowedScopes={ "PlatformApis",
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.OfflineAccess
                    }
                },
            };
        }
    }
}
