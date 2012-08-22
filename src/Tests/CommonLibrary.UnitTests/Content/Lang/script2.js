/* ******************************************************
@author: kishore
@summary: Light-weight javascript style interpreter
@description: javascript runner with integration with c# methods
@notes:
1. Interprets line by line
1. $now
2. $object.method( <params> );
*********************************************************/
var name = "kishore";
var isActive = true;
var age = 32;
var hour = time.hour;

// 1. Build in system variables.
//print(time.hour);

// 2. Call method that exists as a command in the automation script.
HelloWorldAutoMap({ user: name, isactive: isActive, age: age, birthdate: "2/2/1979" });
