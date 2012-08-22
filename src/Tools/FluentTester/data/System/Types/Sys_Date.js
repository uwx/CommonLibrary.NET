/* @hasexpects = true
<expects>
    <expect name="res1a" type="number" value="2012" />	
    <expect name="res1b" type="number" value="6" />
    <expect name="res1c" type="number" value="11" />
    <expect name="res1d" type="number" value="1" />
    <expect name="res1e" type="number" value="8" />
    <expect name="res1f" type="number" value="30" />
    <expect name="res1g" type="number" value="0" />

    <expect name="res1h" type="number" value="2012" />	
    <expect name="res1i" type="number" value="6" />
    <expect name="res1j" type="number" value="11" />
    <expect name="res1k" type="number" value="1" />
    <expect name="res1l" type="number" value="8" />
    <expect name="res1m" type="number" value="30" />
    <expect name="res1n" type="number" value="0" />

    <expect name="res2a" type="number" value="2013" />	
    <expect name="res2b" type="number" value="7" />
    <expect name="res2c" type="number" value="12" />
    <expect name="res2d" type="number" value="5" />
    <expect name="res2e" type="number" value="9" />
    <expect name="res2f" type="number" value="45" />
    <expect name="res2g" type="number" value="20" />

</expects>
*/

// 1. assignment
var date = June 11th 2012 at 8:30 am
var date2 = new Date(2012, 6, 11, 8, 30, 0)

var res1a = date.getFullYear()
var res1b = date.getMonth()
var res1c = date.getDate()
var res1d = date.getDay()
var res1e = date.getHours()
var res1f = date.getMinutes()
var res1g = date.getSeconds()

var res1h = date2.getFullYear()
var res1i = date2.getMonth()
var res1j = date2.getDate()
var res1k = date2.getDay()
var res1l = date2.getHours()
var res1m = date2.getMinutes()
var res1n = date2.getSeconds()


// 2. Set 
date.setFullYear(2013)
date.setMonth(7)
date.setDate(12)
date.setHours(9)
date.setMinutes(45)
date.setSeconds(20)

var res2a = date.getFullYear()
var res2b = date.getMonth()
var res2c = date.getDate()
var res2d = date.getDay()
var res2e = date.getHours()
var res2f = date.getMinutes()
var res2g = date.getSeconds()