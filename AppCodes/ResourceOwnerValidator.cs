using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Office.Work.Platform.Api.DataService;
using Office.Work.Platform.Lib;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.AppCodes
{
    public class ResourceOwnerValidator : IResourceOwnerPasswordValidator
    {
        private readonly DataUserRepository _UserRepository; //用户数据表操作对象
        public ResourceOwnerValidator(GHDbContext ghDbContet)
        {
            _UserRepository = new DataUserRepository(ghDbContet);
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var user = await _UserRepository.GetOneByIdPwdAsync(context.UserName, context.Password);
                //如用户名与密码都正确，User 才不会为 null
                if (user != null)
                {
                    context.Result = new GrantValidationResult(
                            subject: user.Id.ToString(),
                            authenticationMethod: "custom",
                            claims: GetUserClaims(user) //视情况，可以不需要此claims
                            );
                    return;
                }
                else
                {

                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "无效的用户名或密码");
                    return;
                }
            }
            catch (Exception ex)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, ex.Message);
            }
        }

        //build claims array from user data  
        public static Claim[] GetUserClaims(ModelUser user)
        {
            return new Claim[]
            {
                new Claim("user_id", user.Id ?? ""),
                new Claim(JwtClaimTypes.Name,user.Id ?? ""),
                new Claim(JwtClaimTypes.Role, user.Post)
            };
        }
    }
}