/* @hasexpects = true
<expects>
	<expect name="res1a" type="string"  value="FluentScript" />
    <expect name="res1b" type="string"  value="CodeHelix" />
	<expect name="res1c" type="number"  value="100" />
    <expect name="res1d" type="string"  value="FluentScript" />
    <expect name="res1e" type="string"  value="CodeHelix" />
	<expect name="res1f" type="number"  value="100" />
    <expect name="res2a" type="string"  value="FluentScript" />
    <expect name="res2b" type="string"  value="CodeHelix" />
    <expect name="res2c" type="string"  value="FluentScript" />
    <expect name="res2d" type="string"  value="CodeHelix" />
    <expect name="res3a" type="string"  value="FluentScript1" />
    <expect name="res3b" type="string"  value="FluentScript1" />
    <expect name="res3c" type="number"  value="102" />
    <expect name="res3d" type="number"  value="102" />
    <expect name="res4a" type="number"  value="101" />
    <expect name="res4b" type="number"  value="102" />
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

function getPropVal(obj, propName)
{
    return obj[propName]
}


function getPropVal2(obj, propName)
{
    return obj[propName]
}


var book = {  name: "FluentScript", author: "CodeHelix", pages: 100 }


// 1. assigment
var res1a = book.name
var res1b = book.author
var res1c = book.pages
var res1d = book["name"]
var res1e = book["author"]
var res1f = book["pages"]


// 2. function param
var res2a = getPropVal(book, "name")
var res2b = getPropVal(book, "author")
var res2c = getPropVal2( {  name: "FluentScript", author: "CodeHelix", pages: 100 }, "name" )
var res2d = getPropVal2( {  name: "FluentScript", author: "CodeHelix", pages: 100 }, "author" )


// 3. array
var books = [
            { name: "FluentScript1", author: "CodeHelix1", pages: 101 }, 
            { name: "FluentScript2", author: "CodeHelix2", pages: 102 },
            { name: "FluentScript3", author: "CodeHelix3", pages: 103 }
         ]

var res3a = books[0].name
var res3b = books[0]["name"]
var res3c = books[1].pages
var res3d = books[1]["pages"]


// 4. map
var m = { 
            name: 'test', 
            val1: { name: "FluentScript1", author: "CodeHelix1", pages: 101}, 
            val2: { name: "FluentScript2", author: "CodeHelix1", pages: 102}
        }
var res4a = m.val1.pages
var res4b = m.val2.pages


// 5. math
var res5a = 3 * books[0].pages + 1


// 6. compare
var res6a = 0
var res6b = 1
if( book.name == "FluentScript" )  res6a = 1
if( 105 < book.pages ) res6b = 0



// 7. condition
var res7a = 0
if( 101 == books[0].pages && 2 < 4 ) res7a = 1


// 8. Set
var book2 = {  name: "FluentScript", author: "CodeHelix", pages: 100 }
var res8a = book2.name
var res8b = book2.pages
book2.name = "new name"
book2.pages = 200
var res8c = book2.name
var res8d = book2.pages

// Test with nested objects
var m2 = { 
            name: 'test', 
            val1: { name: "FluentScript1", author: "CodeHelix1", pages: 101}, 
            val2: { name: "FluentScript2", author: "CodeHelix1", pages: 102}
        }
var res8e = m2.val1.name
var res8f = m2.val1.pages
m2.val1.name = "new nested name"
m2.val1.pages = 220
var res8g = m2.val1.name
var res8h = m2.val1.pages


// 9. end of script
var resEnd = books[1].author
