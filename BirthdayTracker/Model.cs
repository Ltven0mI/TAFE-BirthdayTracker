using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using CsvHelper;

namespace BirthdayTracker
{
   public class Model
   {
      private const string FRIEND_DATA_FILEPATH = "MyFriendData.csv";

      private List<Friend> friendList;
      public ReadOnlyCollection<Friend> FriendList { get; private set; }


      public Model()
      {
         // Initialize 'friendList' and wrap in ReadOnlyCollection
         friendList = new List<Friend>();
         FriendList = new ReadOnlyCollection<Friend>(friendList);
      }

      public void ReloadFriendData()
      {
         // Clear existing friendList
         friendList.Clear();
         // Read friend data from file and insert into the friendList
         friendList.AddRange(ReadFriendsFromFile(FRIEND_DATA_FILEPATH));
      }

      public void WriteFriendData()
      {
         // Write friend data to data file
         WriteFriendsToFile(FRIEND_DATA_FILEPATH, friendList);
      }

      public List<Friend> ReadFriendsFromFile(string filepath)
      {
         using (var reader = new StreamReader(filepath))
         {
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
               csv.Configuration.HasHeaderRecord = false;
               var records = csv.GetRecords<Friend>();
               return new List<Friend>(records);
            }
         }
      }

      public void WriteFriendsToFile(string filepath, List<Friend> friendList)
      {
         using (var writer = new StreamWriter(filepath))
         {
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
               csv.WriteRecords(friendList);
            }
         }
      }
   }
}