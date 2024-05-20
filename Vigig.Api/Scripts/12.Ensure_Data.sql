
INSERT INTO VigigRole (Id,Name,NormalizedName)
VALUES
    ('f7a7da6d-5ad5-43a2-ae07-205e370cef36', 'Admin', 'ADMIN'),
    ('23b5db3f-338d-4fcf-9e2a-52d5156d52b8', 'Provider', 'PROVIDER'),
    ('74844004-5114-439f-b173-71282eb65181', 'Client', 'CLIENT'),
    ('c7c6d763-de4e-426a-9e0f-737b090cfe93', 'Staff', 'STAFF')

INSERT INTO Building (Id,BuildingName,Note,IsActive,ConcurrencyStamp)
VALUES ('e9f94484-6bf6-43eb-4827-08dc73e1398f','S100','',1,'f76eb0ce-43ce-4b20-9e87-47b9d38d55fc')


INSERT INTO Badge (Id, BadgeName, Description, Benefit, IsActive, ConcurrencyStamp)
VALUES
    ('b3f23a54-70fd-4eb6-5d65-08dc788d384d', 'Promising Provider', 'Dành cho những provider mới tham gia VinGiG. Badge kéo dài trong 1 tuần.', 0.9, 1, 'c96aa422-d6b7-4f7b-8e9d-b1fd11dbd75f'),
    ('efffe6e8-9f90-4384-5d66-08dc788d384d', 'Preferred Provider', 'Dành cho những provider nào có trên 60 booking/ tháng và rating > 4 sao. Badge kéo dài trong 1 tháng', 0.95, 1, 'a78d0f7e-5e0e-445d-8321-89f7b5abc103'),
    ('7ca9319d-679d-4b94-5d67-08dc788d384d', 'Top-rated Provider', 'Dành cho những provider nào có trên 80 booking/ tháng và rating > 4.5 sao. Badge kéo dài trong 1 tháng', 0.9, 1, '834e50d2-b225-4a66-8552-14a09f045c12'),
    ('a4b82d35-b2ee-46ee-5d68-08dc788d384d', 'Best Provider', 'Dành cho 1 provider có lượt booking cao nhất của 1 tháng. Badge kéo dài trong 1 tháng', 0.8, 1, '6909ae94-e826-4ece-9a28-0dba7997ba02'),
    ('c965e2d2-3524-48bc-5d69-08dc788d384d', 'VinGiG Certified', 'Những providers đã được sàn VinGiG kiểm duyệt về chất lượng và các ràng buộc khác để đảm bảo chất lượng của các dịch vụ. Rank cao nhất.', 0.8, 1, '710f1e60-51fa-4c95-85ea-aa428322df94'),
    ('fd648813-9ae3-4870-5d6a-08dc788d384d', 'Non', 'default badge cho provider còn lại', 1, 1, '87756402-7e60-40da-ac81-e354deeb3995')


INSERT INTO VigigUser (Id, UserName, NormalizedUserName, Password, Gender, ProfileImage, Rating, FullName, Email, NormalizedEmail, EmailConfirmed, Phone, Address, CreatedDate, IsActive, ExpirationPlanDate, ConcurrencyStamp,BuildingId,BadgeId)
VALUES
    ('402e0e9c-c48f-49c6-903d-08dc7883df54', 'haiclient', 'HAICLIENT', 'gir2FSSpL06p1qIDJgw5KM15BfHINhfev/ccKIEAgNdLg6Ml', NULL, NULL, NULL, NULL, 'haiclient@gmail.com', 'HAICLIENT@GMAIL.COM', 0, NULL, NULL, '2024-05-20 11:18:16.190', 1, NULL, '582bc127-1fe8-43af-963b-245719bb08af','e9f94484-6bf6-43eb-4827-08dc73e1398f',NULL),
    ('cc9f4164-6aee-4b5a-903e-08dc7883df54', 'haistaff', 'HAISTAFF', '40Xo0GsySK+ctWOs+2HP/w3hFfpX7qJVuLyga4cd8e8v1E/U', NULL, NULL, NULL, NULL, 'haistaff@gmail.com', 'HAISTAFF@GMAIL.COM', 0, NULL, NULL, '2024-05-20 11:19:45.847', 1, NULL, 'a7fd6c07-051a-450b-be03-bba8c0b63914','e9f94484-6bf6-43eb-4827-08dc73e1398f',NULL),
    ('e286b46f-66bc-4950-66bc-08dc78988c2f', 'hunghaiclient1', 'HUNGHAICLIENT1', 'uc4Bj36ZF5j6DaqgTunMjiSOIhP3b0EHUU4iYHWVziLuQimM', NULL, NULL, NULL, NULL, 'hunghaiclient1@gmail.com', 'HUNGHAICLIENT1@GMAIL.COM', 0, NULL, NULL, '2024-05-20 13:46:16.167', 1, NULL, '6cfdba99-908d-4be3-81a7-67ed1fed8bcf','e9f94484-6bf6-43eb-4827-08dc73e1398f',NULL),
    ('3c577aa1-86b6-4ff5-66bd-08dc78988c2f', 'hunghaiprovider1', 'HUNGHAIPROVIDER1', '0SNQ5OwdE8+s1TEpmFdZZNR8UzO/atUBKDgI+rg9/ruFWrlO', NULL, NULL, NULL, NULL, 'hunghaiprovider1@gmail.com', 'HUNGHAIPROVIDER1@GMAIL.COM', 0, NULL, NULL, '2024-05-20 13:46:51.870', 1, NULL, '14a5e30f-18d6-496c-a16f-ab2c59f8f9c6','e9f94484-6bf6-43eb-4827-08dc73e1398f','b3f23a54-70fd-4eb6-5d65-08dc788d384d'),
    ('bdb0c699-26d0-43fa-4eae-08dc789aef56', 'hunghaiprovider2', 'HUNGHAIPROVIDER2', '+e0CTui/MTzZTSDkjS5tyGkoVXY02SeiKSoIKe0YhSA3LshB', NULL, NULL, NULL, NULL, 'hunghaiprovider2@gmail.com', 'HUNGHAIPROVIDER2@GMAIL.COM', 0, NULL, NULL, '2024-05-20 14:03:21.523', 1, NULL, '0240b8e8-69bc-4a10-8079-62e9a27b7a17','e9f94484-6bf6-43eb-4827-08dc73e1398f','b3f23a54-70fd-4eb6-5d65-08dc788d384d'),
    ('d126ffae-7d5a-4aed-02eb-08dc789bd93f', 'provider1', 'PROVIDER1', 'F5YiPvW576GilmpMQ34WzfVns4BL4PsBZSt1yxkkgPXUoIMq', NULL, NULL, NULL, NULL, 'provider1@gmail.com', 'PROVIDER1@GMAIL.COM', 0, NULL, NULL, '2024-05-20 14:09:53.890', 1, NULL, 'dc833c51-a584-4acb-bbc3-493eaa2a361a','e9f94484-6bf6-43eb-4827-08dc73e1398f','b3f23a54-70fd-4eb6-5d65-08dc788d384d'),
    ('cdae0d36-2a4e-4ca6-bf4b-08dc78a6a64b', 'haiprovider', 'HAIPROVIDER', 'pnANP0nofE8qKOHUrPgyFemC8uQ8WIrV7MMCTu4cwVCckkGU', NULL, NULL, NULL, NULL, 'haiprovider@gmail.com', 'HAIPROVIDER@GMAIL.COM', 0, NULL, NULL, '2024-05-20 15:27:12.880', 1, NULL, 'd0214261-7c70-46e3-b5fe-4852ff23fac9','e9f94484-6bf6-43eb-4827-08dc73e1398f','b3f23a54-70fd-4eb6-5d65-08dc788d384d')

INSERT INTO UserRole (UserId, RoleId)
VALUES
    ('402e0e9c-c48f-49c6-903d-08dc7883df54', '74844004-5114-439f-b173-71282eb65181'),
    ('cc9f4164-6aee-4b5a-903e-08dc7883df54', 'c7c6d763-de4e-426a-9e0f-737b090cfe93'),
    ('e286b46f-66bc-4950-66bc-08dc78988c2f', '74844004-5114-439f-b173-71282eb65181'),
    ('3c577aa1-86b6-4ff5-66bd-08dc78988c2f', '23b5db3f-338d-4fcf-9e2a-52d5156d52b8'),
    ('bdb0c699-26d0-43fa-4eae-08dc789aef56', '23b5db3f-338d-4fcf-9e2a-52d5156d52b8'),
    ('d126ffae-7d5a-4aed-02eb-08dc789bd93f', '23b5db3f-338d-4fcf-9e2a-52d5156d52b8'),
    ('cdae0d36-2a4e-4ca6-bf4b-08dc78a6a64b', '23b5db3f-338d-4fcf-9e2a-52d5156d52b8')

INSERT INTO UserToken (UserId, LoginProvider, Name, Value)
VALUES
    ('cc9f4164-6aee-4b5a-903e-08dc7883df54', 'VigigApp', 'RefreshToken', '85b787f9-e3b3-498c-8865-18b513845815'),
    ('d126ffae-7d5a-4aed-02eb-08dc789bd93f', 'VigigApp', 'RefreshToken', 'a3b6bd8d-c62b-403d-a47b-abde36915dbc'),
    ('cdae0d36-2a4e-4ca6-bf4b-08dc78a6a64b', 'VigigApp', 'RefreshToken', '5f460691-285d-4528-a790-b30bf89648cb')

INSERT INTO ServiceCategory VALUES ('fbd2f4c8-a84e-48a9-26d2-08dc788428f3', N'Máy Lạnh', NULL, 1);

INSERT INTO GigService (Id, ServiceName, Description, MinPrice, MaxPrice, Fee, IsActive, ConcurrencyStamp, ServiceCategoryId)
VALUES
    ('ed822221-e281-4305-15c9-08dc788524ed', 'Kiểm tra máy lạnh', 'Kiểm tra máy lạnh còn chạy được không, bụi bẩn như thế nào, gas, đầu nóng đầu lạnh', 20000, 50000, 10000, 1, NULL, 'fbd2f4c8-a84e-48a9-26d2-08dc788428f3'),
    ('60cd2f73-6fb9-4028-15ca-08dc788524ed', 'Vệ sinh máy lạnh', 'Tiến hành tháo giỡ giàn áo, vệ sinh đầu lạnh và đầu nóng', 20000, 50000, 10000, 1, NULL, 'fbd2f4c8-a84e-48a9-26d2-08dc788428f3'),
    ('f97c60a6-78d8-4d35-15cb-08dc788524ed', 'Sửa chữa máy lạnh', 'Sửa chữa các vấn đề liên quan đến máy lạnh', 20000, 50000, 10000, 1, NULL, 'fbd2f4c8-a84e-48a9-26d2-08dc788428f3'),
    ('d3a6686a-cbec-46bd-15cc-08dc788524ed', 'Bơm gas máy lạnh', 'Tiến hành đo lường ga của máy lạnh và bơm đầy', 20000, 50000, 10000, 1, NULL, 'fbd2f4c8-a84e-48a9-26d2-08dc788428f3')

INSERT INTO ProviderService (Id, Rating, TotalBooking, StickerPrice, Description, IsAvailable, IsVisible, IsActive,ProviderId,ServiceId)
VALUES
    ('1e0bf8c8-73bd-415b-6742-08dc78a9439f', 0, 0, 50000, 'Sieu hot this summer', 1, 1, 1,'d126ffae-7d5a-4aed-02eb-08dc789bd93f','ed822221-e281-4305-15c9-08dc788524ed'),
    ('a327ca68-a6bf-45cb-4e7e-08dc78ab7dbe', 0, 0, 50000, 'Sieu hot this summer', 1, 1, 1,'d126ffae-7d5a-4aed-02eb-08dc789bd93f','d3a6686a-cbec-46bd-15cc-08dc788524ed')

select * from ProviderService








