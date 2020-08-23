using System.Reflection.Metadata.Ecma335;
using System;
using CsvHelper.Configuration.Attributes;

namespace BirthdayTracker
{
   /**********************************************************/
   // Filename: Friend.cs
   // Purpose: To represent a friend and it's data.
   // Author: Wade Rauschenbach
   // Version: 0.3.0
   // Date: 21-Aug-2020
   // Tests: N/A
   /**********************************************************/

   /*********************** Changelog ************************/
   // [Unreleased]
   //
   // [0.3.0] 23-Aug-2020
   // | [Added]
   // | - Add IsValidName(string name) for validating a name.
   // | - Add IsValidLikes(string likes) for validating likes.
   // | - Add IsValidDislikes(string dislikes) for validating dislikes.
   // | - Add IsValidBirthMonth(int month) for validating a birth month.
   // | - Add IsValidBirthDay(int day, int month) for validating a birth day.
   // | - Add CompareByValues(Friend other) for comparing friends by value.
   //
   // [0.2.0] 21-Aug-2020
   // | [Added]
   // | - Implement Clone() method for cloning an instance by value.
   //
   // [0.1.0] 07-Aug-2020
   // | [Added]
   // | - Initial Friend implementation.
   /**********************************************************/
   public class Friend : IComparable
   {
      [Index(0)]
      public string Name { get; set; }
      [Index(1)]
      public string Likes { get; set; }
      [Index(2)]
      public string Dislikes { get; set; }
      [Index(3)]
      public int BDayDay { get; set; }
      [Index(4)]
      public int BDayMonth { get; set; }

      /**********************************************************/
      // Method: public Friend (string name, string likes, string dislikes,
      //    int bdayDay, int bdayMonth)
      // Purpose: Constructs a new instance of Friend with the passed values.
      // Inputs: string name - the name of the friend.
      // Inputs: string likes - the likes of the friend.
      // Inputs: string dislikes - the dislikes of the friend.
      // Inputs: int bdayDay - the day of the friend's birthday.
      // Inputs: int bdayDay - the month of the friend's birthday.
      /**********************************************************/
      public Friend(string name, string likes, string dislikes,
      int bdayDay, int bdayMonth)
      {
         Name = name;
         Likes = likes;
         Dislikes = dislikes;
         BDayDay = bdayDay;
         BDayMonth = bdayMonth;
      }


      /**********************************************************/
      // Method:  public bool IsValidName (String name)
      // Purpose: Validates whether the passed 'name' is a
      //          valid name for a friend.
      // Returns: true if 'name' is valid.
      // Returns: false if 'name' is NOT valid.
      // Inputs:  String name
      // Outputs: bool
      /**********************************************************/
      public static bool IsValidName(string name)
      {
         return !String.IsNullOrWhiteSpace(name);
      }

      /**********************************************************/
      // Method:  public bool IsValidLikes (String likes)
      // Purpose: Validates whether the passed 'likes' is a
      //          valid likes for a friend.
      // Returns: true if 'likes' is valid.
      // Returns: false if 'likes' is NOT valid.
      // Inputs:  String likes
      // Outputs: bool
      /**********************************************************/
      public static bool IsValidLikes(string likes)
      {
         return !String.IsNullOrWhiteSpace(likes);
      }

      /**********************************************************/
      // Method:  public bool IsValidDislikes (String dislikes)
      // Purpose: Validates whether the passed 'dislikes' is a
      //          valid dislikes for a friend.
      // Returns: true if 'dislikes' is valid.
      // Returns: false if 'dislikes' is NOT valid.
      // Inputs:  String dislikes
      // Outputs: bool
      /**********************************************************/
      public static bool IsValidDislikes(string dislikes)
      {
         return !String.IsNullOrWhiteSpace(dislikes);
      }

      /**********************************************************/
      // Method:  public bool IsValidBirthMonth (int month)
      // Purpose: Validates whether the passed 'month' is a
      //          valid birth month for a friend.
      // Returns: true if 'month' is valid.
      // Returns: false if 'month' is NOT valid.
      // Inputs:  int month
      // Outputs: bool
      /**********************************************************/
      public static bool IsValidBirthMonth(int month)
      {
         return (month > 0 && month <= 12);
      }

      /**********************************************************/
      // Method:  public bool IsValidBirthDay (int day, int month)
      // Purpose: Validates whether the passed 'day' is a
      //          valid birth day for a friend born in the
      //          passed birth 'month'.
      // Returns: true if 'day' is a valid.
      // Returns: false if either 'day' or 'month' are NOT valid.
      // Inputs:  int day, int month
      // Outputs: bool
      /**********************************************************/
      public static bool IsValidBirthDay(int day, int month)
      {
         // Ensure the passed month is valid
         if (!IsValidBirthMonth(month))
            return false;
         
         // Get the days in the passed month on a leap year (To allow for Leap Day)
         int daysInMonth = DateTime.DaysInMonth(2000, month);
         
         return (day > 0 && day <= daysInMonth);
      }

      /**********************************************************/
      // Method:  public Friend Clone ()
      // Purpose: Creates a copy of this instance by value.
      // Returns: A new Friend instance with the same values as
      //          this instance.
      // Outputs: Friend
      /**********************************************************/
      public Friend Clone()
      {
         // Strings are passed by value, i.e. there is no need
         // to create new string references for the constructor.
         return new Friend(Name, Likes, Dislikes, BDayDay, BDayMonth);
      }

      /**********************************************************/
      // Method:  public bool CompareByValue (Friend other)
      // Purpose: Compares this instance and 'other' by value.
      // Returns: true if all values are identical
      // Returns: false if any values are NOT identical
      // Inputs:  Friend other
      // Outputs: bool
      // Throws:  ArgumentNullException - if 'other' is null
      /**********************************************************/
      public bool CompareByValue(Friend other)
      {
         // Ensure other is not null
         if (other == null)
            throw new ArgumentNullException("The argument 'other' was null.");
         
         // Return whether all values are identical or not
         return (
            Name.ToLower() == other.Name.ToLower() &&
            Likes == other.Likes &&
            Dislikes == other.Dislikes &&
            BDayDay == other.BDayDay &&
            BDayMonth == other.BDayMonth
         );
      }

      /**********************************************************/
      // Method: public int CompareTo (object other)
      // Purpose: Compares this instance to another object.
      // (Only supports Friend / Friend comparison by name.)
      // returns -1 - this instance precedes 'other'
      // returns 0 - this instance is in the same position as 'other'
      // returns 1 - this instance follows 'other'. -or- 'other' is 'null'
      // Inputs: object other - the object to compare against
      // Outputs: int - that indicates whether this instance precedes, follows,
      // or appears in the same position in the sort order as the
      // 'other' parameter.
      /**********************************************************/
      public int CompareTo(object other)
      {
         if (other.GetType() != typeof(Friend))
            throw new NotImplementedException();
         return CompareTo((Friend)other);
      }

      /**********************************************************/
      // Method: public int CompareTo (Friend other)
      // Purpose: Compares this instance to another Friend.
      // returns -1 - this instance precedes 'other'
      // returns 0 - this instance is in the same position as 'other'
      // returns 1 - this instance follows 'other'. -or- 'other' is 'null'
      // Inputs: Friend other - the Friend instance to compare against
      // Outputs: int - that indicates whether this instance precedes, follows,
      // or appears in the same position in the sort order as the
      // 'other' parameter.
      /**********************************************************/
      public int CompareTo(Friend other)
      {
         return Name.CompareTo(other.Name);
      }

      /**********************************************************/
      // Method: public string ToString ()
      // Purpose: creates the string representation of this instance
      // returns the string representation of this instance
      // Outputs: string
      /**********************************************************/
      public override string ToString()
      {
         return string.Format(
            "Friend - {0}\n"
            + "| Name : {1}\n"
            + "| Likes : {2}\n"
            + "| Dislikes : {3}\n"
            + "| BDayDay : {4}\n"
            + "| BDayMonth : {5}",
            Name, Name, Likes, Dislikes,
            BDayDay, BDayMonth);
      }
   }
}