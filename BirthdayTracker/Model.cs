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

      public enum SearchMonth { All, Jan, Feb, March, April, May, June, July, Aug, Sep, Oct, Nov, Dec };
      
      // * Friend List * //
      private List<Friend> friendList;
      public ReadOnlyCollection<Friend> FriendList { get; private set; }
      // END - Friend List //

      public Friend SelectedFriend { get; private set; }
      public SearchMonth SelectedSearchMonth { get; private set; }
      
      // * Search Results * //
      private List<Friend> monthSearchResults;
      public ReadOnlyCollection<Friend> MonthSearchResults { get; private set; }
      // END - Search Results //


      public Model()
      {
         // Initialize 'friendList' and wrap in ReadOnlyCollection
         friendList = new List<Friend>();
         FriendList = new ReadOnlyCollection<Friend>(friendList);

         SelectedFriend = null;
         SelectedSearchMonth = SearchMonth.All;

         // Initialize 'monthSearchResults' and wrap in ReadOnlyCollection
         monthSearchResults = new List<Friend>();
         MonthSearchResults = new ReadOnlyCollection<Friend>(monthSearchResults);
      }


      #region Search Methods
      public void NextSearchMonth()
      {
         int searchIndex = (int)SelectedSearchMonth;
         searchIndex = (searchIndex + 1) % 13;
         SetSearchMonth((SearchMonth)Enum.GetValues(typeof(SearchMonth)).GetValue(searchIndex));
      }
      private void SetSearchMonth(SearchMonth searchMonth)
      {
         // Set 'SelectedSearchMonth' variable
         SelectedSearchMonth = searchMonth;
         // Update the Search Results
         UpdateSearchResults();
      }
      public void UpdateSearchResults()
      {
         // Clear and add the new search results
         monthSearchResults.Clear();
         monthSearchResults.AddRange(GetFriendsByMonth(SelectedSearchMonth));
      }
      #endregion

      #region Find Friend Methods
      public Friend FindFriend(string name)
      {
         Friend sudoFriend = new Friend(name, null, null, 0, 0);
         int result = monthSearchResults.BinarySearch(sudoFriend);
         if (result < 0)
            result = monthSearchResults.FindIndex(c => c.Name.ToLower().Contains(name.ToLower()));
         if (result < 0)
            return null;
         return monthSearchResults[result];
      }
      #endregion

      #region Selected Friend Methods
      public void SetSelectedFriend(Friend friend)
      {
         // Assign the new SelectedFriend value
         SelectedFriend = friend;
      }
      #endregion


      public List<Friend> GetFriendsByMonth(SearchMonth searchMonth)
      {
         // Return a copy of the full FriendList if 'All' is passed
         if (searchMonth == SearchMonth.All)
            return new List<Friend>(friendList);
         
         // Return all friends with a birthday in the specified month
         return friendList.FindAll(c => c.BDayMonth == (int)searchMonth);
      }

      #region CSV Read/Write Methods
      public void ReloadFriendData()
      {
         // Clear existing friendList
         friendList.Clear();
         // Read friend data from file and insert into the friendList
         friendList.AddRange(ReadFriendsFromFile(FRIEND_DATA_FILEPATH));
         // Sort friend list
         friendList.Sort();
      }
      public void WriteFriendData()
      {
         // Write friend data to data file
         WriteFriendsToFile(FRIEND_DATA_FILEPATH, friendList);
      }
      private List<Friend> ReadFriendsFromFile(string filepath)
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
      private void WriteFriendsToFile(string filepath, List<Friend> friendList)
      {
         using (var writer = new StreamWriter(filepath))
         {
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
               csv.WriteRecords(friendList);
            }
         }
      }
      #endregion
   }
}