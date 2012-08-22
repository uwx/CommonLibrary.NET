/* @hasexpects = true
<expects>
	<expect name="res1"  type="number" value="25" />
	<expect name="res2"  type="number" value="1026" />
    <expect name="res3a" type="number" value="1" />
	<expect name="res3b" type="number" value="20" />
	<expect name="res3c" type="number" value="35.8" />

	<expect name="res4a" type="number" value="85" />
    <expect name="res4b" type="number" value="283" />
	<expect name="res5a" type="number" value="38" />
    <expect name="res5b" type="number" value="-7" />
	<expect name="res6a" type="bool" value="true" />
    <expect name="res6b" type="bool" value="true" />
	<expect name="res7a"  type="bool" value="true" />
    <expect name="res7b"  type="bool" value="true" />

    <expect name="resEnd"  type="number" value="850920" />
</expects>
*/


function add( a, b ) { return a + b; }


// 1. assigment
var res1 = $25

// 2. function param
var res2 = add( $1024, 2 )

// 3. array
var ar = [ $1, $20, $35.8 ]
var res3a = ar[0]
var res3b = ar[1]
var res3c = ar[2]

// 4. map
var m = { name: 'test', val1: $85, val2: $283 }
var res4a = m.val1
var res4b = m.val2

// 5. math
var res5a = 2 + $18 * 2
var res5b = 2 - $90 / 10

// 6. compare
var res6a = no
var res6b = no

if( $223.34 == 223.34 ) res6a = true
if $250.28 != 251.58 then res6b = true

// 7. condition
var res7a = no
var res7b = no
if $20 > 17 && 2 > 1 then res7a = yes
if $2 * 1000 == 2000 && 10 != 2 then res7b = yes

// 8. end of script
var resEnd = $850920