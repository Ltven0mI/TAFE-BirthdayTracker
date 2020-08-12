using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using CsvHelper;

namespace BirthdayTracker
{
   /**********************************************************/
   // Filename: Model.cs
   // Purpose: To act as a wrapper for the program's data
   // and provide methods for modifying that data.
   // Author: Wade Rauschenbach
   // Version: 0.1.0
   // Date: 07-Aug-2020
   // Tests: N/A
   /**********************************************************/
   public class Model
   {
      private const string FRIEND_DATA_FILEPATH = "MyFriendData.csv";

      public event EventHandler<ReadOnlyCollection<Friend>> FriendListChanged;
      public event EventHandler<Friend> SelectedFriendChanged;

      public enum SearchMonth { All, Jan, Feb, March, April, May, June, July, Aug, Sep, Oct, Nov, Dec };
      
      // * Original Friend List * //
      private List<Friend> originalFriendList;
      // END - Original Friend List //
      
      // * Friend List * //
      private List<Friend> friendList;
      public ReadOnlyCollection<Friend> FriendList { get; private set; }
      // END - Friend List //

      public Friend SelectedFriend { get; private set; }
      public SearchMonth SelectedSearchMonth { get; private set; }


      public Model()
      {
         // Initialize 'originalFriendList'
         originalFriendList = new List<Friend>();

         // Initialize 'friendList' and wrap in ReadOnlyCollection
         friendList = new List<Friend>();
         FriendList = new ReadOnlyCollection<Friend>(friendList);

         SelectedFriend = null;
         SelectedSearchMonth = SearchMonth.All;
      }


      #region Search Methods
      public void NextSearchMonth()
      {
         int searchIndex = (int)SelectedSearchMonth;
         searchIndex = (searchIndex + 1) % 13;
         // Set 'SelectedSearchMonth' variable
         SelectedSearchMonth = (SearchMonth)Enum.GetValues(typeof(SearchMonth)).GetValue(searchIndex);
         // Update the Search Results
         UpdateSearchResults();
      }
      public void UpdateSearchResults()
      {
         // Clear and add the new search results
         friendList.Clear();
         friendList.AddRange(GetFriendsByMonth(SelectedSearchMonth));

         // Invoke FriendListChanged event
         FriendListChanged?.Invoke(this, FriendList);
      }
      #endregion

      #region Find Friend Methods
      public Friend FindFriend(string name)
      {
         // Sanitize name (ToLower() is called later as to not interrupt the BinarySearch)
         name = name.Trim();
         if (String.IsNullOrEmpty(name))
            return null;

         Friend sudoFriend = new Friend(name, null, null, 0, 0);
         int result = friendList.BinarySearch(sudoFriend);
         if (result < 0)
            result = friendList.FindIndex(c => Regex.IsMatch(c.Name.ToLower(), name.ToLower()));
         if (result < 0)
            return null;
         return friendList[result];
      }
      #endregion

      #region Selected Friend Methods
      public void SetSelectedFriend(Friend friend)
      {
         // Assign the new SelectedFriend value
         SelectedFriend = friend;
         // Invoke SelectedFriendChanged event
         SelectedFriendChanged?.Invoke(this, friend);
      }
      #endregion


      public List<Friend> GetFriendsByMonth(SearchMonth searchMonth)
      {
         // Return a copy of the full FriendList if 'All' is passed
         if (searchMonth == SearchMonth.All)
            return new List<Friend>(originalFriendList);
         
         // Return all friends with a birthday in the specified month
         return originalFriendList.FindAll(c => c.BDayMonth == (int)searchMonth);
      }

      #region CSV Read/Write Methods
      public void ReloadFriendData()
      {
         // Clear existing friendList
         originalFriendList.Clear();
         // Read friend data from file and insert into the friendList
         originalFriendList.AddRange(ReadFriendsFromFile(FRIEND_DATA_FILEPATH));
         // Sort friend list
         originalFriendList.Sort();
      }
      public void WriteFriendData()
      {
         // Write friend data to data file
         WriteFriendsToFile(FRIEND_DATA_FILEPATH, originalFriendList);
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