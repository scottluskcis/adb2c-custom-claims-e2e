using System;
using System.Net;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

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
