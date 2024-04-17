use Vigig
-- create table
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Provider')
BEGIN
CREATE TABLE [Provider] (
    Id                   UNIQUEIDENTIFIER NOT NULL,
    UserName             NVARCHAR(255)    NOT NULL,
    NormalizedUserName   NVARCHAR(255)	  NOT NULL,
    Password             NVARCHAR(MAX)    NOT NULL,
    Gender               NVARCHAR(63),
    ProfileImage         NVARCHAR(MAX),
    Rating               FLOAT,
    FullName             NVARCHAR(450),
    Email                NVARCHAR(255)    NOT NULL,
    NormalizedEmail		 NVARCHAR(255)	  NOT NULL,
    EmailConfirmed		 BIT			  DEFAULT 0,
    Phone                NVARCHAR(10),
    Address              NVARCHAR(450),
    CreatedDate          DATETIME         NOT NULL,
    IsActive             BIT              DEFAULT 1,
    ExpirationPlanDate   DATETIME,
    ConcurrencyStamp			NVARCHAR(MAX)
    );
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ProviderService')
BEGIN
CREATE TABLE [ProviderService] (
    Id              UNIQUEIDENTIFIER NOT NULL,
    Rating          FLOAT,
    TotalBooking    FLOAT,
    StickerPrice    FLOAT,
    Description     NVARCHAR(MAX),
    IsAvailable     BIT             DEFAULT 0,
    IsVisible       BIT             DEFAULT 0,
    IsActive        BIT             DEFAULT 1,
    );
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'GigService')
BEGIN
CREATE TABLE [GigService] (
                              Id          UNIQUEIDENTIFIER NOT NULL,
                              ServiceName NVARCHAR(450)    NOT NULL,
    Description NVARCHAR(MAX),
    MinPrice    FLOAT            NOT NULL,
    MaxPrice    FLOAT            NOT NULL,
    Fee         FLOAT            NOT NULL,
    IsActive    BIT              DEFAULT 1,
    ConcurrencyStamp			NVARCHAR(MAX)
    );
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ServiceCategory')
BEGIN
CREATE TABLE [ServiceCategory] (
                                   Id           UNIQUEIDENTIFIER NOT NULL,
                                   CategoryName NVARCHAR(450)    NOT NULL,
    Description  NVARCHAR(MAX),
    IsActive     BIT              DEFAULT 1
    );
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ServiceImage')
BEGIN
CREATE TABLE [ServiceImage] (
                                Id        UNIQUEIDENTIFIER NOT NULL,
                                ImageUrl  NVARCHAR(MAX)    NOT NULL
    );
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Booking')
BEGIN
CREATE TABLE [Booking] (
                           Id               UNIQUEIDENTIFIER NOT NULL,
                           Apartment        NVARCHAR(255)    NOT NULL,
    StickerPrice     FLOAT            NOT NULL,
    FinalPrice       FLOAT            NOT NULL,
    Status           INT              NOT NULL,
    CreatedDate      DATETIME         NOT NULL,
    ProviderRating   FLOAT,
    ProviderReview   NVARCHAR(MAX),
    CustomerRating   FLOAT,
    CustomerReview   NVARCHAR(MAX),
    ConcurrencyStamp			NVARCHAR(MAX),
    );
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'BookingMessage')
BEGIN
CREATE TABLE [BookingMessage] (
  Id          UNIQUEIDENTIFIER NOT NULL,
  SenderName  NVARCHAR(450)    NOT NULL,
    Content     NVARCHAR(MAX)    NOT NULL,
    SentAt      DATETIME         NOT NULL
    );
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Badge')
BEGIN
CREATE TABLE [Badge] (
                         Id          UNIQUEIDENTIFIER NOT NULL,
                         BadgeName   NVARCHAR(450)    NOT NULL,
    Description NVARCHAR(MAX),
    Benefit     NVARCHAR(450),
    IsActive    BIT              DEFAULT 1,
    ConcurrencyStamp			NVARCHAR(MAX),
    );
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SubscriptionFee')
BEGIN
CREATE TABLE [SubscriptionFee] (
                                   Id          UNIQUEIDENTIFIER NOT NULL,
                                   Amount      FLOAT            DEFAULT 0,
                                   CreatedDate DATETIME         NOT NULL
);
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SubscriptionPlan')
BEGIN
CREATE TABLE [SubscriptionPlan] (
                                    Id           UNIQUEIDENTIFIER NOT NULL,
                                    Description  NVARCHAR(MAX),
    DurationType INT,
    Price        FLOAT,
    IsActive     BIT              DEFAULT 1,
    ConcurrencyStamp			NVARCHAR(MAX)
    );
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Transaction')
BEGIN
CREATE TABLE [Transaction] (
                               Id          UNIQUEIDENTIFIER NOT NULL,
                               Amount      FLOAT            DEFAULT 0,
                               Description NVARCHAR(MAX),
    CreatedDate DATETIME         NOT NULL,
    Status      INT              NOT NULL,
    ConcurrencyStamp			NVARCHAR(MAX),
    );
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Deposit')
BEGIN
CREATE TABLE [Deposit] (
                           Id            UNIQUEIDENTIFIER NOT NULL,
                           Amount        FLOAT            DEFAULT 0,
                           MadeDate      DATETIME         NOT NULL,
                           PaymentMethod NVARCHAR(255)    NOT NULL
    );
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Wallet')
BEGIN
CREATE TABLE [Wallet] (
                          Id       UNIQUEIDENTIFIER NOT NULL,
                          Balance  FLOAT            DEFAULT 0,
                          IsActive BIT              DEFAULT 1,
                          ConcurrencyStamp			NVARCHAR(MAX),
    );
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Building')
BEGIN
CREATE TABLE [Building] (
                            Id           UNIQUEIDENTIFIER NOT NULL,
                            BuildingName NVARCHAR(255)    NOT NULL,
    Note         NVARCHAR(MAX),
    IsActive     BIT              DEFAULT 1,
    ConcurrencyStamp			NVARCHAR(MAX),
    );
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'BookingFee')
BEGIN
CREATE TABLE [BookingFee] (
                              Id          UNIQUEIDENTIFIER NOT NULL,
                              Amount      FLOAT            DEFAULT 0,
                              CreatedDate DATETIME         NOT NULL
);
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Customer')
BEGIN
CREATE TABLE [Customer] (
                            Id           UNIQUEIDENTIFIER NOT NULL,
                            UserName     NVARCHAR(255)    NOT NULL,
    Password     NVARCHAR(MAX)    NOT NULL,
    Gender       NVARCHAR(63),
    ProfileImage NVARCHAR(MAX),
    FullName     NVARCHAR(450),
    Email        NVARCHAR(255)    NOT NULL,
    Phone        NVARCHAR(10),
    Address      NVARCHAR(450),
    CreatedDate  DATETIME         NOT NULL,
    IsActive     BIT              DEFAULT 1,
    ConcurrencyStamp			NVARCHAR(MAX),
    );
END




-- add primary key
ALTER TABLE [Provider]
    ADD CONSTRAINT PK_Provider PRIMARY KEY (Id);

ALTER TABLE [ProviderService]
    ADD CONSTRAINT PK_ProviderService PRIMARY KEY (Id);

ALTER TABLE [GigService]
    ADD CONSTRAINT PK_GigService PRIMARY KEY (Id);

ALTER TABLE [ServiceCategory]
    ADD CONSTRAINT PK_ServiceCategory PRIMARY KEY (Id);

ALTER TABLE [ServiceImage]
    ADD CONSTRAINT PK_ServiceImage PRIMARY KEY (Id);

ALTER TABLE [Booking]
    ADD CONSTRAINT PK_Booking PRIMARY KEY (Id);

ALTER TABLE [BookingMessage]
    ADD CONSTRAINT PK_BookingMessage PRIMARY KEY (Id);

ALTER TABLE [Badge]
    ADD CONSTRAINT PK_Badge PRIMARY KEY (Id);

ALTER TABLE [SubscriptionFee]
    ADD CONSTRAINT PK_SubscriptionFee PRIMARY KEY (Id);

ALTER TABLE [SubscriptionPlan]
    ADD CONSTRAINT PK_SubscriptionPlan PRIMARY KEY (Id);

ALTER TABLE [Transaction]
    ADD CONSTRAINT PK_Transaction PRIMARY KEY (Id);

ALTER TABLE [Deposit]
    ADD CONSTRAINT PK_Deposit PRIMARY KEY (Id);

ALTER TABLE [Wallet]
    ADD CONSTRAINT PK_Wallet PRIMARY KEY (Id);

ALTER TABLE [Building]
    ADD CONSTRAINT PK_Building PRIMARY KEY (Id);

ALTER TABLE [BookingFee]
    ADD CONSTRAINT PK_BookingFee PRIMARY KEY (Id);

ALTER TABLE [Customer]
    ADD CONSTRAINT PK_Customer PRIMARY KEY (Id);

-- add foreign key

ALTER TABLE [ProviderService]
    ADD ProviderId UNIQUEIDENTIFIER NOT NULL;

ALTER TABLE [ProviderService]
    ADD ServiceId UNIQUEIDENTIFIER NOT NULL;

ALTER TABLE [ServiceImage]
    ADD ProviderServiceId UNIQUEIDENTIFIER NOT NULL;

ALTER TABLE [GigService]
    ADD ServiceCategoryId UNIQUEIDENTIFIER NOT NULL;

ALTER TABLE [Booking]
    ADD CustomerId UNIQUEIDENTIFIER NOT NULL;

ALTER TABLE [Booking]
    ADD ProviderServiceId UNIQUEIDENTIFIER NOT NULL;

ALTER TABLE [Booking]
    ADD BuildingId UNIQUEIDENTIFIER NOT NULL;

ALTER TABLE [SubscriptionFee]
    ADD ProviderId UNIQUEIDENTIFIER NOT NULL;

ALTER TABLE [SubscriptionFee]
    ADD SubscriptionPlanId UNIQUEIDENTIFIER NOT NULL;

ALTER TABLE [Provider]
    ADD BuildingId UNIQUEIDENTIFIER NOT NULL;

ALTER TABLE [Provider]
    ADD BadgeId UNIQUEIDENTIFIER NOT NULL;

ALTER TABLE [Transaction]
    ADD WalletId UNIQUEIDENTIFIER NOT NULL;

ALTER TABLE [Transaction]
    ADD DepositId UNIQUEIDENTIFIER NOT NULL;

ALTER TABLE [Transaction]
    ADD BookingFeeId UNIQUEIDENTIFIER NOT NULL;

ALTER TABLE [Transaction]
    ADD SubscriptionFeeId UNIQUEIDENTIFIER NOT NULL;


ALTER TABLE [Wallet]
    ADD ProviderId UNIQUEIDENTIFIER NOT NULL;


ALTER TABLE [BookingFee]
    ADD BookingId UNIQUEIDENTIFIER NOT NULL;

ALTER TABLE [Deposit]
    ADD ProviderId UNIQUEIDENTIFIER NOT NULL;

ALTER TABLE [Customer]
    ADD BuildingId UNIQUEIDENTIFIER NOT NULL;

ALTER TABLE [BookingMessage]
    ADD BookingId UNIQUEIDENTIFIER NOT NULL;

-- add constraint

ALTER TABLE [BookingMessage]
    ADD FOREIGN KEY (BookingId) REFERENCES [Booking](Id)

ALTER TABLE [ProviderService]
    ADD FOREIGN KEY  (ProviderId) REFERENCES [Provider](Id)

ALTER TABLE [ProviderService]
    ADD FOREIGN KEY  (ServiceId) REFERENCES [GigService](Id)

ALTER TABLE [ServiceImage]
    ADD FOREIGN KEY  (ProviderServiceId) REFERENCES [ProviderService](Id)

ALTER TABLE [GigService]
    ADD FOREIGN KEY (ServiceCategoryId) REFERENCES [ServiceCategory](Id)

ALTER TABLE [Booking]
    ADD FOREIGN KEY (CustomerId) REFERENCES [Customer](Id)

ALTER TABLE [Booking]
    ADD FOREIGN KEY (ProviderServiceId) REFERENCES [ProviderService](Id)

ALTER TABLE [Booking]
    ADD FOREIGN KEY (BuildingId) REFERENCES [Building](Id)

ALTER TABLE [SubscriptionFee]
    ADD FOREIGN KEY (ProviderId) REFERENCES [Provider](Id)

ALTER TABLE [SubscriptionFee]
    ADD FOREIGN KEY (SubscriptionPlanId) REFERENCES [SubscriptionPlan](Id)

ALTER TABLE [Provider]
    ADD FOREIGN KEY (BuildingId) REFERENCES [Building](Id)

ALTER TABLE [Provider]
    ADD FOREIGN KEY (BadgeId) REFERENCES [Badge](Id)

ALTER TABLE [Transaction]
    ADD FOREIGN KEY (WalletId) REFERENCES [Wallet](Id)

ALTER TABLE [Transaction]
    ADD FOREIGN KEY (DepositId) REFERENCES [Deposit](Id)

ALTER TABLE [Transaction]
    ADD FOREIGN KEY (BookingFeeId) REFERENCES [BookingFee](Id)

ALTER TABLE [Transaction]
    ADD FOREIGN KEY (SubscriptionFeeId) REFERENCES [SubscriptionFee](Id)

ALTER TABLE [Wallet]
    ADD FOREIGN KEY (ProviderId) REFERENCES [Provider](Id)


ALTER TABLE [BookingFee]
    ADD FOREIGN KEY (BookingId) REFERENCES [Booking](Id)

ALTER TABLE [Deposit]
    ADD FOREIGN KEY (ProviderId) REFERENCES [Provider](Id)

ALTER TABLE [Customer]
    ADD FOREIGN KEY (BuildingId) REFERENCES [Building](Id)










