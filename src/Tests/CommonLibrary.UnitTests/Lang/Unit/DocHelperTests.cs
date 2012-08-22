using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ComLib.Lang.Helpers;


namespace ComLib.Lang.Tests.Unit
{
    [TestFixture]
    public class DocHelperTests
    {
        [Test]
        public void Can_Parse_Doc_Tags_With_Positional_Properties()
        {
            var lines = new List<string>()
            {
                "# @summary: creates an order the stock supplied with the specified fields of date, price, shares and policy.",
                "# this will create scheduled order in the date is t+1.",
                "# @example: conventional sytax, 'orderToBuy( shares(300), \"IBM\", 40.5, new Date(2012, 7, 10)', \"premium policy\"",
                "# @example: fluent syntax,      'order to buy 300 shares, IBM, $40.5, 7/10/2012, premium policy'", 
                "# @arg: shares, Number of shares to buy,     number, shares, 300 shares | shares(300) | new Shares(300)",		   
                "# @arg: stock,  The name of the stock,       string, of,     'IBM' | 'MSFT'",
                "# @arg: price,  The price at which to buy,   number, at,     $40.50",
                "# @arg: date,   The date to buy the stock,   date  , on,     July 10th 2012 | 7/10/2012",
                "# @arg: policy, The policy type for pricing, string, using,  'default pricing' | 'premium pricing'"
            };
            var result = DocHelper.ParseDocTags(lines);
            var docTags = result.Item1;
            Assert.AreEqual(docTags.Args.Count, 5);
            Assert.AreEqual(docTags.Examples.Count, 2);
        }


        [Test]
        public void Can_Parse_Doc_Tags_With_Named_Properties()
        {
            var lines = new List<string>()
            {
                "# @summary: creates an order the stock supplied with the specified fields of date, price, shares and policy.",
                "# this will create scheduled order in the date is t+1.",
                "# @example: conventional sytax, 'orderToBuy( shares(300), \"IBM\", 40.5, new Date(2012, 7, 10)', \"premium policy\"",
                "# @example: fluent syntax,      'order to buy 300 shares, IBM, $40.5, 7/10/2012, premium policy'", 
                "# @arg: name: shares, desc: Number of shares to buy,     type: number,  alias: shares, examples: 300 shares | shares(300) | new Shares(300)",		   
                "# @arg: name: stock,  desc: The name of the stock,       type: string,  alias: of,     examples: 'IBM' | 'MSFT'",
                "# @arg: name: price,  desc: The price at which to buy,   type: number,  alias: at,     examples: $40.50",
                "# @arg: name: date,   desc: The date to buy the stock,   type: date  ,  alias: on,     examples: July 10th 2012 | 7/10/2012",
                "# @arg: name: policy, desc: The policy type for pricing, type: string,  alias: using,  examples: 'default pricing' | 'premium pricing'"
            };
            var result = DocHelper.ParseDocTags(lines);
            var docTags = result.Item1;
            Assert.AreEqual(docTags.Args.Count, 5);
            Assert.AreEqual(docTags.Examples.Count, 2);
        }
    }
}
