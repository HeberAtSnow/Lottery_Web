create table period (
	id serial primary key, 
	grandprizeamt money, 
	startts timestamptz, 
	endts timestamptz);
create table ticketsale (
	id serial primary key, 
	period_id int references period(id), 
	player text,
	ballstring text, 
	ball1 int, 
	ball2 int, 
	ball3 int, 
	ball4 int, 
	ball5 int, 
	powerball int, 
	winlevel int, 
	winamount money, 
	type text check (type in ('custom','quick')) );