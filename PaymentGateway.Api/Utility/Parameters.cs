using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace PaymentGateway.Api.Utility
{
    public class Parameters : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "merchantId",
                In = ParameterLocation.Query,
                Description = "Merchant Id",
                Required = false
            });

            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "paymentId",
                In = ParameterLocation.Query,
                Description = "Payment Id",
                Required = false
            });

        }
    }
}
