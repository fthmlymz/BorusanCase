using Application.Interfaces;
using Domain.Common;
using Domain.Common.Interfaces;
using Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IMediator, Mediator>();
            services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddTransient<IDateTimeService, DateTimeService>();
        }
    }
}
