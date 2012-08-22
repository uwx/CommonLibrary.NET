/* @hasexpects = true
<expects>
	<expect name="res1a1" type="string" value="fluent" />	
    <expect name="res1a2" type="string" value="script" />
    <expect name="res1a3" type="datetime" value="${month}/${day}/${year}" />
    <expect name="res1a4" type="bool" value="true" />
    <expect name="res1a5" type="string" value="fluent" />	
    <expect name="res1a6" type="string" value="script" />
    <expect name="res1a7" type="datetime" value="${month}/${day}/${year}" />
    <expect name="res1a8" type="bool" value="true" />
    <expect name="res1a9" type="bool" value="false" />
    <expect name="res1a10" type="bool" value="false" />
    <expect name="res1a11" type="string" value="fluent script" />
    <expect name="res1a12" type="string" value="fluent script" />
    <expect name="res1a13" type="string" value="kdog@abc.com" />
</expects>
    
*/
function getValue( val, suffix )
{
    return val + suffix
}


// 1. assigment
var user = new Person("fluent", "script")

// fluent properties
var res1a1  = user FirstName
var res1a2  = user LastName
var res1a3  = user BirthDate
var res1a4  = user IsMale
var res1a5  = user firstName
var res1a6  = user lastName
var res1a7  = user birthDate
var res1a8  = user isMale
var res1a9  = IsActive user
var res1a10 = isactive user


// 1a. fluent methods
var res1a11 = user fullname
var res1a12 = fullname user

user changeinfo 'kdog', 'red', kdog@abc.com, yes, $250, April 10th 1980
var res1a13 = user Email

/*




// 1b. fluent static methods
var res1a13 = tofullname user 'kishore', 'reddy'
var res1a14 = user tofullname 'kishore', 'reddy'



// 2. function param
var res2a1 = getValue( user FirstName , " after" )
var res2a2 = getValue( user LastName  , " after" )
var res2a3 = getValue( user BirthDate , " after" )
var res2a4 = getValue( user IsMale    , " after" )
var res2a5 = getValue( firstname user , " after" )
var res2a6 = getValue( user fullname  , " after" )
var res2a7 = getValue( fullname user  , " after" )
var res2a8 = getValue( , " after" )


// 3. array
var ar = [ user  ]
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
*/
user changeinfo 'kdog2', 'red2', kdog@abc.com, yes, $250, April 10th 1980