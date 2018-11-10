CREATE TABLE [dbo].[NotificationQueue ](
    [id_queue] [int] IDENTITY(1,1) NOT NULL,
    [create_date] [datetime] NOT NULL,
    [json_data] [nvarchar](max) NOT NULL,
    [queue_status] [int] NOT NULL,
	[id_type][int] NOT NULL
) ON [PRIMARY]
GO

CREATE UNIQUE CLUSTERED INDEX [PK_NotificationQueue] ON [dbo].[NotificationQueue]
(
    [id_queue] ASC
)
GO
CREATE NONCLUSTERED INDEX [IX_NotificationQueue] ON [dbo].[NotificationQueue]
(
    [create_date] ASC,
    [queue_status] ASC
)
INCLUDE ( [json_data])
GO
