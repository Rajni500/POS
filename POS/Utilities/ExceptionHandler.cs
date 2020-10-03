using Microsoft.AspNetCore.Mvc;

namespace POS.Utilities
{
    public static class ExceptionHandler
    {
        public static JsonResult throwException(string message)
        {
            return new JsonResult("")
            {
                Value = new
                {
                    success = false,
                    error = message,
                },

                StatusCode = 500
            };
        }
    }
}
