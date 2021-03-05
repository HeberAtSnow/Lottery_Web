--get PROFITABILITY of ALL closed periods
with 
winlevel0 as (	select period_id,winlevel, count(*) num from ticketsale where winlevel=0 group by period_ID,winlevel),
winlevel1 as (	select period_id,winlevel, count(*) num from ticketsale where winlevel=1 group by period_ID,winlevel),
winlevel2 as (	select period_id,winlevel, count(*) num from ticketsale where winlevel=2 group by period_ID,winlevel),
winlevel3 as (	select period_id,winlevel, count(*) num from ticketsale where winlevel=3 group by period_ID,winlevel),
winlevel4 as (	select period_id,winlevel, count(*) num from ticketsale where winlevel=4 group by period_ID,winlevel),
winlevel5 as (	select period_id,winlevel, count(*) num from ticketsale where winlevel=5 group by period_ID,winlevel),
winlevel6 as (	select period_id,winlevel, count(*) num from ticketsale where winlevel=6 group by period_ID,winlevel),
winlevel7 as (	select period_id,winlevel, count(*) num from ticketsale where winlevel=7 group by period_ID,winlevel),
winlevel8 as (	select period_id,winlevel, count(*) num from ticketsale where winlevel=8 group by period_ID,winlevel),
winlevel9 as (	select period_id,winlevel, count(*) num from ticketsale where winlevel=9 group by period_ID,winlevel)
select
	period.id Period,
	to_char(
	coalesce(winlevel0.num,0)+coalesce(winlevel1.num,0)+coalesce(winlevel2.num,0)+
	coalesce(winlevel3.num,0)+coalesce(winlevel4.num,0)+coalesce(winlevel5.num,0)+
	coalesce(winlevel6.num,0)+coalesce(winlevel7.num,0)+coalesce(winlevel8.num,0)+
	coalesce(winlevel9.num,0)
	,'FM999,999,999,999,999,999,999') Tickets_Sold,
	to_char(
	(coalesce(winlevel0.num,0)+coalesce(winlevel1.num,0)+coalesce(winlevel2.num,0)+
	 coalesce(winlevel3.num,0)+coalesce(winlevel4.num,0)+coalesce(winlevel5.num,0)+
	 coalesce(winlevel6.num,0)+coalesce(winlevel7.num,0)+coalesce(winlevel8.num,0)+
	 coalesce(winlevel9.num,0) ) * 2
	 ,'FM999,999,999,999,999,999,999') Sales_Dollars,
	 to_char( 
	 (   ( 	 (
	 coalesce(winlevel0.num,0)+coalesce(winlevel1.num,0)+coalesce(winlevel2.num,0)+
	 coalesce(winlevel3.num,0)+coalesce(winlevel4.num,0)+coalesce(winlevel5.num,0)+
	 coalesce(winlevel6.num,0)+coalesce(winlevel7.num,0)+coalesce(winlevel8.num,0)+
	 coalesce(winlevel9.num,0) 
	 )*2 ) -
	 (
	  coalesce(winlevel1.num*grandprizeamt::numeric::int,0)+coalesce(winlevel2.num,0)*1000000+
	 coalesce(winlevel3.num,0)*50000+coalesce(winlevel4.num,0)*100+coalesce(winlevel5.num,0)*100+
	 coalesce(winlevel6.num,0)*7+coalesce(winlevel7.num,0)*7+coalesce(winlevel8.num,0)*4+
	 coalesce(winlevel9.num,0)*4 ) 
	),'FM999,999,999,999,999,999,999') Profit
from
	period 
		left outer join winlevel0 on (period.id=winlevel0.period_id)
		left outer join winlevel1 on (period.id=winlevel1.period_id)
		left outer join winlevel3 on (period.id=winlevel3.period_id)
		left outer join winlevel4 on (period.id=winlevel4.period_id)
		left outer join winlevel2 on (period.id=winlevel2.period_id)
		left outer join winlevel5 on (period.id=winlevel5.period_id)
		left outer join winlevel6 on (period.id=winlevel6.period_id)
		left outer join winlevel7 on (period.id=winlevel7.period_id)
		left outer join winlevel8 on (period.id=winlevel8.period_id)
		left outer join winlevel9 on (period.id=winlevel9.period_id)
order by 1





--get history of ALL closed periods but have output be on ONE LINE PER PERIOD
with winlevel0 as (	select period_id,winlevel, count(*) num from ticketsale where winlevel=0 group by period_ID,winlevel),
winlevel1 as (	select period_id,winlevel, count(*) num from ticketsale where winlevel=1 group by period_ID,winlevel),
winlevel2 as (	select period_id,winlevel, count(*) num from ticketsale where winlevel=2 group by period_ID,winlevel),
winlevel3 as (	select period_id,winlevel, count(*) num from ticketsale where winlevel=3 group by period_ID,winlevel),
winlevel4 as (	select period_id,winlevel, count(*) num from ticketsale where winlevel=4 group by period_ID,winlevel),
winlevel5 as (	select period_id,winlevel, count(*) num from ticketsale where winlevel=5 group by period_ID,winlevel),
winlevel6 as (	select period_id,winlevel, count(*) num from ticketsale where winlevel=6 group by period_ID,winlevel),
winlevel7 as (	select period_id,winlevel, count(*) num from ticketsale where winlevel=7 group by period_ID,winlevel),
winlevel8 as (	select period_id,winlevel, count(*) num from ticketsale where winlevel=8 group by period_ID,winlevel),
winlevel9 as (	select period_id,winlevel, count(*) num from ticketsale where winlevel=9 group by period_ID,winlevel)
select 
	id,
	grandprizeamt,
	startts,
	endts,
	winlevel0.num level0,
	winlevel1.num level1,
	winlevel2.num level2,
	winlevel3.num level3,
	winlevel4.num level4,
	winlevel5.num level5,
	winlevel6.num level6,
	winlevel7.num level7,
	winlevel8.num level8,
	winlevel9.num level9
from 
	period 
		left outer join winlevel0 on (period.id=winlevel0.period_id)
		left outer join winlevel1 on (period.id=winlevel1.period_id)
		left outer join winlevel3 on (period.id=winlevel3.period_id)
		left outer join winlevel4 on (period.id=winlevel4.period_id)
		left outer join winlevel2 on (period.id=winlevel2.period_id)
		left outer join winlevel5 on (period.id=winlevel5.period_id)
		left outer join winlevel6 on (period.id=winlevel6.period_id)
		left outer join winlevel7 on (period.id=winlevel7.period_id)
		left outer join winlevel8 on (period.id=winlevel8.period_id)
		left outer join winlevel9 on (period.id=winlevel9.period_id)
order by id