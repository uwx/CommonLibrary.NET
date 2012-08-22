/* @hasexpects = true
<expects>
    <expect name="res1a"  type="string" value="MSFT" />    
    <expect name="res1b"  type="string" value="pricing policy" />
    <expect name="res1ba"  type="string" value="default premium pricing" />
    <expect name="res1c"  type="string" value="a new MSFT" />
    <expect name="res1d"  type="string" value="a new pricing policy" />
    <expect name="res1e"  type="string" value="MSFT stock" />    
    <expect name="res1f"  type="string" value="pricing policy stock" />
    <expect name="res1g"  type="string" value="a new MSFT stock" />
    <expect name="res1h"  type="string" value="a new pricing policy stock" />

    <expect name="res2a"  type="string" value="MSFT more" />    
    <expect name="res2b"  type="string" value="pricing policy more" />
    <expect name="res2c"  type="string" value="a new MSFT" />
    <expect name="res2d"  type="string" value="a new pricing policy" />
    <expect name="res2e"  type="string" value="MSFT stock" />    
    <expect name="res2f"  type="string" value="pricing policy stock" />
    <expect name="res2g"  type="string" value="a new MSFT stock" />
    <expect name="res2h"  type="string" value="a new pricing policy stock" />
    <expect name="res2i"  type="string" value="a new stock MSFT" />
    <expect name="res2j"  type="string" value="a new stock pricing policy" />

    <expect name="res3a"  type="string" value="MSFT" />
    <expect name="res3b"  type="string" value="pricing policy" />
    <expect name="res3c"  type="string" value="default premium pricing" />

    <expect name="res4a"  type="string" value="MSFT" />
    <expect name="res4b"  type="string" value="pricing policy" />
    <expect name="res4c"  type="string" value="default premium pricing" />

    <expect name="res5a" type="string" value="MSFT3 analytics" />
    <expect name="res5b" type="string" value="pricing policytrue fluent" />
    <expect name="res5c" type="string" value="default premium pricingtrue fluent" />

    <expect name="res6a" type="bool" value="true" />
    <expect name="res6b" type="bool" value="true" />
    <expect name="res6c" type="bool" value="true" />
    <expect name="res6d" type="bool" value="true" />
    <expect name="res6e" type="bool" value="true" />

    <expect name="res7a" type="bool" value="true" />
    <expect name="res7b" type="bool" value="true" />
    <expect name="res7c" type="bool" value="true" />
    <expect name="res7d" type="bool" value="true" />
    <expect name="res7e" type="bool" value="true" />

    <expect name="resEnd" type="string" value="pricing policy" />
</expects>    
*/

@words( MSFT, pricing policy, default premium pricing )


function append1( a )
{
    return a + " more"
}


function append2( a, b )
{
    return a + b
}


function append3( a, b, c )
{
    return a +  b + c
}


// 1. assigment
var res1a = MSFT
var res1b = pricing policy
var res1ba = default premium pricing
var res1c = "a new " + MSFT
var res1d = "a new " + pricing policy
var res1e = MSFT + " stock"
var res1f = pricing policy + " stock"
var res1g = "a new " + MSFT + " stock"
var res1h = "a new " + pricing policy + " stock"


// 2. function param
var res2a = append1( MSFT )
var res2b = append1( pricing policy  )
var res2c = append2( "a new ", MSFT )
var res2d = append2( "a new ", pricing policy  )
var res2e = append2( MSFT, " stock"            )
var res2f = append2( pricing policy, " stock"  )
var res2g = append3( "a new ", MSFT, " stock" )
var res2h = append3( "a new ", pricing policy, " stock" )
var res2i = append3( "a new ", "stock ", MSFT )
var res2j = append3( "a new ", "stock ", pricing policy )


// 3. array
var ar = [MSFT, pricing policy, default premium pricing]
var res3a = ar[0]
var res3b = ar[1]
var res3c = ar[2]


// 4. map
var m = { val1: MSFT, val2: pricing policy, val3: default premium pricing }
var res4a = m.val1
var res4b = m.val2
var res4c = m.val3


// 5. math
var res5a = MSFT + 3 + " analytics"
var res5b = pricing policy + true + " fluent"
var res5c = default premium pricing + true + " fluent"


// 6. compare
var res6a = no
var res6b = no
var res6c = no
var res6d = no
var res6e = no

if ( MSFT == "MSFT" )  res6a = yes
if ( pricing policy == "pricing policy" ) res6b = yes
if ( "MSFT" == MSFT )  res6c = yes
if ( "pricing policy" == pricing policy ) res6d = yes
if ( "default premium pricing" == default premium pricing ) res6e = yes


// 7. condition
var res7a = no
var res7b = no
var res7c = no
var res7d = no
var res7e = no
if ( MSFT == "MSFT" && 1 < 2 ) res7a = true
if (MSFT=="MSFT" && 1 < 2 ) res7b = true
if ( pricing policy == "pricing policy"  && 1 < 2 ) res7c = true
if (pricing policy=="pricing policy"  && 1 < 2 ) res7d = true
if ( pricing policy != "pricing policy2" && 1 < 2 ) res7e = true


// 8. end of script
var resEnd = pricing policy