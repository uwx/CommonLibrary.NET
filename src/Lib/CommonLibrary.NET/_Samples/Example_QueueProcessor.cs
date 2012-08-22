using System;
using System.Collections.Generic;

using ComLib;
using ComLib.Queue;
using ComLib.Application;


namespace ComLib.Samples
{
    /// <summary>
    /// Example for the Queue namespace.
    /// </summary>
    public class Example_QueueProcessor : App
    {
		
        class Quote
        {
            public Quote(string symbol, int price) 
            {
                Symbol = symbol;
                Price = price;
            }

            public string Symbol;
            public int Price;
        }



        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            // NOTE:
            // 1. Just showing the API for queue processing using lamdas as delegates.
            // 2. Typically the Queues.Process method will be called by a scheduler to process something periodically.
    
            // 1. Add queue processing handler for strings WITH OUT specifying a name.
            //    By default, the items in teh queues are processed 5 at a time on each call to Process.            
            Queues.AddProcessorFor<string>(items => items.ForEach(item => Console.WriteLine(item)));
            Queues.Enqueue<string>(new List<string>() { "a", "b", "c", "d", "e", "1", "2", "3", "4", "5" });

            // 1st call only prints a-e
            // 2nd call prints the remainder 1-5
            Queues.Process<string>();
            Queues.Process<string>();
            Console.WriteLine();
            
            // 2. Add queue processing by specifying the handler name.
            //    This is ideal if you have 2 handlers for the same type.            
            Queues.AddProcessorFor<int>("my_handler", items => items.ForEach(i => Console.WriteLine(i)));
            Queues.Enqueue<int>("my_handler", new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            Queues.Process("my_handler");
            Console.WriteLine();

            // 3. Add queue processing custom type.
            Queues.AddProcessorFor<Quote>(items => PrintQuotes(items));
            Queues.Enqueue<Quote>(new List<Quote>() { new Quote("MSFT", 20), new Quote("GOOG", 300), new Quote("CITI", 0),
                                                      new Quote("AIG",  -1), new Quote("HONDA", 80), new Quote("BankOfAmerica", 30),
                                                      new Quote("TOYO", 20), new Quote("CS", 32), new Quote("GS", -1)});
            Queues.Process<Quote>();

            // 4. Add queue processing by specifying the queue processor, custom name("my_quote_queue"), custom type(Quote), custom dequeue size(2).
            Queues.AddProcessor("my_quote_queue", new QueueProcessor<Quote>(2, items => PrintQuotes(items)));
            Queues.Enqueue<Quote>("my_quote_queue", new List<Quote>() { new Quote("MSFT", 20), new Quote("GOOG", 300), new Quote("CITI", 0),
                                                                        new Quote("AIG",  -1), new Quote("HONDA", 80), new Quote("BankOfAmerica", 30),
                                                                        new Quote("TOYO", 20), new Quote("CS", 32), new Quote("GS", -1)});
            Queues.Process("my_quote_queue");
            return BoolMessageItem.True;
        }


        void PrintQuotes(IList<Quote> quotes)
        {
            quotes.ForEach(quote => Console.WriteLine(quote.Symbol + " : " + quote.Price));
        }
		//</doc:example>
    }
}
