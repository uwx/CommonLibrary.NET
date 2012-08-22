/* @hasexpects = true
<expects>
	<expect name="res1a"  type="string"  value="300 IBM 20.55 7/1/2012 12:00:00 AM premium policy" />    
    <expect name="res1b"  type="string"  value="300 IBM 20.55 7/1/2012 12:00:00 AM premium policy" />    
</expects>
*/

// @summary: creates an order the stock supplied with the specified fields of date, price, shares and policy
// this will create scheduled order in the date is t+1
// @example: conventional sytax, 'orderToBuy( shares(300), "IBM", 40.5, new Date(2012, 7, 10)', "premium policy")'
// @example: fluent syntax,      'order to buy 300 shares, IBM, $40.5, 7/10/2012, premium policy' 
// @arg: shares,   Number of shares to buy,       number,   shares,   300 shares | shares(300) | new Shares(300)		   
// @arg: stock,    The name of the stock,         string,   of,       "IBM" | "MSFT"
// @arg: price,    The price at which to buy,     number,   at,       $40.50
// @arg: date,     The date to buy the stock,     date  ,   on,       July 10th 2012 | 7/10/2012
// @arg: policy,   The policy type for pricing,   string,   using,    "default pricing" | "premium pricing"
function orderToBuy( shares, stock, price, date, policy)
{
    return shares + " " + stock + " " + price + " " + date + " " + policy;
}


# @summary: creates an order the stock supplied with the specified fields of date, price, shares and policy
# this will create scheduled order in the date is t+1
# @example: conventional sytax, 'orderToBuy( shares(300), "IBM", 40.5, new Date(2012, 7, 10)', "premium policy")'
# @example: fluent syntax,      'order to buy 300 shares, IBM, $40.5, 7/10/2012, premium policy' 
# @arg: shares,   Number of shares to buy,       number,   shares,   300 shares | shares(300) | new Shares(300)		   
# @arg: stock,    The name of the stock,         string,   of,       "IBM" | "MSFT"
# @arg: price,    The price at which to buy,     number,   at,       $40.50
# @arg: date,     The date to buy the stock,     date  ,   on,       July 10th 2012 | 7/10/2012
# @arg: policy,   The policy type for pricing,   string,   using,    "default pricing" | "premium pricing"
function orderToBuy2(shares, stock, price, date, policy) {
    return shares + " " + stock + " " + price + " " + date + " " + policy;
}


var res1a = orderToBuy(300, "IBM", 20.55, new Date(2012, 7, 1), "premium policy");
var res1b = orderToBuy2(300, "IBM", 20.55, new Date(2012, 7, 1), "premium policy");