SET IDENTITY_INSERT [dbo].[Red] ON
INSERT INTO [dbo].[Red] ([RedID], [nombre]) VALUES (1, N'Red Ethereum')
INSERT INTO [dbo].[Red] ([RedID], [nombre]) VALUES (2, N'Red Binance')
INSERT INTO [dbo].[Red] ([RedID], [nombre]) VALUES (3, N'Red Bitcoin')
SET IDENTITY_INSERT [dbo].[Red] OFF



SET IDENTITY_INSERT [dbo].[Criptomoneda] ON
INSERT INTO [dbo].[Criptomoneda] ([ID], [Nombre], [Precio], [PorcentajeVariacion], [Capitalizacion], [RedID], [CantidadAComprar], [NombreRed], [CantidadAVender]) VALUES (1, N'BNB', 445, -0.62, 74000000, 2, 50, 2, 30)
INSERT INTO [dbo].[Criptomoneda] ([ID], [Nombre], [Precio], [PorcentajeVariacion], [Capitalizacion], [RedID], [CantidadAComprar], [NombreRed], [CantidadAVender]) VALUES (2, N'Bitcoin', 52353, -1, 988000000, 3, 1, 3, 3)
INSERT INTO [dbo].[Criptomoneda] ([ID], [Nombre], [Precio], [PorcentajeVariacion], [Capitalizacion], [RedID], [CantidadAComprar], [NombreRed], [CantidadAVender]) VALUES (3, N'Ethereum', 3656, 1.05, 434000000, 1, 25, 1, 22)
INSERT INTO [dbo].[Criptomoneda] ([ID], [Nombre], [Precio], [PorcentajeVariacion], [Capitalizacion], [RedID], [CantidadAComprar], [NombreRed], [CantidadAVender]) VALUES (4, N'BUSD', 1, -0.05, 11500000, 2, 30000, 2, 25000)
INSERT INTO [dbo].[Criptomoneda] ([ID], [Nombre], [Precio], [PorcentajeVariacion], [Capitalizacion], [RedID], [CantidadAComprar], [NombreRed], [CantidadAVender]) VALUES (5, N'Wrapped BNB', 430, -0.62, 3000000, 2, 50, 2, 60)
INSERT INTO [dbo].[Criptomoneda] ([ID], [Nombre], [Precio], [PorcentajeVariacion], [Capitalizacion], [RedID], [CantidadAComprar], [NombreRed], [CantidadAVender]) VALUES (6, N'AXS', 2, 5, 56000000, 1, 15000, 1, 1000)
INSERT INTO [dbo].[Criptomoneda] ([ID], [Nombre], [Precio], [PorcentajeVariacion], [Capitalizacion], [RedID], [CantidadAComprar], [NombreRed], [CantidadAVender]) VALUES (7, N'Wrapped Bitcoin', 52313, -0.01, 12000000, 3, 1, 3, 1)
INSERT INTO [dbo].[Criptomoneda] ([ID], [Nombre], [Precio], [PorcentajeVariacion], [Capitalizacion], [RedID], [CantidadAComprar], [NombreRed], [CantidadAVender]) VALUES (8, N'Bitcoin Cash', 513, 1.58, 9700000, 3, 50, 3, 55)
SET IDENTITY_INSERT [dbo].[Criptomoneda] OFF