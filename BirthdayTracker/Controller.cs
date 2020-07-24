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
      }


      private void RegisterCallbacks()
      {
         // * Exit - Button - Click * //
         view.exit_Button.Click += ExitButton_Click;
         // END - Exit - Button - Click //
      }

      private void ExitButton_Click(object sender, EventArgs e)
      {
         Application.Exit();
      }
   }
}