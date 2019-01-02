using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Wndrr.ReCaptcha.ServiceConfiguration
{
    public static class ServiceConfiguration
    {
        public static IMvcBuilder AddRecaptcha(this IMvcBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            return builder.AddApplicationPart(assembly).AddControllersAsServices();
        }
    }
}