using System.Collections.Generic;
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
   // | [Added]
   // | - Implement new friend functionality.
   // | - Implement update friend functionality.
   // | - Implement delete friend functionality.
   // | - Implement exit options prompt.
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

         // * New - Button - Click * //
         view.new_Button.Click += (object sender, EventArgs args) =>
         {
            string name = null;
            string likes = null;
            string dislikes = null;
            int birthMonth = 0;
            int birthDay = 0;

            // Validate input fields
            bool validationSuccesful = ValidateFriendData(out name, out likes,
               out dislikes, out birthMonth, out birthDay);
            if (!validationSuccesful)
               return;

            // Store FriendList for usage below
            FriendList friendList = model.FriendList;

            // Ensure a friend doesn't already exist with 'name'
            if (friendList.FindFriendByName(name, true) != null)
            {
               // Display error to user
               string message = String.Format(
                  "ERROR:\n"
                  + "A friend with the name '{0}' already exists.\n"
                  + "(NOTE: friend names are NOT case-sensitive)",
                  name
               );
               MessageBox.Show(message, "ERROR(S) FOUND!", MessageBoxButtons.OK,
                  MessageBoxIcon.Exclamation);
               // Return as there is no point continuing
               return;
            }

            // Prompt user to continue
            string continueMessage =
               $"Create new friend:\n"
               + $"| Name: '{name}'\n"
               + $"| Likes: '{likes}'\n"
               + $"| Dislikes: '{dislikes}'\n"
               + $"| Birth Day: '{birthDay}'\n"
               + $"| Birth Month: '{birthMonth}'\n"
               + "Do you wish to continue?";
            DialogResult continueResult = MessageBox.Show(continueMessage,
               "NEW FRIEND", MessageBoxButtons.YesNo,
               MessageBoxIcon.Question);
            
            // Ensure the user wants to continue
            if (continueResult != DialogResult.Yes)
               return;

            // Add the new friend
            friendList.Add(new Friend(
               name, likes, dislikes, birthDay, birthMonth
            ));

            // Refresh the FriendList filter
            friendList.FilterByMonth(friendList.CurrentMonthFilter);

            // Clear the selected friend (Also clears the text fields)
            friendList.SelectFriend(null);
         };
         // END - New - Button - Click //

         // * Update - Button - Click * //
         view.update_Button.Click += (object sender, EventArgs args) =>
         {
            // Get FriendList for use below
            FriendList friendList = model.FriendList;

            // Ensure there is a friend selected
            Friend selectedFriend = null;
            if (!EnsureFriendIsSelected(out selectedFriend))
               return;

            string name = null;
            string likes = null;
            string dislikes = null;
            int birthMonth = 0;
            int birthDay = 0;

            // Ensure input fields are valid
            bool validationSuccesful = ValidateFriendData(out name, out likes,
               out dislikes, out birthMonth, out birthDay);
            if (!validationSuccesful)
               return;

            // Ensure name is unchanging or is available
            if (name.ToLower() != selectedFriend.Name.ToLower() &&
            friendList.FindFriendByName(name, true) != null)
            {
               // Display error to user
               string message = "ERROR:\n"
               + $"Cannot change name to '{name}' "
               + "as that name is already taken.\n"
               + "(NOTE: friend names are NOT case-sensitive)";
               MessageBox.Show(message, "ERROR(S) FOUND!", MessageBoxButtons.OK,
                  MessageBoxIcon.Exclamation);
               // Return as there is no point continuing
               return;
            }

            // Prompt user to continue
            string continueMessage =
               $"Update friend '{selectedFriend.Name}' to\n"
               + $"| Name: '{name}'\n"
               + $"| Likes: '{likes}'\n"
               + $"| Dislikes: '{dislikes}'\n"
               + $"| Birth Day: '{birthDay}'\n"
               + $"| Birth Month: '{birthMonth}'\n"
               + "Do you wish to continue?";
            DialogResult continueResult = MessageBox.Show(continueMessage,
               "UPDATE FRIEND", MessageBoxButtons.YesNo,
               MessageBoxIcon.Question);
            
            // Ensure the user wants to continue
            if (continueResult != DialogResult.Yes)
               return;
            
            // Update the selected friend
            friendList.Update(selectedFriend.Name,
               new Friend(name, likes, dislikes, birthDay, birthMonth));
            
            // Refresh FriendList filter
            friendList.FilterByMonth(friendList.CurrentMonthFilter);

            // Clear the SelectedFriend (Also clears text fields)
            friendList.SelectFriend(null);
         };
         // END - Update - Button - Click //

         // * Delete - Button - Click * //
         view.delete_Button.Click += (object sender, EventArgs args) =>
         {
            // Ensure there is a friend selected
            Friend selectedFriend = null;
            if (!EnsureFriendIsSelected(out selectedFriend))
               return;
            
            string name = null;
            string likes = null;
            string dislikes = null;
            int birthMonth = 0;
            int birthDay = 0;

            // Ensure input fields are valid
            bool validationSuccesful = ValidateFriendData(out name, out likes,
               out dislikes, out birthMonth, out birthDay);
            if (!validationSuccesful)
               return;
            
            // Ensure the friend data is unchanged
            if (!selectedFriend.CompareByValue(new Friend(
               name, likes, dislikes, birthDay, birthMonth)))
            {
               // Display error to user
               string message = "ERROR:\n"
                  + $"Unable to delete friend named '{selectedFriend.Name}'.\n"
                  + "Please ensure the friend data matches the current friend.";
               MessageBox.Show(message, "ERROR(S) FOUND!", MessageBoxButtons.OK,
                  MessageBoxIcon.Exclamation);
               // Return as there is no point continuing
               return;
            }

            // Prompt user to continue
            string continueMessage =
               $"You are about to delete the friend '{selectedFriend.Name}'\n"
               + "Do you wish to continue?";
            DialogResult continueResult = MessageBox.Show(continueMessage,
               "DELETE FRIEND", MessageBoxButtons.YesNo,
               MessageBoxIcon.Question);
            
            // Ensure the user wants to continue
            if (continueResult != DialogResult.Yes)
               return;

            // Store FriendList for use below
            FriendList friendList = model.FriendList;
            
            // Delete the friend
            friendList.Delete(selectedFriend);

            // Refresh the FriendList filter
            friendList.FilterByMonth(friendList.CurrentMonthFilter);

            // Clear the selected friend (Also clears input fields)
            friendList.SelectFriend(null);
         };
         // END - Delete - Button - Click //

         // * Exit - Button - Click * //
         view.exit_Button.Click += (object sender, EventArgs args) =>
         {
            // Prompt user to save or not
            string message = "You are about to write all records to the "
            + "external file and exit\n\n"
            + "Click YES if you wish to write the records and exit\n"
            + "Click NO if you only want to exit (no writing)\n"
            + "Click CANCEL if you don't want to exit";
            DialogResult exitResult = MessageBox.Show(message, "EXIT OPTIONS",
               MessageBoxButtons.YesNoCancel);
            
            // Check if user accidentally click exit...
            if (exitResult == DialogResult.Cancel)
               return;
            
            // Check if user wants to write records
            if (exitResult == DialogResult.Yes)
               model.WriteFriends();
            
            // Only exit if user chose YES or NO (Just a precaution...)
            if (exitResult == DialogResult.Yes || exitResult == DialogResult.No)
               Application.Exit();
         };
         // END - Exit - Button - Click //

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

      /**********************************************************/
      // Method:  private bool ValidateFriendData (
      //             out string name, out string likes,
      //             out string dislikes, out int birthMonth,
      //             out int birthDay
      //          )
      // Purpose: Validates and parses the user entered friend data.
      //          In the case of invalid data, an error is displayed
      //          to the user describing what data is invalid. 
      // Returns: true if all fields are valid.
      // Returns: false if any fields are NOT valid.
      // Inputs:  out string name, out string likes,
      //          out string dislikes, out int birthMonth,
      //          out int birthDay.
      // Outputs: bool
      /**********************************************************/
      private bool ValidateFriendData(out string name, out string likes,
      out string dislikes, out int birthMonth, out int birthDay)
      {
         // Get input data
         name = view.personsName_TextBox.Text.Trim();
         likes = view.likes_TextBox.Text.Trim();
         dislikes = view.dislikes_TextBox.Text.Trim();
         string rawBirthMonth = view.bdayMonth_TextBox.Text.Trim();
         string rawBirthDay = view.bdayDay_TextBox.Text.Trim();

         // Validate user input
         List<string> errorLines = new List<string>();

         if (!Friend.IsValidName(name))
            errorLines.Add("Require name");
         if (!Friend.IsValidLikes(likes))
            errorLines.Add("Require likes");
         if (!Friend.IsValidDislikes(dislikes))
            errorLines.Add("Require dislikes");

         if (!Int32.TryParse(rawBirthMonth, out birthMonth) ||
            !Friend.IsValidBirthMonth(birthMonth))
            errorLines.Add("Require birth month");

         if (!Int32.TryParse(rawBirthDay, out birthDay) ||
            !Friend.IsValidBirthDay(birthDay, birthMonth))
            errorLines.Add("Require birth date");

         // Ensure validation was successful (Validate the validation...)
         if (errorLines.Count > 0)
         {
            // Display error to user
            string message = $"ERROR:\n{String.Join('\n', errorLines)}";
            MessageBox.Show(message, "ERROR(S) FOUND!", MessageBoxButtons.OK,
               MessageBoxIcon.Exclamation);
            // Return as there is no point continuing
            return false;
         }

         // Return true for success
         return true;
      }

      /**********************************************************/
      // Method:  private bool EnsureFriendIsSelected (out Friend friend)
      // Purpose: Ensure a friend is currently selected and
      //          populates 'friend' with the selected friend.
      //          In the case that no friend is selected
      //          an error is displayed to the user.
      // Returns: true if there is a friend selected
      // Returns: false if there is NOT a friend selected
      // Inputs:  out Friend friend
      // Outputs: bool
      /**********************************************************/
      private bool EnsureFriendIsSelected(out Friend friend)
      {
         // Ensure a friend is selected
         friend = model.FriendList.SelectedFriend;
         if (friend == null)
         {
            // Display error to user
            string message = "ERROR:\n"
            + "No Friend is selected.\n"
            + "Please select a friend and try again.";
            MessageBox.Show(message, "ERROR(S) FOUND!", MessageBoxButtons.OK,
               MessageBoxIcon.Exclamation);
            // Return false for error
            return false;
         }

         // Return true for success
         return true;
      }
   }
}