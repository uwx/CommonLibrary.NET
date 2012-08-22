/* @hasexpects = true
<expects>
	<expect name="res1"  type="datetime" value="6/1/2012" />
	<expect name="res2"  type="datetime" value="3/11/2012" />
    <expect name="res3a" type="datetime" value="1/1/2012" />
	<expect name="res3b" type="datetime" value="2/2/2012" />
	<expect name="res3c" type="datetime" value="3/3/2012" />
	<expect name="res4a" type="datetime" value="1/27/2012" />
    <expect name="res4b" type="datetime" value="8/22/2009" />
   	<expect name="res6a" type="number"   value="1" />
    <expect name="res6b" type="number"   value="1" />
    <expect name="res7a" type="datetime"  value="10/20/2012" />
    <expect name="res12" type="datetime" value="4/20/2012" />
</expects>
*/


function add1( d )
{
    var day = d.getDate() + 1
    return d.setDate(day)
}


// 1. assigment
var res1 = June 1 2012


// 2. function param
var res2 = add1( March 10th, 2012 )


// 3. array
var ar = [ Jan 1, Feb 2nd, March 3 2012]
var res3a = ar[0]
var res3b = ar[1]
var res3c = ar[2]


// 4. map
var m = { name: 'test', val: January 27th, 2012, val2: August 22nd 2009 }
var res4a = m.val
var res4b = m.val2


// 5. math


// 6. compare
var res6a = 0
var res6b = 1
if( feb 20 < march 15th ) res6a = 1
if( apr 1st 2012 < jan 2nd 2012 ) res6b = 0


// 7. condition
var res7a = 0
if( 7/04/2012 < 8/10/2012 && 2 < 4 ) res7a = 10/20/2012

// 8. end of script
var res12 = April 20th 2012