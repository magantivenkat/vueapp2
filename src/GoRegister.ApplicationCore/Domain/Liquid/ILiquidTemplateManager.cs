using Fluid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Liquid
{
    public interface ILiquidTemplateManager
    {
        Task<string> RenderAsync(string template, TemplateContext context, TextEncoder encoder);

        /// <summary>
        /// Validates a Liquid template.
        /// </summary>
        bool Validate(string template, out IEnumerable<string> errors);
    }
}
