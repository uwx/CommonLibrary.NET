# @summary: buy shares of the specified symbol using the specified parameters
# @arg: name: shares, alias: on,    type: date,   examples: 300 shares | shares( 300 ) 
# @arg: name: symbol, alias: of,    type: text,   examples: IBM | "IBM" | 'IBM'
# @arg: name: price,  alias: at,    type: number, examples: $40.25 | 40.25
# @arg: name: date,   alias: on,    type: date,   examples: July 10th 2012 | 7/10/2012
def  order_to_buy( shares, symbol, price, date )
{
	print buying #{shares} shares of '#{symbol}' at #{price} on #{date}	
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
order_to_buy 100, 'MSFT-0', $30.50, Oct 28th at 8:30 am


# Example 2: full function name with commas
order to buy 201, 'MSFT-1', $31.50, Oct 1st at 11:30 am


# Example 3: full function name with commas
order to buy 302 shares, 'MSFT-2', $32.50, Oct 2nd at 2:30 pm


# Example 4: named params
order to buy 403 shares, of: 'MSFT-3', at: $33.50, on: Oct 3rd at 2:30 pm


# Example 4: flexible
order to buy 504 shares of 'MSFT-4' at $34.50 on Oct 4th at 9:20 am

println()
println()
order_to_buy 100, 'MSFT-0', $30.50, 10/25/2012 at 8:30 am
order_to_buy 100, 'MSFT-0', $30.50, 10/26/2012 at 9:31 am
order_to_buy 100, 'MSFT-0', $30.50, 10/27/2012 at 10:32 am
order_to_buy 100, 'MSFT-0', $30.50, 10/28/2012 at 11:33 am

order to buy 130 shares of 'MSFT-0' at $33.33 on 10/26/2012 at 1:30 pm

