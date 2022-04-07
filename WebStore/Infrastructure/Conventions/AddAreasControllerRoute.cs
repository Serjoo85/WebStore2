using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace WebStore.Infrastructure.Conventions;

public class AddAreasControllerRoute : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        var typeNamespace = controller.ControllerType.Namespace;
        if (string.IsNullOrEmpty(typeNamespace)) return;
        const string patternAreaName = @"(?<=.*Areas\.).*(?=\.Controllers.*)";
        Regex regex = new Regex(patternAreaName);
        var result = regex.Match(typeNamespace);
        if(!result.Success) return;
        controller.RouteValues["area"] = result.Value;

    }
}