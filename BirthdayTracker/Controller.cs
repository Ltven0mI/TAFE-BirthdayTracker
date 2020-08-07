using System;
using System.Windows.Forms;

namespace BirthdayTracker
{
   public class Controller
   {
      private Model model;
      private View view;


      public Controller(Model model, View view)
      {
         this.model = model;
         this.view = view;

         RegisterCallbacks();

         model.ReloadFriendData();
         model.UpdateSearchResults();
         view.DisplayFriendList(model.FriendList, model.SelectedFriend);
         view.DisplaySearchMonth(model.SelectedSearchMonth);
      }


      private void RegisterCallbacks()
      {
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
            view.DisplayFriendList(model.MonthSearchResults, model.SelectedFriend);
         };
         // END - Search - Button - Click //

         // * Find - Button - Click * //
         view.find_Button.Click += (object sender, EventArgs args) =>
         {
            // model.SetSelectedFriend(model.MonthSearchResults[1]);
            Friend selectedFriend = model.FindFriend(view.find_TextBox.Text);
            model.SetSelectedFriend(selectedFriend);
            view.DisplayFriendList(model.MonthSearchResults, model.SelectedFriend);

            if (selectedFriend != null)
               view.DisplaySelectedFriendData(selectedFriend);
            else
               view.ClearSelectedFriendData();
         };
         // END - Find - Button - Click //
      }
   }
}