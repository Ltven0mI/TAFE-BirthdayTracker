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
      }


      private void RegisterCallbacks()
      {
         // * Exit - Button - Click * //
         view.exit_Button.Click += (object sender, EventArgs args) => 
         {
            Application.Exit();
         };
         // END - Exit - Button - Click //
      }
   }
}