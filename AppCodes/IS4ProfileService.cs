using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.AppCodes
{
    /// <summary>
    /// IdentityServer提供了接口访问用户信息，但是默认返回的数据只有sub，就是IS4UserValidator.cs中设置的subject: user.Id,，
    /// 要返回更多的信息，需要实现IProfileService接口
    /// </summary>
    public class IS4ProfileService : IProfileService
    {
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            await Task.Run(() =>
            {
                    //depending on the scope accessing the user data.
                    var claims = context.Subject.Claims.ToList();

                    //set issued claims to return
                    context.IssuedClaims = claims.ToList();
            }).ConfigureAwait(false);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            await Task.Run(() =>
            {
                context.IsActive = true;
            }).ConfigureAwait(false);
        }
    }
}
