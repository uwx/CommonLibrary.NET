/* @hasexpects = true
<expects>
	<expect name="res1a"  type="string" value="c#" />
    <expect name="res1b"  type="string" value="fluent" />
    <expect name="res1c"  type="string" value="ruby" />
    <expect name="res1d"  type="string" value="java" />
    <expect name="res1e"  type="string" value="ruby" />
    <expect name="res1f"  type="string" value="ruby" />
    <expect name="res1g"  type="string" value="java" />
    <expect name="res1h"  type="string" value="ruby" />

    <expect name="res2a"  type="string" value="c#" />
    <expect name="res2b"  type="string" value="fluent" />
    <expect name="res2c"  type="string" value="ruby" />
    <expect name="res2d"  type="string" value="java" />

    <expect name="res3a"  type="string" value="c#" />
    <expect name="res3b"  type="string" value="fluent" />
    <expect name="res3c"  type="string" value="ruby" />
    <expect name="res3d"  type="string" value="java" />

    <expect name="res4a"  type="string" value="c#" />
    <expect name="res4b"  type="string" value="fluent" />
    <expect name="res4c"  type="string" value="ruby" />
    <expect name="res4d"  type="string" value="java" />

    <expect name="res6a"  type="bool" value="true" />
    <expect name="res7a"  type="bool" value="true" />
    <expect name="resEnd" type="string" value="c#" />
</expects>
*/

function select( list, index, prop)
{
    return list[index][prop]
}


var books = [   
                { name: 'ruby',   pages: 200,  published: 1/20/2012,  isBestSeller: true,   author: 'matz'  },
                { name: 'fluent', pages: 120,  published: 4/20/2012,  isBestSeller: false,  author: 'kdog'  },
                { name: 'c#',     pages: 140,  published: 8/20/2012,  isBestSeller: true,   author: 'micro' },
                { name: 'java',   pages: 140,  published: 10/20/2012, isBestSeller: false,  author: 'sun'   }
            ]


// 1. assigment
var res1a = ( sort books by book.name         asc  )[0].name
var res1b = ( sort books by book.pages        asc  )[0].name
var res1c = ( sort books by book.published    asc  )[0].name
var res1d = ( sort books by book.isBestSeller asc  )[0].name
var res1e = ( sort books by book.name         desc )[0].name
var res1f = ( sort books by book.pages        desc )[0].name
var res1g = ( sort books by book.published    desc )[0].name
var res1h = ( sort books by book.isBestSeller desc )[0].name



// 2. function param
var res2a = select( sort books by book.name         asc, 0, "name")
var res2b = select( sort books by book.pages        asc, 0, "name")
var res2c = select( sort books by book.published    asc, 0, "name")
var res2d = select( sort books by book.isBestSeller asc, 0, "name")


// 3. array
var ar = [ 
            ( sort books by book.name         asc  )[0].name,
            ( sort books by book.pages        asc  )[0].name,
            ( sort books by book.published    asc  )[0].name,
            ( sort books by book.isBestSeller asc  )[0].name
         ]
var res3a = ar[0]
var res3b = ar[1]
var res3c = ar[2]
var res3d = ar[3]


// 4. map
var m = {             
            val0: ( sort books by book.name         asc  )[0].name,
            val1: ( sort books by book.pages        asc  )[0].name,
            val2: ( sort books by book.published    asc  )[0].name,
            val3: ( sort books by book.isBestSeller asc  )[0].name
        }

var res4a = m.val0
var res4b = m.val1
var res4c = m.val2
var res4d = m.val3


// 5. math
//var res5a = 2 + (books where book.pages == 120)[0].pages


// 6. compare
var res6a = no
if( ( sort books by book.name asc  )[0].name == 'c#' ) res6a = true;


// 7. condition
var res7a = no
if ( ( sort books by book.name asc  )[0].name == 'c#'  && 2 > 1 ) res7a = yes


// 8. end of script
var resEnd = ( sort books by book.name asc  )[0].name