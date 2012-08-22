/* @hasexpects = true
<expects>
    <expect name="res1a" type="string" value="q" />	
    <expect name="res1b" type="number" value="10" />
    <expect name="res1c" type="number" value="39" />
    <expect name="res1d" type="string" value="The quick brown cat jumps over the lazy dog" />
    <expect name="res1e" type="string" value="quick" />
    <expect name="res1f" type="string" value="quick" />

</expects>
*/

// 1. assignment
var text1 = "The quick brown fox jumps over the lazy dog"

var res1a = text1.charAt(4)
var res1b = text1.indexOf("brown")
var res1c = text1.lastIndexOf(" ")
var res1d = text1.replace("fox", "cat")
var res1e = text1.substr(4, 5)
var res1f = text1.substring(4, 8)


