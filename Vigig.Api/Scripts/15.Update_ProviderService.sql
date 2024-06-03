ALTER TABLE [ProviderService]
ADD RatingCount INT

-- Drop the default constraint
ALTER TABLE [ProviderService]
    DROP CONSTRAINT DF__ProviderS__IsAva__38996AB5;

-- Drop the column
ALTER TABLE [ProviderService]
    DROP COLUMN IsAvailable;

