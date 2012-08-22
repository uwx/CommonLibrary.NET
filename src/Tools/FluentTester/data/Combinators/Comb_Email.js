/* @hasexpects = true
<expects>
    <expect name="res1a"  type="string" value="super.man@metrop.com" />    
    <expect name="res1b"  type="string" value="bat_man@gotham.com" />
    <expect name="res1ba"  type="string" value="spider.man.2080@marvel.com" />
    <expect name="res1c"  type="string" value="a new super.man@metrop.com" />
    <expect name="res1d"  type="string" value="a new bat_man@gotham.com" />
    <expect name="res1e"  type="string" value="super.man@metrop.com stock" />    
    <expect name="res1f"  type="string" value="bat_man@gotham.com stock" />
    <expect name="res1g"  type="string" value="a new super.man@metrop.com stock" />
    <expect name="res1h"  type="string" value="a new bat_man@gotham.com stock" />

    <expect name="res2a"  type="string" value="super.man@metrop.com more" />    
    <expect name="res2b"  type="string" value="bat_man@gotham.com more" />
    <expect name="res2c"  type="string" value="a new super.man@metrop.com" />
    <expect name="res2d"  type="string" value="a new bat_man@gotham.com" />
    <expect name="res2e"  type="string" value="super.man@metrop.com stock" />    
    <expect name="res2f"  type="string" value="bat_man@gotham.com stock" />
    <expect name="res2g"  type="string" value="a new super.man@metrop.com stock" />
    <expect name="res2h"  type="string" value="a new bat_man@gotham.com stock" />
    <expect name="res2i"  type="string" value="a new stock super.man@metrop.com" />
    <expect name="res2j"  type="string" value="a new stock bat_man@gotham.com" />

    <expect name="res3a"  type="string" value="super.man@metrop.com" />
    <expect name="res3b"  type="string" value="bat_man@gotham.com" />
    <expect name="res3c"  type="string" value="spider.man.2080@marvel.com" />

    <expect name="res4a"  type="string" value="super.man@metrop.com" />
    <expect name="res4b"  type="string" value="bat_man@gotham.com" />
    <expect name="res4c"  type="string" value="spider.man.2080@marvel.com" />

    <expect name="res5a" type="string" value="super.man@metrop.com3 analytics" />
    <expect name="res5b" type="string" value="bat_man@gotham.comtrue fluent" />
    <expect name="res5c" type="string" value="spider.man.2080@marvel.comtrue fluent" />

    <expect name="res6a" type="bool" value="true" />
    <expect name="res6b" type="bool" value="true" />
    <expect name="res6c" type="bool" value="true" />
    <expect name="res6d" type="bool" value="true" />
    <expect name="res6e" type="bool" value="true" />

    <expect name="res7a" type="bool" value="true" />
    <expect name="res7b" type="bool" value="true" />
    <expect name="res7c" type="bool" value="true" />

    <expect name="resEnd" type="string" value="bat_man@gotham.com" />
</expects>    
*/

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
var res1a = super.man@metrop.com
var res1b = bat_man@gotham.com
var res1ba = spider.man.2080@marvel.com
var res1c = "a new " + super.man@metrop.com
var res1d = "a new " + bat_man@gotham.com
var res1e = super.man@metrop.com + " stock"
var res1f = bat_man@gotham.com + " stock"
var res1g = "a new " + super.man@metrop.com + " stock"
var res1h = "a new " + bat_man@gotham.com + " stock"


// 2. function param
var res2a = append1( super.man@metrop.com )
var res2b = append1( bat_man@gotham.com  )
var res2c = append2( "a new ", super.man@metrop.com )
var res2d = append2( "a new ", bat_man@gotham.com  )
var res2e = append2( super.man@metrop.com, " stock"            )
var res2f = append2( bat_man@gotham.com, " stock"  )
var res2g = append3( "a new ", super.man@metrop.com, " stock" )
var res2h = append3( "a new ", bat_man@gotham.com, " stock" )
var res2i = append3( "a new ", "stock ", super.man@metrop.com )
var res2j = append3( "a new ", "stock ", bat_man@gotham.com )


// 3. array
var ar = [super.man@metrop.com, bat_man@gotham.com, spider.man.2080@marvel.com]
var res3a = ar[0]
var res3b = ar[1]
var res3c = ar[2]


// 4. map
var m = { val1: super.man@metrop.com, val2: bat_man@gotham.com, val3: spider.man.2080@marvel.com }
var res4a = m.val1
var res4b = m.val2
var res4c = m.val3


// 5. math
var res5a = super.man@metrop.com + 3 + " analytics"
var res5b = bat_man@gotham.com + true + " fluent"
var res5c = spider.man.2080@marvel.com + true + " fluent"


// 6. compare
var res6a = no
var res6b = no
var res6c = no
var res6d = no
var res6e = no

if ( super.man@metrop.com == "super.man@metrop.com" )  res6a = yes
if ( bat_man@gotham.com == "bat_man@gotham.com" ) res6b = yes
if ( "super.man@metrop.com" == super.man@metrop.com )  res6c = yes
if ( "bat_man@gotham.com" == bat_man@gotham.com ) res6d = yes
if ( "spider.man.2080@marvel.com" == spider.man.2080@marvel.com ) res6e = yes


// 7. condition
var res7a = no
var res7b = no
var res7c = no
if ( super.man@metrop.com == "super.man@metrop.com" && 1 < 2 ) res7a = true
if ( bat_man@gotham.com == "bat_man@gotham.com"  && 1 < 2 ) res7b = true
if ( bat_man@gotham.com != "bat_man@gotham.com2" && 1 < 2 ) res7c = true


// 8. end of script
var resEnd = bat_man@gotham.com