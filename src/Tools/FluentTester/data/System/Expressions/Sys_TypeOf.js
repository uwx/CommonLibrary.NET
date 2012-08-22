/* @hasexpects = true
<expects>
	<expect name="res1a"  type="string" value="string" />
    <expect name="res1b"  type="string" value="number" />
    <expect name="res1c"  type="string" value="number" />
    <expect name="res1d"  type="string" value="boolean" />
    <expect name="res1e"  type="string" value="boolean" />
    <expect name="res1f"  type="string" value="datetime" />
    <expect name="res1g"  type="string" value="time" />
    <expect name="res1h"  type="string" value="object:list" />
    <expect name="res1i"  type="string" value="object:map" />
    <expect name="res1j"  type="string" value="object:ComLib.Tools.FluentTester.Samples.Person" />

    <expect name="res2a"  type="string" value="type: string name" />
    <expect name="res2b"  type="string" value="type: number name" />
    <expect name="res2c"  type="string" value="type: number name" />
    <expect name="res2d"  type="string" value="type: boolean name" />
    <expect name="res2e"  type="string" value="type: boolean name" />
    <expect name="res2f"  type="string" value="type: datetime name" />
    <expect name="res2g"  type="string" value="type: time name"    />
    <expect name="res2h"  type="string" value="type: object:list name" />
    <expect name="res2i"  type="string" value="type: object:map name" />

    <expect name="res3a"  type="string" value="number" />
    <expect name="res3b"  type="string" value="boolean" />
    <expect name="res3c"  type="string" value="datetime" />
    <expect name="res3d"  type="string" value="time" />
    <expect name="res3e"  type="string" value="string" />

    <expect name="res4a"  type="string" value="datetime" />
    <expect name="res4b"  type="string" value="number" />

    <expect name="res6a"  type="bool" value="true" />
    <expect name="res6b"  type="bool" value="true" />
    <expect name="res6c"  type="bool" value="true" />
    <expect name="res6d"  type="bool" value="true" />
    <expect name="res6e"  type="bool" value="true" />
    <expect name="res6f"  type="bool" value="true" />
    <expect name="res6g"  type="bool" value="true" />
    <expect name="res6h"  type="bool" value="true" />
    <expect name="res6i"  type="bool" value="true" />

    <expect name="res7a"  type="bool" value="true" />
    <expect name="res7b"  type="bool" value="true" />
    <expect name="res7c"  type="bool" value="true" />
    <expect name="res7d"  type="bool" value="true" />
    <expect name="res7e"  type="bool" value="true" />
    <expect name="res7f"  type="bool" value="true" />
    <expect name="res7g"  type="bool" value="true" />
    <expect name="res7h"  type="bool" value="true" />
    <expect name="res7i"  type="bool" value="true" />

    <expect name="resEnd" type="string" value="number" />
</expects>
*/

function getTypeName( prefix, type, suffix  )
{
    return prefix + type + suffix;
}

var names = [ "fluent", "script" ]
var lang  = { name: "fluentscript", author: "codehelix" }

// 1. assigment
var res1a = typeof 'fluentscript'
var res1b = typeof 12
var res1c = typeof 12.34
var res1d = typeof true
var res1e = typeof false
var res1f = typeof new Date()
var res1g = typeof 3pm
var res1h = typeof names
var res1i = typeof lang
var res1j = typeof new Person('fluent', 'script')


// 2. function param
var res2a = getTypeName( "type: ", typeof 'fluentscript' , " name" )
var res2b = getTypeName( "type: ", typeof 12             , " name" )
var res2c = getTypeName( "type: ", typeof 12.34          , " name" )
var res2d = getTypeName( "type: ", typeof true           , " name" ) 
var res2e = getTypeName( "type: ", typeof false          , " name" ) 
var res2f = getTypeName( "type: ", typeof new Date()     , " name" ) 
var res2g = getTypeName( "type: ", typeof 3pm            , " name" ) 
var res2h = getTypeName( "type: ", typeof names          , " name" ) 
var res2i = getTypeName( "type: ", typeof lang           , " name" ) 


// 3. array
var ar = [ typeof 12, typeof true, typeof new Date(), typeof 3pm , typeof 'test' ]
var res3a = ar[0]
var res3b = ar[1]
var res3c = ar[2]
var res3d = ar[3]
var res3e = ar[4]


// 4. map
var m = { name: 'test', type1: typeof new Date(), type2: typeof 22 }
var res4a = m.type1
var res4b = m.type2


// 5. math



// 6. compare
var res6a = no
var res6b = no
var res6c = no
var res6d = no
var res6e = no
var res6f = no
var res6g = no
var res6h = no
var res6i = no

var res7a = no
var res7b = no
var res7c = no
var res7d = no
var res7e = no
var res7f = no
var res7g = no
var res7h = no
var res7i = no

if typeof 'fluentscript' == typeof 'fluentscript' then res6a = true
if typeof 12             == typeof 12             then res6b = true
if typeof 12.34          == typeof 12.34          then res6c = true
if typeof true           == typeof true           then res6d = true
if typeof false          == typeof false          then res6e = true
if typeof new Date()     == typeof new Date()     then res6f = true
if typeof 3pm            == typeof 3pm            then res6g = true
if typeof names          == typeof names          then res6h = true
if typeof lang           == typeof lang           then res6i = true

if typeof 'fluentscript' == "string"              then res7a = true
if typeof 12             == "number"              then res7b = true
if typeof 12.34          == "number"              then res7c = true
if typeof true           == "boolean"             then res7d = true
if typeof false          == "boolean"             then res7e = true
if typeof new Date()     == "datetime"            then res7f = true
if typeof 3pm            == "time"                then res7g = true
if typeof names          == "object:list"         then res7h = true
if typeof lang           == "object:map"          then res7i = true

// 8. end of script
var resEnd = typeof 2893