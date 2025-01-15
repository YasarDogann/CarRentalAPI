using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApi.Business.Excepions
{
    public class CustomException : Exception
    {
        public int StatusCode { get; set; }
        public CustomException(string message, int statusCode = 500) : base(message)
        {
            StatusCode = statusCode;
        }
    }

    // 404 Not Found hatası için özel sınıf
    public class NotFoundException : CustomException
    {
        public NotFoundException(string message) : base(message, 404)
        {
        }
    }

    // 400 Bad Request hatası için özel sınıf
    public class BadRequestException : CustomException
    {
        public BadRequestException(string message) : base(message, 400)
        {
        }
    }

    // 401 Unauthorized hatası için özel sınıf
    public class UnauthorizedException : CustomException
    {
        public UnauthorizedException(string message) : base(message, 401)
        {
        }
    }

    // 403 Forbidden hatası için özel sınıf
    public class ForbiddenException : CustomException
    {
        public ForbiddenException(string message) : base(message, 403)
        {
        }
    }
}
