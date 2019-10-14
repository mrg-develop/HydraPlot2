using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CodersLab.Windows.Controls;

// FIXME
using System.Globalization;
using System.Threading;
using System.IO;

namespace HydraPlot2
{
    public partial class Form1 : Form
    {
        private Panel2Manager panel2Manager;
        private int splitterDistance;

        private Boolean browserHidden = false;

        private StackedStyles currentStackedStyle = StackedStyles.CommonY;

        private string assemblyTitleText;

        private ToolStripManager tsm;

        public ToolStrip GetToolStrip
        {
            get
            {
                return toolStrip3;
            }
        }

        public Form1() {
            new Form1(null);
        }
        public Form1(Browser browser)
        {
            // FIXME
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");

            if (browser != null)
            {
                browser.CatalogUpdated += new CatalogUpdatedEventHandler(browser_CatalogUpdated);
            }

            InitializeComponent();
            //tabControl3.Hide();

            //Form2 f = new Form2();
            //f.Show();
            //profileEditorLink1 = new LinkLabel();
            
            //profileEditorLink1.Top = 10;
            //profileEditorLink1.Click += new EventHandler(profileEditorLink1_Click);
            //profileEditorLink1.Text = Program.GetString("ProfileEditorLink");
            panel2Manager = new Panel2Manager(panel2);
            panel2Manager.GraphWorkspaceLoaded += new Panel2ManagerEvent(panel2Manager_GraphWorkspaceLoaded);
            panel2Manager.GraphWorkspaceDisposed += new Panel2ManagerEvent(panel2Manager_GraphWorkspaceDisposed);
            panel2Manager.GraphWorkspaceLayoutEvent += new Panel2ManagerEvent(panel2Manager_GraphWorkspaceLayoutEvent);
            panel2Manager.GraphWorkspaceTabChanged += new Panel2ManagerEvent(panel2Manager_GraphWorkspaceTabChanged);
            splitterDistance = 209;
            HideBrowser();
            System.Reflection.AssemblyTitleAttribute assemblyTitle = (System.Reflection.AssemblyTitleAttribute)Attribute.GetCustomAttribute(System.Reflection.Assembly.GetExecutingAssembly(),typeof(System.Reflection.AssemblyTitleAttribute));
            assemblyTitleText = assemblyTitle.Title;
            this.Text = assemblyTitleText;

            this.Width = 1024;
            this.Height = 600;
            tsm = new ToolStripManager(this);
            tsm.SetClosedState();
                                                                        
        }

        void panel2Manager_GraphWorkspaceTabChanged(Panel2Manager sender, EventArgs e)
        {
            GraphView gv = sender.GetWorkspace().GetActiveStackedGraphView();

            if (gv != null)
            {
                if (((StackedGraphView)gv).Style == StackedStyles.Stacked)
                {
                    apilarToolStripMenuItem.Checked = true;
                }
                else
                {
                    apilarToolStripMenuItem.Checked = false;
                }
            }
        }

        void panel2Manager_GraphWorkspaceLayoutEvent(Panel2Manager sender, EventArgs e)
        {
            if (sender.GetWorkspace().IsEtoHidden)
            {
                //mostrarEToToolStripMenuItem.Checked = false;
                mostrarEToIPPToolStripMenuItem.Checked = false;
                mostrarEToIPPToolStripMenuItem.Text = "Mostrar panel ETo/IPP";
                mostrarIPPToolStripMenuItem.Enabled = false;
            }
            else
            {
                //mostrarEToToolStripMenuItem.Checked = true;
                mostrarEToIPPToolStripMenuItem.Text = "Mostrar panel ETo/IPP";
                mostrarEToIPPToolStripMenuItem.Checked = true;
                mostrarIPPToolStripMenuItem.Enabled = true;
            }

            if (sender.GetWorkspace().isShowIPPInEtoGraph)
            {
                mostrarIPPToolStripMenuItem.Checked = true;
            }
            else
            {
                mostrarIPPToolStripMenuItem.Checked = false;
            }

            if (sender.GetWorkspace().GetActiveStackedGraphView() != null)
            {
                independienteToolStripMenuItem.Enabled = true;
                sumadoToolStripMenuItem.Enabled = true;
                if (((StackedGraphView)(sender.GetWorkspace().GetActiveStackedGraphView())).Style == StackedStyles.Stacked)
                {
                    apilarToolStripMenuItem.Checked = true;
                }
                else
                {
                    apilarToolStripMenuItem.Checked = false;
                }
            }
            else
            {
                independienteToolStripMenuItem.Enabled = false;
                sumadoToolStripMenuItem.Enabled = false;
            }

            /*
            if (sender.GetWorkspace().IsIPPHidden)
            {
                mostrarToolStripMenuItem.Checked = false;
            }
            else
            {
                mostrarToolStripMenuItem.Checked = true;
            }*/
            
        }

        void panel2Manager_GraphWorkspaceDisposed(Panel2Manager sender, EventArgs e)
        {
            GraphWorkspace ws = sender.GetWorkspace();
            cerrarBaseDeDatosToolStripMenuItem.Enabled = false;
            actualizarBaseDeDatosToolStripMenuItem.Enabled = true;
            gráficoToolStripMenuItem.Enabled = false;
            //panel2Manager.GetWorkspace().Database.Close();
        }

        void panel2Manager_GraphWorkspaceLoaded(Panel2Manager sender, EventArgs e)
        {
            cerrarBaseDeDatosToolStripMenuItem.Enabled = true;
            actualizarBaseDeDatosToolStripMenuItem.Enabled = true;
            gráficoToolStripMenuItem.Enabled = true;
        }

        void ws_WorkSpaceLayoutEvent(GraphWorkspace sender, EventArgs e)
        {
        }

        void browser_CatalogUpdated(Browser sender)
        {
            browserTreeView.Nodes.Clear();
            TreeNode root = new TreeNode("Loggers");
            root.Name = "root";
            foreach (string dbName in sender.GetDatabaseCatalog())
            {
                TreeNode dbParent = root.Nodes.Add(dbName);
                dbParent.Name = dbName;

                string[] channelsAvailable;
                channelsAvailable = Program.GetChannels(dbName);

                foreach (string chName in channelsAvailable)
                {
                    TreeNode child = new TreeNode(chName);
                    child.Name = chName;
                    child.ContextMenuStrip = browserContextMenu;
                    dbParent.Nodes.Add(child);
                }

            }
            browserTreeView.Nodes.Add(root);

            root.Expand();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //HideBrowser(); 
            updateStyleSelectorButtons();

            cerrarBaseDeDatosToolStripMenuItem.Enabled = false;
            actualizarBaseDeDatosToolStripMenuItem.Enabled = true;
            gráficoToolStripMenuItem.Enabled = false;

        }

        private void updateStyleSelectorButtons()
        {
            CommonYStyleToolStripButton8.Checked = false;
            //IndependentYStyleToolStripButton9.Checked = false;
            MultiYStyleToolStripButton10.Checked = false;

            if (currentStackedStyle == StackedStyles.CommonY)
                CommonYStyleToolStripButton8.Checked = true;
            /*if (currentStackedStyle == StackedStyles.MultiStacked)
                IndependentYStyleToolStripButton9.Checked = true;*/
            if (currentStackedStyle == StackedStyles.Stacked)
                MultiYStyleToolStripButton10.Checked = true;
        }

        public void HideBrowser()
        {
            mainContainer.Panel1Collapsed = true;
            mainContainer.Panel1.Hide();
            browserHidden = true;
            
        }

        public void ShowBrowser()
        {
            mainContainer.Panel1Collapsed = false;
            mainContainer.Panel1.Show();
            browserHidden = false;
        }

        private void hideBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.HideBrowser();
        }

        private void addGraphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddPane();
            
        }

        public void AddPane()
        {
                
        }

        private void mainContainer_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void browserTreeView_MouseUp(object sender, MouseEventArgs e)
        {
            #region FIXME OLD CODE
            /* FIXME: OLD CODE
            if (e.Button == MouseButtons.Right)
            {
                CodersLab.Windows.Controls.TreeView tv = (CodersLab.Windows.Controls.TreeView)sender;
                if (tv.SelectedNodes.Count > 0)
                {
                    if (tv.SelectedNodes[0].
                }
                browserContextMenu.Show(browserTreeView, e.Location);
            
            } */
            #endregion
        }

        private void browserContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (browserTreeView.SelectedNodes.Count == 0)
            {
                e.Cancel = true;
            }
        }

        private void graphToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void newGraphToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (browserTreeView.SelectedNodes.Count > 0)
            {
                
                string[] tmp = new string[browserTreeView.SelectedNodes.Count];

                for (int i = 0; i < browserTreeView.SelectedNodes.Count; i++)
                {
                    tmp[i] = browserTreeView.SelectedNodes[i].Text;
                }

                GraphParams gp = new GraphParams(browserTreeView.SelectedNodes[0].Parent.Name, tmp);

                Program.AddNewGraph(gp);
            }

        }

        public void AddSingleGraph(GraphView gv)
        {
            //workAreaContainer1.AddGraph(gv);
            //workAreaSplitter1.AddGraph(gv);

        }

        private void workAreaContainer1_ControlAdded(object sender, ControlEventArgs e)
        {
            //double rowSize = 100.0 / workAreaContainer1.Rows;

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new About();
            f.ShowDialog();
        }

        private void browserTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level == 0) // root
            {
                panel2Manager.HideGraphWorkspace();
                //tabControl3.Hide();
                
            }

            else if (e.Node.Level == 1) // database
            {
                LoggerProfile profile = Program.GetLoggerProfile(e.Node.Name);
                if (profile == null)
                    profile = Program.GetLoggerProfileManagger().CreateNewLoggerProfile(e.Node.Name);
                panel2Manager.ShowGraphWorkspace(profile);
                //tabControl3.TabPages[0].Controls.Clear();
                //tabControl3.SelectedTab = tabPage2;
                //tabControl3.Hide();

                
            }
            
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // BUGFIX: Necesario para resolver el problema del splitterpanel al
            // maximizar la pantalla. Si se mueve el splitter hacia +-1 px los componentes
            // que contiene se redimensionan correctamente (al parecer el problema es con
            // el TabControl
            //mainContainer.SplitterDistance = mainContainer.SplitterDistance - 1;
            //mainContainer.SplitterDistance = mainContainer.SplitterDistance +1 ;
            if (mainContainer.Width > 190)
                mainContainer.SplitterDistance = 190;
            //Console.WriteLine("Form Resized");
        }

        private void mainContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            //Console.WriteLine(mainContainer.SplitterDistance);
        }

        private void importFromDatToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnShowHideBrowser_Click(object sender, EventArgs e)
        {
            if (browserHidden)
                ShowBrowser();
            else
                HideBrowser();
            
        }

        private void EjeYComunMenuItem_Click(object sender, EventArgs e)
        {
            
           
        }

        private void archivoToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void CommonYStyleToolStripButton8_Click(object sender, EventArgs e)
        {
            ToolStripButton b = (ToolStripButton)sender;
            if (!b.Checked)
            {
                stackCommonYGraph();
            }
        }

        private void stackCommonYGraph()
        {
            currentStackedStyle = StackedStyles.Stacked;
            GraphWorkspace ws = panel2Manager.GetWorkspace();
            ws.ChangeStackedGraphStyle(StackedStyles.Stacked);
            updateStyleSelectorButtons();
            

        }

        private void IndependentYStyleToolStripButton9_Click(object sender, EventArgs e)
        {
            ToolStripButton b = (ToolStripButton)sender;
            if (!b.Checked)
            {
                currentStackedStyle = StackedStyles.MultiStacked;
                GraphWorkspace ws = panel2Manager.GetWorkspace();
                ws.ChangeStackedGraphStyle(StackedStyles.MultiStacked);
                updateStyleSelectorButtons();
            }

        }

        private void MultiYStyleToolStripButton10_Click(object sender, EventArgs e)
        {
            ToolStripButton b = (ToolStripButton)sender;
            if (!b.Checked)
            {
                currentStackedStyle = StackedStyles.Stacked;
                GraphWorkspace ws = panel2Manager.GetWorkspace();
                ws.ChangeStackedGraphStyle(StackedStyles.Stacked);
                updateStyleSelectorButtons();
            }
        }


        internal void OpenDatabase()
        {

            OpenFileDialog f = new OpenFileDialog();
            f.Filter = "Bases de datos | *.mdb";
            DialogResult r = f.ShowDialog();
            if (r.Equals(DialogResult.OK))
            {
                string databaseName = Path.GetFileNameWithoutExtension(f.FileName);
                string databaseCompletePath = f.FileName;
                LoggerProfile profile = Program.GetLoggerProfileFromDatabase(databaseCompletePath);
                
                if (profile == null)
                    profile = Program.GetLoggerProfileManagger().CreateNewLoggerProfile(databaseName);
                panel2Manager.ShowGraphWorkspace(profile);
                tsm.SetOpenedState();
            }
        }

        private void abrirBaseDeDatosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.OpenDatabase();
        }

        private void crearBaseDeDatosToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFileDialog s = new SaveFileDialog();
            DialogResult rs = s.ShowDialog();
            Console.WriteLine(s.FileName);
        }

        private void actualizarBaseDeDatosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool newDatabase = false;
            DialogResult r;

            SaveFileDialog s = new SaveFileDialog();
            s.Title = "Seleccione base de datos de destino";
            s.Filter = "Bases de datos | *.mdb";
            r = s.ShowDialog();

            if (r == DialogResult.OK)
            {
                if (!System.IO.File.Exists(s.FileName))
                {
                    r = MessageBox.Show("El archivo no existe ¿Desea crearlo?", "Abrir base de datos", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (r == DialogResult.Yes)
                    {
                        // crear nueva db
                        string appPath = Path.GetDirectoryName(Application.ExecutablePath);
                        System.IO.File.Copy(appPath + "\\databases\\template.mdb", s.FileName);
                        newDatabase = true;
                        

                    }
                    else
                    {
                        // pa la casa
                        return;
                    }
                }


                // IMPORTAR
                OpenFileDialog f = new OpenFileDialog();
                f.Title = "Seleccione archivos a importar";
                f.Filter = "Archivos DAT DotLogger|*.dat|Archivos TXT|*.txt";
                f.Multiselect = true;
                r = f.ShowDialog();
                string selectedProbeId = null;

                if (r == DialogResult.OK)
                {
                    
                    if (newDatabase)
                    {
                        // hacer catalogo de bases de datos disponibles 
                        List<string> catalog = new List<string>(0);
                        string line;
                        string probe_id;
                        foreach (string filename in f.FileNames)
                        {
                            TextReader t = File.OpenText(filename);
                            while  ((line = t.ReadLine()) != null) {
                                probe_id = line.Substring(0, 20).Trim(); // saca el id desde el dat
                                if (!catalog.Contains(probe_id))
                                {
                                    catalog.Add(probe_id);
                                }
                            }
                            t.Close();
                        }

                        // mostrar catálogo y seleccionar logger
                        DbSelector dbselector = new DbSelector(catalog.ToArray());
                        dbselector.ShowDialog();
                        selectedProbeId = dbselector.SelectedProbeId;
                        Database db = new Database();
                        db.OpenFullPath(s.FileName);
                        DatabaseInfo dbi = db.GetDbInfo();
                        dbi.ProbeId = selectedProbeId;
                        db.SetDbInfo(dbi);
                        db.Close();

                    }

                    // importar stuff
                    Database d = new Database();
                    d.OpenFullPath(s.FileName);
                    if (!newDatabase)
                    {
                        selectedProbeId = d.GetDbInfo().ProbeId;
                    }
                    
                    
                    Importer importer = new Importer(d);
                    ImporterMonitor monitor = new ImporterMonitor();
                    ImporterWorker iw = new ImporterWorker(f.FileNames,selectedProbeId, importer, monitor);
                    iw.RunWorkerAsync();
                    monitor.SetRunningMode();
                    monitor.ShowDialog();
                    //importer.ImportMultipleDatFiles(f.FileNames);
                    d.Close();

                    string databaseName = Path.GetFileNameWithoutExtension(s.FileName);
                    string databaseCompletePath = s.FileName;
                    LoggerProfile profile = Program.GetLoggerProfileFromDatabase(databaseCompletePath);

                    if (profile == null)
                        profile = Program.GetLoggerProfileManagger().CreateNewLoggerProfile(databaseName);
                    panel2Manager.ShowGraphWorkspace(profile);

                }
            }



        }

        internal void SetOpenedDatabase(string p)
        {
            this.Text = assemblyTitleText + " - " + p;
            
        }

        private void cerrarBaseDeDatosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel2Manager.HideGraphWorkspace();
            this.Text = assemblyTitleText;
        }

        private void mostrarEToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // OLD
            if (panel2Manager.GetWorkspace().IsEtoHidden)
            {
                panel2Manager.GetWorkspace().ShowEto();
                //mostrarEToToolStripMenuItem.Checked = true;
            }
            else
            {
                panel2Manager.GetWorkspace().HideEto();
                //mostrarEToToolStripMenuItem.Checked = false;
            }
        }

        private void SwitchEtoIPPPanel()
        {
            if (panel2Manager.GetWorkspace().IsEtoHidden)
            {
                panel2Manager.GetWorkspace().ShowEto();
                tsm.SetETOIPPPanelActive();
            }
            else
            {
                panel2Manager.GetWorkspace().HideEto();
                tsm.SetETOIPPPanelInactive();
            }
        }

        private void mostrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // OLD
            if (panel2Manager.GetWorkspace().IsIPPHidden)
            {
                panel2Manager.GetWorkspace().ShowIPP();
                //mostrarToolStripMenuItem.Checked = true;
            }
            else
            {
                panel2Manager.GetWorkspace().HideIPP();
                //mostrarToolStripMenuItem.Checked = false;
            }

        }

        private void distribuirEspaciosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel2Manager.GetWorkspace().OrganizeActiveGraphGraphView();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About aboutBox = new About();
            aboutBox.ShowDialog();
        }

        private void apilarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((StackedGraphView)(panel2Manager.GetWorkspace().GetActiveStackedGraphView())).Style == StackedStyles.CommonY)
            {
                stackCommonYGraph();
            }
            else
            {
                unstackAndShowCommonYGraph();
            }
        }

        private void unstackAndShowCommonYGraph()
        {
            currentStackedStyle = StackedStyles.CommonY;
            GraphWorkspace ws = panel2Manager.GetWorkspace();
            ws.ChangeStackedGraphStyle(StackedStyles.CommonY);
            updateStyleSelectorButtons();
        }

        private void seleccionarSensoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Sensor> sensorList = panel2Manager.GetWorkspace().GetActiveStackedGraphView().SensorList;
            SensorSelector selector = new SensorSelector(ref sensorList);
            selector.ShowDialog();
            
            panel2Manager.GetWorkspace().GetActiveStackedGraphView().Refresh();
            panel2Manager.GetWorkspace().ResynchAllXAxisInWorkspace();
            
            
        }

        private void seleccioanrSensoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Sensor> sensorList = panel2Manager.GetWorkspace().GetActiveSummedGraphView().SensorList;
            SensorSelector selector = new SensorSelector(ref sensorList);
            selector.ShowDialog();
            panel2Manager.GetWorkspace().GetActiveSummedGraphView().Refresh();
        }

        private void mostrarSumadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GraphWorkspace ws = panel2Manager.GetWorkspace();
            ws.ShowSummedWaterSalinityGraphs();

        }

        private void mostrarApiladosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GraphWorkspace ws = panel2Manager.GetWorkspace();
            ws.ShowStackedWaterSalinityGraphs();

        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel2Manager.Exit();
            Program.Exit();
        }

        private void mostrarEToIPPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwitchEtoIPPPanel();
        }

        private void mostrarIPPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (panel2Manager.GetWorkspace().isShowIPPInEtoGraph)
            {
                panel2Manager.GetWorkspace().HideIPPInEtoGraph();
            }
            else
            {
                panel2Manager.GetWorkspace().ShowIPPInEtoGraph();
            }
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            abrirBaseDeDatosToolStripMenuItem.PerformClick();
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            actualizarBaseDeDatosToolStripMenuItem.PerformClick();
        }

        private void apilarButton_Click(object sender, EventArgs e)
        {
            apilarToolStripMenuItem.PerformClick();
        }

        private void mostrarETO_Click(object sender, EventArgs e)
        {
            mostrarEToIPPToolStripMenuItem.PerformClick();
        }

        private void mostrarIPP_Click(object sender, EventArgs e)
        {
            mostrarIPPToolStripMenuItem.PerformClick();
        }
    }
}
