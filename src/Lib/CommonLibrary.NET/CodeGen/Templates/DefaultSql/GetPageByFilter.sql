
CREATE PROCEDURE [dbo].[<%= model.TableName %>_GetByFilter]
(
	@Filter varchar(250),
	@PageIndex int,
	@PageSize int,
	@TotalRows int output
)
AS

BEGIN
	SET NOCOUNT ON;
	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON

	-- Create a temp table TO store the select results
    CREATE TABLE #posts
    (
        row int IDENTITY (0, 1) NOT NULL,
        id int
    )
	declare @sql as nvarchar(4000)
	
	-- create dynamic sql
	set @sql = 'SELECT <%= model.TableName %>.id FROM <%= model.TableName %> '
	if @Filter is not null and @Filter <> ''
		set @sql = @sql + 'WHERE ' + Convert(VarChar(250), @Filter)

	-- print @sql
	-- If owner, get back all the posts, regardless if they expired or not.
	INSERT INTO #posts 
		exec sp_executesql @sql
	
	-- Now obtain the <%= model.TableName %> in the page requested
	SELECT    <%= model.TableName %>.*
	FROM      <%= model.TableName %>
	WHERE	  <%= model.TableName %>.Id IN
			(
				SELECT	id 
				FROM  #posts
				WHERE row between ((@PageIndex - 1) * @PageSize) and ( @PageIndex * @PageSize ) -1
			)
	ORDER BY CreateDate desc
	
	-- Get the total number of records that matched the search.
	select @TotalRows = count(*) from #posts
END