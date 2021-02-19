﻿create table period (
	id serial primary key, 
	grandprizeamt currency, 
	startts timestamptz, 
	endts timestamptz);
create table ticketsale (
	id serial primary key, 
	period_id int references period(id), 
	ballstring text, 
	ball1 int, 
	ball2 int, 
	ball3 int, 
	ball4 int, 
	ball5 int, 
	powerball int, 
	winlevel int, 
	winamount currency, 
	type text check (type in ('custom','quick')) );