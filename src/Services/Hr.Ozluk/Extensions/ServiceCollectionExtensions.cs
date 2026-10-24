using Hr.Ozluk.Application.Interfaces;
using Hr.Ozluk.Application.Services;
using Hr.Ozluk.Infrastructure.Persistence;
using Hr.Ozluk.Infrastructure.Repositories;

namespace Hr.Ozluk.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOzlukServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection(MongoDbSettings.SectionName));
        services.AddSingleton<OzlukDbContext>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IPersonService, PersonService>();
        return services;
    }
}
