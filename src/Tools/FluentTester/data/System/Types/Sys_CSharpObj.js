/* @hasexpects = true
<expects>
    <expect name="res1a" type="string" value="fluent" />	
    <expect name="res1b" type="string" value="script" />
    <expect name="res1c" type="datetime" value="${month}/${day}/${year}" />
    <expect name="res1d" type="bool" value="true" />
    <expect name="res1e" type="number" value="2012" />
    <expect name="res1f" type="string" value="Queens" />
    <expect name="res1g" type="string" value="NY" />

    <expect name="res2a" type="string" value="fluent2" />	
    <expect name="res2b" type="string" value="script2" />
    <expect name="res2c" type="datetime" value="2/20/1980" />
    <expect name="res2d" type="bool" value="false" />
    <expect name="res2e" type="string" value="Miami" />
    <expect name="res2f" type="string" value="Florida" />

    <expect name="res3a" type="string" value="fluent2 script2" />
    <expect name="res3b" type="string" value="kishore red" />
    <expect name="res3c" type="string" value="Miami, Florida USA" />
    <expect name="res3d" type="string" value="Miami Florida CountryX" />
    <expect name="res3e" type="string" value="jane, doe, False, 2012, 20.5" />

    <expect name="res4a" type="string" value="home cooked" />
    <expect name="res4b" type="string" value="turkey with gravy" />
    <expect name="res4c" type="string" value="john@bronx.com" />

    <expect name="res5a" type="string" value="john doe" />
</expects>
*/

// 1. assignment
var user = new Person("fluent", "script")

var res1f = user.Address.City
var res1a = user.FirstName
var res1b = user.LastName
var res1c = user.BirthDate
var res1d = user.IsMale
var res1g = user.Address.State
var res1e = user.BirthDate.Year

user.FirstName = "fluent2"
user.LastName  = "script2"
user.BirthDate = Feb 20th 1980
user.IsMale    = false
user.Address.City  = "Miami"
user.Address.State = "Florida"

var res2a = user.FirstName
var res2b = user.LastName
var res2c = user.BirthDate
var res2d = user.IsMale
var res2e = user.Address.City
var res2f = user.Address.State

// Call method on user 
var res3a = user.FullName()

// Call static method on user
var res3b = Person.ToFullName("kishore", "red")

// Call method on address
var res3c = user.Address.FullLocation(1, true)
var res3d = user.Address.FullLocation(2, false)

// Call method with multiple arguments with different types.
var res3e = user.BuildContactInfo("jane", "doe", false, 6/1/2012, 20.5 )


// Access list object and add item.
user.FavoriteFoods.push("home cooked")
var res4a = user.FavoriteFoods[0]

// Supply a list as a parameter to check for conversion to generics list from List<object> in fluentscript
user.AddFavoriteFoodsList( ["sushi", "indian", "turkey with gravy" ] );
var res4b = user.FavoriteFoods[3]

var friends = [ 
                new Person("fred", "c", "fred@bronx.com", true, 50000.5 ),
                new Person("john", "c", "john@bronx.com", true, 70000.5 ),
                new Person("amit", "c", "amit@queens.com", true, 40000.5 ),
                new Person("hugo", "c", "hugo@jersey.com", true, 80000.5 )
              ]

user.AddFriendsList( friends )
var res4c = user.Friends[1].Email


// 5. function parameters
function append( val, text )
{
    return val + text
}


var res5a = append( user.Friends[1].FirstName, " doe")

