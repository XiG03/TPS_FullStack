namespace TPS_FullStack.Server
{
    public class ServiceDefault<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public Status status {get; set;}
        public T? Data { get; set; }
    }
    public enum Status
    {
        NotFound,
        Invalid,
    }
}


