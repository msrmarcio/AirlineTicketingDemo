use ReservationDb
--DELETE FROM Reservations
SELECT * FROM RESERVATIONS
 
use SchedulerDb
--DELETE FROM ScheduledReservations
SELECT * FROM ScheduledReservations
 
use PaymentDb
--DELETE FROM Payments
SELECT * FROM Payments
 
use NotificationDB
--DELETE FROM Notifications
SELECT * FROM Notifications

use CustomerHistoryDb 
-- DELETE FROM  dbo.NotificationRecord
-- DELETE FROM  dbo.PaymentRecord
-- DELETE FROM  dbo.ReservationRecord
-- DELETE FROM  dbo.Histories
select * from dbo.Histories
select * from dbo.NotificationRecord
select * from dbo.PaymentRecord
select * from dbo.ReservationRecord


SELECT 
    h.CustomerEmail,
    
    r.ReservationId,
    r.Amount AS ReservationAmount,
    r.CreatedAt AS ReservationCreatedAt,
    
    p.Status AS PaymentStatus,
    p.ProcessedAt AS PaymentProcessedAt,
    
    n.Type AS NotificationType,
    n.SentAt AS NotificationSentAt

FROM dbo.Histories h
LEFT JOIN dbo.ReservationRecord r ON r.CustomerHistoryId = h.Id
LEFT JOIN dbo.PaymentRecord p ON p.CustomerHistoryId = h.Id
LEFT JOIN dbo.NotificationRecord n ON n.CustomerHistoryId = h.Id
WHERE h.CustomerEmail = 'kiko@email.com'
ORDER BY h.CustomerEmail, r.CreatedAt, p.ProcessedAt, n.SentAt;

