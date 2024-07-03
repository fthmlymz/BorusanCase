using Application.Interfaces.Repositories;
using Domain.Common;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Interfaces;
using System.Linq.Expressions;

namespace Application.Behaviors
{
    public class SearchBehavior<TQuery, TEntity, TDto>
          where TQuery : IRequest<PaginatedResult<TDto>>, IPaginatedSearchQuery
          where TEntity : BaseAuditableEntity, IPaginatedSearchEntity
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SearchBehavior<TQuery, TEntity, TDto>> _logger;

        public SearchBehavior(IUnitOfWork unitOfWork, ILogger<SearchBehavior<TQuery, TEntity, TDto>> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<PaginatedResult<TDto>> Handle(TQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handle method called with request: {request}");

            var cacheKey = GetCacheKey(request);
            _logger.LogInformation($"Cache key generated: {cacheKey}");

           
            var result = await SearchEntities(request);
            _logger.LogInformation($"Search method called with request: {request}");
            _logger.LogInformation($"Search result: {result}");

            _logger.LogInformation($"Cached: {cacheKey}");

            return result;
        }

        #region Cache Helpers
        private string GetCacheKey(TQuery request)
        {
            var cacheKey = $"{typeof(TEntity).Name}_Search_";
            foreach (var property in typeof(TQuery).GetProperties())
            {
                var value = property.GetValue(request);
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    cacheKey += value.ToString() + "_";
                }
            }
            cacheKey = cacheKey.TrimEnd('_');
            return cacheKey;
        }
        #endregion

        #region Search Helpers
        private async Task<PaginatedResult<TDto>> SearchEntities(TQuery request)
        {
            _logger.LogInformation($"SearchEntities called with request: {request}");

            var searchPredicate = BuildSearchPredicate(request);

            var totalCount = await _unitOfWork.Repository<TEntity>()
                .Where(searchPredicate)
                .CountAsync();

            //_logger.LogInformation($"Total count: {totalCount}");

            var result = await _unitOfWork.Repository<TEntity>()
                .Where(searchPredicate)
                .OrderBy(p => p.Name)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectToType<TDto>()
                .ToListAsync();

            //_logger.LogInformation($"Search results: {result}");

            return PaginatedResult<TDto>.Create(result, totalCount, request.PageNumber, request.PageSize);
        }
        #endregion

        #region Build Search Predicate Helpers
        private Expression<Func<TEntity, bool>> BuildSearchPredicate(TQuery request)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "entity");
            Expression predicate = Expression.Constant(true);

            var entityProperties = typeof(TEntity).GetProperties().Select(p => p.Name).ToHashSet();

            foreach (var property in typeof(TQuery).GetProperties())
            {
                if (!entityProperties.Contains(property.Name))
                {
                    continue;
                }

                var value = property.GetValue(request);
                if (value != null)
                {
                    var propertyType = property.PropertyType;
                    var entityProperty = Expression.Property(parameter, property.Name);

                    if (propertyType == typeof(string))
                    {
                        var valueExpression = Expression.Constant(value.ToString(), typeof(string));
                        var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                        var propertyToLowerExpression = Expression.Call(entityProperty, toLowerMethod);
                        var valueToLowerExpression = Expression.Call(valueExpression, toLowerMethod);

                        predicate = Expression.AndAlso(
                            predicate,
                            Expression.Call(
                                propertyToLowerExpression,
                                nameof(string.Contains),
                                Type.EmptyTypes,
                                valueToLowerExpression
                            )
                        );
                    }
                    else if (propertyType == typeof(int) || propertyType == typeof(int?))
                    {
                        predicate = Expression.AndAlso(
                            predicate,
                            Expression.Equal(
                                entityProperty,
                                Expression.Constant(value, typeof(int))
                            )
                        );
                    }
                    else if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                    {
                        predicate = Expression.AndAlso(
                            predicate,
                            Expression.Equal(
                                entityProperty,
                                Expression.Constant(value, typeof(Guid))
                            )
                        );
                    }
                    else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                    {
                        predicate = Expression.AndAlso(
                            predicate,
                            Expression.Equal(
                                entityProperty,
                                Expression.Constant(value, typeof(DateTime))
                            )
                        );
                    }
                }
            }

            return Expression.Lambda<Func<TEntity, bool>>(predicate, parameter);
        }

        /*private Expression<Func<TEntity, bool>> BuildSearchPredicate(TQuery request)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "p");
            Expression body = Expression.Constant(false);
            bool isAnySearchFieldProvided = false;

            foreach (var property in typeof(TQuery).GetProperties())
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(request);

                if (propertyValue != null && property.PropertyType == typeof(string))
                {
                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    var propertyExpression = Expression.Property(parameter, propertyName);
                    var valueExpression = Expression.Constant(propertyValue);
                    var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                    var propertyToLowerExpression = Expression.Call(propertyExpression, toLowerMethod);
                    var valueToLowerExpression = Expression.Call(valueExpression, toLowerMethod);
                    var containsExpression = Expression.Call(propertyToLowerExpression, containsMethod, valueToLowerExpression);
                    body = Expression.OrElse(body, containsExpression);
                    isAnySearchFieldProvided = true;

                    _logger.LogInformation($"Property {propertyName} with value {propertyValue} is being processed.");
                }
            }
            if (!isAnySearchFieldProvided)
            {
                _logger.LogInformation("No search field provided, defaulting to true.");
                return Expression.Lambda<Func<TEntity, bool>>(Expression.Constant(true), parameter);
            }

            _logger.LogInformation("Search predicate successfully built.");
            return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
        }*/
        #endregion
    }
}