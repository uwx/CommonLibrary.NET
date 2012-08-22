/* @hasexpects = true
<expects>
    <expect name="res1a1"  type="string" value="http://www.yahoo.com" />
    <expect name="res1a2"  type="string" value="http://kdog.mysite.com" />
    <expect name="res1a3"  type="string" value="http://news.yahoo.com/blogs/sideshow/winged-roller-coaster-swarm-rips-arms-off-crash-183513325.html" />
    <expect name="res1a4"  type="string" value="www.yahoo.com" />
    <expect name="res1a5"  type="string" value="www.abc-def.123.com" />
    <expect name="res1a6"  type="string" value="c:\users\kishore\resume.doc" />
    <expect name="res1a7"  type="string" value="c:/users/kishore/resume.doc" />
    <expect name="res1a8"  type="string" value="ftp://lang:2930/upload" />
    <expect name="res1a9"  type="string" value="https://news.yahoo.com/blogs/sideshow/winged-roller-coaster-swarm-rips-arms-off-crash-183513325.html" />
     
    <expect name="res1a10"  type="string" value="hi http://www.yahoo.com jinks"/>
    <expect name="res1a11"  type="string" value="hi http://kdog.mysite.com jinks"/>
    <expect name="res1a12"  type="string" value="hi http://news.yahoo.com/blogs/sideshow/winged-roller-coaster-swarm-rips-arms-off-crash-183513325.html jinks"/>
    <expect name="res1a13"  type="string" value="hi www.yahoo.com jinks"/>
    <expect name="res1a14"  type="string" value="hi www.abc-def.123.com jinks"/>
    <expect name="res1a15"  type="string" value="hi c:\users\kishore\resume.doc jinks"/>
    <expect name="res1a16"  type="string" value="hi c:/users/kishore/resume.doc jinks"/>
    <expect name="res1a17"  type="string" value="hi ftp://lang:2930/upload jinks"/>
    <expect name="res1a18"  type="string" value="hi https://news.yahoo.com/blogs/sideshow/winged-roller-coaster-swarm-rips-arms-off-crash-183513325.html jinks"/>
 
    <expect name="res2a"    type="string" value="http://www.yahoo.com www.yahoo.com" />
    <expect name="res2b"    type="string" value="http://www.yahoo.com https://www.yahoo.com" />
    <expect name="res2c"    type="string" value="http://www.yahoo.com c:/temp/log.txt" />
    <expect name="res2d"    type="string" value="www.yahoo.com https://www.yahoo.com" />
    <expect name="res2e"    type="string" value="www.yahoo.com c:/temp/log.txt" />
    <expect name="res2f"    type="string" value="c:/temp/log.txt https://www.yahoo.com" />
    <expect name="res2g"    type="string" value="c:/temp/log.txt www.yahoo.com" /> 

    <expect name="res3a" type="string" value="http://www.yahoo.com" />
	<expect name="res3b" type="string" value="www.yahoo.com" />
	<expect name="res3c" type="string" value="c:/dev/build.txt" />

    <expect name="res4a" type="string" value="http://www.yahoo.com" />
	<expect name="res4b" type="string" value="www.yahoo.com" />
	<expect name="res4c" type="string" value="c:/dev/build.txt" />

    <expect name="res5a" type="string" value="http://www.google.com analytics 3" />
    <expect name="res5b" type="string" value="c:/dev/build.txt fluent true" />
    
    <expect name="res6a" type="bool" value="true" />
    <expect name="res6b" type="bool" value="true" />
    <expect name="res6c" type="bool" value="true" />
    <expect name="res6d" type="bool" value="true" />
    <expect name="res6e" type="bool" value="true" />


    <expect name="res7a" type="bool" value="true" />
    <expect name="res7b" type="bool" value="true" />
    
    <expect name="resEnd" type="string" value="http://news.yahoo.com/blogs/sideshow/winged-roller-coaster-swarm-rips-arms-off-crash-183513325.html" />
</expects>    
*/


function append1( a )
{
    return a + " more"
}


function append2( a, b )
{
    return a + " " + b
}


// 1. assigment
var res1a1 = http://www.yahoo.com
var res1a2 = http://kdog.mysite.com
var res1a3 = http://news.yahoo.com/blogs/sideshow/winged-roller-coaster-swarm-rips-arms-off-crash-183513325.html
var res1a4 = www.yahoo.com
var res1a5 = www.abc-def.123.com
var res1a6 = c:\users\kishore\resume.doc
var res1a7 = c:/users/kishore/resume.doc
var res1a8 = ftp://lang:2930/upload
var res1a9 = https://news.yahoo.com/blogs/sideshow/winged-roller-coaster-swarm-rips-arms-off-crash-183513325.html

var res1a10 = "hi " + http://www.yahoo.com + " jinks"
var res1a11 = "hi " + http://kdog.mysite.com + " jinks"
var res1a12 = "hi " + http://news.yahoo.com/blogs/sideshow/winged-roller-coaster-swarm-rips-arms-off-crash-183513325.html + " jinks"
var res1a13 = "hi " + www.yahoo.com + " jinks"
var res1a14 = "hi " + www.abc-def.123.com + " jinks"
var res1a15 = "hi " + c:\users\kishore\resume.doc + " jinks"
var res1a16 = "hi " + c:/users/kishore/resume.doc + " jinks"
var res1a17 = "hi " + ftp://lang:2930/upload + " jinks"
var res1a18 = "hi " + https://news.yahoo.com/blogs/sideshow/winged-roller-coaster-swarm-rips-arms-off-crash-183513325.html + " jinks"


// 2. function param
var res2a = append2( http://www.yahoo.com, www.yahoo.com)
var res2b = append2( http://www.yahoo.com, https://www.yahoo.com)
var res2c = append2( http://www.yahoo.com, c:/temp/log.txt)
var res2d = append2( www.yahoo.com, https://www.yahoo.com)
var res2e = append2( www.yahoo.com, c:/temp/log.txt)
var res2f = append2( c:/temp/log.txt, https://www.yahoo.com)
var res2g = append2( c:/temp/log.txt, www.yahoo.com)


// 3. array
var ar = [ http://www.yahoo.com, www.yahoo.com, c:/dev/build.txt ]
var res3a = ar[0]
var res3b = ar[1]
var res3c = ar[2]


// 4. map
var m = { site1: http://www.yahoo.com, site2: www.yahoo.com, file: c:/dev/build.txt }
var res4a = m.site1
var res4b = m.site2
var res4c = m.file


// 5. math
var res5a = http://www.google.com + " analytics " + 3
var res5b = c:/dev/build.txt + " fluent " + true

// 6. compare
var res6a = no
var res6b = no
var res6c = no
var res6d = no
var res6e = no

if ( http://www.yahoo.com == "http://www.yahoo.com" )  res6a = yes
if ( www.yahoo.com == "www.yahoo.com" ) res6b = yes
if ( https://www.yahoo.com == "https://www.yahoo.com" ) res6c = yes
if ( c:/dev/build.txt == "c:/dev/build.txt" ) res6d = yes
if ( ftp://ftp.microsoft.com == "ftp://ftp.microsoft.com" ) res6e = yes


// 7. condition
var res7a = no
var res7b = no
if ( www.fluentscript.com == "www.fluentscript.com" && 1 < 2 ) res7a = true
if ( c:/fluentscript.exe != "www.fluentscript.com" && 1 < 2 ) res7b = true


// 8. end of script
var resEnd = http://news.yahoo.com/blogs/sideshow/winged-roller-coaster-swarm-rips-arms-off-crash-183513325.html