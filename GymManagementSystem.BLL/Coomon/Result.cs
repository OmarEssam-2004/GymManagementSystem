using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Coomon
{
    public record Result(bool Success, string? error = null, ResultKind Kind = ResultKind.Ok)
    {
        public static Result Ok() => new Result(true);
        public static Result NotFound(string message = "Not Found") => new Result(false, message, ResultKind.NotFound);
        public static Result Conflict(string message, ResultKind kind = ResultKind.Conflict) => new Result(false, message, kind);
        public static Result ValidationFailed(string message) => new Result(false, message, ResultKind.ValidationFailed);
    }

    public record Result<T>(bool Success, T? Value, string? error = null, ResultKind Kind = ResultKind.Ok)
    {
        public static Result<T> Ok(T value) => new (true, value);
        public static Result<T> NotFound(string message = "Not Found") => new (false, default, message, ResultKind.NotFound);
        public static Result<T> Conflict(string message, ResultKind kind = ResultKind.Conflict) => new (false, default, message, kind);
        public static Result<T> ValidationFailed(string message) => new (false, default, message, ResultKind.ValidationFailed);
    }

}
