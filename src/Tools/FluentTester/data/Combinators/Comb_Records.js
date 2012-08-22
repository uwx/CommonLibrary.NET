/* @hasexpects = true
<expects>
	<expect name="res1a" type="string"  value="FluentScript1" />
    <expect name="res1b" type="string"  value="CodeHelix1" />
	<expect name="res1c" type="number"  value="101" />
    <expect name="res1d" type="string"  value="FluentScript1" />
    <expect name="res1e" type="string"  value="CodeHelix1" />
	<expect name="res1f" type="number"  value="101" />

    <expect name="res2a" type="string"  value="FluentScript1" />
    <expect name="res2b" type="number"  value="102" />
    
    <expect name="res3a" type="string"  value="Fluent" />
    <expect name="res3b" type="string"  value="Fluent" />
    <expect name="res3c" type="number"  value="102" />
    <expect name="res3d" type="number"  value="102" />
    
    <expect name="res4a" type="string"  value="Kdog" />
    <expect name="res4b" type="string"  value="CodeHelix2" />  
    
    <expect name="res5a" type="number"  value="304" />
    <expect name="res6a" type="number"  value="1" />
    <expect name="res6b" type="number"  value="1" />
    <expect name="res7a" type="number"  value="1" />

    <expect name="res8a" type="string"  value="FluentScript1" />
    <expect name="res8b" type="number"  value="101" />
    <expect name="res8c" type="string"  value="new name" />
    <expect name="res8d" type="number"  value="200" />
    <expect name="res8e" type="string"  value="Amycat" />
    <expect name="res8f" type="string"  value="Ruby" />
    <expect name="res8g" type="string"  value="new author" />
    <expect name="res8h" type="string"  value="new book" />
</expects>


    
*/

function getTableVal(obj, rowNum, columnName)
{
    return obj[rowNum][columnName]
}



var books = [  
                name             |     pages   |  author
                "FluentScript1"  ,     101     ,  "CodeHelix1"
                "FluentScript2"  ,     102     ,  "CodeHelix2"
                "FluentScript3"  ,     103     ,  "CodeHelix3"
            ]

// 1. assigment
var res1a = books[0].name
var res1b = books[0].author
var res1c = books[0].pages
var res1d = books[0]["name"]
var res1e = books[0]["author"]
var res1f = books[0]["pages"]


// 2. function param
var res2a = getTableVal(books, 0, "name")
var res2b = getTableVal(books, 1, "pages")



// 3. array
var tables = [
                [  
                    author           |     age    |  book
                    "Kdog"           ,     35     ,  "Fluent"
                    "Amycat"         ,     36     ,  "Ruby"
                    "Maxpup"         ,     37     ,  "Scala"
                ],
                [  
                    name             |     pages   |  author
                    "FluentScript1"  ,     101     ,  "CodeHelix1"
                    "FluentScript2"  ,     102     ,  "CodeHelix2"
                    "FluentScript3"  ,     103     ,  "CodeHelix3"
                ]
            ]

var res3a = tables[0][0].book
var res3b = tables[0][0]["book"]
var res3c = tables[1][1].pages
var res3d = tables[1][1]["pages"]


// 4. map
var m = { 
            name: 'test', 
            val1:   [  
                        author           |     age    |  book
                        "Kdog"           ,     35     ,  "Fluent"
                        "Amycat"         ,     36     ,  "Ruby"
                        "Maxpup"         ,     37     ,  "Scala"
                    ],
            val2:   [ 
                        name             |     pages   |  author
                        "FluentScript1"  ,     101     ,  "CodeHelix1"
                        "FluentScript2"  ,     102     ,  "CodeHelix2"
                        "FluentScript3"  ,     103     ,  "CodeHelix3"
                    ] 
        }

var res4a = m.val1[0].author
var res4b = m.val2[1].author



// 5. math
var res5a = 3 * books[0].pages + 1


// 6. compare
var res6a = 0
var res6b = 1
if( books[0].name == "FluentScript1" )  res6a = 1
if( 105 < books[0].pages ) res6b = 0



// 7. condition
var res7a = 0
if( 101 == books[0].pages && 2 < 4 ) res7a = 1


// 8. Set
var books2 = [  
                name             |     pages   |  author
                "FluentScript1"  ,     101     ,  "CodeHelix1"
                "FluentScript2"  ,     102     ,  "CodeHelix2"
                "FluentScript3"  ,     103     ,  "CodeHelix3"
             ]
var res8a = books2[0].name
var res8b = books2[0].pages
books2[0].name = "new name"
books2[0].pages = 200
var res8c = books2[0].name
var res8d = books2[0].pages

// Test with nested objects
var tables2 = [
                [  
                    author           |     age    |  book
                    "Kdog"           ,     35     ,  "Fluent"
                    "Amycat"         ,     36     ,  "Ruby"
                    "Maxpup"         ,     37     ,  "Scala"
                ],
                [  
                    name             |     pages   |  author
                    "FluentScript1"  ,     101     ,  "CodeHelix1"
                    "FluentScript2"  ,     102     ,  "CodeHelix2"
                    "FluentScript3"  ,     103     ,  "CodeHelix3"
                ]
            ]
var res8e = tables2[0][1].author
var res8f = tables2[0][1].book

tables2[0][1].author = "new author"
tables2[0][1].book = "new book"

var res8g = tables2[0][1].author
var res8h = tables2[0][1].book


// 9. end of script
var books3 = [  
                name             |     pages   |  author
                "FluentScript1"  ,     101     ,  "CodeHelix1"
                "FluentScript2"  ,     102     ,  "CodeHelix2"
                "FluentScript3"  ,     103     ,  "CodeHelix3"
             ]