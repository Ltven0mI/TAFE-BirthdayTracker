using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace BirthdayTracker
{
   /**********************************************************/
   // Filename:   FriendList.cs
   // Purpose:    To provide a container for storing, sorting,
   //             and finding friends.
   // Author:     Wade Rauschenbach
   // Version:    1.0.0
   // Date:       26-Aug-2020
   // Tests:      N/A
   /**********************************************************/

   /*********************** Changelog ************************/
   // [Unreleased]
   // 
   // [1.0.0] 26-Aug-2020
   // | [Added]
   // | - Add XML documentation comments for public API.
   //
   // [0.2.0] 23-Aug-2020
   // | [Added]
   // | - Add Update(string name, Friend friend) for updating friends.
   // | - Add Delete(Friend friend) for deleting friends.
   // 
   // [0.1.0] 20-Aug-2020
   // | [Added]
   // | - Initial FriendList implementation.
   /**********************************************************/

   /// <summary>
   /// To provide a container for storing, sorting, and finding friends.
   /// </summary>
   public class FriendList
   {
      /// <summary>
      /// Represents the method that will handle and event when the event
      /// provides data.
      /// </summary>
      /// <param name="sender">
      /// The <c>object</c> where the event originated.
      /// </param>
      /// <param name="friend">The <see cref="Friend"/>.</param>
      public delegate void FriendEventHandler(object sender, Friend friend);
      
      /// <summary>
      /// Invoked when <see cref="FriendList.SelectedFriend"/> is changed.
      /// <para>
      /// The new <see cref="FriendList.SelectedFriend"/> is passed as
      /// the second argument.
      /// </para>
      /// </summary>
      public event FriendEventHandler SelectedFriendChanged;

      /// <summary>
      /// Invoked when <see cref="FriendList.CurrentMonthFilter"/> is changed.
      /// <para>
      /// The new <see cref="FriendList.CurrentMonthFilter"/> is passed as
      /// the second argument.
      /// </para>
      /// </summary>
      public event EventHandler<MonthFilter> MonthFilterChanged;

      /// <summary>
      /// Invoked when <see cref="FriendList.Filtered"/> is changed.
      /// <para>
      /// The modified <see cref="FriendList.Filtered"/> is passed as
      /// the second argument.
      /// </para>
      /// </summary>
      public event EventHandler<ReadOnlyCollection<Friend>> FilteredChanged;

      private List<Friend> unfiltered;
      /// <summary>
      /// Contains all friends.
      /// <para>
      /// Modify <see cref="FriendList.Unfiltered"/> using
      /// <see cref="FriendList.SetUnfiltered(List{Friend})"/>.
      /// </para>
      /// </summary>
      /// <value>The list of unfiltered friends.</value>
      public ReadOnlyCollection<Friend> Unfiltered { get; private set; }
      
      // Don't modify individual friends in the Filtered list as they are all clones.
      private List<Friend> filtered;
      /// <summary>
      /// Contains all friends from <see cref="FriendList.Unfiltered"/>
      /// that satisfy the <see cref="CurrentMonthFilter"/> after a call to
      /// <see cref="FriendList.FilterByMonth(MonthFilter)"/>.
      /// <para>
      /// <b>NOTE:</b> No friends from <see cref="FriendList.Filtered"/> should
      /// be modified as they are clones ( see <see cref="Friend.Clone()"/> ).
      /// </para>
      /// </summary>
      /// <value>The list of filtered friends.</value>
      public ReadOnlyCollection<Friend> Filtered { get; private set; }

      // Don't modify SelectedFriend directly as it will always be a clone.
      /// <summary>
      /// The <see cref="Friend"/> last selected with
      /// <see cref="FriendList.SelectFriend(Friend)"/>.
      /// <para>
      /// <b>NOTE:</b> <see cref="FriendList.SelectedFriend"/> should not be
      /// modified directly as it will always be a clone
      /// ( see <see cref="Friend.Clone()"/> ).
      /// </para>
      /// </summary>
      /// <value>The currently selected <see cref="Friend"/>.</value>
      public Friend SelectedFriend { get; private set; }

      /// <summary>
      /// The <see cref="MonthFilter"/> last used in
      /// <see cref="FriendList.FilterByMonth(MonthFilter)"/>.
      /// </summary>
      /// <value>
      /// The current <see cref="MonthFilter"/>
      /// ( e.g. <see cref="MonthFilter.Jan"/> ).
      /// </value>
      public MonthFilter CurrentMonthFilter { get; private set; }


      /**********************************************************/
      // Method:  public FriendList ()
      // Purpose: Creates a new empty instance of FriendList.
      /**********************************************************/

      /// <summary>Creates a new empty instance of FriendList.</summary>
      public FriendList()
      {
         // Initialize the unfiltered list
         unfiltered = new List<Friend>();
         Unfiltered = new ReadOnlyCollection<Friend>(unfiltered);

         // Initialize the filtered list
         filtered = new List<Friend>();
         Filtered = new ReadOnlyCollection<Friend>(filtered);

         // Initialize the CurrentMonthFilter
         CurrentMonthFilter = MonthFilter.All;
      }

      /**********************************************************/
      // Method:  public void SetUnfiltered (List<Friend> friends)
      // Purpose: Replaces the elements in 'Unfiltered'
      //          with the elements in the 'friends' parameter,
      //          then sorts 'Unfiltered'.
      //          (Does not modify 'Filtered')
      // Inputs:  List<Friend> friends
      /**********************************************************/

      /// <summary>
      /// Replaces the elements in <see cref="FriendList.Unfiltered"/>
      /// with the elements in the <paramref name="friends"/> parameter,
      /// then sorts <see cref="FriendList.Unfiltered"/>.
      /// <para>(Does not modify <see cref="FriendList.Filtered"/>)</para>
      /// </summary>
      /// <param name="friends">
      /// The elements to replace <see cref="FriendList.Unfiltered"/> with.
      /// </param>
      public void SetUnfiltered(List<Friend> friends)
      {
         // Clear the unfiltered list, then add the new friends
         unfiltered.Clear();
         unfiltered.AddRange(friends);
         // Sort the unfiltered list (BinarySearch requires a sorted list)
         unfiltered.Sort();
      }

      /**********************************************************/
      // Method:  public void Add (Friend friend)
      // Purpose: Add 'friend' to 'Unfiltered',
      //          then sorts 'Unfiltered'.
      //          (Does not modify'Filtered')
      // Inputs:  Friend friend
      // Throws:  InvalidOperationException - if a friend already exists
      //          with the same name (Names are NOT case-sensitive).
      /**********************************************************/

      /// <summary>
      /// Add <paramref name="friend"/> to <see cref="FriendList.Unfiltered"/>,
      /// then sorts <see cref="FriendList.Unfiltered"/>.
      /// <para>(Does not modify <see cref="FriendList.Filtered"/>)</para>
      /// </summary>
      /// <param name="friend">
      /// The <see cref="Friend"/> to add to <see cref="FriendList.Unfiltered"/>.
      /// </param>
      public void Add(Friend friend)
      {
         // Ensure no friend exists with the same name
         if (unfiltered.Exists(c => c.Name.ToLower() == friend.Name.ToLower()))
            throw new InvalidOperationException(String.Format(
               "A friend with the name '{0}' already exists, "
               + "cannot add multiple friends with the same name.",
               friend.Name
            ));
         // Add a copy of the passed friend
         unfiltered.Add(friend.Clone());
         // Sort the unfiltered list (BinarySearch requires a sorted list)
         unfiltered.Sort();
      }

      /**********************************************************/
      // Method:  public void Update (string name, Friend friend)
      // Purpose: Updates the existing Friend with the name 'name' to
      //          match the data in 'friend'.
      // Inputs:  string name, Friend friend
      // Throws:  InvalidOperationException - when no friend exists
      //          with the name 'name'.
      /**********************************************************/

      /// <summary>
      /// Updates the existing <see cref="Friend"/> with the name
      /// <paramref name="name"/> to match the data in <paramref name="friend"/>.
      /// </summary>
      /// <param name="name">
      /// The name of the existing <see cref="Friend"/> to update.
      /// </param>
      /// <param name="friend">
      /// A <see cref="Friend"/> instance containing the data
      /// (e.g. <c>Name</c>, <c>Likes</c>, etc, ...) to replace in the
      /// existing <see cref="Friend"/>.
      /// </param>
      /// <exception cref="System.InvalidOperationException">
      /// Thrown when no <see cref="Friend"/> exists with
      /// the name <paramref name="name"/>.
      /// </exception>
      public void Update(string name, Friend friend)
      {
         // Attempt to get an existing Friend with 'name'
         Friend existingFriend = unfiltered.Find(
            c => c.Name.ToLower() == name.ToLower());
         
         // Ensure an existing friend was found
         if (existingFriend == null)
            throw new InvalidOperationException(
               $"No friend exists with the name '{name}'.");
         
         // Update data in existing friend
         existingFriend.Name = friend.Name;
         existingFriend.Likes = friend.Likes;
         existingFriend.Dislikes = friend.Dislikes;
         existingFriend.BDayMonth = friend.BDayMonth;
         existingFriend.BDayDay = friend.BDayDay;

         // Sort the unfiltered list (BinarySearch requires a sorted list)
         unfiltered.Sort();
      }

      /**********************************************************/
      // Method:  public void Delete (Friend friend)
      // Purpose: Deletes 'friend' from 'Unfiltered'.
      // Inputs:  Friend friend
      // Throws:  InvalidOperationException - when either the passed
      //          'friend' does not exist in 'Unfiltered',
      //          or the passed 'friend' doesn't match the
      //          existing 'friend'.
      /**********************************************************/

      /// <summary>
      /// Deletes <paramref name="friend"/> from <see cref="FriendList.Unfiltered"/>.
      /// </summary>
      /// <param name="friend">The <see cref="Friend"/> to delete.</param>
      /// <exception cref="System.InvalidOperationException">
      /// Thrown when either the <paramref name="friend"/>
      /// does not exist in <see cref="FriendList.Unfiltered"/>,
      /// or it doesn't match the existing <see cref="Friend"/>.
      /// </exception>
      public void Delete(Friend friend)
      {
         // Get the existing friend with a matching name
         Friend existingFriend = unfiltered.Find(
            c => c.Name.ToLower() == friend.Name.ToLower());
         
         // Ensure the friend to delete exists
         if (existingFriend == null)
            throw new InvalidOperationException(
               $"No friend with the name '{friend.Name}' exists."
            );
         
         // Ensure the passed friend matches the existing friend
         if (existingFriend.CompareByValue(friend) == false)
            throw new InvalidOperationException(
               "The passed friend does not match the existing friend."
            );
         
         // Remove friend
         unfiltered.Remove(existingFriend);

         // Sort the unfiltered list (BinarySearch requires a sorted list)
         unfiltered.Sort();
      }

      /**********************************************************/
      // Method:  public void SelectFriend (Friend friend)
      // Purpose: Sets the passed 'friend' as the current 'SelectedFriend'
      //          and invokes 'SelectedFriendChanged' with the
      //          new 'SelectedFriend' value.
      // Inputs:  Friend friend
      // Throws:  InvalidOperationException - when 'friend'
      //          does not exist in 'Unfiltered'.
      /**********************************************************/

      /// <summary>
      /// Sets <paramref name="friend"/> as the current
      /// <see cref="FriendList.SelectedFriend"/> and invokes
      /// <see cref="FriendList.SelectedFriendChanged"/> with the new
      /// <see cref="FriendList.SelectedFriend"/>.
      /// </summary>
      /// <param name="friend">
      /// The new value for <see cref="FriendList.SelectedFriend"/>
      /// </param>
      /// <exception cref="System.InvalidOperationException">
      /// Thrown when <paramref name="friend"/> does not exist in
      /// <see cref="FriendList.Unfiltered"/>.
      /// </exception>
      public void SelectFriend(Friend friend)
      {
         // Ensure the passed friend exists in this FriendList, or is null
         if (friend != null && !unfiltered.Exists(c => c.Name == friend.Name))
            throw new InvalidOperationException(String.Format(
               "Friend with name '{0}' is not present in this FriendList.",
               friend.Name
            ));
         
         // Assign the new SelectedFriend and invoke SelectedFriendChanged
         SelectedFriend = friend;
         SelectedFriendChanged?.Invoke(this, friend);
      }

      /**********************************************************/
      // Method:  public Friend FindFriendByName (string name)
      // Purpose: Finds and returns the Friend who's name
      //          matches/partially matches 'name'.
      //          By default 'Filtered' will be searched,
      //          however if ignoreFilter' is 'true' then
      //          'Unfiltered' will be searched.
      // Returns: Friend if a friend with the given 'name' was found
      // Returns: null if no friend with the given 'name' was found
      // Inputs:  string name, bool ignoreFilter=false
      // Outputs: Friend
      /**********************************************************/

      /// <summary>
      /// Finds and returns the the <see cref="Friend"/> who's name
      /// matches/partially matches <paramref name="name"/>.
      /// <para>
      /// By default <see cref="Filtered"/> will be searched,
      /// however if <paramref name="ignoreFilter"/> is <c>true</c>
      /// then <see cref="Unfiltered"/> will be searched.
      /// </para>
      /// </summary>
      /// <param name="name">
      /// The name/portion of the name of the <see cref="Friend"/> to find.
      /// </param>
      /// <param name="ignoreFilter">
      /// <c>false</c> (default) <see cref="FriendList.Filtered"/> will be searched,
      /// <c>true</c> <see cref="FriendList.Unfiltered"/> will be searched.
      /// </param>
      /// <returns>
      /// The found <see cref="Friend"/>, otherwise <c>null</c> if no
      /// <see cref="Friend"/> was found.
      /// </returns>
      public Friend FindFriendByName(string name, bool ignoreFilter=false)
      {
         // Sanitize name (ToLower() is called later as to not interrupt the BinarySearch)
         name = name.Trim();
         if (String.IsNullOrEmpty(name))
            return null;
         
         // Get List to search through
         List<Friend> listToSearch = ignoreFilter ? unfiltered : filtered;

         // Perform a BinarySearch on the List
         Friend dummyFriend = new Friend(name, null, null, 0, 0);
         int result = listToSearch.BinarySearch(dummyFriend);

         // If BinarySearch failed, fallback to less strict search
         if (result < 0)
            result = listToSearch.FindIndex(c => Regex.IsMatch(c.Name.ToLower(), name.ToLower()));
         
         // Return if no match was found
         if (result < 0)
            return null;
         
         // Return found friend
         return listToSearch[result];
      }

      /**********************************************************/
      // Method:  public void FilterByMonth (MonthFilter monthFilter)
      // Purpose: Filters friends from 'Unfiltered' into 'Filtered'
      //          based on whether they satisfy the 'monthFilter'.
      //          E.g. if 'monthFilter == MonthFilter.Jan' then
      //          only friends who's birthdays fall on January
      //          will be in 'Filtered'.
      // Inputs:  MonthFilter monthFilter
      /**********************************************************/

      /// <summary>
      /// Filters friends from <see cref="FriendList.Unfiltered"/>
      /// into <see cref="FriendList.Filtered"/> based on whether
      /// they satisfy the <paramref name="monthFilter"/>.
      /// <para>
      /// E.g. if <c>monthFilter == MonthFilter.Jan</c> then
      /// only friends who's birthdays fall on January
      /// will be in <see cref="FriendList.Filtered"/>.
      /// </para>
      /// </summary>
      /// <param name="monthFilter">
      /// The <see cref="MonthFilter"/> that all friends in
      /// <see cref="FriendList.Filtered"/> must satisfy.
      /// E.g. <see cref="MonthFilter.Jan"/>
      /// </param>
      public void FilterByMonth(MonthFilter monthFilter)
      {
         CurrentMonthFilter = monthFilter;

         // Get friends by birth month (these are the original instances, i.e. they need to be cloned)
         List<Friend> filteredFriends = GetFriendsByBirthMonth(monthFilter.Month);

         // Clear Filtered in prep for new friends :D
         filtered.Clear();

         // Fill 'filtered' with copies of the filtered friends
         foreach (Friend friend in filteredFriends)
         {
            // Add the cloned friend to the 'filtered' list
            filtered.Add(friend.Clone());
         }

         // Invoke events
         MonthFilterChanged?.Invoke(this, CurrentMonthFilter);
         FilteredChanged?.Invoke(this, Filtered);
      }

      /**********************************************************/
      // Method:  private List<Friend> GetFriendsByBirthMonth (int month)
      // Purpose: Get all friends who's birthdays fall on the given 'month'.
      //          If 'month <= 0' then a List containing all friends is returned.
      //          (The friends returned by this method are the original
      //          instances, meaning they should be cloned before being
      //          made available outside of this class.)
      // Returns: A List of friends who's birthday falls on the given 'month'.
      // Inputs:  int month
      // Outputs: List<Friend>
      /**********************************************************/

      /// <summary>
      /// Get all friends who's birthdays fall on the given
      /// <paramref name="month"/>.
      /// <para>
      /// If <c><paramref name="month"/> &lt;= 0</c> then a
      /// List containing all friends is returned.
      /// </para>
      /// <para>
      /// (The friends returned by this method are the original instances,
      /// meaning they should be cloned (see <see cref="Friend.Clone"/>)
      /// before being made available outside of this class.)
      /// </para>
      /// </summary>
      /// <param name="month">
      /// <c>&lt;= 0</c> to get all friends,
      /// <c>1</c> for friends born in January, ... ,
      /// <c>12</c> for friends born in December.
      /// </param>
      /// <returns>
      /// A List of friends who's birthday falls on the given <paramref name="month"/>
      /// </returns>
      private List<Friend> GetFriendsByBirthMonth(int month)
      {
         // Return a List containing the original friends, if '<= 0' is passed
         if (month <= 0)
            return new List<Friend>(unfiltered);
         
         // Return all friends with a birthday in the specified month
         return unfiltered.FindAll(c => c.BDayMonth == month);
      }

      /**********************************************************/
      // Method:  public Friend GetFirstFriend ()
      // Purpose: Get the 'Friend' that appears first in 'Filtered'.
      // Returns: The first 'Friend' if 'Filtered' is not empty.
      // Returns: null if 'Filtered' is empty.
      // Outputs: Friend
      /**********************************************************/

      /// <summary>
      /// Get the <see cref="Friend"/> that appears first in
      /// <see cref="FriendList.Filtered"/>.
      /// </summary>
      /// <returns>
      /// The first <see cref="Friend"/> if <see cref="FriendList.Filtered"/>
      /// is not empty, otherwise <c>null</c>.
      /// </returns>
      public Friend GetFirstFriend()
      {
         // Ensure there are friends in the Filtered list
         if (filtered.Count <= 0)
            return null;
         
         // Return the first friend
         return filtered[0];
      }

      /**********************************************************/
      // Method:  public Friend GetPrevFriend ()
      // Purpose: Get the 'Friend' that appears before 'SelectedFriend'
      //          in 'Filtered'.
      // Returns: The previous 'Friend' if 'Filtered' is not empty
      //          and contains 'SelectedFriend'.
      // Returns: null if 'Filtered' is empty,
      //          does not contain 'SelectedFriend',
      //          or 'SelectedFriend' is null.
      // Outputs: Friend
      /**********************************************************/

      /// <summary>
      /// Get the <see cref="Friend"/> that appears before
      /// <see cref="FriendList.SelectedFriend"/> in
      /// <see cref="FriendList.Filtered"/>.
      /// </summary>
      /// <returns>
      /// The previous <see cref="Friend"/> if <see cref="FriendList.Filtered"/>
      /// is not empty and contains <see cref="FriendList.SelectedFriend"/>,
      /// otherwise <c>null</c> if <see cref="FriendList.Filtered"/> is empty,
      /// does not contain <see cref="FriendList.SelectedFriend"/>,
      /// or <see cref="FriendList.SelectedFriend"/> is <c>null</c>.
      /// </returns>
      public Friend GetPrevFriend()
      {
         // Ensure there are friends in the Filtered list
         // and that 'SelectedFriend' is not null
         if (SelectedFriend == null || filtered.Count <= 0)
            return null;
         
         // Ensure the 'SelectedFriend' is in the 'Filtered' list
         int currentIndex = filtered.IndexOf(SelectedFriend);
         if (currentIndex < 0)
            return null;
         
         // Return the previous friend, with wrapping
         // (The 'filtered.Count +' is required as mod ('%')
         // doesn't work with negative numbers.)
         return filtered[(filtered.Count + currentIndex - 1) % filtered.Count];
      }

      /**********************************************************/
      // Method:  public Friend GetNextFriend ()
      // Purpose: Get the 'Friend' that appears after 'SelectedFriend'
      //          in 'Filtered'.
      // Returns: The next 'Friend' if 'Filtered' is not empty
      //          and contains 'SelectedFriend'.
      // Returns: null if 'Filtered' is empty,
      //          does not contain 'SelectedFriend',
      //          or 'SelectedFriend' is null.
      // Outputs: Friend
      /**********************************************************/

      /// <summary>
      /// Get the <see cref="Friend"/> that appears after
      /// <see cref="FriendList.SelectedFriend"/> in
      /// <see cref="FriendList.Filtered"/>.
      /// </summary>
      /// <returns>
      /// The next <see cref="Friend"/> if <see cref="FriendList.Filtered"/>
      /// is not empty and contains <see cref="FriendList.SelectedFriend"/>,
      /// otherwise <c>null</c> if <see cref="FriendList.Filtered"/> is empty,
      /// does not contain <see cref="FriendList.SelectedFriend"/>,
      /// or <see cref="FriendList.SelectedFriend"/> is <c>null</c>.
      /// </returns>
      public Friend GetNextFriend()
      {
         // Ensure there are friends in the Filtered list
         // and that 'SelectedFriend' is not null
         if (SelectedFriend == null || filtered.Count <= 0)
            return null;
         
         // Ensure the 'SelectedFriend' is in the 'Filtered' list
         int currentIndex = filtered.IndexOf(SelectedFriend);
         if (currentIndex < 0)
            return null;
         
         // Return the next friend, with wrapping
         return filtered[(currentIndex + 1) % filtered.Count];
      }

      /**********************************************************/
      // Method:  public Friend GetLastFriend ()
      // Purpose: Get the 'Friend' that appears last in 'Filtered'.
      // Returns: The last 'Friend' if 'Filtered' is not empty.
      // Returns: null if 'Filtered' is empty.
      // Outputs: Friend
      /**********************************************************/
      
      /// <summary>
      /// Get the <see cref="Friend"/> that appears last in
      /// <see cref="FriendList.Filtered"/>.
      /// </summary>
      /// <returns>
      /// The last <see cref="Friend"/> if <see cref="FriendList.Filtered"/>
      /// is not empty, otherwise <c>null</c>.
      /// </returns>
      public Friend GetLastFriend()
      {
         // Ensure there are friends in the Filtered list
         if (filtered.Count <= 0)
            return null;
         
         // Return the last friend
         return filtered[filtered.Count - 1];
      }
   }
}