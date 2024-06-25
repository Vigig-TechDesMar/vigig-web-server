namespace Vigig.Service.Models.Response.DashBoard;

public class AdminCashFlowDashBoard
{
    public double Revevue { get; set; }
    public double TotalBookingFee { get; set; }
    public double TotalSubscriptionFee { get; set; }

    public double BookingFeeSuccess { get; set; }
    public double BookingFeeFail { get; set; }
    public double BookingFeePending { get; set; }
    public double BookingFeeSuccessNo { get; set; }
    public double BookingFeeFailNo { get; set; }
    public double BookingFeePendingNo { get; set; }
    
    public double SubscriptionFeeSuccess { get; set; }
    public double SubscriptionFeeFail { get; set; }
    public double SubscriptionFeePending { get; set; }
    public double SubscriptionFeeSuccessNo { get; set; }
    public double SubscriptionFeeFailNo { get; set; }
    public double SubscriptionFeePendingNo { get; set; }
    
    public double DepositSucces { get; set; }
    public double DepositFail { get; set; }
    public double DepositPending { get; set; }
    public double DepositSuccesNo { get; set; }
    public double DepositFailNo { get; set; }
    public double DepositPendingNo { get; set; }
}