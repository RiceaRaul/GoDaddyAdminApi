using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Runtime.Serialization;

namespace Common.Extensions
{

    public class SwaggerIgnoreSchema : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema,SchemaFilterContext context)
        {
            if(schema?.Properties is null)
            {
                return;
            }

            var ignoreDataMemberProperties = context.Type.GetProperties()
                .Where(t => t.GetCustomAttribute<IgnoreDataMemberAttribute>() is not null);

            foreach(var ignoreDataMemberProperty in ignoreDataMemberProperties)
            {
                var propertyToHide = schema.Properties.Keys
                    .SingleOrDefault(x => x.ToLower() == ignoreDataMemberProperty.Name.ToLower());

                if(propertyToHide is not null)
                {
                    schema.Properties.Remove(propertyToHide);
                }
            }
        }
    }
    public static class SwaggerExtensions
    {
        public static void SetIgnoreSchema(this SwaggerGenOptions opt)
        {
            opt.SchemaFilter<SwaggerIgnoreSchema>();
        }
    }
}
