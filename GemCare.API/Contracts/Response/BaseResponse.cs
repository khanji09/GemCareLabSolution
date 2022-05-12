using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GemCare.API.Contracts.Response
{
    public interface IBaseResponse
    {
        HttpStatusCode Statuscode { get; set; }
        string Message { get; set; }
        bool Haserror { get; set; }
    }
    public interface ISingleResponse<TResponse> : IBaseResponse
    {
        TResponse Result { get; set; }
    }

    public interface IListResponse<TResponse> : IBaseResponse
    {
        public List<TResponse> Result { get; set; }
    }
    public class BaseResponse : IBaseResponse
    {
        public HttpStatusCode Statuscode { get; set; }
        public bool Haserror { get; set; }
        public string Message { get; set; }
    }

    public class SingleResponse<TResponse> : ISingleResponse<TResponse>
    {
        public HttpStatusCode Statuscode { get; set; }
        public string Message { get; set; }
        public bool Haserror { get; set; }
        public TResponse Result { get; set; }
    }

    public class ListResponse<TResponse> : IListResponse<TResponse>
    {
        public HttpStatusCode Statuscode { get; set; }
        public string Message { get; set; }
        public bool Haserror { get; set; }
        public List<TResponse> Result { get; set; }
    }

    public static class ResponseExtension
    {
        public static void ToHttpExceptionResponse(this IBaseResponse response, string exMessage)
        {
            response.Statuscode = HttpStatusCode.InternalServerError;
            response.Haserror = true;
            response.Message = exMessage;
        }

        public static void ToHttpForbiddenResponse(this IBaseResponse response, string errMessage)
        {
            response.Statuscode = HttpStatusCode.BadRequest;
            response.Haserror = true;
            response.Message = errMessage;
        }
    }
}
