 --create overnight prices
select 
pd."date" , 
pd.symbol , 
pd2."close" as prev_day_close,
pd."open",
pd2."date" as prev_day
from prices_daily pd 
join prices_daily pd2
on pd.symbol = pd2.symbol 
where pd2."date" = (select max(pd2."date")  from prices_daily pd2 where pd2."date" < pd."date")
order by pd.symbol , pd."date" 
