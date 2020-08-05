using System;
using CsvHelper.Configuration.Attributes;

namespace BirthdayTracker
{
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


      public Friend(string name, string likes, string dislikes,
      int bdayDay, int bdayMonth)
      {
         Name = name;
         Likes = likes;
         Dislikes = dislikes;
         BDayDay = bdayDay;
         BDayMonth = bdayMonth;
      }

      public int CompareTo(object other)
      {
         throw new NotImplementedException();
      }

      public int CompareTo(Friend other)
      {
         return Name.CompareTo(other.Name);
      }

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