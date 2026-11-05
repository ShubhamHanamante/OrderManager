
PRAGMA foreign_keys = ON;

BEGIN TRANSACTION;

-- ==========================================
-- 1. CUSTOMERS
-- ==========================================

INSERT OR IGNORE INTO Customers
(CustomerId, CustomerCode, FirstName, LastName, Email, Phone)
VALUES
(1, 'CUST-1001', 'Aarav', 'Sharma', 'aarav@example.com', '9000000001'),
(2, 'CUST-1002', 'Priya', 'Deshmukh', 'priya@example.com', '9000000002'),
(3, 'CUST-1003', 'Rohan', 'Patil', 'rohan@example.com', '9000000003'),
(4, 'CUST-1004', 'Neha', 'Kulkarni', 'neha@example.com', '9000000004'),
(5, 'CUST-1005', 'Aditya', 'Joshi', 'aditya@example.com', '9000000005');


-- ==========================================
-- 2. PRODUCTS
-- All prices are stored in paise.
-- ==========================================

INSERT OR IGNORE INTO Products
(ProductId, SKU, ProductName, Description, UnitPricePaise, StockQuantity)
VALUES
(1, 'ELEC-001', 'Wireless Headphones', 'Bluetooth over-ear headphones', 249900, 45),
(2, 'ELEC-002', 'Wireless Mouse', 'Ergonomic wireless mouse', 79900, 120),
(3, 'ELEC-003', 'Mechanical Keyboard', 'RGB mechanical keyboard', 349900, 35),
(4, 'ELEC-004', '24-inch Full HD Monitor', 'Full HD IPS monitor', 899900, 20),
(5, 'ELEC-005', 'USB-C Hub', '7-in-1 USB-C connectivity hub', 149900, 75),
(6, 'ELEC-006', 'Aluminum Laptop Stand', 'Adjustable aluminum laptop stand', 99900, 60),
(7, 'ELEC-007', 'HD Webcam', '1080p HD webcam', 219900, 40),
(8, 'ELEC-008', '1TB NVMe SSD', 'High-speed 1TB NVMe SSD', 599900, 25);


-- ==========================================
-- 3. ORDERS
-- ==========================================

INSERT OR IGNORE INTO Orders
(OrderId, OrderNumber, CustomerId, OrderDateUtc,
 OrderStatus, SubtotalPaise, DiscountPaise,
 ShippingPaise, TaxPaise, TotalPaise,
 ShippingAddress)
VALUES
(1, 'ORD-2026-1001', 1, '2026-10-25T09:15:00Z',
 'Delivered', 329800, 29800, 0, 54000, 354000,
 'Viman Nagar, Pune, Maharashtra'),

(2, 'ORD-2026-1002', 1, '2026-10-30T11:30:00Z',
 'Shipped', 499800, 0, 9900, 89964, 599664,
 'Viman Nagar, Pune, Maharashtra'),

(3, 'ORD-2026-1003', 2, '2026-11-02T08:45:00Z',
 'Confirmed', 899900, 50000, 0, 152982, 1002882,
 'Baner, Pune, Maharashtra'),

(4, 'ORD-2026-1004', 2, '2026-11-04T14:20:00Z',
 'Pending', 319800, 19900, 4900, 53982, 358782,
 'Baner, Pune, Maharashtra'),

(5, 'ORD-2026-1005', 3, '2026-10-18T10:00:00Z',
 'Delivered', 599900, 0, 0, 107982, 707882,
 'Kothrud, Pune, Maharashtra'),

(6, 'ORD-2026-1006', 4, '2026-10-20T12:00:00Z',
 'Cancelled', 349900, 0, 0, 62982, 412882,
 'Wakad, Pune, Maharashtra'),

(7, 'ORD-2026-1007', 5, '2026-11-01T16:45:00Z',
 'Shipped', 249900, 0, 0, 44982, 294882,
 'Hinjewadi, Pune, Maharashtra'),

(8, 'ORD-2026-1008', 3, '2026-11-04T09:30:00Z',
 'Processing', 379700, 10000, 4900, 68346, 442946,
 'Kothrud, Pune, Maharashtra');


-- ==========================================
-- 4. ORDER ITEMS
-- ==========================================

INSERT OR IGNORE INTO OrderItems
(OrderItemId, OrderId, ProductId, ProductNameSnapshot,
 SKU, Quantity, UnitPricePaise, DiscountPaise, LineTotalPaise)
VALUES
(1, 1, 1, 'Wireless Headphones', 'ELEC-001', 1, 249900, 0, 249900),
(2, 1, 2, 'Wireless Mouse', 'ELEC-002', 1, 79900, 0, 79900),

(3, 2, 3, 'Mechanical Keyboard', 'ELEC-003', 1, 349900, 0, 349900),
(4, 2, 5, 'USB-C Hub', 'ELEC-005', 1, 149900, 0, 149900),

(5, 3, 4, '24-inch Full HD Monitor', 'ELEC-004', 1, 899900, 0, 899900),

(6, 4, 7, 'HD Webcam', 'ELEC-007', 1, 219900, 0, 219900),
(7, 4, 6, 'Aluminum Laptop Stand', 'ELEC-006', 1, 99900, 0, 99900),

(8, 5, 8, '1TB NVMe SSD', 'ELEC-008', 1, 599900, 0, 599900),

(9, 6, 3, 'Mechanical Keyboard', 'ELEC-003', 1, 349900, 0, 349900),

(10, 7, 1, 'Wireless Headphones', 'ELEC-001', 1, 249900, 0, 249900),

(11, 8, 2, 'Wireless Mouse', 'ELEC-002', 2, 79900, 0, 159800),
(12, 8, 7, 'HD Webcam', 'ELEC-007', 1, 219900, 0, 219900);


-- ==========================================
-- 5. PAYMENTS
-- ==========================================

INSERT OR IGNORE INTO Payments
(PaymentId, OrderId, PaymentReference, PaymentMethod,
 PaymentStatus, AmountPaise, PaidAtUtc)
VALUES
(1, 1, 'PAY-1001', 'UPI', 'Paid', 354000, '2026-10-25T09:16:00Z'),
(2, 2, 'PAY-1002', 'CreditCard', 'Paid', 599664, '2026-10-30T11:32:00Z'),
(3, 3, 'PAY-1003', 'NetBanking', 'Paid', 1002882, '2026-11-02T08:47:00Z'),
(4, 4, 'PAY-1004', 'CashOnDelivery', 'Pending', 358782, NULL),
(5, 5, 'PAY-1005', 'UPI', 'Paid', 707882, '2026-10-18T10:02:00Z'),
(6, 6, 'PAY-1006', 'CreditCard', 'Refunded', 412882, '2026-10-20T12:02:00Z'),
(7, 7, 'PAY-1007', 'CashOnDelivery', 'Pending', 294882, NULL),
(8, 8, 'PAY-1008', 'UPI', 'Paid', 442946, '2026-11-04T09:32:00Z');


-- ==========================================
-- 6. SHIPMENTS
-- ==========================================

INSERT OR IGNORE INTO Shipments
(ShipmentId, OrderId, Carrier, TrackingNumber,
 ShipmentStatus, TrackingUrl, ShippedAtUtc,
 EstimatedDeliveryUtc, DeliveredAtUtc)
VALUES
(1, 1, 'BlueDart', 'BD123456789',
 'Delivered', 'https://example.com/track/BD123456789',
 '2026-10-26T08:00:00Z',
 '2026-10-29T18:00:00Z',
 '2026-10-28T14:30:00Z'),

(2, 2, 'Delhivery', 'DL123456789',
 'InTransit', 'https://example.com/track/DL123456789',
 '2026-11-01T09:00:00Z',
 '2026-11-07T18:00:00Z', NULL),

(3, 3, 'BlueDart', 'BD223456789',
 'LabelCreated', 'https://example.com/track/BD223456789',
 NULL, '2026-11-08T18:00:00Z', NULL),

(4, 4, 'Delhivery', 'DL223456789',
 'Pending', 'https://example.com/track/DL223456789',
 NULL, '2026-11-10T18:00:00Z', NULL),

(5, 5, 'DTDC', 'DT123456789',
 'Delivered', 'https://example.com/track/DT123456789',
 '2026-10-19T08:00:00Z',
 '2026-10-23T18:00:00Z',
 '2026-10-22T13:00:00Z'),

(6, 7, 'Ekart', 'EK123456789',
 'OutForDelivery', 'https://example.com/track/EK123456789',
 '2026-11-03T08:00:00Z',
 '2026-11-05T18:00:00Z', NULL),

(7, 8, 'EcomExpress', 'EC123456789',
 'LabelCreated', 'https://example.com/track/EC123456789',
 NULL, '2026-11-09T18:00:00Z', NULL);


-- ==========================================
-- 7. ORDER STATUS HISTORY
-- ==========================================

INSERT OR IGNORE INTO OrderStatusHistory
(HistoryId, OrderId, Status, StatusDescription, ChangedAtUtc)
VALUES

-- Order 1001
(1, 1, 'Pending', 'Order placed successfully', '2026-10-25T09:15:00Z'),
(2, 1, 'Confirmed', 'Order confirmed', '2026-10-25T09:30:00Z'),
(3, 1, 'Processing', 'Order is being prepared', '2026-10-25T14:00:00Z'),
(4, 1, 'Shipped', 'Order handed over to BlueDart', '2026-10-26T08:00:00Z'),
(5, 1, 'Delivered', 'Order delivered successfully', '2026-10-28T14:30:00Z'),

-- Order 1002
(6, 2, 'Pending', 'Order placed successfully', '2026-10-30T11:30:00Z'),
(7, 2, 'Confirmed', 'Payment received and order confirmed', '2026-10-30T11:35:00Z'),
(8, 2, 'Processing', 'Order is being prepared', '2026-10-31T10:00:00Z'),
(9, 2, 'Shipped', 'Order handed over to Delhivery', '2026-11-01T09:00:00Z'),

-- Order 1003
(10, 3, 'Pending', 'Order placed successfully', '2026-11-02T08:45:00Z'),
(11, 3, 'Confirmed', 'Order confirmed', '2026-11-02T09:00:00Z'),

-- Order 1004
(12, 4, 'Pending', 'Order placed successfully', '2026-11-04T14:20:00Z'),

-- Order 1005
(13, 5, 'Pending', 'Order placed successfully', '2026-10-18T10:00:00Z'),
(14, 5, 'Confirmed', 'Order confirmed', '2026-10-18T10:15:00Z'),
(15, 5, 'Processing', 'Order is being prepared', '2026-10-18T15:00:00Z'),
(16, 5, 'Shipped', 'Order handed over to DTDC', '2026-10-19T08:00:00Z'),
(17, 5, 'Delivered', 'Order delivered successfully', '2026-10-22T13:00:00Z'),

-- Order 1006
(18, 6, 'Pending', 'Order placed successfully', '2026-10-20T12:00:00Z'),
(19, 6, 'Confirmed', 'Order confirmed', '2026-10-20T12:15:00Z'),
(20, 6, 'Cancelled', 'Order cancelled by customer', '2026-10-20T16:00:00Z'),

-- Order 1007
(21, 7, 'Pending', 'Order placed successfully', '2026-11-01T16:45:00Z'),
(22, 7, 'Confirmed', 'Order confirmed', '2026-11-01T17:00:00Z'),
(23, 7, 'Processing', 'Order is being prepared', '2026-11-02T10:00:00Z'),
(24, 7, 'Shipped', 'Order handed over to Ekart', '2026-11-03T08:00:00Z'),

-- Order 1008
(25, 8, 'Pending', 'Order placed successfully', '2026-11-04T09:30:00Z'),
(26, 8, 'Confirmed', 'Payment received and order confirmed', '2026-11-04T09:35:00Z'),
(27, 8, 'Processing', 'Order is being prepared', '2026-11-04T12:00:00Z');

COMMIT;
