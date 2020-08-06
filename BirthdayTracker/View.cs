using System.Text;
using System.Collections.ObjectModel;
using System;
using System.Windows.Forms;

namespace BirthdayTracker
{
   public class View
   {
      // * PersonsName - TextBox * //
      public TextBox personsName_TextBox { get; private set; }
      // END - PersonsName - TextBox //

      // * Likes - TextBox * //
      public TextBox likes_TextBox { get; private set; }
      // END - Likes - TextBox //

      // * Dislikes - TextBox * //
      public TextBox dislikes_TextBox { get; private set; }
      // END - Dislikes - TextBox //
      
      // * BDayDay - TextBox * //
      public TextBox bdayDay_TextBox { get; private set; }
      // END - BDayDay - TextBox //
      
      // * BDayMonth - TextBox * //
      public TextBox bdayMonth_TextBox { get; private set; }
      // END - BDayMonth - TextBox //
      
      // * Find - TextBox * //
      public TextBox find_TextBox { get; private set; }
      // END - Find - TextBox //
      
      // * Find - Button * //
      public Button find_Button { get; private set; }
      // END - Find - Button //
      
      // * New - Button * //
      public Button new_Button { get; private set; }
      // END - New - Button //

      // * Update - Button * //      
      public Button update_Button { get; private set; }
      // END - Update - Button //

      // * Delete - Button * //
      public Button delete_Button { get; private set; }
      // END - Delete - Button //
      
      // * NavFirst - Button * //
      public Button navFirst_Button { get; private set; }
      // END - NavFirst - Button //

      // * NavPrev - Button * //
      public Button navPrev_Button { get; private set; }
      // END - NavPrev - Button //
      
      // * NavNext - Button * //
      public Button navNext_Button { get; private set; }
      // END - NavNext - Button //
      
      // * NavLast - Button * //
      public Button navLast_Button { get; private set; }
      // END - NavLast - Button //
      
      // * BirthdayList - TextBox * //
      public TextBox birthdayList_TextBox { get; private set; }
      // END - BirthdayList - TextBox //
      
      // * Search - Button * //
      public Button search_Button { get; private set; }
      // END - Search - Button //
      
      // * Search - TextBox * //
      public TextBox search_TextBox { get; private set; }
      // END - Search - TextBox //
      
      // * Exit - Button * //
      public Button exit_Button { get; private set; }
      // END - Exit - Button //


      public View(
         TextBox personsName_TextBox,
         TextBox likes_TextBox,
         TextBox dislikes_TextBox,
         TextBox bdayDay_TextBox,
         TextBox bdayMonth_TextBox,
         TextBox find_TextBox,
         Button find_Button,
         Button new_Button,
         Button update_Button,
         Button delete_Button,
         Button navFirst_Button,
         Button navPrev_Button,
         Button navNext_Button,
         Button navLast_Button,
         TextBox birthdayList_TextBox,
         Button search_Button,
         TextBox search_TextBox,
         Button exit_Button
      )
      {
         this.personsName_TextBox = personsName_TextBox;
         this.likes_TextBox = likes_TextBox;
         this.dislikes_TextBox = dislikes_TextBox;
         this.bdayDay_TextBox = bdayDay_TextBox;
         this.bdayMonth_TextBox = bdayMonth_TextBox;
         this.find_TextBox = find_TextBox;
         this.find_Button = find_Button;
         this.new_Button = new_Button;
         this.update_Button = update_Button;
         this.delete_Button = delete_Button;
         this.navFirst_Button = navFirst_Button;
         this.navPrev_Button = navPrev_Button;
         this.navNext_Button = navNext_Button;
         this.navLast_Button = navLast_Button;
         this.birthdayList_TextBox = birthdayList_TextBox;
         this.search_Button = search_Button;
         this.search_TextBox = search_TextBox;
         this.exit_Button = exit_Button;
      }

      public void DisplayFriendList(ReadOnlyCollection<Friend> friendList, int selectedIndex=-1)
      {
         string[] lines = new string[friendList.Count];
         for (int i=0; i<friendList.Count; i++)
         {
            Friend friend = friendList[i];
            char paddingChar = (i == selectedIndex) ? '_' : ' ';
            lines[i] = friend.Name.PadRight(14, paddingChar)
            + friend.Likes.PadRight(16, paddingChar)
            + friend.Dislikes.PadRight(16, paddingChar)
            + friend.BDayDay.ToString().PadRight(8, paddingChar)
            + friend.BDayMonth.ToString().PadRight(8, paddingChar);
         }
         birthdayList_TextBox.Text = String.Join("\r\n", lines);
      }

      public void DisplaySearchMonth(Model.SearchMonth searchMonth)
      {
         string text = "";
         if (searchMonth == Model.SearchMonth.All)
            text = searchMonth.ToString();
         else
            text = String.Format("{0} - {1}",
               (int)searchMonth, searchMonth);
         search_TextBox.Text = text;
      }
   }
}