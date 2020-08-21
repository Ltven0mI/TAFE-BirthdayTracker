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
   // Version:    0.1.0
   // Date:       20-Aug-2020
   // Tests:      N/A
   /**********************************************************/

   /*********************** Changelog ************************/
   // [Unreleased]
   // 
   // [0.1.0] 20-Aug-2020
   // | [Added]
   // | - Initial FriendList implementation.
   /**********************************************************/
   public class FriendList
   {
      public delegate void FriendEventHandler(object sender, Friend friend);
      public event FriendEventHandler SelectedFriendChanged;

      public event EventHandler<MonthFilter> MonthFilterChanged;

      public event EventHandler<ReadOnlyCollection<Friend>> FilteredChanged;

      private List<Friend> unfiltered;
      public ReadOnlyCollection<Friend> Unfiltered { get; private set; }
      
      // Don't modify individual friends in the Filtered list as they are all clones.
      private List<Friend> filtered;
      public ReadOnlyCollection<Friend> Filtered { get; private set; }

      // Don't modify SelectedFriend directly as it will always be a clone.
      public Friend SelectedFriend { get; private set; }

      public MonthFilter CurrentMonthFilter { get; private set; }


      /**********************************************************/
      // Method:  public FriendList ()
      // Purpose: Creates a new empty instance of FriendList.
      /**********************************************************/
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
      // Purpose: Replaces the elements in the internal 'unfiltered'
      //          list with the elements in the passed 'friends' list,
      //          then sorts the 'unfiltered' list.
      //          (Does not update the 'Filtered' list)
      // Inputs:  List<Friend> friends
      /**********************************************************/
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
      // Purpose: Add 'friend' to this FriendList.
      //          (Does not update the 'Filtered' list)
      // Inputs:  Friend friend
      // Throws:  InvalidOperationException - if a friend already exists
      //          with the same name (Names are NOT case-sensitive).
      /**********************************************************/
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
      // Method:  public void SelectFriend (Friend friend)
      // Purpose: Sets the passed 'friend' as the current 'SelectedFriend'
      //          and invokes the 'SelectedFriendChanged' event with the
      //          new 'SelectedFriend' value.
      // Inputs:  Friend friend
      // Throws:  InvalidOperationException - when the passed 'friend'
      //          does not exist in this instance.
      /**********************************************************/
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
      // Purpose: Finds and returns the the Friend who's name
      //          matches/partially matches the passed 'name'.
      //          By default the 'Filtered' list will be searched,
      //          however 'ignoreFilter' can be passed as true
      //          to search all friends.
      // Returns: Friend if a friend with the given 'name' was found
      // Returns: null if no friend with the given 'name' was found
      // Inputs:  string name, bool ignoreFilter=false
      // Outputs: Friend
      /**********************************************************/
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
      // Purpose: Filters the internal 'Filtered' list to only contain
      //          friends who's birthday falls on the passed month.
      // Inputs:  MonthFilter monthFilter
      /**********************************************************/
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
      // Purpose: Get all Friends who's birthdays fall on the given 'month'.
      //          If 'month' is '<= 0' then a List containing all Friends is returned.
      //          (The friends returned by this method are the original
      //          instances, meaning they should be cloned before being
      //          made available outside of this class.)
      // Returns: A List of Friends
      // Inputs:  int month
      // Outputs: List<Friend>
      /**********************************************************/
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
      // Purpose: Get the first friend in the 'Filtered' list.
      // Returns: Friend if the 'Filtered' list is not empty,
      // Returns: null if the 'Filtered' list is empty.
      // Outputs: Friend
      /**********************************************************/
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
      // Purpose: Get the previous friend in relation to the
      //          'SelectedFriend' in the 'Filtered' list.
      // Returns: Friend if the 'Filtered' list is not empty
      //          and contains the current 'SelectedFriend'.
      // Returns: null if the 'Filtered' list is empty,
      //          does not contain the 'SelectedFriend',
      //          or the 'SelectedFriend' is null.
      // Outputs: Friend
      /**********************************************************/
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
      // Purpose: Get the next friend in relation to the
      //          'SelectedFriend' in the 'Filtered' list.
      // Returns: Friend if the 'Filtered' list is not empty
      //          and contains the current 'SelectedFriend'.
      // Returns: null if the 'Filtered' list is empty,
      //          does not contain the 'SelectedFriend',
      //          or the 'SelectedFriend' is null.
      // Outputs: Friend
      /**********************************************************/
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
      // Purpose: Get the last friend in the 'Filtered' list.
      // Returns: Friend if the 'Filtered' list is not empty,
      // Returns: null if the 'Filtered' list is empty.
      // Outputs: Friend
      /**********************************************************/
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

// namespace BirthdayTracker
// {
//    /**********************************************************/
//    // Filename:   FriendList.cs
//    // Purpose:    To provide a wrapper for friend lists and
//    //             related methods.
//    // Author:     Wade Rauschenbach
//    // Version:    0.1.0
//    // Date:       19-Aug-2020
//    // Tests:      N/A
//    /**********************************************************/
//    public class FriendList
//    {
//       // * SearchMonth * //
//       public enum SearchMonth { All, Jan, Feb, March, April, May, June, July, Aug, Sep, Oct, Nov, Dec };
//       public SearchMonth SelectedSearchMonth { get; private set; }
//       // END - SearchMonth //

//       // * SelectedFriend * //
//       public Friend SelectedFriend { get; private set; }
//       // END - SelectedFriend //

//       // * Unfiltered List * //
//       private List<Friend> unfiltered;
//       public ReadOnlyCollection<Friend> Unfiltered { get; private set; }
//       // END - UnfilteredList //

//       // * Filtered List * //
//       private List<Friend> filtered;
//       public ReadOnlyCollection<Friend> Filtered { get; private set; }
//       // END - Filtered List //


//       /**********************************************************/
//       // Method:  public FriendList ()
//       // Purpose: constructs and initializes a new instance of FriendList
//       /**********************************************************/
//       public FriendList()
//       {
//          SelectedSearchMonth = SearchMonth.All;
//          SelectedFriend = null;

//          // Initialize 'unfiltered' and wrap in ReadOnlyCollection
//          unfiltered = new List<Friend>();
//          Unfiltered = new ReadOnlyCollection<Friend>(unfiltered);

//          // Initialize 'filtered' and wrap in ReadOnlyCollection
//          filtered = new List<Friend>();
//          Filtered = new ReadOnlyCollection<Friend>(filtered);
//       }


//       /**********************************************************/
//       // Method:  public void Add (Friend friend)
//       // Purpose: Add the passed friend, then sorts the
//       //          internal list alphabetically by name.
//       // Inputs:  Friend friend
//       /**********************************************************/
//       public void Add(Friend friend)
//       {
//          // Ensure 'friend' has not already been added
//          if (unfiltered.Contains(friend))
//             return;

//          // Add 'friend' and sort to ensure BinarySearch will work
//          unfiltered.Add(friend);
//          unfiltered.Sort();

//          // Update filter results
//          UpdateFiltered();
//       }

//       /**********************************************************/
//       // Method:  public void AddRange (IEnumerable<Friend> collection)
//       // Purpose: Add the friends from the passed 'collection',
//       //          then sorts the internal list alphabetically by name.
//       // Inputs:  IEnumerable<Friend> collection
//       /**********************************************************/
//       public void AddRange(IEnumerable<Friend> collection)
//       {
//          // Iterate over the collection
//          foreach (Friend friend in collection)
//          {
//             // Only add the Friends that are not already added
//             if (!unfiltered.Contains(friend))
//                unfiltered.Add(friend);
//          }
         
//          // Sort to ensure BinarySearch will work
//          unfiltered.Sort();

//          // Update filter results
//          UpdateFiltered();
//       }
      
//       /**********************************************************/
//       // Method:  public void Clear ()
//       // Purpose: Clears all Friends.
//       /**********************************************************/
//       public void Clear()
//       {
//          filtered.Clear();
//          // TODO: Invoke FilteredListChanged
//          unfiltered.Clear();
//          // TODO: Invoke UnfilteredListChanged
//       }
      
//       /**********************************************************/
//       // Method:  public List<Friend> GetAll ()
//       // Purpose: Get all un-filtered Friends.
//       // Returns: A list of all un-filtered Friends
//       // Outputs: List<Friend>
//       /**********************************************************/
//       public List<Friend> GetAll()
//       {
//          // Return a new List of all friends
//          return new List<Friend>(unfiltered);
//       }

//       /**********************************************************/
//       // Method:  public List<Friend> GetAllByBirthMonth (int month)
//       // Purpose: Get all Friends who's birthdays fall on the given 'month'.
//       //          If 'month' is '<= 0' then a List containing all Friends is returned.
//       // Returns: A List of Friends
//       // Inputs:  int month
//       // Outputs: List<Friend>
//       /**********************************************************/
//       public List<Friend> GetAllByBirthMonth(int month)
//       {
//          // Return a List containing the original friends, if '<= 0' is passed
//          if (month <= 0)
//             return new List<Friend>(unfiltered);
         
//          // Return all friends with a birthday in the specified month
//          return unfiltered.FindAll(c => c.BDayMonth == month);
//       }
      
//       /**********************************************************/
//       // Method:  public void FilterByBirthMonth (SearchMonth searchMonth)
//       // Purpose: Filters the internal 'Filtered' list to only contain
//       //          friends who's birthday falls on the passed month.
//       // Inputs:  SearchMonth searchMonth
//       /**********************************************************/
//       public void FilterByBirthMonth(SearchMonth searchMonth)
//       {
//          // Update the month filter
//          SelectedSearchMonth = searchMonth;
//          // TODO: Invoke SearchMonthChanged

//          // Update filter results
//          UpdateFiltered();
//       }

//       /**********************************************************/
//       // Method:  public void NextSearchMonth()
//       // Purpose: Cycles to the next search month and updates the filtered results
//       //          (e.g. from 'All' to 'Jan', or from 'March' to 'April')
//       /**********************************************************/
//       public void NextSearchMonth()
//       {
//          int searchIndex = (int)SelectedSearchMonth;
//          searchIndex = (searchIndex + 1) % 13;
//          // Filter the FriendList by the new search month
//          FilterByBirthMonth((SearchMonth)Enum.GetValues(typeof(SearchMonth)).GetValue(searchIndex));
//       }

//       private void UpdateFiltered()
//       {
//          // Clear and add the new filter results
//          filtered.Clear();
//          filtered.AddRange(GetAllByBirthMonth((int)SelectedSearchMonth));

//          // TODO: Invoke FilteredListChanged
//       }
      
//       /**********************************************************/
//       // Method:  public void SetSelectedFriend (Friend friend)
//       // Purpose: Sets the selected friend and
//       //          invokes the 'SelectedFriendChanged' event.
//       //          Inputs: Friend friend
//       /**********************************************************/
//       public void SetSelectedFriend(Friend friend)
//       {
//          // Assign the new SelectedFriend value
//          SelectedFriend = friend;
//          // TODO: Invoke SelectedFriendChanged
//       }

//       /**********************************************************/
//       // Method:  public Friend FindFriendByName (string name)
//       // Purpose: Finds and returns the the Friend who's name
//       //          matches/partially matches the passed 'name'.
//       //          By default the 'Filtered' list will be searched,
//       //          however 'ignoreFilter' can be passed as true
//       //          to search all friends.
//       // Returns: Friend if a friend with the given 'name' was found
//       // Returns: null if no friend with the given 'name' was found
//       // Inputs:  string name, bool ignoreFilter=false
//       // Outputs: Friend
//       /**********************************************************/
//       public Friend FindFriendByName(string name, bool ignoreFilter=false)
//       {
//          // Sanitize name (ToLower() is called later as to not interrupt the BinarySearch)
//          name = name.Trim();
//          if (String.IsNullOrEmpty(name))
//             return null;
         
//          // Get List to search through
//          List<Friend> listToSearch = ignoreFilter ? unfiltered : filtered;

//          // Perform a BinarySearch on the List
//          Friend dummyFriend = new Friend(name, null, null, 0, 0);
//          int result = listToSearch.BinarySearch(dummyFriend);

//          // If BinarySearch failed, fallback to less strict search
//          if (result < 0)
//             result = listToSearch.FindIndex(c => Regex.IsMatch(c.Name.ToLower(), name.ToLower()));
         
//          // Return if no match was found
//          if (result < 0)
//             return null;
         
//          // Return found friend
//          return listToSearch[result];
//       }
//    }
// }