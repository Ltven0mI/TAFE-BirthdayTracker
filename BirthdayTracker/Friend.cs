using System;

namespace BirthdayTracker
{
   public class Friend : IComparable
   {
      public string Name { get; private set; }
      public string Likes { get; private set; }
      public string Dislikes { get; private set; }
      public int BDayDay { get; private set; }
      public int BDayMonth { get; private set; }


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