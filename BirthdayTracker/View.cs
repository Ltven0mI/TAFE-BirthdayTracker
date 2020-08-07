using System.Linq;
using System.Collections.Generic;
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
      public readonly int[] BIRTHDAYLIST_COLUMN_SIZES = {12, 16, 16, 7, 7};
      public readonly string[] BIRTHDAYLIST_COLUMN_HEADERS = {"Person", "Likes", "Dislikes", "Day", "Month"};
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

      public void DisplayFriendList(ReadOnlyCollection<Friend> friendList, Friend selectedFriend=null)
      {
         // Initialize Lines List
         List<string> lines = new List<string>();

         // Construct header
         string header = "";
         for (int i=0; i<BIRTHDAYLIST_COLUMN_HEADERS.Length; i++)
            header += BIRTHDAYLIST_COLUMN_HEADERS[i].PadRight(BIRTHDAYLIST_COLUMN_SIZES[i]);
         
         // Insert header into Lines list
         lines.Add(header);

         // Insert horizontal seperator into Lines list
         lines.Add(new String('-', BIRTHDAYLIST_COLUMN_SIZES.Sum()));

         // Iterate over all friends in passed collection
         for (int i=0; i<friendList.Count; i++)
         {
            // Get current friend
            Friend friend = friendList[i];

            // Use '_' as padding if selected row, otherwise use ' ' (whitespace)
            char paddingChar = (friend == selectedFriend) ? '_' : ' ';

            // Construct friend line
            string line = friend.Name.PadRight(BIRTHDAYLIST_COLUMN_SIZES[0], paddingChar)
            + friend.Likes.PadRight(BIRTHDAYLIST_COLUMN_SIZES[1], paddingChar)
            + friend.Dislikes.PadRight(BIRTHDAYLIST_COLUMN_SIZES[2], paddingChar)
            + friend.BDayDay.ToString().PadRight(BIRTHDAYLIST_COLUMN_SIZES[3], paddingChar)
            + friend.BDayMonth.ToString().PadRight(BIRTHDAYLIST_COLUMN_SIZES[4], paddingChar);

            // Add friend line to list
            lines.Add(line);
         }

         if (friendList.Count <= 0)
            lines.Add("No Friends to Display.");

         // Update BirthdayList TextBox Text
         birthdayList_TextBox.Text = String.Join("\r\n", lines);
      }

      public void DisplaySearchMonth(Model.SearchMonth searchMonth)
      {
         if (searchMonth == Model.SearchMonth.All)
            search_TextBox.Text = searchMonth.ToString();
         else
            search_TextBox.Text = String.Format("{0} - {1}",
               (int)searchMonth, searchMonth);
      }

      public void DisplaySelectedFriendData(Friend friend)
      {
         personsName_TextBox.Text = friend?.Name;
         likes_TextBox.Text = friend?.Likes;
         dislikes_TextBox.Text = friend?.Dislikes;
         bdayDay_TextBox.Text = friend?.BDayDay.ToString();
         bdayMonth_TextBox.Text = friend?.BDayMonth.ToString();
      }
   }
}