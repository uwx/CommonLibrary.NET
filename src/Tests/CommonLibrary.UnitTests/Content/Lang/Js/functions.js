/* ******************************************************
@author: kishore
@summary: Light-weight javascript style interpreter
@description: javascript runner with integration with c# methods
@notes:
1. Interprets line by line
1. $now
2. $object.method( <params> );
*********************************************************/

var result = "function test";


function createUser(username, isMale, buyerRating) 
{
    var result = "empty";
    if (isMale) 
    {
        result = username + " male " + buyerRating;
    }
    return result;
}

result = createUser("user1", true, 4.2);
