USE AdventureWorks2014
GO

CREATE PROCEDURE	sp_IngresarResena
(
	@productName NVARCHAR(50),
	@reviewerName NVARCHAR(50),
	@reviewDate DATETIME,
	@emailAddress NVARCHAR(50),
	@rating INT,
	@comments NVARCHAR(3850) NULL,
	@modifiedDate DATETIME	
)
AS
BEGIN
	DECLARE @productId	INT 

	SELECT	@productId = ProductID
	FROM	Production.Product
	WHERE Name = @productName

	INSERT INTO Production.ProductReview (ProductID, ReviewerName, ReviewDate, EmailAddress, Rating, Comments, ModifiedDate)
				VALUES (@productId, @reviewerName, @reviewDate, @emailAddress, @rating, @comments, @modifiedDate)
END