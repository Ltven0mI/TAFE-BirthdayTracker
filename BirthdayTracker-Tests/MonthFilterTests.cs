using Microsoft.VisualStudio.TestTools.UnitTesting;
using BirthdayTracker;

namespace BirthdayTracker_Tests
{
    [TestClass]
    public class MonthFilterTests
    {
        [TestMethod]
        public void GetNext_All_ReturnJan()
        {
            var instance = MonthFilter.All;
            var expected = MonthFilter.Jan;
            var result = instance.GetNext();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetNext_Dec_Return_All()
        {
            MonthFilter instance = MonthFilter.Dec;
            MonthFilter expected = MonthFilter.All;
            MonthFilter result = instance.GetNext();
            Assert.AreEqual(expected, result);
        }
    }
}