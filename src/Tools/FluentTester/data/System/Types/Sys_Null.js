/* @hasexpects = true
<expects>
	<expect name="res1"  type="null" value="null" />
	<expect name="res2"  type="null" value="null" />
	<expect name="res3"  type="null" value="null" />
	<expect name="res4"  type="null" value="null" />
    <expect name="res5"  type="null" value="null" />
	<expect name="res6a" type="number" value="3" />
	<expect name="res7"  type="number" value="1" />
    <expect name="res8"  type="number" value="1" />
</expects>
*/


function flip( a )
{
    if( a ) 
        return no;
    
    return yes;
}


function flip2( a, b )
{
    if( a == true  && b == true ) return yes;    
    if( a == false && b == false) return yes;
    
    return no;
}


// 1. assigment
var res1 = null

// 2. function param
var res2 = flip( null )
var res3 = flip2( yes, null )

// 3. array
var ar = [ yes, null, no, no]
var res4a = ar[1]

// 4. map
var m = { name: 'test', valy: null, valn: no }
var res5a = m.valy

// 6. compare
var res7 = null
var res8 = null

if res7 == null then res7  = 1
if res8 != null then res8  = 1

// 8. end of script
var res13 = null