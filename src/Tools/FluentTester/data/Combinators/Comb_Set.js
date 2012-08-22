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
set res1 = yes

// 2. function param
set res2 = flip( yes )
set res3 = flip( no )
set res3a = flip2( yes, yes )
set res3b = flip2( yes, no )
set res3c = flip2( no, yes )
set res3d = flip2( no, no )

// 3. array
set ar = [ yes, yes, no, no]
set res4a = ar[1]
set res4b = ar[2]

// 4. map
set m = { name: 'test', valy: yes, valn: no }
set res5a = m.valy
set res5b = m.valn

// 5. math
set res6a = 2 + yes
set res6b = 2 - yes

// 6. compare
set res7 = no
set res8 = no
set res9 = no
set res10 = no

if res7 is no then res7  = 1
if res8 == no then res8  = 1
if ( res9 is no )  res9  = 1
if ( res10 == no ) res10 = 1

// 7. condition
set res11 = no
set res12 = no
if yes is true then res11 = yes
if no is false then res12 = yes

// 8. end of script
set res13 = yes