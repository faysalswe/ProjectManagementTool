create table Project(
Id int primary key identity,
[Name] nvarchar(256),
CodeName nvarchar(30),
[Description] nvarchar(256),
PossibleStartDate date,
PossibleEndDate date,
Duration int,
FilesPath nvarchar(100),
[Status] nvarchar(50)
)

create table AssignResource(
Id int primary key identity,
ProjectId int foreign key references Project(Id),
[UserId] nvarchar(128) foreign key references dbo.AspNetUsers(Id))

create table Task(
Id int primary key identity,
ProjectId int foreign key references Project(Id),
[UserId] nvarchar(128) foreign key references dbo.AspNetUsers(Id),
[Description] nvarchar(256),
Duedate date,
[Priority] nvarchar(50)
)

create table Comment(
Id int primary key identity,
TaskId int foreign key references Task(Id),
Comment nvarchar(200),
[DateTime] datetime
)