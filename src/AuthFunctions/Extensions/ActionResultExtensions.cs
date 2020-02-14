using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Security.Core.Models;

namespace AuthFunctions.Extensions
{
    public static class ActionResultExtensions
    {
        public static IActionResult AsActionResult(this Exception ex)
        {
            IActionResult result;
            if (ex is ArgumentException)
            {
                result = new ConflictObjectResult(new B2CResponseContent(
                    ex.Message,
                    HttpStatusCode.Conflict));
            }
            else
            { 
                result = new BadRequestObjectResult(new B2CResponseContent(
                    ex.Message,
                    HttpStatusCode.InternalServerError));
            }

            return result;
        }
    }
}
