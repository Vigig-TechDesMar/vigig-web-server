using AutoMapper;
using Vigig.Common.Helpers;
using Vigig.Domain.Dtos;
using Vigig.Domain.Dtos.Badge;
using Vigig.Domain.Dtos.Booking;
using Vigig.Domain.Dtos.Building;
using Vigig.Domain.Dtos.Complaint;
using Vigig.Domain.Dtos.Event;
using Vigig.Domain.Dtos.Fees;
using Vigig.Domain.Dtos.Service;
using Vigig.Domain.Dtos.SubscriptionPlan;
using Vigig.Domain.Dtos.Voucher;
using Vigig.Domain.Dtos.Wallet;
using Vigig.Domain.Dtos.VigigUser;
using Vigig.Domain.Entities;
using Vigig.Domain.Enums;
using Vigig.Service.Models.Request.Authentication;
using Vigig.Service.Models.Request.Badge;
using Vigig.Service.Models.Request.Building;
using Vigig.Service.Models.Request.Complaint;
using Vigig.Service.Models.Request.Event;
using Vigig.Service.Models.Request.Fees;
using Vigig.Service.Models.Request.GigService;
using Vigig.Service.Models.Request.Service;
using Vigig.Service.Models.Request.SubscriptionPlan;
using Vigig.Service.Models.Request.Voucher;
using Vigig.Service.Models.Request.Wallet;
using Vigig.Service.Models.Response.Authentication;

namespace Vigig.Service.AutoMappings;

public static class AutoMapperConfiguration
{
    public static void RegisterMaps(IMapperConfigurationExpression mapper)
    {
        CreateUserMaps(mapper);
        CreateBuildingMaps(mapper);
        CreateServiceMaps(mapper);
        CreateBadgeMaps(mapper);
        CreateSubscriptionPlanMaps(mapper);
        CreateFeeMaps(mapper);
        CreateWalletMaps(mapper);
        CreateEventMaps(mapper);
        CreateComplaintMaps(mapper);
        CreateVoucherMaps(mapper);
        CreateVigigUserMaps(mapper);
        CreateBookingMaps(mapper);
    }

    public static void CreateUserMaps(IMapperConfigurationExpression mapper)
    {
        mapper.CreateMap<RegisterRequest, VigigUser>();
        mapper.CreateMap<VigigUser,RegisterResponse>();
    }

    public static void CreateBuildingMaps(IMapperConfigurationExpression mapper)
    {
        mapper.CreateMap<Building, DtoBuilding>();
        mapper.CreateMap<CreateBuildingRequest, Building>();
    }

    public static void CreateServiceMaps(IMapperConfigurationExpression mapper)
    {
        mapper.CreateMap<GigServiceRequest, GigService>();
        mapper.CreateMap<ServiceCategoryRequest, ServiceCategory>();
       
        mapper.CreateMap<GigService, DtoGigService>();
        mapper.CreateMap<ServiceCategory,DtoServiceCategory>();
        mapper.CreateMap<UpdateServiceCategoryRequest, ServiceCategory>()
            .ForMember(sc => sc.Description, opt => opt.Condition( c => !string.IsNullOrWhiteSpace(c.Description)))
            .ForMember(sc => sc.CategoryName, opt => opt.Condition(c => !string.IsNullOrWhiteSpace(c.CategoryName)));

        mapper.CreateMap<UpdateGigServiceRequest, GigService>()
            .ForMember(s => s.Description ,opt => opt.Condition(r => !string.IsNullOrWhiteSpace(r.Description)))
            .ForMember(s => s.ServiceName ,opt => opt.Condition(r => !string.IsNullOrWhiteSpace(r.ServiceName)));
        mapper.CreateMap<ProviderService, DtoProviderService>()
            .ForMember(dto => dto.ServiceName, opt => opt.MapFrom(x => x.Service.ServiceName))
            .ForMember(dto => dto.ProviderName, opt => opt.MapFrom(x => x.Provider.UserName));
        mapper.CreateMap<ServiceImage, DtoServiceImage>();
    }

    public static void CreateBadgeMaps(IMapperConfigurationExpression mapper)
    {
        mapper.CreateMap<CreateBadgeRequest, Badge>();
        mapper.CreateMap<UpdateBadgeRequest, Badge>()
            .ForMember(b => b.BadgeName,opt => opt.Condition(c => !string.IsNullOrWhiteSpace(c.BadgeName)))
            .ForMember(b => b.Description,opt => opt.Condition(c => !string.IsNullOrWhiteSpace(c.Description)))
            .ForMember(b => b.Benefit,opt => opt.Condition(c => !string.IsNullOrWhiteSpace(c.Benefit)));
        mapper.CreateMap<Badge, DtoBadge>();
        mapper.CreateMap<Badge, DtoBadgeWithStatus>();

    }
  
    public static void CreateSubscriptionPlanMaps(IMapperConfigurationExpression mapper)
    {
        mapper.CreateMap<CreateSubscriptionPlanRequest, SubscriptionPlan>();
        mapper.CreateMap<UpdateSubscriptionPlanRequest, SubscriptionPlan>()
            .ForMember(b => b.Description, opt => opt.Condition(c => !string.IsNullOrWhiteSpace(c.Description)));
        mapper.CreateMap<SubscriptionPlan, DtoSubscriptionPlan>();
        
    }

    public static void CreateFeeMaps(IMapperConfigurationExpression mapper)
    {
        //SubscriptionFee
        mapper.CreateMap<CreateSubscriptionFeeRequest, SubscriptionFee>();
        mapper.CreateMap<UpdateSubscriptionFeeRequest, SubscriptionFee>();
        mapper.CreateMap<SubscriptionFee, DtoSubscriptionFee>();
        
        //BookingFee
        mapper.CreateMap<CreateBookingFeeRequest, BookingFee>();
        mapper.CreateMap<UpdateBookingFeeRequest, BookingFee>();
        mapper.CreateMap<BookingFee, DtoSubscriptionFee>();

        //Deposit
        mapper.CreateMap<CreateDepositRequest, Deposit>();
        mapper.CreateMap<UpdateDepositRequest, Deposit>()
            .ForMember(c=> c.PaymentMethod, opt=> opt.Condition(m=> !string.IsNullOrEmpty(m.PaymentMethod)));
        mapper.CreateMap<Deposit, DtoDeposit>();
    }

    public static void CreateWalletMaps(IMapperConfigurationExpression mapper)
    {
        //Transaction
        mapper.CreateMap<CreateTransactionRequest, Transaction>();
        mapper.CreateMap<UpdateTransactionRequest, Transaction>();
        mapper.CreateMap<Transaction, DtoTransaction>();
        
        //Wallet
        mapper.CreateMap<CreateWalletRequest, Wallet>();
        mapper.CreateMap<UpdateWalletRequest, Wallet>();
        mapper.CreateMap<Wallet, DtoWallet>();
    }

    public static void CreateEventMaps(IMapperConfigurationExpression mapper)
    {
        //Event
        mapper.CreateMap<Event,DtoEvent>();
        mapper.CreateMap<CreateEventRequest,Event>();
        mapper.CreateMap<UpdateEventRequest,Event>()
            .ForMember(b=>b.EventTitle, opt=> opt.Condition(c=> !string.IsNullOrWhiteSpace(c.EventTitle)))
            .ForMember(b=> b.EventDescription, opt=> opt.Condition(c=> !string.IsNullOrWhiteSpace(c.EventDescription)));

        //Banner
        mapper.CreateMap<Banner, DtoBanner>();
        mapper.CreateMap<CreateBannerRequest,Banner>();
        mapper.CreateMap<UpdateBannerRequest,Banner>().
            ForMember(b=> b.AltText, opt => opt.Condition(c=> !string.IsNullOrWhiteSpace(c.AltText)));

        //PopUp
        mapper.CreateMap<PopUp, DtoPopUp>();
        mapper.CreateMap<CreatePopUpRequest, PopUp>();
        mapper.CreateMap<UpdatePopUpRequest,PopUp>().
            ForMember(b=> b.Title, opt => opt.Condition(c=> !string.IsNullOrWhiteSpace(c.Title))).
            ForMember(b=> b.SubTitle, opt => opt.Condition(c=> !string.IsNullOrWhiteSpace(c.SubTitle)));


        //LeaderBoard
        mapper.CreateMap<LeaderBoard,DtoLeaderBoard>();
        mapper.CreateMap<CreateLeaderBoardRequest,LeaderBoard>();
        mapper.CreateMap<UpdateLeaderBoardRequest,LeaderBoard>().
            ForMember(b=> b.Name, opt => opt.Condition(c=> !string.IsNullOrWhiteSpace(c.Name)));

        //ProviderKPI
        mapper.CreateMap<ProviderKPI, DtoProviderKPI>();
        mapper.CreateMap<CreateProviderKPIRequest, ProviderKPI>();
        mapper.CreateMap<UpdateProviderKPIRequest, ProviderKPI>();

        //EventImage
        mapper.CreateMap<EventImage,DtoEventImage>();
        mapper.CreateMap<CreateEventRequest,EventImage>();
        mapper.CreateMap<UpdateEventRequest,EventImage>();
    }
    
    public static void CreateComplaintMaps(IMapperConfigurationExpression mapper)
    {
        //ComplaintType
        mapper.CreateMap<ComplaintType,DtoComplaintType>();
        mapper.CreateMap<CreateComplaintTypeRequest,ComplaintType>();
        mapper.CreateMap<UpdateComplaintTypeRequest,ComplaintType>().
            ForMember(b=> b.Name, opt => opt.Condition(c=> !string.IsNullOrWhiteSpace(c.Name))).
            ForMember(b=> b.Description, opt => opt.Condition(c=> !string.IsNullOrWhiteSpace(c.Description)));

        
        //Complaint
        mapper.CreateMap<Complaint,DtoComplaint>();
        mapper.CreateMap<CreateComplaintRequest,Complaint>();
        mapper.CreateMap<UpdateComplaintRequest,Complaint>();
        
    }
    
    public static void CreateVoucherMaps(IMapperConfigurationExpression mapper)
    {
        //Voucher
        mapper.CreateMap<Voucher,DtoVoucher>();
        mapper.CreateMap<CreateVoucherRequest,Voucher>();
        mapper.CreateMap<UpdateVoucherRequest,Voucher>().
            ForMember(b=> b.Content, opt => opt.Condition(c=> !string.IsNullOrWhiteSpace(c.Content)));
        
        //ClaimedVoucher
        mapper.CreateMap<ClaimedVoucher,DtoClaimedVoucher>();
        mapper.CreateMap<CreateClaimedVoucherRequest,ClaimedVoucher>();
        mapper.CreateMap<UpdateClaimedVoucherRequest,ClaimedVoucher>();
    }
  
    public static void CreateVigigUserMaps(IMapperConfigurationExpression mapper)
    {
        mapper.CreateMap<VigigUser, DtoUserProfile>();
    }

    public static void CreateBookingMaps(IMapperConfigurationExpression mapper)
    {
        mapper.CreateMap<Booking, DtoPlacedBooking>()
        .ForMember(dto => dto.ProviderName, opt => opt.MapFrom(x => x.ProviderService.Provider.UserName))
        .ForMember(dto => dto.ProviderServiceName, opt => opt.MapFrom(x => x.ProviderService.Service.ServiceName))
        .ForMember(dto => dto.BuildingName, opt => opt.MapFrom(x => x.Building.BuildingName))
        .ForMember(dto => dto.Status, opt => opt.MapFrom(x => EnumHelper.TranslateEnum(x.Status)));
        mapper.CreateMap<Booking,DtoBooking>()
            .ForMember(dto => dto.ProviderName, opt => opt.MapFrom(x => x.ProviderService.Provider.UserName))
            .ForMember(dto => dto.ProviderServiceName, opt => opt.MapFrom(x => x.ProviderService.Service.ServiceName))
            .ForMember(dto => dto.BuildingName, opt => opt.MapFrom(x => x.Building.BuildingName))
            .ForMember(dto => dto.Status, opt => opt.MapFrom(x => EnumHelper.TranslateEnum(x.Status)))
            .ForMember(dto => dto.IsCancellable, opt => opt.MapFrom(x => x.Status == BookingStatus.Pending))
            .ForMember(dto => dto.ProviderProfileImage, opt => opt.MapFrom(x => x.ProviderService.Provider.ProfileImage))
            .ForMember(dto => dto.BookerProfileImage, opt => opt.MapFrom(x => x.VigigUser.ProfileImage))
            .ForMember(dto => dto.FinalPrice, opt => opt.MapFrom(x => (x.FinalPrice == 0) ? x.StickerPrice:x.FinalPrice));
        mapper.CreateMap<Booking, DtoBookChat>()
            .ForMember(dto => dto.ProviderName, opt => opt.MapFrom(x => x.ProviderService.Provider.UserName))
            .ForMember(dto => dto.ProviderProfileImage,
                opt => opt.MapFrom(x => x.ProviderService.Provider.ProfileImage))
            .ForMember(dto => dto.ClientName, opt => opt.MapFrom(x => x.VigigUser.UserName))
            .ForMember(dto => dto.ClientProfileImage, opt => opt.MapFrom(x => x.VigigUser.ProfileImage))
            .ForMember(dto => dto.ChatTitle, opt=> opt.MapFrom(x => x.ProviderService.Service.ServiceName +" - "+ x.CreatedDate.Date))
            .ForMember(dto => dto.LastMessage, opt =>
            {
                opt.Condition(x => x.BookingMessages.Any());
                opt.MapFrom(x => x.BookingMessages.OrderByDescending(x => x.SentAt).FirstOrDefault().Content);
            })
            .ForMember(dto => dto.LastMessageSentAt, opt =>
            {
                opt.Condition(x => x.BookingMessages.Any());
                opt.MapFrom(x => x.BookingMessages.OrderByDescending(x => x.SentAt).FirstOrDefault().SentAt.ToString());
            });
        mapper.CreateMap<BookingMessage, DtoBookingMessage>() 
            .ForMember(dto => dto.SenderName, opt => opt.MapFrom(x => x.SenderName));
    }
}