set books = [  name	       |	 pages   |  artist
               'batman'    |	 110     |  'john'
               'xmen'      |	 120     |  'lee'
               'daredevil' |	 140     |  'maleev' ]; 
set favorites = books where book.pages > 120; 
print( favorites[0].name );