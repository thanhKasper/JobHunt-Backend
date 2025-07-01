using System.Security.Claims;
using JobHunt.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JobHunt.Ui.CustomModelBinders;

public class UserIdBinder(IJwtService jwtService) : IModelBinder
{
    private readonly IJwtService _jwtService = jwtService;
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        string authorizationValue = Convert.ToString(bindingContext.HttpContext.Request.Headers.Authorization)!;

        string token = authorizationValue.Substring(7);

        var claimsPrinciple = _jwtService.GetPrincipalsFromJWT(token);

        if (claimsPrinciple is null)
            bindingContext.Result = ModelBindingResult.Success(null);
        else if (String.IsNullOrEmpty(claimsPrinciple.FindFirstValue(ClaimTypes.NameIdentifier)))
            bindingContext.Result = ModelBindingResult.Success(null);
        else
            bindingContext.Result = ModelBindingResult.Success(
                Guid.Parse(claimsPrinciple.FindFirstValue(ClaimTypes.NameIdentifier)!));

        return Task.CompletedTask;
    }
}