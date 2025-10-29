using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework
{
    public interface IUpdateModel
    {
        Task<bool> TryUpdateModelAsync<TModel>(TModel model) where TModel : class;
        Task<bool> TryUpdateModelAsync<TModel>(TModel model, string prefix) where TModel : class;
        Task<bool> TryUpdateModelAsync<TModel>(TModel model, string prefix, params Expression<Func<TModel, object>>[] includeExpressions) where TModel : class;
        bool TryValidateModel(object model);
        bool TryValidateModel(object model, string prefix);
        ModelStateDictionary ModelState { get; }
        IFormCollection Form { get; }
    }
}
