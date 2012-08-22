/* @hasexpects = true
<expects>
	<expect name="res1"  type="bool" value="true" />
	<expect name="res2"  type="bool" value="false" />
	<expect name="res3"  type="bool" value="true" />
    <expect name="res3a" type="bool" value="true" />
	<expect name="res3b" type="bool" value="false" />
	<expect name="res3c" type="bool" value="false" />
	<expect name="res3d" type="bool" value="true" />
	<expect name="res4a" type="bool" value="true" />
    <expect name="res4b" type="bool" value="false" />
	<expect name="res5a" type="bool" value="true" />
    <expect name="res5b" type="bool" value="false" />
	<expect name="res6a" type="number" value="3" />
    <expect name="res6b" type="number" value="1" />
	<expect name="res7"  type="number" value="1" />
    <expect name="res8"  type="number" value="1" />
    <expect name="res9"  type="number" value="1" />
    <expect name="res10" type="number" value="1" />
    <expect name="res11" type="bool" value="true" />
    <expect name="res12" type="bool" value="true" />
    <expect name="res13" type="bool" value="true" />
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
var res1 = yes

// 2. function param
var res2 = flip( yes )
var res3 = flip( no )
var res3a = flip2( yes, yes )
var res3b = flip2( yes, no )
var res3c = flip2( no, yes )
var res3d = flip2( no, no )

// 3. array
var ar = [ yes, yes, no, no]
var res4a = ar[1]
var res4b = ar[2]

// 4. map
var m = { name: 'test', valy: yes, valn: no }
var res5a = m.valy
var res5b = m.valn

// 5. math
var res6a = 2 + yes
var res6b = 2 - yes

// 6. compare
var res7 = no
var res8 = no
var res9 = no
var res10 = no

if res7 is no then res7  = 1
if res8 == no then res8  = 1
if ( res9 is no )  res9  = 1
if ( res10 == no ) res10 = 1

// 7. condition
var res11 = no
var res12 = no
if yes is true then res11 = yes
if no is false then res12 = yes

// 8. end of script
var res13 = yes