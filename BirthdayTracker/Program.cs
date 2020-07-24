using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BirthdayTracker
{
    static class Program
    {
        public static Model MODEL { get; private set; }
        public static View VIEW { get; private set; }
        public static Controller CONTROLLER { get; private set; }
        public static BirthdayTrackerGUI GUI { get; private set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            GUI = new BirthdayTrackerGUI();

            MODEL = new Model();
            VIEW = new View(
                GUI.personsName_Textbox,
                GUI.likes_Textbox,
                GUI.dislikes_Textbox,
                GUI.bdayDay_Textbox,
                GUI.bdayMonth_Textbox,
                GUI.find_Textbox,
                GUI.find_Button,
                GUI.new_Button,
                GUI.update_Button,
                GUI.delete_Button,
                GUI.navFirst_Button,
                GUI.navPrev_Button,
                GUI.navNext_Button,
                GUI.navLast_Button,
                GUI.birthdayList_Textbox,
                GUI.search_Button,
                GUI.search_Textbox,
                GUI.exit_Button
            );
            CONTROLLER = new Controller(MODEL, VIEW);

            Application.Run(GUI);

        }
    }
}
