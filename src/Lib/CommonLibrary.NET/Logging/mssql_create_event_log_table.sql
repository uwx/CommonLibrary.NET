/****** Object:  Table [dbo].[event_log]    Script Date: 12/08/2009 22:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[event_log](
	[id] [int] NOT NULL IDENTITY(1,1),
	[log_level] [int] NOT NULL,
	[application] [nvarchar](255) NULL,
	[message] [nvarchar](255) NULL,
	[exception] [nvarchar](255) NULL,
	[computer] [nvarchar](255) NULL,
	[user_name] [nvarchar](255) NULL,
	[timestamp] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_event_log] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[event_log] ADD  CONSTRAINT [DF_log_timestamp]  DEFAULT (getdate()) FOR [timestamp]
GO


