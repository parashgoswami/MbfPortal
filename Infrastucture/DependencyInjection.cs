using Application.Common.Interfaces;
using Infrastucture.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastucture;
public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddTransient<IExcelService, ExcelService>();
    }
}
