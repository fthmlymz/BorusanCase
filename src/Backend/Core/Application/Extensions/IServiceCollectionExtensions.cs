using Application.Behaviors;
using Application.Common.Exceptions;
using Application.Common.Filters;
using Application.Features.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Application.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddControllersValidator();

            services.AddMapster();
            services.AddMediator();
            services.AddValidators();
            services.AddSearch();

            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;

                opt.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState);
                    throw new BadRequestExceptionCustom(problemDetails.Errors);
                };
            });

            services.AddScoped<NotificationService>();
        }

        private static void AddMapster(this IServiceCollection services)
        {
            var assemblies = new[] { Assembly.GetExecutingAssembly() };
            TypeAdapterConfig.GlobalSettings.Scan(assemblies);
        }

        private static void AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }

        private static void AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }

        private static void AddSearch(this IServiceCollection services)
        {
            services.AddTransient(typeof(SearchBehavior<,,>));
        }

        #region Controller Validations
        private static void AddControllersValidator(this IServiceCollection services)
        {
            services.AddControllers(opt =>
            {
                opt.Filters.Add(new ValidateFilterAttribute());
                opt.Filters.Add(typeof(ValidateJsonModelFilter));
            })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.IgnoreReadOnlyFields = true;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull;
                });
        }
        #endregion
    }
}
