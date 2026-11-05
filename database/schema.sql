
PRAGMA foreign_keys = ON;

-- 1. Customers
CREATE TABLE IF NOT EXISTS Customers (
    CustomerId INTEGER PRIMARY KEY AUTOINCREMENT,
    CustomerCode TEXT NOT NULL UNIQUE,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL,
    Email TEXT NOT NULL UNIQUE,
    Phone TEXT,
    IsActive INTEGER NOT NULL DEFAULT 1
        CHECK (IsActive IN (0, 1)),
    CreatedAtUtc TEXT NOT NULL DEFAULT
        (strftime('%Y-%m-%dT%H:%M:%fZ', 'now'))
);

-- 2. Products
CREATE TABLE IF NOT EXISTS Products (
    ProductId INTEGER PRIMARY KEY AUTOINCREMENT,
    SKU TEXT NOT NULL UNIQUE,
    ProductName TEXT NOT NULL,
    Description TEXT,
    UnitPricePaise INTEGER NOT NULL CHECK (UnitPricePaise >= 0),
    StockQuantity INTEGER NOT NULL DEFAULT 0 CHECK (StockQuantity >= 0),
    IsActive INTEGER NOT NULL DEFAULT 1
        CHECK (IsActive IN (0, 1)),
    CreatedAtUtc TEXT NOT NULL DEFAULT
        (strftime('%Y-%m-%dT%H:%M:%fZ', 'now'))
);

-- 3. Orders
CREATE TABLE IF NOT EXISTS Orders (
    OrderId INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderNumber TEXT NOT NULL UNIQUE,
    CustomerId INTEGER NOT NULL,
    OrderDateUtc TEXT NOT NULL,
    OrderStatus TEXT NOT NULL DEFAULT 'Pending'
        CHECK (OrderStatus IN (
            'Pending', 'Confirmed', 'Processing',
            'Shipped', 'Delivered', 'Cancelled', 'Returned'
        )),
    SubtotalPaise INTEGER NOT NULL CHECK (SubtotalPaise >= 0),
    DiscountPaise INTEGER NOT NULL DEFAULT 0 CHECK (DiscountPaise >= 0),
    ShippingPaise INTEGER NOT NULL DEFAULT 0 CHECK (ShippingPaise >= 0),
    TaxPaise INTEGER NOT NULL DEFAULT 0 CHECK (TaxPaise >= 0),
    TotalPaise INTEGER NOT NULL CHECK (
        TotalPaise = SubtotalPaise - DiscountPaise
                    + ShippingPaise + TaxPaise
    ),
    Currency TEXT NOT NULL DEFAULT 'INR',
    ShippingAddress TEXT NOT NULL,
    CreatedAtUtc TEXT NOT NULL DEFAULT
        (strftime('%Y-%m-%dT%H:%M:%fZ', 'now')),
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId),
    CHECK (DiscountPaise <= SubtotalPaise)
);

-- 4. Order Items
CREATE TABLE IF NOT EXISTS OrderItems (
    OrderItemId INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderId INTEGER NOT NULL,
    ProductId INTEGER NOT NULL,
    ProductNameSnapshot TEXT NOT NULL,
    SKU TEXT NOT NULL,
    Quantity INTEGER NOT NULL CHECK (Quantity > 0),
    UnitPricePaise INTEGER NOT NULL CHECK (UnitPricePaise >= 0),
    DiscountPaise INTEGER NOT NULL DEFAULT 0 CHECK (DiscountPaise >= 0),
    LineTotalPaise INTEGER NOT NULL CHECK (
        LineTotalPaise = (Quantity * UnitPricePaise) - DiscountPaise
    ),
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

-- 5. Payments
CREATE TABLE IF NOT EXISTS Payments (
    PaymentId INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderId INTEGER NOT NULL,
    PaymentReference TEXT NOT NULL UNIQUE,
    PaymentMethod TEXT NOT NULL
        CHECK (PaymentMethod IN (
            'UPI', 'CreditCard', 'DebitCard',
            'NetBanking', 'Wallet', 'CashOnDelivery'
        )),
    PaymentStatus TEXT NOT NULL DEFAULT 'Pending'
        CHECK (PaymentStatus IN (
            'Pending', 'Authorized', 'Paid',
            'Failed', 'Refunded', 'PartiallyRefunded'
        )),
    AmountPaise INTEGER NOT NULL CHECK (AmountPaise > 0),
    Currency TEXT NOT NULL DEFAULT 'INR',
    PaidAtUtc TEXT,
    CreatedAtUtc TEXT NOT NULL DEFAULT
        (strftime('%Y-%m-%dT%H:%M:%fZ', 'now')),
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
);

-- 6. Shipments
CREATE TABLE IF NOT EXISTS Shipments (
    ShipmentId INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderId INTEGER NOT NULL,
    Carrier TEXT NOT NULL,
    TrackingNumber TEXT UNIQUE,
    ShipmentStatus TEXT NOT NULL DEFAULT 'Pending'
        CHECK (ShipmentStatus IN (
            'Pending', 'LabelCreated', 'Shipped',
            'InTransit', 'OutForDelivery', 'Delivered',
            'Failed', 'Returned'
        )),
    TrackingUrl TEXT,
    ShippedAtUtc TEXT,
    EstimatedDeliveryUtc TEXT,
    DeliveredAtUtc TEXT,
    CreatedAtUtc TEXT NOT NULL DEFAULT
        (strftime('%Y-%m-%dT%H:%M:%fZ', 'now')),
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
);

-- 7. Order Status History
CREATE TABLE IF NOT EXISTS OrderStatusHistory (
    HistoryId INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderId INTEGER NOT NULL,
    Status TEXT NOT NULL,
    StatusDescription TEXT,
    ChangedBy TEXT NOT NULL DEFAULT 'System',
    ChangedAtUtc TEXT NOT NULL DEFAULT
        (strftime('%Y-%m-%dT%H:%M:%fZ', 'now')),
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
);

-- Indexes for frequently queried columns
CREATE INDEX IF NOT EXISTS IX_Orders_CustomerId
    ON Orders(CustomerId);

CREATE INDEX IF NOT EXISTS IX_Orders_OrderDateUtc
    ON Orders(OrderDateUtc);

CREATE INDEX IF NOT EXISTS IX_Orders_OrderStatus
    ON Orders(OrderStatus);

CREATE INDEX IF NOT EXISTS IX_OrderItems_OrderId
    ON OrderItems(OrderId);

CREATE INDEX IF NOT EXISTS IX_Payments_OrderId
    ON Payments(OrderId);

CREATE INDEX IF NOT EXISTS IX_Shipments_OrderId
    ON Shipments(OrderId);

CREATE INDEX IF NOT EXISTS IX_OrderStatusHistory_OrderId
    ON OrderStatusHistory(OrderId);
