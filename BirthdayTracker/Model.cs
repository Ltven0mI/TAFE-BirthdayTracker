using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using CsvHelper;

namespace BirthdayTracker
{
   /**********************************************************/
   // Filename:   Model.cs
   // Purpose:    To act as a wrapper for the program's data
   //             and provide methods for modifying that data.
   // Author:     Wade Rauschenbach
   // Version:    0.2.0
   // Date:       21-Aug-2020
   // Tests:      N/A
   /**********************************************************/

   /*********************** Changelog ************************/
   // [Unreleased]
   //
   // [0.2.0] 21-Aug-2020
   // | [Changed]
   // | - FriendList has been changed from a ReadOnlyCollection to
   // |   an instance of the FriendList class.
   // | - ReloadFriendData() has been renamed to ReadFriends().
   // | - WriteFriendData() has been renamed to WriteFriends().
   // | [Removed]
   // | - FriendListChanged has been removed.
   // | - SelectedFriendChanged has been removed.
   // | - SearchMonth has been removed and superseded by MonthFilter.
   // | - SelectedFriend has been removed.
   // | - SelectedSearchMonth has been removed.
   // | - NextSearchMonth() has been removed.
   // | - UpdateSearchResults() has been removed.
   // | - FindFriend(string name) has been removed.
   // | - SetSelectedFriend(Friend friend) has been removed.
   // | - GetFriendsByMonth(SearchMonth searchMonth) has been removed.
   // 
   // [0.1.0] 07-Aug-2020
   // | [Added]
   // | - Initial Model implementation.
   /**********************************************************/
   public class Model
   {
      private const string FRIEND_DATA_FILEPATH = "MyFriendData.csv";
      
      public FriendList FriendList { get; private set; }

      /**********************************************************/
      // Method:  public Model ()
      // Purpose: constructs and initializes a new instance of Model
      /**********************************************************/
      public Model()
      {
         // Initialize 'FriendList'
         FriendList = new FriendList();
      }

      /**********************************************************/
      // Method:  public void ReadFriends ()
      // Purpose: Reads friend data from the file specified
      //          in 'FRIEND_DATA_FILEPATH'.
      //          Does NOT update search results.
      /**********************************************************/
      public void ReadFriends()
      {
         // Read friend data from file and insert into the friendList
         FriendList.SetUnfiltered(ReadFriendsFromFile(FRIEND_DATA_FILEPATH));
      }
      
      /**********************************************************/
      // Method:  public void WriteFriends ()
      // Purpose: Writes friend data to the file specified
      //          in 'FRIEND_DATA_FILEPATH'.
      /**********************************************************/
      public void WriteFriends()
      {
         // Write friends data to file
         WriteFriendsToFile(FRIEND_DATA_FILEPATH, FriendList.Unfiltered);
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
      private void WriteFriendsToFile(string filepath, IEnumerable<Friend> friendList)
      {
         using (var writer = new StreamWriter(filepath))
         {
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
               csv.Configuration.HasHeaderRecord = false;
               csv.WriteRecords(friendList);
            }
         }
      }
   }
}