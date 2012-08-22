using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;

using NUnit.Framework;

using ComLib.Paging;


namespace CommonLibrary.Tests
{
    [TestFixture]
    public class PagerTests
    {
        [Test]
        public void IsNotMultiplePages()
        {
            Pager pager = new Pager(1, 1);
            Assert.IsFalse(pager.IsMultiplePages);
            CheckNavigation(pager, false, false, false, false);
            CheckPages(pager, 1, 1, 1, 1, 1);
        }


        [Test]
        public void IsMultiplePages()
        {
            Pager pager = new Pager(1, 4);
            Assert.IsTrue(pager.IsMultiplePages);
            CheckNavigation(pager, false, false, false, false);
            CheckPages(pager, 1, 1, 1, 4, 2);
        }


        [Test]
        public void CanMoveToNextPage_SingleRange()
        {
            Pager pager = new Pager(1, 4);
            CheckPages(pager, 1, 1, 1, 4, 2);
            CheckNavigation(pager, false, false, false, false);

            pager.MoveNext();
            CheckPages(pager, 2, 1, 1, 4, 3);
            CheckNavigation(pager, false, false, false, false);
        }


        [Test]
        public void CanMoveToNextPage_ManyRanges()
        {
            Pager pager = new Pager(1, 18, new PagerSettings() { NumberPagesToDisplay = 5 });
            CheckPages(pager, 1, 1, 1, 5, 2);
            CheckNavigation(pager, false, false, true, true);

            pager.MoveNext();
            CheckPages(pager, 2, 1, 1, 5, 3);
            CheckNavigation(pager, false, false, true, true);
        }


        [Test]
        public void CanMoveToNextRange_2Ranges()
        {
            Pager pager = new Pager(1, 10, new PagerSettings() { NumberPagesToDisplay = 5 });
            CheckPages(pager, 1, 1, 1, 5, 2);
            CheckNavigation(pager, false, false, true, true);

            // Move to last page in first range ( 1 - 5 )
            pager.MoveToPage(5);
            CheckPages(pager, 5, 4, 1, 5, 6);
            CheckNavigation(pager, false, false, true, true);


            // Move to first page in next range ( 6 - 10 )
            pager.MoveNext();
            CheckPages(pager, 6, 5, 6, 10, 7);
            CheckNavigation(pager, true, true, false, false);
        }


        [Test]
        public void CanMoveTo2ndRange_3Ranges()
        {
            Pager pager = new Pager(1, 15, new PagerSettings() { NumberPagesToDisplay = 5 });
            CheckPages(pager, 1, 1, 1, 5, 2);
            CheckNavigation(pager, false, false, true, true);

            // Move to last page in first range ( 1 - 5 )
            pager.MoveToPage(5);
            CheckPages(pager, 5, 4, 1, 5, 6);
            CheckNavigation(pager, false, false, true, true);


            // Move to first page in next range ( 6 - 10 )
            pager.MoveNext();
            CheckPages(pager, 6, 5, 6, 10, 7);
            CheckNavigation(pager, true, true, true, true);
        }


        [Test]
        public void CanMoveTo3rdRange_3Ranges()
        {
            Pager pager = new Pager(1, 13, new PagerSettings() { NumberPagesToDisplay = 5 });
            CheckPages(pager, 1, 1, 1, 5, 2);
            CheckNavigation(pager, false, false, true, true);

            // Move to last page 5 in first range ( 1 - 5 )
            pager.MoveToPage(5);
            CheckPages(pager, 5, 4, 1, 5, 6);
            CheckNavigation(pager, false, false, true, true);

            // Move to first page 6 in next range ( 6 - 10 )
            pager.MoveNext();
            CheckPages(pager, 6, 5, 6, 10, 7);
            CheckNavigation(pager, true, true, true, true);

            // Move to last page 10 in 2nd range ( 6 - 10 )
            pager.MoveToPage(10);
            CheckPages(pager, 10, 9, 6, 10, 11);
            CheckNavigation(pager, true, true, true, true);

            // Move to first page 11 in last range ( 9 - 13 )
            pager.MoveNext();
            CheckPages(pager, 11, 10, 9, 13, 12);
            CheckNavigation(pager, true, true, false, false);

            // Move to previous page 10 in 2nd range ( 6 - 10 )
            pager.MovePrevious();
            CheckPages(pager, 10, 9, 6, 10, 11);
            CheckNavigation(pager, true, true, true, true);

        }


        private void CheckPages(Pager pager, int current, int previous, int starting, int ending, int next)
        {
            Assert.AreEqual(pager.CurrentPage, current);
            Assert.AreEqual(pager.PreviousPage, previous);
            Assert.AreEqual(pager.StartingPage, starting);
            Assert.AreEqual(pager.EndingPage, ending);
            Assert.AreEqual(pager.NextPage, next);
        }


        private void CheckNavigation(Pager pager, bool showFirst, bool showPrevious, bool showNext, bool showLast)
        {
            Assert.AreEqual(pager.CanShowFirst, showFirst);
            Assert.AreEqual(pager.CanShowPrevious, showPrevious);
            Assert.AreEqual(pager.CanShowNext, showNext);
            Assert.AreEqual(pager.CanShowLast, showLast);
        }
    }
}
