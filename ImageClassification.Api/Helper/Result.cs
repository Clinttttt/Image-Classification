using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Bson;

namespace ImageClassification.Api.Helper
{
    public  class Result<T>
    {
        public  int status_code { get; set; }
        public  string? error { get; set; }
        public  T? value { get; set; }
        public bool is_success { get; set; }

        public  Result(bool is_success, T? value, int status_Code = 200,  string? Error = null )
        {
            this.is_success = is_success;
            this.status_code = status_Code;      
            this.value = value;
            Error = is_success ? null : error;
        }

        public static Result<T> Success(T value) => new(true, value);
        public static Result<T> Failure(string error) => new(false, default, 400);

        public static Result<T> NotFound() => new(false, default, 404);
        public static Result<T> Unauthorized() => new(false, default, 401);
        public static Result<T> Conflict() => new(false, default, 409);
        public static Result<T> Forbidden() => new(false, default, 403);
        public static Result<T> NoContent() => new(true, default, 204);
        public static Result<T> InternalServerError() => new(false, default, 500);



    }
}
