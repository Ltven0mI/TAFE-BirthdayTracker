using System.Linq;
using System.Collections.ObjectModel;
using System;
using System.Windows.Forms;

namespace BirthdayTracker
{
   /**********************************************************/
   // Filename: Controller.cs
   // Purpose: To act as the interface between the program's
   // GUI (View) and the program's data (Model).
   // Author: Wade Rauschenbach
   // Version: 0.2.0
   // Date: 21-Aug-2020
   // Tests: N/A
   /**********************************************************/

   /*********************** Changelog ************************/
   // [Unreleased]
   // 
   // [0.2.0] 21-Aug-2020
   // | [Changed]
   // | - Controller private code overhauled to work with the new
   // |   FriendList class workflow.
   // 
   // [0.1.0] 07-Aug-2020
   // | [Added]
   // | - Initial Controller implementation.
   /**********************************************************/
   public class Controller
   {
      private Model model;
      private View view;

      /**********************************************************/
      // Method: public Controller (Model model, View view)
      // Purpose: Registers callbacks in 'model' and 'view' and
      // initializes 'model' and 'view' with required values.
      // Inputs: Model model - the corresponding Model of the MVC
      // Inputs: View view - the corresponding View of the MVC
      /**********************************************************/
      public Controller(Model model, View view)
      {
         this.model = model;
         this.view = view;

         RegisterCallbacks();

         model.ReadFriends();
         model.FriendList.FilterByMonth(MonthFilter.All);
         model.FriendList.SelectFriend(null);
      }

      private void RegisterCallbacks()
      {
         // * Exit - Button - Click * //
         view.exit_Button.Click += (object sender, EventArgs args) => 
         {
            Application.Exit();
         };
         // END - Exit - Button - Click //

         // * Find - TextBox - KeyPress * //
         view.find_TextBox.KeyPress += (object sender, KeyPressEventArgs args) =>
         {
            // If enter was pressed: Perform click on Find Button
            if (args.KeyChar == '\r')
            {
               view.find_Button.PerformClick();
               args.Handled = true;
            }
         };
         // END - Find - KeyPress - Enter //

         // * Find - Button - Click * //
         view.find_Button.Click += (object sender, EventArgs args) =>
         {
            // Retrieve and sanitize queryString
            string queryString = view.find_TextBox.Text.Trim();
            view.find_TextBox.Text = queryString;

            // Ensure queryString is not empty
            if (String.IsNullOrEmpty(queryString))
            {
               MessageBox.Show("Please enter a name.", "NO NAME");
               return;
            }

            // Find friend using the sanitized queryString
            Friend foundFriend = model.FriendList.FindFriendByName(queryString);
            model.FriendList.SelectFriend(foundFriend);

            // If no friend was found: Display message to user
            if (foundFriend == null)
            {
               string msg = String.Format("Sorry, no name found for '{0}'", queryString);
               MessageBox.Show(msg, "NOT FOUND", MessageBoxButtons.OK);
            }
         };
         // END - Find - Button - Click //

         // * Search - Button - Click * //
         view.search_Button.Click += (object sender, EventArgs args) =>
         {
            // Filter the FriendList by the next MonthFilter
            FriendList friendList = model.FriendList;
            friendList.FilterByMonth(friendList.CurrentMonthFilter.GetNext());
            friendList.SelectFriend(null);
         };
         // END - Search - Button - Click //

         // * NavFirst - Button - Click * //
         view.navFirst_Button.Click += (object sender, EventArgs args) =>
         {
            // Select the first friend in the FriendList
            FriendList friendList = model.FriendList;
            friendList.SelectFriend(friendList.GetFirstFriend());
         };
         // END - NavFirst - Button - Click //

         // * NavPrev - Button - Click * //
         view.navPrev_Button.Click += (object sender, EventArgs args) =>
         {
            // Select the previous friend in the FriendList
            FriendList friendList = model.FriendList;
            friendList.SelectFriend(friendList.GetPrevFriend());
         };
         // END - NavPrev - Button - Click //

         // * NavNext - Button - Click * //
         view.navNext_Button.Click += (object sender, EventArgs args) =>
         {
            // Select the next friend in the FriendList
            FriendList friendList = model.FriendList;
            friendList.SelectFriend(friendList.GetNextFriend());
         };
         // END - NavNext - Button - Click //

         // * NavLast - Button - Click * //
         view.navLast_Button.Click += (object sender, EventArgs args) =>
         {
            // Select the last friend in the FriendList
            FriendList friendList = model.FriendList;
            friendList.SelectFriend(friendList.GetLastFriend());
         };
         // END - NavLast - Button - Click //

         // * FriendList - SelectedFriendChanged * //
         model.FriendList.SelectedFriendChanged += (object sender, Friend selectedFriend) =>
         {
            // Display the data of the changed SelectedFriend
            view.DisplaySelectedFriendData(selectedFriend);

            // Re-display FriendList to reflect the new SelectedFriend
            view.DisplayFriendList(((FriendList)sender).Filtered, selectedFriend);
         };
         // END - FriendList - SelectedFriendChanged //

         // * FriendList - MonthFilterChanged * //
         model.FriendList.MonthFilterChanged += (object sender, MonthFilter monthFilter) =>
         {
            // Display the changed MonthFilter
            view.DisplaySearchMonth(monthFilter);
         };
         // END - FriendList - MonthFilterChanged //

         // * FriendList - FilteredChanged * //
         model.FriendList.FilteredChanged += (object sender, ReadOnlyCollection<Friend> filtered) =>
         {
            // Display the changed Filtered list
            view.DisplayFriendList(filtered, ((FriendList)sender).SelectedFriend);
         };
         // END - FriendList - FilteredChanged //
      }
   }
}