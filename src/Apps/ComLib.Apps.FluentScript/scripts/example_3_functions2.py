

# Example 1: hours
def hours, hour, hrs, hr( amount  )
{
	return new Time(0, amount, 0, 0)
}


# Example 2: minutes
def minutes, minute, mins, min( amount )
{
	return new Time( 0, 0, amount, 0 )
}


time = 3 hours + 2 hr + 40 minutes
print time is #{time}
println()


# enable use of units.
enable units
total = 5 inches + 3 feet + 2 yards
print total is #{total.Value} inches


println()
v = 0.9.8.8
print fluentscript version is #{v.Text()}