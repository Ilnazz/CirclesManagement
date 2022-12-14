USE [circles_management]
GO
/****** Object:  Table [dbo].[Circle]    Script Date: 18.10.2022 21:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Circle](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[IsWorking] [bit] NOT NULL,
 CONSTRAINT [PK_CIrcle] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Circle_Pupil]    Script Date: 18.10.2022 21:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Circle_Pupil](
	[CircleID] [int] NOT NULL,
	[PupilID] [int] NOT NULL,
	[IsAttending] [bit] NOT NULL,
 CONSTRAINT [PK_Circle_Pupil] PRIMARY KEY CLUSTERED 
(
	[CircleID] ASC,
	[PupilID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Classroom]    Script Date: 18.10.2022 21:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Classroom](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Number] [int] NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Classroom] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Grade]    Script Date: 18.10.2022 21:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Grade](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](4) NOT NULL,
 CONSTRAINT [PK_Grade] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lesson]    Script Date: 18.10.2022 21:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lesson](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TimetableID] [int] NOT NULL,
	[Date] [date] NOT NULL,
 CONSTRAINT [PK_Lesson] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lesson_Pupil]    Script Date: 18.10.2022 21:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lesson_Pupil](
	[LessonID] [int] NOT NULL,
	[PupilID] [int] NOT NULL,
	[WasInClass] [bit] NOT NULL,
 CONSTRAINT [PK_Lesson_Pupil] PRIMARY KEY CLUSTERED 
(
	[LessonID] ASC,
	[PupilID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pupil]    Script Date: 18.10.2022 21:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pupil](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[Patronymic] [nvarchar](50) NOT NULL,
	[GradeID] [int] NOT NULL,
 CONSTRAINT [PK_Pupil] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 18.10.2022 21:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teacher]    Script Date: 18.10.2022 21:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teacher](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[Patronymic] [nvarchar](50) NOT NULL,
	[IsWorking] [bit] NOT NULL,
 CONSTRAINT [PK_Teacher] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Timetable]    Script Date: 18.10.2022 21:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Timetable](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TeacherID] [int] NOT NULL,
	[ClassroomID] [int] NOT NULL,
	[CircleID] [int] NOT NULL,
	[WeekDayID] [int] NOT NULL,
	[Time] [time](7) NOT NULL,
 CONSTRAINT [PK_Timetable] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 18.10.2022 21:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Login] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[TeacherID] [int] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WeekDay]    Script Date: 18.10.2022 21:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WeekDay](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](11) NOT NULL,
 CONSTRAINT [PK_WeekDay] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Circle] ON 

INSERT [dbo].[Circle] ([ID], [Title], [IsWorking]) VALUES (1, N'Программирование ', 1)
INSERT [dbo].[Circle] ([ID], [Title], [IsWorking]) VALUES (2, N'Веселый спорт', 1)
INSERT [dbo].[Circle] ([ID], [Title], [IsWorking]) VALUES (3, N'Робототехника', 0)
INSERT [dbo].[Circle] ([ID], [Title], [IsWorking]) VALUES (4, N'Занимательная математика', 1)
INSERT [dbo].[Circle] ([ID], [Title], [IsWorking]) VALUES (5, N'Танцы ', 1)
INSERT [dbo].[Circle] ([ID], [Title], [IsWorking]) VALUES (6, N'Бокс ', 0)
INSERT [dbo].[Circle] ([ID], [Title], [IsWorking]) VALUES (7, N'Скорочтение  ', 1)
INSERT [dbo].[Circle] ([ID], [Title], [IsWorking]) VALUES (8, N'Изобразительное искусство', 1)
SET IDENTITY_INSERT [dbo].[Circle] OFF
GO
INSERT [dbo].[Circle_Pupil] ([CircleID], [PupilID], [IsAttending]) VALUES (1, 1, 1)
INSERT [dbo].[Circle_Pupil] ([CircleID], [PupilID], [IsAttending]) VALUES (1, 2, 1)
INSERT [dbo].[Circle_Pupil] ([CircleID], [PupilID], [IsAttending]) VALUES (1, 3, 1)
INSERT [dbo].[Circle_Pupil] ([CircleID], [PupilID], [IsAttending]) VALUES (1, 4, 0)
INSERT [dbo].[Circle_Pupil] ([CircleID], [PupilID], [IsAttending]) VALUES (1, 5, 1)
INSERT [dbo].[Circle_Pupil] ([CircleID], [PupilID], [IsAttending]) VALUES (1, 6, 1)
INSERT [dbo].[Circle_Pupil] ([CircleID], [PupilID], [IsAttending]) VALUES (1, 7, 1)
INSERT [dbo].[Circle_Pupil] ([CircleID], [PupilID], [IsAttending]) VALUES (2, 8, 0)
INSERT [dbo].[Circle_Pupil] ([CircleID], [PupilID], [IsAttending]) VALUES (2, 9, 1)
INSERT [dbo].[Circle_Pupil] ([CircleID], [PupilID], [IsAttending]) VALUES (2, 10, 1)
INSERT [dbo].[Circle_Pupil] ([CircleID], [PupilID], [IsAttending]) VALUES (2, 11, 1)
INSERT [dbo].[Circle_Pupil] ([CircleID], [PupilID], [IsAttending]) VALUES (2, 12, 0)
INSERT [dbo].[Circle_Pupil] ([CircleID], [PupilID], [IsAttending]) VALUES (2, 13, 1)
INSERT [dbo].[Circle_Pupil] ([CircleID], [PupilID], [IsAttending]) VALUES (2, 14, 1)
INSERT [dbo].[Circle_Pupil] ([CircleID], [PupilID], [IsAttending]) VALUES (3, 1, 1)
INSERT [dbo].[Circle_Pupil] ([CircleID], [PupilID], [IsAttending]) VALUES (3, 2, 0)
GO
SET IDENTITY_INSERT [dbo].[Classroom] ON 

INSERT [dbo].[Classroom] ([ID], [Number], [Title]) VALUES (1, 11, N'Кабинет с партами')
INSERT [dbo].[Classroom] ([ID], [Number], [Title]) VALUES (2, 20, N'Кабинет творчества')
INSERT [dbo].[Classroom] ([ID], [Number], [Title]) VALUES (3, 10, N'Спорт зал')
INSERT [dbo].[Classroom] ([ID], [Number], [Title]) VALUES (4, 21, N'Кабинет с ПК')
SET IDENTITY_INSERT [dbo].[Classroom] OFF
GO
SET IDENTITY_INSERT [dbo].[Grade] ON 

INSERT [dbo].[Grade] ([ID], [Title]) VALUES (1, N'1 А')
INSERT [dbo].[Grade] ([ID], [Title]) VALUES (2, N'1 Б')
INSERT [dbo].[Grade] ([ID], [Title]) VALUES (3, N'2 А')
INSERT [dbo].[Grade] ([ID], [Title]) VALUES (4, N'2 Б')
INSERT [dbo].[Grade] ([ID], [Title]) VALUES (5, N'3 А')
INSERT [dbo].[Grade] ([ID], [Title]) VALUES (6, N'3 Б')
INSERT [dbo].[Grade] ([ID], [Title]) VALUES (7, N'4 А')
INSERT [dbo].[Grade] ([ID], [Title]) VALUES (8, N'4 Б')
INSERT [dbo].[Grade] ([ID], [Title]) VALUES (9, N'5 А')
INSERT [dbo].[Grade] ([ID], [Title]) VALUES (10, N'5 Б')
INSERT [dbo].[Grade] ([ID], [Title]) VALUES (11, N'6 А')
INSERT [dbo].[Grade] ([ID], [Title]) VALUES (12, N'6 Б')
INSERT [dbo].[Grade] ([ID], [Title]) VALUES (13, N'7 А')
INSERT [dbo].[Grade] ([ID], [Title]) VALUES (14, N'7 Б')
INSERT [dbo].[Grade] ([ID], [Title]) VALUES (15, N'8 А')
INSERT [dbo].[Grade] ([ID], [Title]) VALUES (16, N'8 Б')
INSERT [dbo].[Grade] ([ID], [Title]) VALUES (17, N'9 А')
INSERT [dbo].[Grade] ([ID], [Title]) VALUES (18, N'9 Б')
INSERT [dbo].[Grade] ([ID], [Title]) VALUES (19, N'10 А')
INSERT [dbo].[Grade] ([ID], [Title]) VALUES (20, N'10 Б')
INSERT [dbo].[Grade] ([ID], [Title]) VALUES (21, N'11 А')
INSERT [dbo].[Grade] ([ID], [Title]) VALUES (22, N'11 Б')
SET IDENTITY_INSERT [dbo].[Grade] OFF
GO
SET IDENTITY_INSERT [dbo].[Lesson] ON 

INSERT [dbo].[Lesson] ([ID], [TimetableID], [Date]) VALUES (1, 1, CAST(N'2022-09-05' AS Date))
INSERT [dbo].[Lesson] ([ID], [TimetableID], [Date]) VALUES (2, 2, CAST(N'2022-09-05' AS Date))
INSERT [dbo].[Lesson] ([ID], [TimetableID], [Date]) VALUES (3, 3, CAST(N'2022-09-05' AS Date))
INSERT [dbo].[Lesson] ([ID], [TimetableID], [Date]) VALUES (4, 4, CAST(N'2022-09-06' AS Date))
INSERT [dbo].[Lesson] ([ID], [TimetableID], [Date]) VALUES (5, 5, CAST(N'2022-09-06' AS Date))
INSERT [dbo].[Lesson] ([ID], [TimetableID], [Date]) VALUES (6, 6, CAST(N'2022-09-07' AS Date))
INSERT [dbo].[Lesson] ([ID], [TimetableID], [Date]) VALUES (7, 7, CAST(N'2022-09-07' AS Date))
INSERT [dbo].[Lesson] ([ID], [TimetableID], [Date]) VALUES (8, 8, CAST(N'2022-09-07' AS Date))
SET IDENTITY_INSERT [dbo].[Lesson] OFF
GO
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (1, 1, 1)
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (1, 2, 1)
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (1, 3, 1)
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (1, 4, 1)
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (1, 5, 1)
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (1, 6, 1)
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (1, 7, 1)
GO
SET IDENTITY_INSERT [dbo].[Pupil] ON 

INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID]) VALUES (1, N'Астахова ', N'София ', N'Иванова', 1)
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID]) VALUES (2, N'Гончарова ', N'Ульяна ', N'Мирославовна ', 2)
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID]) VALUES (3, N'Спиридонова ', N'Василиса ', N'Марковна ', 3)
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID]) VALUES (4, N'Денисова ', N'Анастасия ', N'Данииловна ', 4)
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID]) VALUES (5, N'Никифоров ', N'Федор ', N'Никитич ', 5)
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID]) VALUES (6, N'Черкасов ', N'Артем ', N'Георгиевич', 6)
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID]) VALUES (7, N'Румянцев ', N'Роман ', N'Даниилович ', 7)
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID]) VALUES (8, N'Куликова ', N'Алена ', N'Александровна ', 8)
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID]) VALUES (9, N'Царев ', N'Алесандр ', N'Александрович ', 9)
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID]) VALUES (10, N'Антипова ', N'Анастасия ', N'Владимировна ', 10)
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID]) VALUES (11, N'Шаповалова ', N'Дарья ', N'Миронова ', 11)
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID]) VALUES (12, N'Ефимова ', N'Елизавета ', N'Ильинична ', 12)
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID]) VALUES (13, N'Симонов ', N'Всеволод ', N'Николаевич ', 13)
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID]) VALUES (14, N'Грачева ', N'Мария ', N'Львовна ', 14)
SET IDENTITY_INSERT [dbo].[Pupil] OFF
GO
SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([ID], [Title]) VALUES (1, N'AssociateDirectior')
INSERT [dbo].[Role] ([ID], [Title]) VALUES (2, N'Teacher')
SET IDENTITY_INSERT [dbo].[Role] OFF
GO
SET IDENTITY_INSERT [dbo].[Teacher] ON 

INSERT [dbo].[Teacher] ([ID], [LastName], [FirstName], [Patronymic], [IsWorking]) VALUES (1, N'Панкратов  ', N'Максим', N'Константинов', 1)
INSERT [dbo].[Teacher] ([ID], [LastName], [FirstName], [Patronymic], [IsWorking]) VALUES (2, N'Матвеева ', N'Анастасия ', N'Андреева', 1)
INSERT [dbo].[Teacher] ([ID], [LastName], [FirstName], [Patronymic], [IsWorking]) VALUES (3, N'Калачев', N'Артем', N'Алесандров ', 0)
INSERT [dbo].[Teacher] ([ID], [LastName], [FirstName], [Patronymic], [IsWorking]) VALUES (4, N'Иванова ', N'Ева', N'Дмитриевна', 1)
INSERT [dbo].[Teacher] ([ID], [LastName], [FirstName], [Patronymic], [IsWorking]) VALUES (5, N'Степанов ', N'Олег', N'Владимиров', 1)
INSERT [dbo].[Teacher] ([ID], [LastName], [FirstName], [Patronymic], [IsWorking]) VALUES (7, N'Алексеев', N'Денис', N'Олегович', 0)
INSERT [dbo].[Teacher] ([ID], [LastName], [FirstName], [Patronymic], [IsWorking]) VALUES (8, N'А', N'Б', N'В', 0)
SET IDENTITY_INSERT [dbo].[Teacher] OFF
GO
SET IDENTITY_INSERT [dbo].[Timetable] ON 

INSERT [dbo].[Timetable] ([ID], [TeacherID], [ClassroomID], [CircleID], [WeekDayID], [Time]) VALUES (1, 1, 1, 1, 1, CAST(N'08:00:00' AS Time))
INSERT [dbo].[Timetable] ([ID], [TeacherID], [ClassroomID], [CircleID], [WeekDayID], [Time]) VALUES (2, 2, 2, 1, 1, CAST(N'09:00:00' AS Time))
INSERT [dbo].[Timetable] ([ID], [TeacherID], [ClassroomID], [CircleID], [WeekDayID], [Time]) VALUES (3, 3, 2, 1, 1, CAST(N'10:00:00' AS Time))
INSERT [dbo].[Timetable] ([ID], [TeacherID], [ClassroomID], [CircleID], [WeekDayID], [Time]) VALUES (4, 4, 4, 1, 2, CAST(N'11:00:00' AS Time))
INSERT [dbo].[Timetable] ([ID], [TeacherID], [ClassroomID], [CircleID], [WeekDayID], [Time]) VALUES (5, 5, 1, 2, 2, CAST(N'12:00:00' AS Time))
INSERT [dbo].[Timetable] ([ID], [TeacherID], [ClassroomID], [CircleID], [WeekDayID], [Time]) VALUES (6, 1, 2, 2, 3, CAST(N'13:00:00' AS Time))
INSERT [dbo].[Timetable] ([ID], [TeacherID], [ClassroomID], [CircleID], [WeekDayID], [Time]) VALUES (7, 2, 3, 2, 3, CAST(N'14:00:00' AS Time))
INSERT [dbo].[Timetable] ([ID], [TeacherID], [ClassroomID], [CircleID], [WeekDayID], [Time]) VALUES (8, 3, 4, 2, 3, CAST(N'15:00:00' AS Time))
SET IDENTITY_INSERT [dbo].[Timetable] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([ID], [RoleID], [Name], [Login], [Password], [TeacherID]) VALUES (1, 1, N'Иванов Иван Иванович', N'ivanov_i_i', N'ivanov_i_i', NULL)
INSERT [dbo].[User] ([ID], [RoleID], [Name], [Login], [Password], [TeacherID]) VALUES (2, 2, N'Александров Александр Александрович', N'alexandrov_a_a', N'alexandrov_a_a', 1)
INSERT [dbo].[User] ([ID], [RoleID], [Name], [Login], [Password], [TeacherID]) VALUES (3, 2, N'Иванов И. И.', N'и', N'и', 2)
INSERT [dbo].[User] ([ID], [RoleID], [Name], [Login], [Password], [TeacherID]) VALUES (4, 2, N'А Б. В.', N'Г', N'Д', NULL)
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[WeekDay] ON 

INSERT [dbo].[WeekDay] ([ID], [Title]) VALUES (1, N'Понедельник')
INSERT [dbo].[WeekDay] ([ID], [Title]) VALUES (2, N'Вторник')
INSERT [dbo].[WeekDay] ([ID], [Title]) VALUES (3, N'Среда')
INSERT [dbo].[WeekDay] ([ID], [Title]) VALUES (4, N'Четверг')
INSERT [dbo].[WeekDay] ([ID], [Title]) VALUES (5, N'Пятница')
INSERT [dbo].[WeekDay] ([ID], [Title]) VALUES (6, N'Суббота')
INSERT [dbo].[WeekDay] ([ID], [Title]) VALUES (7, N'Воскресенье')
SET IDENTITY_INSERT [dbo].[WeekDay] OFF
GO
ALTER TABLE [dbo].[Circle_Pupil]  WITH CHECK ADD  CONSTRAINT [FK_Circle_Pupil_CIrcle] FOREIGN KEY([CircleID])
REFERENCES [dbo].[Circle] ([ID])
GO
ALTER TABLE [dbo].[Circle_Pupil] CHECK CONSTRAINT [FK_Circle_Pupil_CIrcle]
GO
ALTER TABLE [dbo].[Circle_Pupil]  WITH CHECK ADD  CONSTRAINT [FK_Circle_Pupil_Pupil] FOREIGN KEY([PupilID])
REFERENCES [dbo].[Pupil] ([ID])
GO
ALTER TABLE [dbo].[Circle_Pupil] CHECK CONSTRAINT [FK_Circle_Pupil_Pupil]
GO
ALTER TABLE [dbo].[Lesson]  WITH CHECK ADD  CONSTRAINT [FK_Lesson_Timetable] FOREIGN KEY([TimetableID])
REFERENCES [dbo].[Timetable] ([ID])
GO
ALTER TABLE [dbo].[Lesson] CHECK CONSTRAINT [FK_Lesson_Timetable]
GO
ALTER TABLE [dbo].[Lesson_Pupil]  WITH CHECK ADD  CONSTRAINT [FK_Lesson_Pupil_Lesson] FOREIGN KEY([LessonID])
REFERENCES [dbo].[Lesson] ([ID])
GO
ALTER TABLE [dbo].[Lesson_Pupil] CHECK CONSTRAINT [FK_Lesson_Pupil_Lesson]
GO
ALTER TABLE [dbo].[Lesson_Pupil]  WITH CHECK ADD  CONSTRAINT [FK_Lesson_Pupil_Pupil] FOREIGN KEY([PupilID])
REFERENCES [dbo].[Pupil] ([ID])
GO
ALTER TABLE [dbo].[Lesson_Pupil] CHECK CONSTRAINT [FK_Lesson_Pupil_Pupil]
GO
ALTER TABLE [dbo].[Pupil]  WITH CHECK ADD  CONSTRAINT [FK_Pupil_Grade] FOREIGN KEY([GradeID])
REFERENCES [dbo].[Grade] ([ID])
GO
ALTER TABLE [dbo].[Pupil] CHECK CONSTRAINT [FK_Pupil_Grade]
GO
ALTER TABLE [dbo].[Timetable]  WITH CHECK ADD  CONSTRAINT [FK_Timetable_CIrcle] FOREIGN KEY([CircleID])
REFERENCES [dbo].[Circle] ([ID])
GO
ALTER TABLE [dbo].[Timetable] CHECK CONSTRAINT [FK_Timetable_CIrcle]
GO
ALTER TABLE [dbo].[Timetable]  WITH CHECK ADD  CONSTRAINT [FK_Timetable_Classroom] FOREIGN KEY([ClassroomID])
REFERENCES [dbo].[Classroom] ([ID])
GO
ALTER TABLE [dbo].[Timetable] CHECK CONSTRAINT [FK_Timetable_Classroom]
GO
ALTER TABLE [dbo].[Timetable]  WITH CHECK ADD  CONSTRAINT [FK_Timetable_Teacher] FOREIGN KEY([TeacherID])
REFERENCES [dbo].[Teacher] ([ID])
GO
ALTER TABLE [dbo].[Timetable] CHECK CONSTRAINT [FK_Timetable_Teacher]
GO
ALTER TABLE [dbo].[Timetable]  WITH CHECK ADD  CONSTRAINT [FK_Timetable_WeekDay] FOREIGN KEY([WeekDayID])
REFERENCES [dbo].[WeekDay] ([ID])
GO
ALTER TABLE [dbo].[Timetable] CHECK CONSTRAINT [FK_Timetable_WeekDay]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Teacher] FOREIGN KEY([TeacherID])
REFERENCES [dbo].[Teacher] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Teacher]
GO
