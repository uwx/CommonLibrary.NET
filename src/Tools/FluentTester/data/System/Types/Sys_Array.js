/* @hasexpects = true
<expects>
	<expect name="res1a" type="string"  value="FluentScript" />
    <expect name="res1b" type="string"  value="CodeHelix" />
	<expect name="res1c" type="number"  value="100" />
    <expect name="res2a" type="string"  value="FluentScript" />
    <expect name="res2b" type="string"  value="CodeHelix" />
    <expect name="res2c" type="string"  value="FluentScript" />
    <expect name="res2d" type="string"  value="CodeHelix" />
    <expect name="res3a" type="string"  value="FluentScript1" />
    <expect name="res3b" type="number"  value="101" />
    <expect name="res3c" type="string"  value="FluentScript2" />
    <expect name="res3d" type="number"  value="102" />
    <expect name="res4a" type="string"  value="test" />
    <expect name="res4b" type="number"  value="101" />
    <expect name="res4c" type="number"  value="102" />
    <expect name="res5a" type="number"  value="304" />
    <expect name="res6a" type="number"  value="1" />
    <expect name="res6b" type="number"  value="1" />
    <expect name="res7a" type="number"  value="1" />
    <expect name="res8a" type="string"  value="FluentScript" />
    <expect name="res8b" type="number"  value="100" />
    <expect name="res8c" type="string"  value="new name" />
    <expect name="res8d" type="number"  value="200" />
    <expect name="res8e" type="string"  value="FluentScript1" />
    <expect name="res8f" type="number"  value="101" />
    <expect name="res8g" type="string"  value="new nested name" />
    <expect name="res8h" type="number"  value="220" />
    <expect name="resEnd" type="string" value="CodeHelix2" />
</expects>
*/

var newLineInArray = [ 1, 
                       2,
                     ]

function getPropVal(obj, propName)
{
    return obj[propName]
}


function getPropVal2(obj, propName)
{
    return obj[propName]
}


var book = ["FluentScript", "CodeHelix", 100 ]


// 1. assigment
var res1a = book[0]
var res1b = book[1]
var res1c = book[2]


// 2. function param
var res2a = getPropVal(book, 0)
var res2b = getPropVal(book, 1)
var res2c = getPropVal2( [ "FluentScript", "CodeHelix", 100 ], 0 )
var res2d = getPropVal2( [ "FluentScript", "CodeHelix", 100 ], 1 )



// 3. array
var books = [
                [ "FluentScript1", "CodeHelix1", 101 ], 
                [ "FluentScript2", "CodeHelix2", 102 ],
                [ "FluentScript3", "CodeHelix3", 103 ]
            ]

var res3a = books[0][0]
var res3b = books[0][2]
var res3c = books[1][0]
var res3d = books[1][2]


// 4. map
var m = [ 
            'test', 
            [ "FluentScript1", "CodeHelix1", 101 ], 
            [ "FluentScript2", "CodeHelix1", 102 ]
        ]
var res4a = m[0]
var res4b = m[1][2]
var res4c = m[2][2]


// 5. math
var res5a = 3 * books[0][2] + 1


// 6. compare
var res6a = 0
var res6b = 1
if( book[0] == "FluentScript" )  res6a = 1
if( 105 < book[2] ) res6b = 0


// 7. condition
var res7a = 0
if( 101 == books[0][2] && 2 < 4 ) res7a = 1


// 8. set values.
var book2 = ["FluentScript", "CodeHelix", 100]
var res8a = book2[0]
var res8b = book2[2]
book2[0] = "new name"
book2[2] = 200
var res8c = book2[0]
var res8d = book2[2]

var m2 = [
            'test',
            ["FluentScript1", "CodeHelix1", 101],
            ["FluentScript2", "CodeHelix1", 102]
        ]
var res8e = m2[1][0]
var res8f = m2[1][2]
m2[1][0] = "new nested name"
m2[1][2] = 220
var res8g = m2[1][0]
var res8h = m2[1][2]


// 9. end of script
var resEnd = books[1][1]