/* @hasexpects = true
<expects>
	<expect name="res1a"  type="string" value="book 2" />
    <expect name="res1b"  type="string" value="book 2" />
    <expect name="res1c"  type="string" value="kdog" />
    <expect name="res1d"  type="string" value="homeslice" />
    <expect name="res1e"  type="string" value="book 3" />

    <expect name="res2a"  type="string" value="book 2" />
    <expect name="res2b"  type="string" value="book 2" />
    <expect name="res2c"  type="string" value="kdog" />
    <expect name="res2d"  type="string" value="homeslice" />
    <expect name="res2e"  type="string" value="book 3" />

    <expect name="res3a"  type="string" value="book 2" />
    <expect name="res3b"  type="string" value="book 2" />
    <expect name="res3c"  type="string" value="kdog" />
    <expect name="res3d"  type="string" value="homeslice" />
    <expect name="res3e"  type="string" value="book 3" />

    <expect name="res4a"  type="string" value="book 2" />
    <expect name="res4b"  type="string" value="book 2" />
    <expect name="res4c"  type="string" value="kdog" />
    <expect name="res4d"  type="string" value="homeslice" />
    <expect name="res4e"  type="string" value="book 3" />

    <expect name="res6a"  type="bool" value="true" />
    <expect name="res6b"  type="bool" value="true" />

    <expect name="res7a"  type="bool" value="true" />
    <expect name="resEnd" type="number" value="140" />
</expects>
*/

function select( list, index, prop)
{
    return list[index][prop]
}


var books = [ 
                { name: 'book 1', pages: 200, published: 1/20/2012, author: 'homey'     },
                { name: 'book 2', pages: 120, published: 4/20/2012, author: 'kdog'      },
                { name: 'book 3', pages: 140, published: 8/20/2012, author: 'homeslice' }
            ]


// 1. assigment
var res1a = ( books where book.pages == 120)[0].name
var res1b = ( books where book.name  == 'book 2')[0].name
var res1c = ( books where book.published is 4/20/2012)[0].author
var res1d = ( books where book.published is after 2/20/2012)[1].author
var res1e = ( books where book.pages < 200 and book.author is 'homeslice')[0].name


// 2. function param
var res2a = select( books where book.pages == 120, 0, "name")
var res2b = select( books where book.name  == 'book 2', 0, "name")
var res2c = select( books where book.published is 4/20/2012, 0, "author")
var res2d = select( books where book.published is after 2/20/2012, 1, "author")
var res2e = select( books where book.pages < 200 and book.author is 'homeslice', 0, "name")


// 3. array
var ar = [ 
            ( books where book.pages == 120)[0].name,
            ( books where book.name  == 'book 2')[0].name,
            ( books where book.published is 4/20/2012)[0].author,
            ( books where book.published is after 2/20/2012)[1].author,
            ( books where book.pages < 200 and book.author is 'homeslice')[0].name
         ]
var res3a = ar[0]
var res3b = ar[1]
var res3c = ar[2]
var res3d = ar[3]
var res3e = ar[4]


// 4. map
var m = { 
            val0: ( books where book.pages == 120)[0].name,
            val1: ( books where book.name  == 'book 2')[0].name,
            val2: ( books where book.published is 4/20/2012)[0].author,
            val3: ( books where book.published is after 2/20/2012)[1].author,
            val4: ( books where book.pages < 200 and book.author is 'homeslice')[0].name
        }

var res4a = m.val0
var res4b = m.val1
var res4c = m.val2
var res4d = m.val3
var res4e = m.val4


// 5. math
//var res5a = 2 + (books where book.pages == 120)[0].pages


// 6. compare
var res6a = no
var res6b = no
if( (books where book.pages == 120)[0].name == 'book 2' ) res6a = true;
if( (books where book.pages < 200 and book.author is 'homeslice')[0].pages <= 150) res6b = true;


// 7. condition
var res7a = no
if ( (books where book.pages == 120)[0].name == 'book 2' && 2 > 1 ) res7a = yes


// 8. end of script
var resEnd = (books where book.pages < 200 and book.author is 'homeslice')[0].pages