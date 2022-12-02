using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace HydraPlot2
{
    public delegate void CatalogUpdatedEventHandler(Browser sender);

    public class Browser
    {
        public event CatalogUpdatedEventHandler CatalogUpdated;

        public string[] GetDatabaseCatalog()
        {
            List<string> dbFiles = new List<string>(0);
            DirectoryInfo di = new DirectoryInfo("databases");
            FileInfo[] f = di.GetFiles("*.mdb");

            foreach (FileInfo file in f)
            {
                //if (!file.Name.Equals("template"))
                if (!Path.GetFileNameWithoutExtension(file.Name).Equals("template"))
                    dbFiles.Add(Path.GetFileNameWithoutExtension(file.Name));
                
                
            }

            return dbFiles.ToArray();            
        }

        private void onCatalogUpdated()
        {
            if (CatalogUpdated != null)
            {
                CatalogUpdated(this);
            }
        }

        public void Refresh()
        {
            onCatalogUpdated();
        }
    }
}
