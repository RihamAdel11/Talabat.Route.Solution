using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services.Contract;

namespace Talabat.Route.APIs.Helpers
{
	public class CachedAttribute : Attribute, IAsyncActionFilter
	{
		private readonly int _timeToLiveInSecond;

		public CachedAttribute(int timeToLiveInSecond)
		{
			_timeToLiveInSecond = timeToLiveInSecond;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var responseCacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
			// Ask CLR For Creating Object From "ResponseCacheService" Explicitly

			var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

			var response = await responseCacheService.GetCacheResponseAsync(cacheKey);

			if (!string.IsNullOrEmpty(response))
			{
				var result = new ContentResult()
				{
					Content = response,
					ContentType = "application/json",
					StatusCode = 200
				};

				context.Result = result;
				return;
			} // Response is not cached

			var executedActionContext = await next.Invoke(); // Will Excute next Action Filter or Action Itself

			if (executedActionContext.Result is OkObjectResult okObjectResult && okObjectResult.Value is not null)
				await responseCacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSecond));
		}

		private string GenerateCacheKeyFromRequest(HttpRequest request)
		{
			var keyBuilder = new StringBuilder();

			keyBuilder.Append(request.Path); // /api/product

			foreach (var (key, value) in request.Query.OrderBy(X => X.Key))
			{
				keyBuilder.Append($"|{key}-{value}");
			}
			return keyBuilder.ToString();
		}


	}
}

