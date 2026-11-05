
-- Get order details for a specific customer.
-- CustomerId will eventually come from authenticated user identity.

SELECT
    o.OrderNumber,
    c.CustomerCode,
    c.FirstName || ' ' || c.LastName AS CustomerName,
    o.OrderDateUtc,
    o.OrderStatus,

    printf('%.2f', o.SubtotalPaise / 100.0) AS Subtotal,
    printf('%.2f', o.DiscountPaise / 100.0) AS Discount,
    printf('%.2f', o.ShippingPaise / 100.0) AS Shipping,
    printf('%.2f', o.TaxPaise / 100.0) AS Tax,
    printf('%.2f', o.TotalPaise / 100.0) AS TotalAmount,

    p.PaymentMethod,
    p.PaymentStatus,

    s.Carrier,
    s.TrackingNumber,
    s.ShipmentStatus,
    s.EstimatedDeliveryUtc

FROM Orders o

INNER JOIN Customers c
    ON c.CustomerId = o.CustomerId

LEFT JOIN Payments p
    ON p.OrderId = o.OrderId

LEFT JOIN Shipments s
    ON s.OrderId = o.OrderId

WHERE
    o.OrderNumber = 'ORD-2026-1002'
    AND o.CustomerId = 3;
