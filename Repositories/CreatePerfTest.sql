﻿IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PerfTest' and xtype='U')
BEGIN
	CREATE TABLE [dbo].[PerfTest](
		[ID] [uniqueidentifier] NOT NULL,
		[Data] [varchar](max) NULL,
	 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY])

	ALTER TABLE [dbo].[PerfTest] ADD  DEFAULT (newsequentialid()) FOR [ID]
END