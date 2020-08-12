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
   // Version: 0.1.0
   // Date: 07-Aug-2020
   // Tests: N/A
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

         model.ReloadFriendData();
         model.UpdateSearchResults();
         view.DisplayFriendList(model.FriendList, model.SelectedFriend);
         view.DisplaySearchMonth(model.SelectedSearchMonth);

         view.DisplaySelectedFriendData(null);
      }

      private void RegisterCallbacks()
      {
         // * FriendList - Changed * //
         model.FriendListChanged += (object sender, ReadOnlyCollection<Friend> friendList) =>
         {
            // Display the SelectedFriend data only...
            // if it exists in the current FriendList
            if (model.SelectedFriend != null)
               view.DisplaySelectedFriendData(friendList.First(c => c == model.SelectedFriend));

            // Display the changed FriendList
            view.DisplayFriendList(friendList, model.SelectedFriend);
         };
         // END - FriendList - Changed //

         // * SelectedFriend - Changed * //
         model.SelectedFriendChanged += (object sender, Friend selectedFriend) =>
         {
            // Re-display FriendList to reflect the new SelectedFriend
            view.DisplayFriendList(model.FriendList, selectedFriend);

            // Display the data of the changed SelectedFriend
            view.DisplaySelectedFriendData(selectedFriend);
         };
         // END - SelectedFriend - Changed //

         // * Exit - Button - Click * //
         view.exit_Button.Click += (object sender, EventArgs args) => 
         {
            Application.Exit();
         };
         // END - Exit - Button - Click //

         // * Search - Button - Click * //
         view.search_Button.Click += (object sender, EventArgs args) =>
         {
            model.NextSearchMonth();
            view.DisplaySearchMonth(model.SelectedSearchMonth);
         };
         // END - Search - Button - Click //

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
            Friend selectedFriend = model.FindFriend(queryString);
            model.SetSelectedFriend(selectedFriend);

            // If no friend was found: Display message to user
            if (selectedFriend == null)
            {
               string msg = String.Format("Sorry, no name found for '{0}'", queryString);
               MessageBox.Show(msg, "NOT FOUND", MessageBoxButtons.OK);
            }
         };
         // END - Find - Button - Click //
      }
   }
}