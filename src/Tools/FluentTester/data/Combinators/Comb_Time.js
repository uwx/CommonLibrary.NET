/* @hasexpects = true
<expects>
	<expect name="res1"  type="time"    value="21:45" />
    <expect name="res2a" type="number"  value="19" />
	<expect name="res2b" type="number"  value="45" />
	<expect name="res3a" type="time"    value="8:00" />
	<expect name="res3b" type="time"    value="12:30" />
	<expect name="res3c" type="time"    value="16:00" />
	<expect name="res4a" type="time"    value="14:00" />
    <expect name="res4b" type="time"    value="7:30" />
   	<expect name="res6a" type="number"  value="1" />
    <expect name="res6b" type="number"  value="1" />
    <expect name="res7a" type="time"    value="9:00" />
    <expect name="resEnd" type="time"    value="10:15" />
</expects>
*/

function getTimePart(time, part)
{
    if(part == "hour")
        return time.Hours
    return time.Minutes
}


// 1. assigment
var res1 = 9:45 pm


// 2. function param
var res2a = getTimePart(7:30 pm, "hour")
var res2b = getTimePart(5:45 pm, "minutes")


// 3. array
var ar = [ 8am, 12:30 am, 4pm ]
var res3a = ar[0]
var res3b = ar[1]
var res3c = ar[2]


// 4. map
var m = { name: 'test', val: 2pm, val2: 7:30 am }
var res4a = m.val
var res4b = m.val2


// 5. math


// 6. compare
var res6a = 0
var res6b = 1
if( 5:30 pm < 8 pm ) res6a = 1
if( 6am < 4am ) res6b = 0


// 7. condition
var res7a = 0
if( 9am < 11 pm && 2 < 4 ) res7a = 9am

// 8. end of script
var resEnd = 10:15 am