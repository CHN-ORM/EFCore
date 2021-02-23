select * from ycp


update a
set a.unit = b.unit
from ycp a
inner join ycp b
on b.equip_no = 1 and a.yc_no = b.yc_no
