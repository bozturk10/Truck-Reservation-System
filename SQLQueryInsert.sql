DECLARE @ID INT

insert into [dbo].[NotifQueueMeta] ([QueueCreateDate],[IdType],[Status]) 
values (GETDATE(),'1',0); 

SELECT @ID = SCOPE_IDENTITY()

INSERT INTO [dbo].[NotifQueueData]
SELECT '{ url: "'+ NF.Email +'", parameter: " previous:'+  '5' + ' new: ' +'6' + '" }' 
FROM tblRezervasyonlar R
INNER JOIN Inserted I ON I.NakliyeBelgesi = R.YuklemeEmriId
INNER JOIN Deleted  D ON D.NakliyeBelgesi = I.NakliyeBelgesi
INNER JOIN tblYuklemeEmirleri YR ON R.YuklemeEmriId = YR.NakliyeBelgesi
INNER JOIN tblNakliyeFirmalari NF ON YR.NakliyeciId = NF.FirmaKodu
