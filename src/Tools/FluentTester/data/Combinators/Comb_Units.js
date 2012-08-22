/* @hasexpects = true
<expects>
    <expect name="res1a1"  type="number" value="3" />
    <expect name="res1a2"  type="number" value="36" />
    <expect name="res1a3"  type="number" value="108" />
    <expect name="res1a4"  type="number" value="190080" />

    <expect name="res2a"  type="number" value="42" />
    <expect name="res2b"  type="number" value="114" />

    <expect name="res3a"  type="number" value="36" />
    <expect name="res3b"  type="number" value="3" />
    <expect name="res3c"  type="number" value="108" />

    <expect name="res4a"  type="number" value="36" />
    <expect name="res4b"  type="number" value="3" />
    <expect name="res4c"  type="number" value="108" />
</expects>    
*/

// Turn on the units features.
enable units;

function addUnits( u1, u2 )
{
    var result = u1 + u2
    return result.BaseValue
}


// 1. assigment
var res1a1 = ( 3 inches ).BaseValue
var res1a2 = ( 3 feet   ).BaseValue
var res1a3 = ( 3 yards  ).BaseValue
var res1a4 = ( 3 miles  ).BaseValue

// 2. function param
var res2a = addUnits( 3 feet,  6 inches )
var res2b = addUnits( 3 yards, 6 inches )


// 3. array
var ar = [ 3 feet, 3 inches, 3 yards ]
var res3a = ar[0].BaseValue
var res3b = ar[1].BaseValue
var res3c = ar[2].BaseValue


// 4. map
var m = { val1: 3 feet, val2: 3 inches, val3: 3 yards }
var res4a = m.val1.BaseValue
var res4b = m.val2.BaseValue
var res4c = m.val3.BaseValue


/*
// 5. math
var res5a = http://www.google.com + " analytics " + 3
var res5b = c:/dev/build.txt + " fluent " + true

// 6. compare
var res6a = no
var res6b = no
var res6c = no
var res6d = no
var res6e = no

if ( 3 feet == 1 yard )  res6a = yes
if ( 1 yard == 36 inches ) res6b = yes
if ( 3 inches == 3 inches ) res6c = yes



// 7. condition
var res7a = no
var res7b = no
if ( www.fluentscript.com == "www.fluentscript.com" && 1 < 2 ) res7a = true
if ( c:/fluentscript.exe != "www.fluentscript.com" && 1 < 2 ) res7b = true


// 8. end of script
var resEnd = http://news.yahoo.com/blogs/sideshow/winged-roller-coaster-swarm-rips-arms-off-crash-183513325.html
*/