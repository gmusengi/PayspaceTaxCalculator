namespace PayspaceTaxCalculator.Domain.DTOs
{
    public class PayspaceResponse<T>
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public T Response { get; set; }
        public Guid Id { get; set; }
    }
}