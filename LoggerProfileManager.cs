using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Resources;
using System.Data.Odbc;

namespace HydraPlot2
{
    public class LoggerProfileManager
    {
        XmlDocument profiles;
        const string PROFILE_PATH = "config\\Profiles.xml";

        public LoggerProfileManager() {
            try
            {
                profiles = new XmlDocument();
                if (!System.IO.File.Exists(PROFILE_PATH))
                {
                    System.IO.FileStream f = System.IO.File.Create(PROFILE_PATH);
                    System.IO.StreamWriter w = new System.IO.StreamWriter(f);
                    w.WriteLine("<!DOCTYPE Profiles [ <!ELEMENT Profiles ANY> <!ELEMENT Profile ANY> <!ATTLIST Profile id ID #REQUIRED> ]>");
                    w.WriteLine("<Profiles></Profiles>");
                    w.Close();

                }

                profiles.Load(PROFILE_PATH);
            }
            catch (XmlException xmlEx)
            {

                System.Windows.Forms.MessageBox.Show(Program.GetString("LoggerProfileManagerLoadXMLError") + ": " + xmlEx.Message, 
                    "Logger Profile Manager Error",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public LoggerProfile GetLoggerProfile(string loggerId) 
        {
            List<string> w = new List<string>(0);
            List<string> s = new List<string>(0);
            List<string> t = new List<string>(0);

            XmlElement xmlProfile = profiles.GetElementById(loggerId);
            if (xmlProfile != null)
            {
                loadSensorProfile(xmlProfile, w, s, t);
                return new LoggerProfile(loggerId, w.ToArray(), s.ToArray(), t.ToArray());

            }
            else
            {
                return null;
            }
        }

        private void loadSensorProfile(XmlElement xmlProfile, 
                                       List<string> w,
                                       List<string> s,
                                       List<string> t)
        {
            XmlNodeList waterSensors = xmlProfile.GetElementsByTagName("WaterSensor");
            foreach (XmlElement e in waterSensors)
            {
                w.Add(e.GetAttribute("id"));
            }

            XmlNodeList salinitySensors = xmlProfile.GetElementsByTagName("SalinitySensor");
            foreach (XmlElement e in salinitySensors)
            {
                s.Add(e.GetAttribute("id"));
            }

            XmlNodeList tempSensors = xmlProfile.GetElementsByTagName("TempSensor");
            foreach (XmlElement e in tempSensors)
            {
                t.Add(e.GetAttribute("id"));
            }

        }
        public void SaveLoggerProfile(LoggerProfile loggerProfile)
        {
            Database db = new Database();
            db.OpenFullPath(loggerProfile.DataBasePath);
            string cmdStr;

            foreach (string waterSensor in loggerProfile.GetWaterSensors())
            {
                cmdStr = "INSERT INTO profile VALUES ('" + waterSensor + "' , 0)";
                db.ExecuteCommand(cmdStr);

            }

            foreach (string salinitySensor in loggerProfile.GetSalinitySensors())
            {
                cmdStr = "INSERT INTO profile VALUES ('" + salinitySensor + "' , 1)";
                db.ExecuteCommand(cmdStr);
            }

            foreach (string tempSensor in loggerProfile.GetTempSensors())
            {
                cmdStr = "INSERT INTO profile VALUES ('" + tempSensor + "' , 2)";
                db.ExecuteCommand(cmdStr);
            }

            db.Close();


            /* OLD XML STUFF
            XmlElement xmlProfile = profiles.GetElementById(loggerProfile.LoggerID);
            if (xmlProfile == null)
            {
                xmlProfile = profiles.CreateElement("Profile");
                xmlProfile.SetAttribute("id", loggerProfile.LoggerID);
                profiles.DocumentElement.AppendChild(xmlProfile);
            }

            xmlProfile.RemoveAll();
            xmlProfile.SetAttribute("id", loggerProfile.LoggerID);   

                foreach (string waterSensor in loggerProfile.GetWaterSensors())
                {
                    XmlElement e = profiles.CreateElement("WaterSensor");
                    e.SetAttribute("id", waterSensor);
                    xmlProfile.AppendChild(e);
                }

                foreach (string salinitySensor in loggerProfile.GetSalinitySensors())
                {
                    XmlElement e = profiles.CreateElement("SalinitySensor");
                    e.SetAttribute("id", salinitySensor);
                    xmlProfile.AppendChild(e);
                }

                foreach (string tempSensor in loggerProfile.GetTempSensors())
                {
                    XmlElement e = profiles.CreateElement("TempSensor");
                    e.SetAttribute("id", tempSensor);
                    xmlProfile.AppendChild(e);
                }

                profiles.Save(PROFILE_PATH);

                //List<string> w = new List<string>(0);
                //List<string> s = new List<string>(0);
                //List<string> t = new List<string>(0);

                //loadSensorProfile(xmlProfile, w, s, t);
                //loggerProfile.SetWaterSensors(w.ToArray());
                //loggerProfile.SetSalinitySensors(s.ToArray());
                //loggerProfile.SetTempSensors(t.ToArray());

                */
            
        }

        public LoggerProfile CreateNewLoggerProfile(string loggerId)
        {
            List<string> w = new List<string>(0);
            List<string> s = new List<string>(0);
            List<string> t = new List<string>(0);

            LoggerProfile profile = new LoggerProfile(loggerId, w.ToArray(), s.ToArray(), t.ToArray());
            return profile;
        }




        internal LoggerProfile GetLoggerProfileFromDatabase(string databasePath)
        {
            Database db = new Database();
            db.OpenFullPath(databasePath);
            LoggerProfile profile = db.GetLoggerProfile();
            db.Close();
            return profile;
        }
    }
}
