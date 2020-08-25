using Microsoft.VisualStudio.TestTools.UnitTesting;
using BirthdayTracker;

namespace BirthdayTracker_Tests
{
    [TestClass]
    public class FriendTests
    {
        // * IsValidName ** //
        [TestMethod]
        public void IsValidName_EmptyName_ReturnFalse()
        {
            var expected = false;
            var result = Friend.IsValidName("");
            Assert.AreEqual(expected, result);
        }
        
        [TestMethod]
        public void IsValidName_NullName_ReturnFalse()
        {
            var expected = false;
            var result = Friend.IsValidName(null);
            Assert.AreEqual(expected, result);
        }
        
        [TestMethod]
        public void IsValidName_SimpleName_ReturnTrue()
        {
            var expected = true;
            var result = Friend.IsValidName("Simple Name");
            Assert.AreEqual(expected, result);
        }
        // END - IsValidName //

        // * IsValidLikes * //
        [TestMethod]
        public void IsValidLikes_EmptyLikes_ReturnFalse()
        {
            var expected = false;
            var result = Friend.IsValidLikes("");
            Assert.AreEqual(expected, result);
        }
        
        [TestMethod]
        public void IsValidLikes_NullLikes_ReturnFalse()
        {
            var expected = false;
            var result = Friend.IsValidLikes(null);
            Assert.AreEqual(expected, result);
        }
        
        [TestMethod]
        public void IsValidLikes_SimpleLikes_ReturnTrue()
        {
            var expected = true;
            var result = Friend.IsValidLikes("Simple Likes");
            Assert.AreEqual(expected, result);
        }
        // END - IsValidLikes //

        // * IsValidDislikes * //
        [TestMethod]
        public void IsValidDislikes_EmptyDislikes_ReturnFalse()
        {
            var expected = false;
            var result = Friend.IsValidDislikes("");
            Assert.AreEqual(expected, result);
        }
        
        [TestMethod]
        public void IsValidDislikes_NullDislikes_ReturnFalse()
        {
            var expected = false;
            var result = Friend.IsValidDislikes(null);
            Assert.AreEqual(expected, result);
        }
        
        [TestMethod]
        public void IsValidDislikes_SimpleDislikes_ReturnTrue()
        {
            var expected = true;
            var result = Friend.IsValidDislikes("Simple Dislikes");
            Assert.AreEqual(expected, result);
        }
        // END - IsValidDislikes //

        // * IsValidBirthMonth * //
        [TestMethod]
        public void IsValidBirthMonth_LessThanZero_ReturnFalse()
        {
            var expected = false;
            var result = Friend.IsValidBirthMonth(-1);
            Assert.AreEqual(expected, result);
        }
        
        [TestMethod]
        public void IsValidBirthMonth_GreaterThanTwelve_ReturnFalse()
        {
            var expected = false;
            var result = Friend.IsValidBirthMonth(13);
            Assert.AreEqual(expected, result);
        }
        
        [TestMethod]
        public void IsValidBirthMonth_FirstMonth_ReturnTrue()
        {
            var expected = true;
            var result = Friend.IsValidBirthMonth(1);
            Assert.AreEqual(expected, result);
        }
        // END - IsValidBirthMonth //

        // * IsValidBirthDay * //
        [TestMethod]
        public void IsValidBirthDay_Month1DayNegative_ReturnFalse()
        {
            var month = 1;
            var day = -1;
            var expected = false;
            var result = Friend.IsValidBirthDay(day, month);
            Assert.AreEqual(expected, result);
        }
        
        [TestMethod]
        public void IsValidBirthDay_Month1DayGreaterThan31_ReturnFalse()
        {
            var month = 1;
            var day = 32;
            var expected = false;
            var result = Friend.IsValidBirthDay(day, month);
            Assert.AreEqual(expected, result);
        }
        
        [TestMethod]
        public void IsValidBirthDay_Month1Day1_ReturnTrue()
        {
            var month = 1;
            var day = 1;
            var expected = true;
            var result = Friend.IsValidBirthDay(day, month);
            Assert.AreEqual(expected, result);
        }
        // END - IsValidBirthDay //

        // * CompareByValue * //
        [TestMethod]
        public void CompareByValue_DifferentValues_ReturnFalse()
        {
            var other = new Friend("John", "Apples", "Bananas", 1, 2);
            var instance = new Friend("Josh", "Grape", "Orange", 10, 12);
            var expected = false;
            var result = instance.CompareByValue(other);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CompareByValue_SameValuesLowerCaseName_ReturnTrue()
        {
            var other = new Friend("john", "Apples", "Bananas", 1, 2);
            var instance = new Friend("John", "Apples", "Bananas", 1, 2);
            var expected = true;
            var result = instance.CompareByValue(other);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CompareByValue_SameValues_ReturnTrue()
        {
            var other = new Friend("John", "Apples", "Bananas", 1, 2);
            var instance = new Friend("John", "Apples", "Bananas", 1, 2);
            var expected = true;
            var result = instance.CompareByValue(other);
            Assert.AreEqual(expected, result);
        }
        // END - CompareByValue //
    }
}