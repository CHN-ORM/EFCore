-- 本日
SELECT DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0)

-- 本周第一天
SELECT DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()) - DATEPART(Weekday, GETDATE()) + 2, 0) 


-- 本月第一天
SELECT DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0)
-- 下月第一天
SELECT DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) + 1, 0)


SELECT GETDATE()

SELECT DATEDIFF(DAY, 0, GETDATE())
SELECT DATEPART(Weekday, GETDATE()) 

SELECT DATEADD(DAY, 44211,0)

SELECT DATEADD(DAY, DATEDIFF(DAY, 0, '2020-05-14') - DATEPART(Weekday, '2020-05-14') + 2, 0) 

