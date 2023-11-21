using Microsoft.EntityFrameworkCore.Storage;

namespace Talabat.APIS.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statusCode , string? Message = null ) 
        {
            statusCode = statusCode;
            Message = Message ?? GetDefaultMessageForStatusCode(StatusCode);
        }

        private string? GetDefaultMessageForStatusCode(int? statusCode)
        {
            //500 => Internal Server Error
            //400 => Bad Request
            //401 => UnAuthorized
            //404 => NotFound
            return statusCode switch
            {
                400 => "Bad Request",
                401 => "You Are Not Authorized",
                404 => "Resource Not Found",
                500 => "Internal Server Error",
                _ => null
            };
        }
    }
}
