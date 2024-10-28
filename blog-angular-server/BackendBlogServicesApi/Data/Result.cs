namespace BackendBlogServicesApi.Data
{
    public class Result<T>
    {
        public T Value { get; set; }
        public string Error { get; set; }
        public bool IsSuccess => Error == null;
        public string Message { get; set; }

        public static Result<T> Success(T value, string message = "") => new Result<T> { Value = value, Message = message };
        public static Result<T> Failure(T value, string error) => new Result<T> { Value = value , Error = error };
    }

    public class RutasConfiguracion
    {
        public string[] RutasAutenticacion { get; set; }
    }

}
