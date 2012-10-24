# Dates 
set birthday = January 1st 1979
if birthday is before 10/24/1991 then 
	println you can vote

	
# Day 
if today is not monday then 
	println thank god it's not the start of the work week!

	
# Money 
set salary = $250
if salary is more than $200 then 
	println I worked overtime

	
# time 
set time = 3 pm
if time is before 5pm then 
	println still have to work!
	

# select
set books = [  
				name         |	 pages   |  artist
                'batman'     ,	 110     ,  'john'
                'xmen'       ,	 120     ,  'lee'
                'daredevil'  ,	 140     ,  'maleev'
            ]

			
set favorites = books where book.pages > 120
println( favorites[0].name )

# url
set site = www.yahoo.com
println( site )


# aggregates 
set numbers = [1, 2, 3, 4]
println( sum of numbers )


