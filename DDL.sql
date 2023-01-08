create table universities_monitoring.Moderator
(
    Id bigint auto_increment
        primary key,
    PasswordSHA256Hash tinyblob not null
);

create table universities_monitoring.University
(
    Id bigint auto_increment
        primary key,
    Name varchar(128) charset utf8mb3 not null,
    constraint University_Name_uindex
        unique (Name)
);

create table universities_monitoring.UniversityService
(
    Id bigint auto_increment
        primary key,
    IPAddress tinyblob not null,
    UniversityId bigint not null,
    constraint UniversityService_University_Id_fk
        foreign key (UniversityId) references universities_monitoring.University (Id)
            on delete cascade
);

create table universities_monitoring.UniversityServiceStateChange
(
    Id bigint auto_increment
        primary key,
    ServiceId bigint not null,
    IsOnline tinyint(1) not null,
    constraint UniversityServiceStateChange_UniversityService_Id_fk
        foreign key (ServiceId) references universities_monitoring.UniversityService (Id)
            on delete cascade
);

create table universities_monitoring.User
(
    Id bigint auto_increment
        primary key,
    Username varchar(64) not null,
    PasswordSHA256Hash tinyblob not null,
    Email varchar(256) null,
    TelegramTag varchar(128) null,
    SendEmailNotification tinyint(1) default 0 not null,
    SendTelegramNotification tinyint(1) default 0 not null,
    constraint User_Email_uindex
        unique (Email),
    constraint User_TelegramTag_uindex
        unique (TelegramTag),
    constraint User_Username_uindex
        unique (Username)
);

create table universities_monitoring.UniversityServiceReport
(
    Id bigint auto_increment
        primary key,
    Content varchar(4096) charset utf8mb3 null,
    ServiceId bigint not null,
    IssuerId bigint not null,
    IsOnline tinyint(1) not null,
    constraint UniversityServiceReport_UniversityService_Id_fk
        foreign key (ServiceId) references universities_monitoring.UniversityService (Id)
            on delete cascade,
    constraint UniversityServiceReport_User_Id_fk
        foreign key (IssuerId) references universities_monitoring.User (Id)
            on delete cascade
);

create table universities_monitoring.UserRateOfService
(
    Id bigint auto_increment
        primary key,
    Rate tinyint not null,
    Comment varchar(4096) charset utf8mb3 null,
    AuthorId bigint not null,
    ServiceId bigint not null,
    constraint UserRateOfService_UniversityService_Id_fk
        foreign key (ServiceId) references universities_monitoring.UniversityService (Id)
            on delete cascade,
    constraint UserRateOfService_User_Id_fk
        foreign key (AuthorId) references universities_monitoring.User (Id)
            on delete cascade
);

create table universities_monitoring.UserSubscribeToService
(
    Id bigint auto_increment
        primary key,
    UserId bigint not null,
    ServiceId bigint not null,
    constraint UserSubscribeToService_UniversityService_Id_fk
        foreign key (ServiceId) references universities_monitoring.UniversityService (Id)
            on delete cascade,
    constraint UserSubscribeToService_User_Id_fk
        foreign key (UserId) references universities_monitoring.User (Id)
            on delete cascade
);

