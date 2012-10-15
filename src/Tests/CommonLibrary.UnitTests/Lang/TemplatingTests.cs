using System;
using NUnit.Framework;



namespace ComLib.Lang.Tests.Templating
{


    [TestFixture]
    public class Templating_Tests
    {
        [Test]
        public void Can_Run()
        {
            var text = "<h1 class=\"title\"><%= widget.Title %></h1>" + Environment.NewLine
                     + "<% var pages = Pages.All(); %>" + Environment.NewLine
                     + "<% var username = \"users \\\" name\"; %>" + Environment.NewLine
                     + "<!-- This is a \" \\ <comment>tag</comment> -->" + Environment.NewLine
                     + "<% if ( pages.Count > 0 ) { %>" + Environment.NewLine
                     + "   <ul> "                       + Environment.NewLine
                     + "      <% for(var page in pages) { %>" + Environment.NewLine
                     + "         <li><%= page.Title %></li>"  + Environment.NewLine
                     + "      <% } %>"                        + Environment.NewLine
                     + "   </ul>"                       + Environment.NewLine
                     + " <% } %>";
            var finaltext = ComLib.Lang.Templating.Templater.Render(text);
        }
    }
}
