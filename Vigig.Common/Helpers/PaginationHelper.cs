using AutoMapper;
using Vigig.Common.Models;

namespace Vigig.Common.Helpers;

public class PaginationHelper
{
    public static PaginatedResult BuildPaginatedResult<T, TDto>(IMapper? mapper, IQueryable<T> source, int pageSize, int pageIndex)
    {
        var total = source.Count();
        if (total == 0)
        {
            return new PaginatedResult
            {
                PageIndex = 1,
                PageSize = pageSize,
                Items = new List<TDto>(),
                LastPage = 1,
                IsLastPage = true,
                Total = total
            };
        }
        pageSize = Math.Max(1, pageSize);
        var lastPage = (int)Math.Ceiling((decimal)total / pageSize);
        lastPage = Math.Max(1, lastPage);
        pageIndex = Math.Min(pageIndex, lastPage);
        var isLastPage = pageIndex == lastPage;
        var paginatedResult = new PaginatedResult
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            LastPage = lastPage,
            IsLastPage = isLastPage,
            Total = total
        };
        var results = source.Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);
        paginatedResult.Items = results;
        paginatedResult.Items = mapper is null ? results : mapper.Map<IEnumerable<TDto>>(results.AsEnumerable());
        return paginatedResult;
    }
    public static PaginatedResult BuildPaginatedResult<T, TDto>(IMapper? mapper, ICollection<T> source, int pageSize, int pageIndex)
    {
        var total = source.Count;
        if (total == 0)
        {
            return new PaginatedResult
            {
                PageIndex = 1,
                PageSize = pageSize,
                Items = new List<TDto>(),
                LastPage = 1,
                IsLastPage = true,
                Total = total
            };
        }

        pageSize = Math.Max(1, pageSize);
        var lastPage = (int)Math.Ceiling((decimal)total / pageSize);
        lastPage = Math.Max(1, lastPage);
        pageIndex = Math.Min(pageIndex, lastPage);
        var isLastPage = pageIndex == lastPage;

        var paginatedResult = new PaginatedResult
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            LastPage = lastPage,
            IsLastPage = isLastPage,
            Total = total
        };
        
        var results = source.Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);
        paginatedResult.Items = results;
        paginatedResult.Items = mapper is null ? results.ToList() : mapper.Map<IEnumerable<TDto>>(results.AsEnumerable());
        return paginatedResult;
    }
    
    public static PaginatedResult BuildPaginatedResult<T>(IQueryable<T> source, int pageSize, int pageIndex)
    {
        return BuildPaginatedResult<T, T>(null, source, pageSize, pageIndex);
    }
    
    public static PaginatedResult BuildPaginatedResult<T>(ICollection<T> source, int pageSize, int pageIndex)
    {
        return BuildPaginatedResult<T, T>(null, source, pageSize, pageIndex);
    }
}