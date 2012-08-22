/* ***************************************************************
@@MetaData-Enabled
@author: kishore
@summary: This script does the following:
1. Gets total users in the system
2. Deletes users from the system
3. Creates users in the system
******************************************************************/
var username = "kishore";
var password = "password";
var email = "kishore@mymail.com";

// NOTES:
// The "User" command maps to the class that has attribute CommandAttribute(Name="User" ...
// The property IsMultiMethodEnabled = true, tells framework you can call multple methods on "User" command
// The property AutoHandleMethodCalls = true, tells framework to handle invoking the methods on the class associated with "User" command
BlogTest.Delete(2);
BlogTest.Create("blog 1", 20, true);
BlogTest.Create({ title: "blog 1", refid: 20, activate: true } );
BlogTest.DeleteAll();