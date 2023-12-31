USE [master]
GO
/****** Object:  Database [cm_generated]    Script Date: 31.12.2023 11:43:06 ******/
CREATE DATABASE [cm_generated]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'cm_generated', FILENAME = N'C:\Users\Ильназ\cm_generated.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'cm_generated_log', FILENAME = N'C:\Users\Ильназ\cm_generated_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [cm_generated] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [cm_generated].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [cm_generated] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [cm_generated] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [cm_generated] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [cm_generated] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [cm_generated] SET ARITHABORT OFF 
GO
ALTER DATABASE [cm_generated] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [cm_generated] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [cm_generated] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [cm_generated] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [cm_generated] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [cm_generated] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [cm_generated] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [cm_generated] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [cm_generated] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [cm_generated] SET  DISABLE_BROKER 
GO
ALTER DATABASE [cm_generated] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [cm_generated] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [cm_generated] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [cm_generated] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [cm_generated] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [cm_generated] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [cm_generated] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [cm_generated] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [cm_generated] SET  MULTI_USER 
GO
ALTER DATABASE [cm_generated] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [cm_generated] SET DB_CHAINING OFF 
GO
ALTER DATABASE [cm_generated] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [cm_generated] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [cm_generated] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [cm_generated] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [cm_generated] SET QUERY_STORE = OFF
GO
USE [cm_generated]
GO
/****** Object:  Table [dbo].[Circle]    Script Date: 31.12.2023 11:43:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Circle](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[IsWorking] [bit] NOT NULL,
	[MaxNumberOfPupils] [int] NOT NULL,
 CONSTRAINT [PK_Circles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Classroom]    Script Date: 31.12.2023 11:43:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Classroom](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Number] [int] NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Classrooms] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Grade]    Script Date: 31.12.2023 11:43:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Grade](
	[ID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Title] [nvarchar](4) NOT NULL,
 CONSTRAINT [PK_Grades] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Group]    Script Date: 31.12.2023 11:43:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Group](
	[ID] [int] NOT NULL,
	[TeacherID] [int] NOT NULL,
	[CircleID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Group_Pupil]    Script Date: 31.12.2023 11:43:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Group_Pupil](
	[GroupID] [int] NOT NULL,
	[PupilID] [int] NOT NULL,
	[IsAttending] [bit] NOT NULL,
 CONSTRAINT [PK_Group_Pupil] PRIMARY KEY CLUSTERED 
(
	[GroupID] ASC,
	[PupilID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lesson]    Script Date: 31.12.2023 11:43:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lesson](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TimetableID] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[IsConducted] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Lessons] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lesson_Pupil]    Script Date: 31.12.2023 11:43:06 ******/
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
/****** Object:  Table [dbo].[Pupil]    Script Date: 31.12.2023 11:43:06 ******/
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
	[IsStudying] [bit] NOT NULL,
 CONSTRAINT [PK_Pupils] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 31.12.2023 11:43:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teacher]    Script Date: 31.12.2023 11:43:06 ******/
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
 CONSTRAINT [PK_Teachers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Timetable]    Script Date: 31.12.2023 11:43:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Timetable](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ClassroomID] [int] NOT NULL,
	[WeekDayID] [int] NOT NULL,
	[Time] [time](7) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[GroupID] [int] NOT NULL,
 CONSTRAINT [PK_Timetables] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 31.12.2023 11:43:06 ******/
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
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WeekDay]    Script Date: 31.12.2023 11:43:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WeekDay](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](11) NOT NULL,
 CONSTRAINT [PK_WeekDays] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Circle] ON 
GO
INSERT [dbo].[Circle] ([ID], [Title], [IsWorking], [MaxNumberOfPupils]) VALUES (1, N'Программирование ', 1, 10)
GO
INSERT [dbo].[Circle] ([ID], [Title], [IsWorking], [MaxNumberOfPupils]) VALUES (2, N'Веселый спорт', 1, 15)
GO
INSERT [dbo].[Circle] ([ID], [Title], [IsWorking], [MaxNumberOfPupils]) VALUES (3, N'Робототехника', 1, 15)
GO
INSERT [dbo].[Circle] ([ID], [Title], [IsWorking], [MaxNumberOfPupils]) VALUES (4, N'Занимательная математика', 1, 10)
GO
INSERT [dbo].[Circle] ([ID], [Title], [IsWorking], [MaxNumberOfPupils]) VALUES (5, N'Танцы ', 1, 10)
GO
INSERT [dbo].[Circle] ([ID], [Title], [IsWorking], [MaxNumberOfPupils]) VALUES (6, N'Бокс ', 1, 15)
GO
INSERT [dbo].[Circle] ([ID], [Title], [IsWorking], [MaxNumberOfPupils]) VALUES (7, N'Скорочтение  ', 1, 15)
GO
INSERT [dbo].[Circle] ([ID], [Title], [IsWorking], [MaxNumberOfPupils]) VALUES (8, N'Изобразительное искусство', 0, 15)
GO
INSERT [dbo].[Circle] ([ID], [Title], [IsWorking], [MaxNumberOfPupils]) VALUES (1002, N'Бисероплетение', 1, 20)
GO
SET IDENTITY_INSERT [dbo].[Circle] OFF
GO
SET IDENTITY_INSERT [dbo].[Classroom] ON 
GO
INSERT [dbo].[Classroom] ([ID], [Number], [Title], [IsActive]) VALUES (1, 11, N'Кабинет с партами', 1)
GO
INSERT [dbo].[Classroom] ([ID], [Number], [Title], [IsActive]) VALUES (2, 20, N'Кабинет творчества', 1)
GO
INSERT [dbo].[Classroom] ([ID], [Number], [Title], [IsActive]) VALUES (3, 10, N'Спорт зал', 1)
GO
INSERT [dbo].[Classroom] ([ID], [Number], [Title], [IsActive]) VALUES (4, 21, N'Кабинет с ПК', 1)
GO
SET IDENTITY_INSERT [dbo].[Classroom] OFF
GO
INSERT [dbo].[Grade] ([ID], [IsActive], [Title]) VALUES (1, 1, N'1 А')
GO
INSERT [dbo].[Grade] ([ID], [IsActive], [Title]) VALUES (2, 1, N'1 Б')
GO
INSERT [dbo].[Grade] ([ID], [IsActive], [Title]) VALUES (3, 1, N'2 А')
GO
INSERT [dbo].[Grade] ([ID], [IsActive], [Title]) VALUES (4, 1, N'2 Б')
GO
INSERT [dbo].[Grade] ([ID], [IsActive], [Title]) VALUES (5, 1, N'3 А')
GO
INSERT [dbo].[Grade] ([ID], [IsActive], [Title]) VALUES (6, 1, N'3 Б')
GO
INSERT [dbo].[Grade] ([ID], [IsActive], [Title]) VALUES (7, 1, N'4 А')
GO
INSERT [dbo].[Grade] ([ID], [IsActive], [Title]) VALUES (8, 1, N'4 Б')
GO
INSERT [dbo].[Grade] ([ID], [IsActive], [Title]) VALUES (9, 1, N'5 А')
GO
INSERT [dbo].[Grade] ([ID], [IsActive], [Title]) VALUES (10, 1, N'5 Б')
GO
INSERT [dbo].[Grade] ([ID], [IsActive], [Title]) VALUES (11, 1, N'6 А')
GO
INSERT [dbo].[Grade] ([ID], [IsActive], [Title]) VALUES (12, 1, N'6 Б')
GO
INSERT [dbo].[Grade] ([ID], [IsActive], [Title]) VALUES (13, 1, N'7 А')
GO
INSERT [dbo].[Grade] ([ID], [IsActive], [Title]) VALUES (14, 1, N'7 Б')
GO
INSERT [dbo].[Grade] ([ID], [IsActive], [Title]) VALUES (15, 1, N'8 А')
GO
INSERT [dbo].[Grade] ([ID], [IsActive], [Title]) VALUES (16, 1, N'8 Б')
GO
INSERT [dbo].[Grade] ([ID], [IsActive], [Title]) VALUES (17, 1, N'9 А')
GO
INSERT [dbo].[Grade] ([ID], [IsActive], [Title]) VALUES (18, 1, N'9 Б')
GO
INSERT [dbo].[Grade] ([ID], [IsActive], [Title]) VALUES (19, 1, N'10 А')
GO
INSERT [dbo].[Grade] ([ID], [IsActive], [Title]) VALUES (20, 1, N'10 Б')
GO
INSERT [dbo].[Grade] ([ID], [IsActive], [Title]) VALUES (21, 1, N'11 А')
GO
INSERT [dbo].[Grade] ([ID], [IsActive], [Title]) VALUES (22, 1, N'11 Б')
GO
INSERT [dbo].[Group] ([ID], [TeacherID], [CircleID], [IsActive]) VALUES (1, 1, 1, 1)
GO
INSERT [dbo].[Group] ([ID], [TeacherID], [CircleID], [IsActive]) VALUES (2, 2, 2, 1)
GO
INSERT [dbo].[Group] ([ID], [TeacherID], [CircleID], [IsActive]) VALUES (3, 3, 3, 1)
GO
INSERT [dbo].[Group] ([ID], [TeacherID], [CircleID], [IsActive]) VALUES (4, 4, 4, 1)
GO
INSERT [dbo].[Group] ([ID], [TeacherID], [CircleID], [IsActive]) VALUES (5, 5, 5, 1)
GO
INSERT [dbo].[Group_Pupil] ([GroupID], [PupilID], [IsAttending]) VALUES (1, 15, 1)
GO
INSERT [dbo].[Group_Pupil] ([GroupID], [PupilID], [IsAttending]) VALUES (1, 16, 1)
GO
INSERT [dbo].[Group_Pupil] ([GroupID], [PupilID], [IsAttending]) VALUES (1, 17, 1)
GO
INSERT [dbo].[Group_Pupil] ([GroupID], [PupilID], [IsAttending]) VALUES (1, 18, 1)
GO
INSERT [dbo].[Group_Pupil] ([GroupID], [PupilID], [IsAttending]) VALUES (1, 20, 1)
GO
INSERT [dbo].[Group_Pupil] ([GroupID], [PupilID], [IsAttending]) VALUES (1, 26, 1)
GO
INSERT [dbo].[Group_Pupil] ([GroupID], [PupilID], [IsAttending]) VALUES (2, 18, 1)
GO
INSERT [dbo].[Group_Pupil] ([GroupID], [PupilID], [IsAttending]) VALUES (2, 19, 1)
GO
INSERT [dbo].[Group_Pupil] ([GroupID], [PupilID], [IsAttending]) VALUES (3, 20, 1)
GO
SET IDENTITY_INSERT [dbo].[Lesson] ON 
GO
INSERT [dbo].[Lesson] ([ID], [TimetableID], [Date], [IsConducted], [IsActive]) VALUES (1, 1, CAST(N'2023-12-05T16:18:18.933' AS DateTime), 0, 0)
GO
INSERT [dbo].[Lesson] ([ID], [TimetableID], [Date], [IsConducted], [IsActive]) VALUES (3, 1, CAST(N'2023-12-05T16:34:36.140' AS DateTime), 1, 1)
GO
INSERT [dbo].[Lesson] ([ID], [TimetableID], [Date], [IsConducted], [IsActive]) VALUES (4, 2, CAST(N'2023-12-05T16:37:01.380' AS DateTime), 1, 1)
GO
INSERT [dbo].[Lesson] ([ID], [TimetableID], [Date], [IsConducted], [IsActive]) VALUES (1002, 1005, CAST(N'2023-12-18T21:06:22.097' AS DateTime), 0, 1)
GO
INSERT [dbo].[Lesson] ([ID], [TimetableID], [Date], [IsConducted], [IsActive]) VALUES (2002, 1005, CAST(N'2023-12-19T22:47:04.727' AS DateTime), 0, 1)
GO
INSERT [dbo].[Lesson] ([ID], [TimetableID], [Date], [IsConducted], [IsActive]) VALUES (3002, 1, CAST(N'2023-12-20T17:49:36.720' AS DateTime), 0, 1)
GO
SET IDENTITY_INSERT [dbo].[Lesson] OFF
GO
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (3, 15, 1)
GO
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (3, 16, 0)
GO
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (3, 17, 0)
GO
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (3, 18, 0)
GO
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (4, 18, 0)
GO
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (4, 19, 1)
GO
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (1002, 15, 1)
GO
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (1002, 16, 0)
GO
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (1002, 17, 1)
GO
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (1002, 18, 0)
GO
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (2002, 15, 0)
GO
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (2002, 16, 0)
GO
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (2002, 17, 0)
GO
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (2002, 18, 0)
GO
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (2002, 26, 0)
GO
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (3002, 15, 0)
GO
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (3002, 16, 0)
GO
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (3002, 17, 0)
GO
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (3002, 18, 0)
GO
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (3002, 20, 0)
GO
INSERT [dbo].[Lesson_Pupil] ([LessonID], [PupilID], [WasInClass]) VALUES (3002, 26, 0)
GO
SET IDENTITY_INSERT [dbo].[Pupil] ON 
GO
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID], [IsStudying]) VALUES (15, N'Астахова ', N'София ', N'Иванова', 1, 1)
GO
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID], [IsStudying]) VALUES (16, N'Гончарова ', N'Ульяна ', N'Мирославовна ', 1, 1)
GO
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID], [IsStudying]) VALUES (17, N'Спиридонова ', N'Василиса ', N'Марковна ', 1, 1)
GO
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID], [IsStudying]) VALUES (18, N'Денисова ', N'Анастасия ', N'Данииловна ', 1, 1)
GO
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID], [IsStudying]) VALUES (19, N'Никифоров ', N'Федор ', N'Никитич ', 1, 1)
GO
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID], [IsStudying]) VALUES (20, N'Черкасов ', N'Артем ', N'Георгиевич', 2, 1)
GO
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID], [IsStudying]) VALUES (21, N'Румянцев ', N'Роман ', N'Даниилович ', 2, 1)
GO
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID], [IsStudying]) VALUES (22, N'Куликова ', N'Алена ', N'Александровна ', 2, 1)
GO
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID], [IsStudying]) VALUES (23, N'Царев ', N'Алесандр ', N'Александрович ', 3, 1)
GO
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID], [IsStudying]) VALUES (24, N'Антипова ', N'Анастасия ', N'Владимировна ', 3, 1)
GO
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID], [IsStudying]) VALUES (25, N'Шаповалова ', N'Дарья ', N'Миронова ', 3, 1)
GO
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID], [IsStudying]) VALUES (26, N'Ефимова ', N'Елизавета ', N'Ильинична ', 3, 1)
GO
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID], [IsStudying]) VALUES (27, N'Симонов ', N'Всеволод ', N'Николаевич ', 3, 1)
GO
INSERT [dbo].[Pupil] ([ID], [LastName], [FirstName], [Patronymic], [GradeID], [IsStudying]) VALUES (28, N'Грачева ', N'Мария ', N'Львовна ', 3, 1)
GO
SET IDENTITY_INSERT [dbo].[Pupil] OFF
GO
SET IDENTITY_INSERT [dbo].[Role] ON 
GO
INSERT [dbo].[Role] ([ID], [Title]) VALUES (1, N'AssociateDirectior')
GO
INSERT [dbo].[Role] ([ID], [Title]) VALUES (2, N'Teacher')
GO
SET IDENTITY_INSERT [dbo].[Role] OFF
GO
SET IDENTITY_INSERT [dbo].[Teacher] ON 
GO
INSERT [dbo].[Teacher] ([ID], [LastName], [FirstName], [Patronymic], [IsWorking]) VALUES (1, N'Панкратов  ', N'Максим', N'Константинов', 1)
GO
INSERT [dbo].[Teacher] ([ID], [LastName], [FirstName], [Patronymic], [IsWorking]) VALUES (2, N'Матвеева ', N'Анастасия ', N'Андреева', 1)
GO
INSERT [dbo].[Teacher] ([ID], [LastName], [FirstName], [Patronymic], [IsWorking]) VALUES (3, N'Калачев', N'Артем', N'Алесандров ', 1)
GO
INSERT [dbo].[Teacher] ([ID], [LastName], [FirstName], [Patronymic], [IsWorking]) VALUES (4, N'Иванова ', N'Ева', N'Дмитриевна', 1)
GO
INSERT [dbo].[Teacher] ([ID], [LastName], [FirstName], [Patronymic], [IsWorking]) VALUES (5, N'Степанов ', N'Олег', N'Владимиров', 1)
GO
INSERT [dbo].[Teacher] ([ID], [LastName], [FirstName], [Patronymic], [IsWorking]) VALUES (6, N'Алексеев', N'Денис', N'Олегович', 1)
GO
SET IDENTITY_INSERT [dbo].[Teacher] OFF
GO
SET IDENTITY_INSERT [dbo].[Timetable] ON 
GO
INSERT [dbo].[Timetable] ([ID], [ClassroomID], [WeekDayID], [Time], [IsActive], [GroupID]) VALUES (1, 1, 1, CAST(N'08:00:00' AS Time), 1, 1)
GO
INSERT [dbo].[Timetable] ([ID], [ClassroomID], [WeekDayID], [Time], [IsActive], [GroupID]) VALUES (2, 2, 1, CAST(N'09:40:00' AS Time), 1, 2)
GO
INSERT [dbo].[Timetable] ([ID], [ClassroomID], [WeekDayID], [Time], [IsActive], [GroupID]) VALUES (3, 3, 1, CAST(N'11:50:00' AS Time), 1, 3)
GO
INSERT [dbo].[Timetable] ([ID], [ClassroomID], [WeekDayID], [Time], [IsActive], [GroupID]) VALUES (4, 4, 1, CAST(N'13:40:00' AS Time), 1, 4)
GO
INSERT [dbo].[Timetable] ([ID], [ClassroomID], [WeekDayID], [Time], [IsActive], [GroupID]) VALUES (1002, 1, 2, CAST(N'08:00:00' AS Time), 1, 5)
GO
INSERT [dbo].[Timetable] ([ID], [ClassroomID], [WeekDayID], [Time], [IsActive], [GroupID]) VALUES (1003, 2, 2, CAST(N'09:40:00' AS Time), 1, 3)
GO
INSERT [dbo].[Timetable] ([ID], [ClassroomID], [WeekDayID], [Time], [IsActive], [GroupID]) VALUES (1004, 3, 3, CAST(N'08:00:00' AS Time), 1, 5)
GO
INSERT [dbo].[Timetable] ([ID], [ClassroomID], [WeekDayID], [Time], [IsActive], [GroupID]) VALUES (1005, 1, 4, CAST(N'11:50:00' AS Time), 1, 1)
GO
INSERT [dbo].[Timetable] ([ID], [ClassroomID], [WeekDayID], [Time], [IsActive], [GroupID]) VALUES (1006, 4, 5, CAST(N'16:50:00' AS Time), 1, 4)
GO
INSERT [dbo].[Timetable] ([ID], [ClassroomID], [WeekDayID], [Time], [IsActive], [GroupID]) VALUES (1007, 3, 6, CAST(N'11:50:00' AS Time), 1, 3)
GO
INSERT [dbo].[Timetable] ([ID], [ClassroomID], [WeekDayID], [Time], [IsActive], [GroupID]) VALUES (1008, 2, 6, CAST(N'15:20:00' AS Time), 1, 3)
GO
SET IDENTITY_INSERT [dbo].[Timetable] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 
GO
INSERT [dbo].[User] ([ID], [RoleID], [Name], [Login], [Password], [TeacherID]) VALUES (1, 2, N'Максим', N'maxim', N'maxim', 1)
GO
INSERT [dbo].[User] ([ID], [RoleID], [Name], [Login], [Password], [TeacherID]) VALUES (2, 2, N'Анастасия ', N'nastya', N'nastya', 2)
GO
INSERT [dbo].[User] ([ID], [RoleID], [Name], [Login], [Password], [TeacherID]) VALUES (3, 2, N'Артем', N'artem', N'artem', 3)
GO
INSERT [dbo].[User] ([ID], [RoleID], [Name], [Login], [Password], [TeacherID]) VALUES (4, 2, N'Ева', N'eva', N'eva', 4)
GO
INSERT [dbo].[User] ([ID], [RoleID], [Name], [Login], [Password], [TeacherID]) VALUES (5, 2, N'Олег', N'oleg', N'oleg', 5)
GO
INSERT [dbo].[User] ([ID], [RoleID], [Name], [Login], [Password], [TeacherID]) VALUES (6, 2, N'Денис', N'denis', N'denis', 6)
GO
INSERT [dbo].[User] ([ID], [RoleID], [Name], [Login], [Password], [TeacherID]) VALUES (7, 1, N'Дмитрий', N'dmitriy', N'dmitriy', NULL)
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[WeekDay] ON 
GO
INSERT [dbo].[WeekDay] ([ID], [Title]) VALUES (1, N'Понедельник')
GO
INSERT [dbo].[WeekDay] ([ID], [Title]) VALUES (2, N'Вторник')
GO
INSERT [dbo].[WeekDay] ([ID], [Title]) VALUES (3, N'Среда')
GO
INSERT [dbo].[WeekDay] ([ID], [Title]) VALUES (4, N'Четверг')
GO
INSERT [dbo].[WeekDay] ([ID], [Title]) VALUES (5, N'Пятница')
GO
INSERT [dbo].[WeekDay] ([ID], [Title]) VALUES (6, N'Суббота')
GO
INSERT [dbo].[WeekDay] ([ID], [Title]) VALUES (7, N'Воскресенье')
GO
SET IDENTITY_INSERT [dbo].[WeekDay] OFF
GO
/****** Object:  Index [IX_FK_Group_Circle]    Script Date: 31.12.2023 11:43:06 ******/
CREATE NONCLUSTERED INDEX [IX_FK_Group_Circle] ON [dbo].[Group]
(
	[CircleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Group_Teacher]    Script Date: 31.12.2023 11:43:06 ******/
CREATE NONCLUSTERED INDEX [IX_FK_Group_Teacher] ON [dbo].[Group]
(
	[TeacherID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Group_Pupil_Pupil]    Script Date: 31.12.2023 11:43:06 ******/
CREATE NONCLUSTERED INDEX [IX_FK_Group_Pupil_Pupil] ON [dbo].[Group_Pupil]
(
	[PupilID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Lesson_Timetable]    Script Date: 31.12.2023 11:43:06 ******/
CREATE NONCLUSTERED INDEX [IX_FK_Lesson_Timetable] ON [dbo].[Lesson]
(
	[TimetableID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Lesson_Pupil_Pupil]    Script Date: 31.12.2023 11:43:06 ******/
CREATE NONCLUSTERED INDEX [IX_FK_Lesson_Pupil_Pupil] ON [dbo].[Lesson_Pupil]
(
	[PupilID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Pupil_Grade]    Script Date: 31.12.2023 11:43:06 ******/
CREATE NONCLUSTERED INDEX [IX_FK_Pupil_Grade] ON [dbo].[Pupil]
(
	[GradeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Timetable_Classroom]    Script Date: 31.12.2023 11:43:06 ******/
CREATE NONCLUSTERED INDEX [IX_FK_Timetable_Classroom] ON [dbo].[Timetable]
(
	[ClassroomID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Timetable_Group]    Script Date: 31.12.2023 11:43:06 ******/
CREATE NONCLUSTERED INDEX [IX_FK_Timetable_Group] ON [dbo].[Timetable]
(
	[GroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Timetable_WeekDay]    Script Date: 31.12.2023 11:43:06 ******/
CREATE NONCLUSTERED INDEX [IX_FK_Timetable_WeekDay] ON [dbo].[Timetable]
(
	[WeekDayID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_User_Role]    Script Date: 31.12.2023 11:43:06 ******/
CREATE NONCLUSTERED INDEX [IX_FK_User_Role] ON [dbo].[User]
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_User_Teacher]    Script Date: 31.12.2023 11:43:06 ******/
CREATE NONCLUSTERED INDEX [IX_FK_User_Teacher] ON [dbo].[User]
(
	[TeacherID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Group]  WITH CHECK ADD  CONSTRAINT [FK_Group_Circle] FOREIGN KEY([CircleID])
REFERENCES [dbo].[Circle] ([ID])
GO
ALTER TABLE [dbo].[Group] CHECK CONSTRAINT [FK_Group_Circle]
GO
ALTER TABLE [dbo].[Group]  WITH CHECK ADD  CONSTRAINT [FK_Group_Teacher] FOREIGN KEY([TeacherID])
REFERENCES [dbo].[Teacher] ([ID])
GO
ALTER TABLE [dbo].[Group] CHECK CONSTRAINT [FK_Group_Teacher]
GO
ALTER TABLE [dbo].[Group_Pupil]  WITH CHECK ADD  CONSTRAINT [FK_Group_Pupil_Group] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Group] ([ID])
GO
ALTER TABLE [dbo].[Group_Pupil] CHECK CONSTRAINT [FK_Group_Pupil_Group]
GO
ALTER TABLE [dbo].[Group_Pupil]  WITH CHECK ADD  CONSTRAINT [FK_Group_Pupil_Pupil] FOREIGN KEY([PupilID])
REFERENCES [dbo].[Pupil] ([ID])
GO
ALTER TABLE [dbo].[Group_Pupil] CHECK CONSTRAINT [FK_Group_Pupil_Pupil]
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
ALTER TABLE [dbo].[Timetable]  WITH CHECK ADD  CONSTRAINT [FK_Timetable_Classroom] FOREIGN KEY([ClassroomID])
REFERENCES [dbo].[Classroom] ([ID])
GO
ALTER TABLE [dbo].[Timetable] CHECK CONSTRAINT [FK_Timetable_Classroom]
GO
ALTER TABLE [dbo].[Timetable]  WITH CHECK ADD  CONSTRAINT [FK_Timetable_Group] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Group] ([ID])
GO
ALTER TABLE [dbo].[Timetable] CHECK CONSTRAINT [FK_Timetable_Group]
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
USE [master]
GO
ALTER DATABASE [cm_generated] SET  READ_WRITE 
GO
