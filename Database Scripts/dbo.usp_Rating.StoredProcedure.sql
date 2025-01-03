USE [resturentfood]
GO
/****** Object:  StoredProcedure [dbo].[usp_Rating]    Script Date: 1/3/2025 3:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[usp_Rating]

@OpMode INT = 0,
@RatingID BIGINT = 0,
@OrderID BIGINT = 0,
@FoodRating INT = 0,
@WaiterRating INT = 0,
@RestaurantRating INT = 0

AS
BEGIN
	IF @OpMode = 0
	BEGIN
		SELECT RatingID, OrderID, FoodRating, WaiterRating, RestaurantRating
		FROM UserRating;
	END
	ELSE IF @OpMode = 1 -- Insert Rating
	BEGIN
		INSERT INTO UserRating (FoodRating, WaiterRating, RestaurantRating, OrderID)
		VALUES (@FoodRating, @WaiterRating, @RestaurantRating, @OrderID);
	END
END
GO
