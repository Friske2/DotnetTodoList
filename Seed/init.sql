create table Todo
(
    Id          int identity
        primary key,
    Title       nvarchar(255)                   not null,
    Description nvarchar(max),
    IsCompleted bit       default 0             not null,
    Priority    tinyint   default 1             not null,
    DueDate     datetime2,
    CreatedAt   datetime2 default sysdatetime() not null,
    UpdatedAt   datetime2 default sysdatetime() not null,
    DeletedAt   datetime2
)
    go

create index IX_Todo_IsCompleted_DueDate
    on Todo (IsCompleted, DueDate)
    where [DeletedAt] IS NULL
go

-- Trigger to auto-update UpdatedAt on row change
CREATE TRIGGER trg_Todo_UpdatedAt
    ON Todo
    AFTER UPDATE
              AS
BEGIN
    SET NOCOUNT ON;
UPDATE Todo
SET UpdatedAt = SYSDATETIME()
    FROM Todo t
    INNER JOIN inserted i ON t.Id = i.Id;
END;
go

