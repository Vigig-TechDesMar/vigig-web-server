﻿namespace Vigig.Domain.Dtos.Booking;

public class DtoBookingResponse
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid ProviderId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string ProviderName { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public bool IsCancellable { get; set; }
    public string? Status { get; set; }
}