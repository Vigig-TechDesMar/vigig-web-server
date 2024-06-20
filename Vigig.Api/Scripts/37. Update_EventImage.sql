ALTER TABLE EventImage 
    DROP COLUMN StartDate

ALTER TABLE EventImage
    DROP COLUMN EndDate

ALTER TABLE EventImage
    DROP COLUMN Field

ALTER TABLE EventImage
    ADD ImageUrl NVARCHAR(MAX) NOT NULL
         
         