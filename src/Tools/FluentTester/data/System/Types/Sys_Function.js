/* @hasexpects = true
<expects>
	<expect name="res1a"  type="number"  value="2" />
    <expect name="res1b"  type="number"  value="3" />
	<expect name="res1c"  type="number"  value="6" />
    <expect name="res2a"  type="number"  value="3" />
    <expect name="res2b"  type="number"  value="4" />
    <expect name="res2c"  type="number"  value="5" />
    <expect name="res2d"  type="number"  value="6" />
    <expect name="res3a"  type="number"  value="2" />
    <expect name="res3b"  type="number"  value="3" />
    <expect name="res3c"  type="number"  value="4" />
    <expect name="res4a"  type="number"  value="2" />
    <expect name="res4b"  type="number"  value="3" />
    <expect name="res4c"  type="number"  value="4" />
    <expect name="res5a"  type="number"  value="16" />
    <expect name="res6a"  type="number"  value="1" />
    <expect name="res6b"  type="number"  value="1" />
    <expect name="res7a"  type="number"  value="1" />
    <expect name="resEnd" type="number"  value="4" />
</expects>
*/


function add1(a) { return a + 1; }
function add2(a, b) { return a + b; }
function add3(a, b, c) { return a + b + c; }


// 1. assigment
var res1a = add1(1)
var res1b = add2(1, 2)
var res1c = add3(1, 2, 3)


// 2. function param
var res2a = add2( 1, add2( 1, 1 ) )
var res2b = add2( 1, add2( 1, 2 ) )
var res2c = add2( 1, add2( 1, 3 ) )
var res2d = add2( 1, add2( 1, 4 ) )



// 3. array
var values = [
                add2(1, 1), 
                add2(1, 2),
                add2(1, 3)
             ]

var res3a = values[0]
var res3b = values[1]
var res3c = values[2]


// 4. map
var m = {
            val1 : add2( 1, 1 ),
            val2 : add2( 1, 2 ),
            val3 : add2( 1, 3 )
        }

var res4a = m.val1
var res4b = m.val2
var res4c = m.val3


// 5. math
var res5a = 3 * add2(2, 3) + 1


// 6. compare
var res6a = 0
var res6b = 1

if( add3(1, 2, 3 ) == 6 )  res6a = 1
if( 105 < add2(100, 1) ) res6b = 0


// 7. condition
var res7a = 0
if( 101 == add2(100, 1) && 2 < 4 ) res7a = 1


// 8. end of script
var resEnd = add2(1, 3)