Select Sellers.Surname, Sellers.Name, SUM(Sales.Quantity) AS Total
FROM Sellers LEFT join Sales on Sellers.ID = Sales.IDsel
WHERE Sales.date BETWEEN '2013-10-01' AND '2013-10-07'
GROUP BY Sellers.ID
ORDER BY Sellers.Surname, Sellers.Name;