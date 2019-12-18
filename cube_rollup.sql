-------------------------------------------------------

:setvar NAME "SampleGroupingFunctions"

-------------------------------------------------------

IF OBJECT_ID('$(TEST_SCHEMA).$(TEST_PREFIX)$(NAME)', 'P') IS NOT NULL
    DROP PROCEDURE [$(TEST_SCHEMA)].[$(TEST_PREFIX)$(NAME)];
GO
 
-------------------------------------------------------

PRINT 'Create procedure [$(TEST_SCHEMA)].[$(TEST_PREFIX)$(NAME)]';
PRINT '---';
GO

CREATE PROCEDURE [$(TEST_SCHEMA)].[$(TEST_PREFIX)$(NAME)]
AS
BEGIN
	-- https://dzone.com/articles/t-sql-rollup-and-cube
	-- https://www.sqlpedia.pl/wielokrotne-grupowanie-grouping-sets-rollup-cube/

	IF OBJECT_ID('tempdb..#Source') IS NOT NULL
		DROP TABLE #Source;

	SELECT * 
		INTO #Source
	FROM (
	VALUES 
		('India', 'Delhi', 'East Delhi', 9),
		('India', 'Delhi', 'South Delhi', 8),
		('India', 'Delhi', 'North Delhi', 5.5),
		('India', 'Delhi', 'West Delhi', 7.5),
		('India', 'Karnataka', 'Bangalore', 9.5),
		('India', 'Karnataka', 'Belur', 2.5),
		('India', 'Karnataka', 'Manipal', 1.5),
		('India', 'Maharastra', 'Mumbai', 30),
		('India', 'Maharastra', 'Pune', 20),
		('Poland', 'Mazowsze', 'Warszawa', 11),
		('Poland', 'Mazowsze', 'Pruszkow', 6.5)
	) AS t ([Country], [State], [City], [Population]);

	--- ROLLUP --- czyli agregaty dla hierarchii

	IF OBJECT_ID('tempdb..#Actual') IS NOT NULL
		DROP TABLE #Actual;
    IF OBJECT_ID('tempdb..#Expected') IS NOT NULL
		DROP TABLE #Expected;

	SELECT [Country], [State], [City], SUM ([Population]) AS Population2 
		INTO #Actual
	FROM #Source
	GROUP BY [Country], [State], [City] WITH ROLLUP -- ROLLUP([Country], [State], [City])
	ORDER BY [Country], [State], [City], [Population2];

	SELECT TOP(0) * INTO #Expected FROM #Actual;

    INSERT INTO #Expected
    VALUES
		(NULL, NULL, NULL, 111),
		('India', NULL, NULL, 93.5),
		('India', 'Delhi', NULL, 30.0),
        ('India', 'Delhi', 'East Delhi', 9),
		('India', 'Delhi', 'South Delhi', 8),
		('India', 'Delhi', 'North Delhi', 5.5),
		('India', 'Delhi', 'West Delhi', 7.5),
		('India', 'Karnataka', NULL, 13.5),
		('India', 'Karnataka', 'Bangalore', 9.5),
		('India', 'Karnataka', 'Belur', 2.5),
		('India', 'Karnataka', 'Manipal', 1.5),
		('India', 'Maharastra', NULL, 50.0),
		('India', 'Maharastra', 'Mumbai', 30),
		('India', 'Maharastra', 'Pune', 20),
		('Poland', NULL, NULL, 17.5),
		('Poland', 'Mazowsze', NULL, 17.5),
		('Poland', 'Mazowsze', 'Warszawa', 11),
		('Poland', 'Mazowsze', 'Pruszkow', 6.5);

	EXEC tSQLt.AssertEqualsTable '#Expected', '#Actual';

	--- CUBE --- czyli kazdy z kazdym

	IF OBJECT_ID('tempdb..#ActualCube') IS NOT NULL
		DROP TABLE #ActualCube;
    IF OBJECT_ID('tempdb..#ExpectedCube') IS NOT NULL
		DROP TABLE #ExpectedCube;

	SELECT [Country], [State], [City], SUM ([Population]) AS Population2 
		INTO #ActualCube
	FROM #Source
	WHERE [Country] = 'Poland'
	GROUP BY [Country], [State], [City] WITH CUBE -- CUBE([Country], [State], [City])
	ORDER BY [Country], [State], [City], [Population2];

	SELECT TOP(0) * INTO #ExpectedCube FROM #ActualCube;

    INSERT INTO #ExpectedCube
    VALUES
		(NULL, NULL, NULL, 17.5),
		(NULL, 'Mazowsze', NULL, 17.5),
		(NULL, 'Mazowsze', 'Warszawa', 11),
		(NULL, 'Mazowsze', 'Pruszkow', 6.5),
		(NULL, NULL, 'Warszawa', 11),
		(NULL, NULL, 'Pruszkow', 6.5),

		('Poland', NULL, NULL, 17.5),
		('Poland', NULL, 'Pruszkow', 6.5),
		('Poland', NULL, 'Warszawa', 11),

		('Poland', 'Mazowsze', NULL, 17.5),
		('Poland', 'Mazowsze', 'Warszawa', 11),
		('Poland', 'Mazowsze', 'Pruszkow', 6.5);

	EXEC tSQLt.AssertEqualsTable '#ExpectedCube', '#ActualCube';

	--- GROUPING SETS --- podobnie jak w rollupie tworzy podsumowania dla hierarchii, ale okrelalmy ktore elementy hierarchii nas interesuja

	IF OBJECT_ID('tempdb..#ActualGroupingSets') IS NOT NULL
		DROP TABLE #ActualGroupingSets;
    IF OBJECT_ID('tempdb..#ExpectedGroupingSets') IS NOT NULL
		DROP TABLE #ExpectedGroupingSets;

	SELECT [Country], [City], SUM ([Population]) AS Population2 
		INTO #ActualGroupingSets
	FROM #Source
	GROUP BY GROUPING SETS (
		([Country]),
		([Country], [City])
	)
	ORDER BY [Country], [City], [Population2];

	SELECT TOP(0) * INTO #ExpectedGroupingSets FROM #ActualGroupingSets;

    INSERT INTO #ExpectedGroupingSets
    VALUES
		('India', NULL, 93.5),
        ('India', 'East Delhi', 9),
		('India', 'South Delhi', 8),
		('India', 'North Delhi', 5.5),
		('India', 'West Delhi', 7.5),
		('India', 'Bangalore', 9.5),
		('India', 'Belur', 2.5),
		('India', 'Manipal', 1.5),
		('India', 'Mumbai', 30),
		('India', 'Pune', 20),
		('Poland', NULL, 17.5),
		('Poland', 'Warszawa', 11),
		('Poland', 'Pruszkow', 6.5);

	EXEC tSQLt.AssertEqualsTable '#ExpectedGroupingSets', '#ActualGroupingSets';
END;
GO

-------------------------------------------------------
