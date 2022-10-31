create database pvdiplomarbeit collate utf8mb4_general_ci;
use pvdiplomarbeit;

create table users(
	userId int unsigned not null auto_increment,
	name varchar(100) not null,
	email varchar(150) null,
	password varchar(300) not null,
	--isLogged tinyint not null,
	PRIMARY KEY (userId)
);

insert into users values(null, "admin", "admin@gmail.com", sha2("admin", 512));
insert into users values(null, "AdminAdmin123", "admin@gmail.com", sha2("AdminAdmin123", 512));
