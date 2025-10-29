using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Framework.Domain
{
    public static class IQueryableResultExtensions
    {
        public static async Task<Result<TSource>> FindResultAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, string message = null, CancellationToken cancellationToken = default)
        {
            var item = await source.FirstOrDefaultAsync(predicate);
            if (item == null)
            {
                return Result.ResourceNotFound<TSource>(message);
            }
            return Result.Ok(item);
        }
    }
}
