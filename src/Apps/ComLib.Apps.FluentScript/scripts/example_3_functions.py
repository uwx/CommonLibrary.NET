println()


# refill inventory
def refill_inventory( id, amount)
{
	println( "refilling inventory for: " + id.toUpperCase() + ": " + amount )
}


# ordering stock
def orderToBuy( symbol, amount)
{
	println()
	print ordering stock of #{ symbol.toUpperCase() } with #{amount} shares
}


# Example 1: actual name
refill_inventory 'infiniti-g35', 3


# Example 2: without underscore
refill inventory 'honda-civic', amount: 21


# Example 3: spaces converted to camel case
order to buy 'ibm', 300

