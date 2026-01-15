using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace KeepFit.Backend.API.Filter;

public class GenericControllerOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        //On récupère le Descriptor du Controller
        var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

        if (descriptor == null) return;

        //On récupère le nom de l'entité (ex: Exercise)
        var entityName = descriptor.ControllerName;
        
        var pluralName = entityName.EndsWith("s") ? entityName : entityName + "s";

        //On récupère le nom EXACT de la méthode C# (ex: "GetAllAsync")
        var methodName = context.MethodInfo.Name;

        //On applique les descriptions
        if (string.IsNullOrWhiteSpace(operation.Summary))
        {
            switch (methodName)
            {
                case "GetAllAsync":
                case "GetAll": // Au cas où le suffixe Async est supprimé
                    operation.Summary = $"Récupère la liste des {pluralName}";
                    operation.Description = $"Retourne une liste paginée de {pluralName}.";
                    break;

                case "GetAsync":
                case "Get":
                    operation.Summary = $"Récupère un(e) {entityName} par ID";
                    operation.Description = $"Retourne les détails spécifiques de l'entité {entityName}.";
                    break;

                case "CreateAsync":
                case "Create":
                    operation.Summary = $"Crée un(e) {entityName}";
                    operation.Description = $"Ajoute une nouvelle entité {entityName} à la base de données.";
                    break;

                case "DeleteAsync":
                case "Delete":
                    operation.Summary = $"Supprime un(e) {entityName}";
                    operation.Description = $"Supprime définitivement l'entité {entityName}.";
                    break;
            }
        }
    }
}