using System;
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
   // Version:    0.3.0
   // Date:       24-Aug-2020
   // Tests:      N/A
   /**********************************************************/

   /*********************** Changelog ************************/
   // [Unreleased]
   //
   // [0.3.0] 24-Aug-2020
   // | [Changed]
   // | - Change FRIEND_DATA_FILEPATH from private to public.
   // | - Change FRIEND_DATA_FILEPATH from public back to private.
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
      // Throws:  ArgumentException - if the save file path is an empty string.
      //          ArgumentNullException - if save file path is null.
      //          FileNotFoundException - if the save file cannot be found.
      //          DirectoryNotFoundException - if the specified save file
      //              path is invalid.
      //          IOException - if save file path includes incorrect or invalid
      //             syntax for file name, directory name, or volume label.
      //          TypeConverterException - if an error occurs while reading
      //             from the CSV file.
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
      // Throws:  UnauthorizedAccessException - if access is denied.
      //          ArgumentException - if the save filepath is an empty
      //             string or contains the name of a system device.
      //          ArgumentNullException - if the save filepath is null.
      //          DirectoryNotFoundExcetpion - if the save filepath
      //             is invalid.
      //          PathToLongException - if the save filepath exceeds the
      //             system defined maximum length.
      //          IOException - if save filepath contains invalid syntax.
      //          SecurityException - if the caller does not have
      //             the required permission.
      /**********************************************************/
      public void WriteFriends()
      {
         // Write friends data to file
         WriteFriendsToFile(FRIEND_DATA_FILEPATH, FriendList.Unfiltered);
      }
      
      /**********************************************************/
      // Method:  private List<Friend> ReadFriendsFromFile (string filepath)
      // Purpose: Read friend records from a CSV file at 'filepath'
      // Returns: List<Friend> containing friend records on success
      // Inputs:  string filepath
      // Outputs: List<Friend>
      // Throws:  ArgumentException - if 'filepath' is an empty string.
      //          ArgumentNullException - if 'filepath' is null.
      //          FileNotFoundException - if the file cannot be found.
      //          DirectoryNotFoundException - if the specified path is invalid.
      //          IOException - if 'filepath' includes incorrect or invalid
      //             syntax for file name, directory name, or volume label.
      //          TypeConverterException - if an error occurs while reading
      //             from the CSV file.
      /**********************************************************/
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

      /**********************************************************/
      // Method:  private void WriteFriendsToFile (string filepath,
      //             IEnumerable<Friend> friendList)
      // Purpose: Write friend records to CSV file at 'filepath'.
      // Inputs:  string filepath, IEnumerable<Friend> friendList
      // Throws:  UnauthorizedAccessException - if access is denied.
      //          ArgumentException - if 'filepath' is an empty string
      //             or contains the name of a system device.
      //          ArgumentNullException - if 'filepath' is null.
      //          DirectoryNotFoundExcetpion - if the specified
      //             'filepath' is invalid.
      //          PathToLongException - if 'filepath' exceeds the
      //             system defined maximum length.
      //          IOException - if 'filepath' contains invalid syntax.
      //          SecurityException - if the caller does not have
      //             the required permission.
      /**********************************************************/
      private void WriteFriendsToFile(string filepath,
         IEnumerable<Friend> friendList)
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