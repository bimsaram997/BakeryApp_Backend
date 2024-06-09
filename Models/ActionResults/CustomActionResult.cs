using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels.Custom_action_result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ActionResults
{
    public class CustomActionResult<T> : IActionResult
    {
        private readonly CustomActionResultVM<T> _result;
        public CustomActionResult(CustomActionResultVM<T> result)
        {
            _result = result;
        }
        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(_result.Exception ?? _result.Data as object)
            {
                StatusCode = _result.Exception != null ? StatusCodes.Status500InternalServerError : StatusCodes.Status200OK,
            };
            await objectResult.ExecuteResultAsync(context);
        }
    }
}
