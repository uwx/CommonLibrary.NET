/* @hasexpects = true
<expects>
	<expect name="res1"   type="dayofweek"  value="Saturday" />
    <expect name="res2a"  type="dayofweek"  value="Tuesday" />
	<expect name="res2b"  type="dayofweek"  value="Monday" />
	<expect name="res3a"  type="dayofweek"  value="Tuesday" />
	<expect name="res3b"  type="dayofweek"  value="Friday" />
	<expect name="res3c"  type="dayofweek"  value="Sunday" />
	<expect name="res4a"  type="dayofweek"  value="Monday" />
    <expect name="res4b"  type="dayofweek"  value="Saturday" />
   	<expect name="res6a"  type="number"     value="1" />
    <expect name="res6b"  type="number"     value="1" />
    <expect name="res7a"  type="dayofweek"  value="Sunday" />
    <expect name="resEnd" type="dayofweek"  value="Thursday" />
</expects>
*/

function getNextDay(day, part)
{
    if day is monday then return tuesday
    if day is tuesday then return wednesday
    if day is wednesday then return thursday
    if day is thursday then return Friday
    if day is Friday then return saturday
    if day is saturday then return Sunday
    if day is Sunday then return monday
    return Monday
}


// 1. assigment
var res1 = Saturday


// 2. function param
var res2a = getNextDay(Monday, "dummy")
var res2b = getNextDay(Sunday, "dummy")


// 3. array
var ar = [ tuesday, Friday, sunday ]
var res3a = ar[0]
var res3b = ar[1]
var res3c = ar[2]


// 4. map
var m = { name: 'test', val: Monday, val2: Saturday }
var res4a = m.val
var res4b = m.val2


// 5. math


// 6. compare
var res6a = 0
var res6b = 1
if( tuesday < Friday ) res6a = 1
if( Saturday < Monday ) res6b = 0


// 7. condition
var res7a = 0
if( Sunday is before Monday && 2 < 4 ) res7a = Sunday

// 8. end of script
var resEnd = thursday