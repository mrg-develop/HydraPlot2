using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HydraPlot2
{
    static class Program
    {
        private static Form1 mainForm;
        private static Browser browser;
        private static LoggerProfileManager profileManager;
        private static System.Resources.ResourceManager resourceManager;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            resourceManager = new System.Resources.ResourceManager("Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
            browser = new Browser();
            profileManager = new LoggerProfileManager();
            
            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainForm = new Form1(browser);

            //browser.Refresh();

            Application.Run(mainForm);
        }

        public static void HideBrowser()
        {
            mainForm.HideBrowser();
        }

        public static void AddNewGraph(GraphParams gp)
        {
            Database db = new Database();
            db.Open(gp.DatabaseName);
            GraphData g = new GraphData(db, gp.ChannelNames, GraphData.GraphDataTypes.UNDEFINED);
            SingleGraphView gv = new SingleGraphView(g);
            mainForm.AddSingleGraph(gv);
            db.Close();
        }

        public static string GetString(string name)
        {
            return Properties.Resources.ResourceManager.GetString(name, System.Threading.Thread.CurrentThread.CurrentUICulture);
        }

        public static LoggerProfile GetLoggerProfile(string loggerId)
        {
            return profileManager.GetLoggerProfile(loggerId);
        }

        public static LoggerProfile GetLoggerProfileFromDatabase(string databasePath)
        {
            return profileManager.GetLoggerProfileFromDatabase(databasePath);
        }

        internal static void ShowProfileEditor(LoggerProfile profile)
        {
            ProfileEditor p = new ProfileEditor(profile);
            p.ShowDialog();
        }

        public static string[] GetChannels(string loggerId)
        {
            List<string> channels = new List<string>(0);
            Database d = new Database();
            d.Open(loggerId);
            foreach (string chName in d.ChannelsAvailable)
            {
                channels.Add(chName);
            }
            d.Close();

            return channels.ToArray();

        }

        public static LoggerProfileManager GetLoggerProfileManagger()
        {
            return profileManager;
        }

        private static bool profileSensorsChanged = true;

        public static bool ProfileSensorsChanged {
            get
            {
                return profileSensorsChanged;
            }

            set
            {
                profileSensorsChanged = value;
            }
        }

        public static void OpenDatabase()
        {
            mainForm.OpenDatabase();
        }

        internal static void SetMainFormTitle(string p)
        {
            mainForm.SetOpenedDatabase(p);
        }

        internal static string[] GetChannels(LoggerProfile profile)
        {
            List<string> channels = new List<string>(0);
            Database d = new Database();
            d.OpenFullPath(profile.DataBasePath);
            foreach (string chName in d.ChannelsAvailable)
            {
                channels.Add(chName);
            }
            d.Close();

            return channels.ToArray();
        }

        internal static void Exit()
        {
            mainForm.Close();
            Application.Exit();
        }
    }

}
