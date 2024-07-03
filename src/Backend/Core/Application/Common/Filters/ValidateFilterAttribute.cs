﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared;

namespace Application.Common.Filters
{
    public class ValidateFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Handles the action execution by validating the model state.
        /// </summary>
        /// <param name="context">The ActionExecutingContext for the action.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                context.Result = new BadRequestObjectResult(Result<NoContent>.FailureAsync(errors).Result);
            }
        }
    }
}
