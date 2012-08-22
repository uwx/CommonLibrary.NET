/* @hasexpects = true
<expects>
	<expect name="res1" type="number" value="6" />
	<expect name="res2" type="number" value="7" />
	<expect name="res3" type="number" value="6" />
	<expect name="res4" type="number" value="6" />
	<expect name="res5" type="number" value="8" />
	<expect name="res6" type="number" value="5" />
	<expect name="res7" type="number" value="1" />
    <expect name="res8" type="number" value="1" />
    <expect name="res9" type="number" value="6" />
</expects>
*/


function add1( a ) { return a + 1; }
var nums = [1, 2, 3 ]

// SUM -----------------------------------------
// 1. assigment
var res1 = sum of nums

// 2. function param
var res2 = add1( sum of nums )

// 3. array
var ar = [ 4, sum of nums]
var res3 = ar[1]

// 4. map
var m = { name: 'test', val : sum of nums }
var res4 = m.val

// 5. math
var res5 = 2 + sum of nums
var res6 = sum of nums - 1

// 6. compare
var res7 = 0
if 7 > sum of nums then res7 = 1

// 7. condition
var res8 = 0
if sum of nums then res8 = 1

// 8. end of script
var res9 = sum of nums