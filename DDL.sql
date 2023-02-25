create table Moderator
(
    Id                 bigint unsigned auto_increment
        primary key,
    PasswordSHA256Hash tinyblob not null
);

create table MonitoringModules
(
    Id  bigint unsigned auto_increment
        primary key,
    Url int not null,
    constraint MonitoringModules_Url_uindex
        unique (Url)
);

create table University
(
    Id   bigint unsigned auto_increment
        primary key,
    Name varchar(128) charset utf8mb3 not null,
    constraint University_Name_uindex
        unique (Name)
);

create table UniversityService
(
    Id           bigint unsigned auto_increment
        primary key,
    UniversityId bigint unsigned               not null,
    Name         varchar(128) charset utf8mb3  not null,
    Url          varchar(2048) charset utf8mb3 not null,
    constraint UniversityService_University_Id_fk
        foreign key (UniversityId) references University (Id)
            on delete cascade
);

create table UniversityServiceStateChange
(
    Id        bigint unsigned auto_increment
        primary key,
    ServiceId bigint unsigned                     not null,
    IsOnline  tinyint(1)                          not null,
    ChangedAt timestamp default CURRENT_TIMESTAMP not null,
    constraint UniversityServiceStateChange_UniversityService_Id_fk
        foreign key (ServiceId) references UniversityService (Id)
            on delete cascade
);

create table User
(
    Id                    bigint unsigned auto_increment
        primary key,
    Username              varchar(64)  not null,
    PasswordSHA256Hash    tinyblob     not null,
    Email                 varchar(256) null,
    SendEmailNotification tinyint(1)   not null,
    constraint User_Email_uindex
        unique (Email),
    constraint User_Username_uindex
        unique (Username)
);

create table UniversityServiceReport
(
    Id        bigint unsigned auto_increment
        primary key,
    Content   varchar(4096) charset utf8mb3      null,
    ServiceId bigint unsigned                    not null,
    IssuerId  bigint unsigned                    not null,
    IsOnline  tinyint(1)                         not null,
    AddedAt   datetime default CURRENT_TIMESTAMP not null,
    IsSolved  tinyint(1)                         not null,
    constraint UniversityServiceReport_UniversityService_Id_fk
        foreign key (ServiceId) references UniversityService (Id)
            on delete cascade,
    constraint UniversityServiceReport_User_Id_fk
        foreign key (IssuerId) references User (Id)
            on delete cascade
);

create table UserRateOfService
(
    Id        bigint unsigned auto_increment
        primary key,
    Rate      tinyint                            not null,
    Comment   varchar(4096) charset utf8mb3      null,
    AuthorId  bigint unsigned                    not null,
    ServiceId bigint unsigned                    not null,
    AddedAt   datetime default CURRENT_TIMESTAMP not null,
    constraint UserRateOfService_UniversityService_Id_fk
        foreign key (ServiceId) references UniversityService (Id)
            on delete cascade,
    constraint UserRateOfService_User_Id_fk
        foreign key (AuthorId) references User (Id)
            on delete cascade
);

create table UserSubscribeToService
(
    Id        bigint unsigned auto_increment
        primary key,
    UserId    bigint unsigned not null,
    ServiceId bigint unsigned not null,
    constraint UserSubscribeToService_UniversityService_Id_fk
        foreign key (ServiceId) references UniversityService (Id)
            on delete cascade,
    constraint UserSubscribeToService_User_Id_fk
        foreign key (UserId) references User (Id)
            on delete cascade
);

