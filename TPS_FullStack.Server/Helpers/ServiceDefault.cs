namespace TPS_FullStack.Server
{
    public class ServiceDefault<T>
    {
        public bool Success { get; set; }
        public int statusCode { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}


