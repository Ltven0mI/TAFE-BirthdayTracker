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
         view.DisplayFriendList(model.FriendList);
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
         };
         // END - Search - Button - Click //
      }
   }
}