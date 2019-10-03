SELECT SUM(WizzDeposit.Difference) AS SumDifference 
FROM 
(SELECT wd.DepositAmount - LEAD(wd.DepositAmount) 
 OVER (ORDER BY wd.Id) AS [Difference]
 FROM WizzardDeposits AS wd) AS [WizzDeposit]