using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SharedLibrary.Models
{
    public class ResponseModel<TModel> where TModel : class
    {
        public TModel Result { get; private set; }
        public int StatusCode { get; private set; }

        [JsonIgnore]
        public bool IsSuccessful { get; private set; }
        public ErrorDto Error { get; private set; }


        public static ResponseModel<TModel> Success(int statusCode)
        {
            return new ResponseModel<TModel>()
            {
                Result = default,
                StatusCode = statusCode,
                IsSuccessful = true
            };
        }

        public static ResponseModel<TModel> Success(TModel result, int statusCode)
        {
            return new ResponseModel<TModel>()
            {
                Result = result,
                StatusCode = statusCode,
                IsSuccessful = true
            };
        }

        public static ResponseModel<TModel> Fail(ErrorDto errorDto, int statusCode)
        {
            return new ResponseModel<TModel>()
            {
                Error = errorDto,
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }

        public static ResponseModel<TModel> Fail(string errorMessage, int statusCode, bool isShow)
        {
            return new ResponseModel<TModel>()
            {
                Error = new ErrorDto(errorMessage, isShow),
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }
    }
}
