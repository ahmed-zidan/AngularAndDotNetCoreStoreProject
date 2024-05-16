using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApi.Errors
{
    public class ApiError
    {
        public ApiError(int statusCode, string errorMessage, string messageDetail = null)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
            MessageDetail = messageDetail;
        }

        public int StatusCode { get; set; }
        public string ErrorMessage{ get; set; }
        public string MessageDetail { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
