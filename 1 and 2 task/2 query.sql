SELECT 
    Products.Name AS Product, 
    Sellers.Surname, 
    Sellers.Name, 
    (SUM(Sales.Quantity) / TotalSales.TotalQuantity) * 100 AS Percentage
FROM Sellers 
LEFT join Sales on Sellers.ID = Sales.IDsel
LEFT JOIN Products ON Products.ID = Sales.IDProd
LEFT JOIN 
	    (SELECT 
         IDProd, 
         SUM(Quantity) AS TotalQuantity
     FROM Sales 
     WHERE Date BETWEEN '2013-10-01' AND '2013-10-07'
     GROUP BY IDProd) AS TotalSales 
     ON TotalSales.IDProd = Products.ID --нужно для счета общено количества
INNER JOIN 
    (SELECT 
         IDProd 
     FROM Arrivals 
     WHERE  Date BETWEEN '2013-09-07' AND '2013-10-07'
     GROUP BY IDProd) AS FilteredArrivals
     ON FilteredArrivals.IDProd = Products.ID --фильтруем только приходы по опр датам
WHERE (Sales.date BETWEEN '2013-10-01' AND '2013-10-07') 
GROUP BY Sellers.ID, Products.ID
ORDER BY Sellers.Surname, Sellers.Name;