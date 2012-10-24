# @summary: buy shares of the specified symbol using the specified parameters
# @arg: name: shares, alias: on,    type: date,   examples: 300 shares | shares( 300 ) 
# @arg: name: symbol, alias: of,    type: text,   examples: IBM | "IBM" | 'IBM'
# @arg: name: price,  alias: at,      type: number, examples: $40.25 | 40.25
# @arg: name: date,   alias: on,     type: date,   examples: July 10th 2012 | 7/10/2012
def  order_to_buy( shares, symbol, price, date )
{
    print simulating an stock purchase
    print buying #{shares} of #{symbol} at #{price} on #{date} 
}

order_to_buy 300, MICRO SOFT, $31.50, 8/15/2012 at 8:30 am

order to buy 300, IBM, $31.50, 10/21/2012 at 3:30 pm

order to buy 300 of 'IBM' at $35.50 on 9/15/2012 at 11:30 am
