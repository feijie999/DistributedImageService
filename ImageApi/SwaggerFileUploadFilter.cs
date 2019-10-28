using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ImageApi
{
    public class SwaggerFileUploadFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!context.ApiDescription.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase) &&
                !context.ApiDescription.HttpMethod.Equals("PUT", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var fileParameters = context.ApiDescription.ActionDescriptor.Parameters
                .Where(n => n.ParameterType == typeof(IFormFile) ||
                            (n.BindingInfo != null && n.BindingInfo.BindingSource == BindingSource.FormFile)).ToList();

            if (fileParameters.Count == 0)
            {
                return;
            }
            foreach (var fileParameter in fileParameters)
            {
                var parameter = operation.Parameters.FirstOrDefault(n => n.Name == fileParameter.Name);
                if (parameter == null)
                {
                    continue;
                }
                operation.Parameters.Remove(parameter);
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = parameter.Name,
                    Description = parameter.Description,
                    Required = parameter.Required
                });
            }
        }
    }
}
