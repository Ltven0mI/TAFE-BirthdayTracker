using System;
using CsvHelper.Configuration.Attributes;

namespace BirthdayTracker
{
   /**********************************************************/
   // Filename: Friend.cs
   // Purpose: To represent a friend and it's data.
   // Author: Wade Rauschenbach
   // Version: 0.1.0
   // Date: 07-Aug-2020
   // Tests: N/A
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