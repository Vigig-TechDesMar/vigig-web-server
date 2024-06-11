namespace Vigig.Domain.Interfaces;

public interface ICash
{
    Guid Id { get; set; }
    double Amount { get; set; }

    DateTime CreatedDate { get; set; }
    
    int Status { get; set; }
    
    // void MarkAsPaid();
    // void MarkAsPending();
    // void MarkAsFailed();
}