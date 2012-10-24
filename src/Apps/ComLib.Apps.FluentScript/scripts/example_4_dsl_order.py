# @summary: buy shares of the specified symbol using the specified parameters
# @arg: name: shares, alias: on,    type: date,   examples: 300 shares | shares( 300 ) 
# @arg: name: symbol, alias: of,    type: text,   examples: IBM | "IBM" | 'IBM'
# @arg: name: price,  alias: at,    type: number, examples: $40.25 | 40.25
# @arg: name: date,   alias: on,    type: date,   examples: July 10th 2012 | 7/10/2012
def  order_to_buy( shares, symbol, price, date )
{
	print buying #{shares} of '#{symbol}' at #{price} on #{date}	
	println()
}


# Example using a postfix to create a shares object
def shares(amount)
{
	# e.g. new Shares(300)
	# return new Shares( amount )
	# not accessing c# code at the moment so just return number.
	return amount
}


#println( 300 shares )


# Example 1: full function name with commas
order_to_buy 100, 'MSFT', $31.50, Oct 28th at 8:30 am


# Example 2: full function name with commas
order to buy 201, 'MSFT', $31.50, Oct 24th at 11:30 am


# Example 3: full function name with commas
order to buy 302 shares, 'IBM', $150.50, Oct 24th at 2:30 pm


# Example 4: named params
order to buy 403 shares, of: 'MSFT', at: $150.50, on: Oct 24th at 2:30 pm


# Example 4: flexible
order to buy 504 shares of 'MSFT' at $120.50 on Oct 24th at 9:20 am


