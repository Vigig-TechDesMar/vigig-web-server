

using System.Collections;
using System.Linq.Expressions;
using Vigig.Common.Models;
using Vigig.Service.Models.Common;

namespace Vigig.Common.Helpers;

public class SearchHelper
{
    public static IEnumerable BuildSearchResult<T>(IEnumerable<T> src,SearchUsingGet  request)
    {
        // if (!string.IsNullOrEmpty(request.SearchField))
        // {
        //     src = src
        //         .Where(p => ReflectionHelper.GetStringValueByName(typeof(T), request.SearchField, p).Contains(request.SearchValue ?? string.Empty, StringComparison.OrdinalIgnoreCase))
        //         .ToList();
        if (!string.IsNullOrEmpty(request.SearchField))
        {
            src = src
                .Where(p => ReflectionHelper.GetStringValueByName(typeof(T), request.SearchField, p)
                    .Contains(request.SearchValue ?? String.Empty, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        if (!string.IsNullOrEmpty(request.SortField))
        {
            Expression<Func<T, object>> orderExpr = p =>
                ReflectionHelper.GetValueByName(typeof(T), request.SortField, p);
            src = request.Descending
                ? src.OrderByDescending(orderExpr.Compile()).AsQueryable()
                : src.OrderBy(orderExpr.Compile()).AsQueryable().ToList();
        }
        return src;
    }
}