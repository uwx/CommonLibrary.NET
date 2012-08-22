/* @hasexpects = true
<expects>
	<expect name="res1a1"   type="number"  value="1" />
    <expect name="res1a2"   type="number"  value="1" />
	<expect name="res1a3"   type="number"  value="1" />
	<expect name="res1a4"   type="number"  value="1" />
	<expect name="res1a5"   type="number"  value="1" />
	<expect name="res1a6"   type="number"  value="1" />
	<expect name="res1a7"   type="number"  value="1" />
    <expect name="res1a8"   type="number"  value="1" />
    <expect name="res1a9"   type="number"  value="2" />
    <expect name="res1a10"  type="number"  value="2" />
    <expect name="res2a"    type="number"  value="2" />
    <expect name="res2b"    type="number"  value="2" />
    <expect name="res2c"    type="number"  value="3" />
    <expect name="res3a"    type="number"  value="1" />
    <expect name="res3b"    type="number"  value="1" />
    <expect name="res3c"    type="number"  value="2" />
    <expect name="res4a"    type="number"  value="1" />
    <expect name="res4b"    type="number"  value="1" />
    <expect name="res4c"    type="number"  value="2" />    
    <expect name="res5a"    type="number"  value="2" />
    <expect name="res5b"    type="number"  value="2" />
    <expect name="res5c"    type="number"  value="3" />
    <expect name="res6a"    type="number"  value="1" />
    <expect name="res6b"    type="number"  value="1" />
    <expect name="res6c"    type="number"  value="1" />
    <expect name="res7a"    type="number"  value="1" />
</expects>
*/


function get1()
{
    return 1;
}


function add1( a )
{
    return a + 1
}


function 'get 2'
{
    return 2
}


// 1. assigment
var res1a1 = run get1();
var res1a2 = run get1()
var res1a3 = run get1;
var res1a4 = run get1
var res1a5 = run 'get 2'
var res1a5 = run function get1();
var res1a6 = run function get1()
var res1a7 = run function get1;
var res1a8 = run function get1
var res1a9 = run function 'get 2';
var res1a10 = run function 'get 2'



// 2. function param
var res2a = add1( run get1()  )
var res2b = add1( run get1    )
var res2c = add1( run 'get 2' )


// 3. array
var ar = [ run get1(), run get1, run 'get 2' ]
var res3a = ar[0]
var res3b = ar[1]
var res3c = ar[2]


// 4. map
var m = { name: 'test', val1: run get1, val2: run get1(), val3: run 'get 2' }
var res4a = m.val1
var res4b = m.val2
var res4c = m.val3


// 5. math
var res5a = run get1()  + 1
var res5b = run get1    + 1
var res5c = run 'get 2' + 1


// 6. compare
var res6a = 0
var res6b = 1
var res6c = 0
if( run get1 < 8 )    res6a = 1
if( 2 < run get1 )    res6b = 0
if( 1 < run 'get 2' ) res6c = 1


// 7. condition
var res7a = 0
if( 1 < run 'get 2' && 2 < 4 ) res7a = 1


// 8. end of script
run function get1