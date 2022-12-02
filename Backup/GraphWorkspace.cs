using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace HydraPlot2
{


    public partial class GraphWorkspace : UserControl
    {
        private LoggerProfile profile;
        private bool sensorsChanged;
        private Dictionary<string, bool> sensorsChangedFor;
        private Database database;
        private WaterGraph waterGraph;
        private SalinityGraph salinityGraph;
        private TemperatureGraph temperatureGraph;
        private WaterSalinityGraph waterSalinityGraph;
        private EToGraph etoGraph;
        private IPPGraph ippGraph;
        private EtoIPPGraph etoIPPGraph;

        private SplitBasedWorkAreaSplitter waterSplit;
        private SplitBasedWorkAreaSplitter salinitySplit;
        private SplitBasedWorkAreaSplitter temperatureSplit;
        private SplitBasedWorkAreaSplitter waterSalinitySplit;

        public event WorkSpaceEventHandler WorkSpaceLayoutEvent;
        public event WorkSpaceEventHandler WorkSpaceTabChangedEvent;

        public Database Database
        {
            get { return database; }
        }
        
        private bool showEto;
        private bool showIPP;
        private bool showIPPInEtoGraph;
        

        public bool isShowIPPInEtoGraph
        {
            get { return showIPPInEtoGraph; }
        }

        private void applyChangeEtoIPPGraphDisplayToTab(SplitBasedWorkAreaSplitter split, bool newState)
        {
            EToIPPGraphView gv = (EToIPPGraphView)split.GraphViews[0];
            gv.ShowSecondBar = newState;
            gv.Refresh();
            split.SynchXAxisTo(split.GraphViews[1]);

        }

        private void changeEtoIPPGraphDisplay(bool newState)
        {
            if (waterSplit != null)
            {
                applyChangeEtoIPPGraphDisplayToTab(waterSplit, newState);
            }

            if (salinitySplit != null)
            {
                applyChangeEtoIPPGraphDisplayToTab(salinitySplit, newState);
            }

            if (temperatureSplit != null)
            {
                applyChangeEtoIPPGraphDisplayToTab(temperatureSplit, newState);
            }

            if (waterSalinitySplit != null)
            {
                applyChangeEtoIPPGraphDisplayToTab(waterSalinitySplit, newState);
            }

        }

        public void ShowIPPInEtoGraph()
        {
            changeEtoIPPGraphDisplay(true);
            showIPPInEtoGraph = true;
            onWorkSpaceEvent();
        }

        public void HideIPPInEtoGraph()
        {
            changeEtoIPPGraphDisplay(false);
            showIPPInEtoGraph = false;
            onWorkSpaceEvent();
        }

        private void onWorkSpaceEvent()
        {
            if (WorkSpaceLayoutEvent != null)
            {
                WorkSpaceLayoutEvent(this, new GraphWorkSpaceEventArgs());
            }
        }

        public void ShowEto() {
            showEto = true;
            if (waterSplit != null)
                waterSplit.ShowContainer(0);

            if (salinitySplit != null)
                salinitySplit.ShowContainer(0);

            if (temperatureSplit != null)
                temperatureSplit.ShowContainer(0);

            if (waterSalinitySplit != null)
                waterSalinitySplit.ShowContainer(0);

            onWorkSpaceEvent();
        }

        public void HideEto()
        {
            showEto = false;
            if (waterSplit != null)
                waterSplit.HideContainer(0);

            if (salinitySplit != null)
                salinitySplit.HideContainer(0);

            if (temperatureSplit != null)
                temperatureSplit.HideContainer(0);

            if (waterSalinitySplit != null)
                waterSalinitySplit.HideContainer(0);

            onWorkSpaceEvent();
        }

        public bool IsEtoHidden
        {
            get
            {
                return !showEto;
            }
        }

        public void ShowIPP()
        {
            showIPP = true;

            if (waterSplit != null)
                waterSplit.ShowContainer(1);

            if (salinitySplit != null)
                salinitySplit.ShowContainer(1);

            if (temperatureSplit != null)
                temperatureSplit.ShowContainer(1);

            if (waterSalinitySplit != null)
                waterSalinitySplit.ShowContainer(1);

            onWorkSpaceEvent();
        }

        public void HideIPP()
        {
            showIPP = false;

            if (waterSplit != null)
                waterSplit.HideContainer(1);

            if (salinitySplit != null)
                salinitySplit.HideContainer(1);

            if (temperatureSplit != null)
                temperatureSplit.HideContainer(1);

            if (waterSalinitySplit != null)
                waterSalinitySplit.HideContainer(1);


            onWorkSpaceEvent();
        }

        public bool IsIPPHidden
        {
            get
            {
                return !showIPP;
            }
        }

        public GraphWorkspace(LoggerProfile profile, Database database)
        {
            InitializeComponent();
            this.profile = profile;
            this.database = database;
            sensorsChanged = false;
            waterGraphTab.Tag = "waterGraph";
            salinityGraphTab.Tag = "salinityGraph";
            temperatureGraphTab.Tag = "temperatureGraph";
            waterSalinityGraphTab.Tag = "waterSalinityGraph";
            sensorsChangedFor = new Dictionary<string, bool>();
            sensorsChangedFor["waterGraph"] = false;
            sensorsChangedFor["salinityGraph"] = true;
            sensorsChangedFor["temperatureGraph"] = true;
            sensorsChangedFor["waterSalinityGraph"] = true;
            
        }

        private void redrawTabPage(TabPage tabpage)
        {

        #region waterGraph
            if (tabpage.Tag.Equals("waterGraph"))
            {
                if (profile.GetWaterSensors().Length == 0)
                {
                    // mostrar link editor para profile agua
                    waterGraphTab.Controls.Add(buildNoSensorsForGraphEditorLink(profile));
                }
                else
                {
                    waterGraphTab.Controls.Clear();
                    waterSplit = new SplitBasedWorkAreaSplitter();
                    SplitBasedWorkAreaSplitter split = waterSplit;

                    split.Name = "split";
                    
                    if (!database.isOpen) 
                        database.OpenFullPath(profile.DataBasePath);
                    
                    waterGraph = new WaterGraph(profile, database);
                    WaterGraph wg = waterGraph;

                    if (etoIPPGraph == null)
                        etoIPPGraph = new EtoIPPGraph(profile, database);
                    EtoIPPGraph etoIPPG = etoIPPGraph;
                    GraphView etoIPPGraphView = etoIPPG.GetETOIPPBarGraph();
                    ((EToIPPGraphView)etoIPPGraphView).ShowSecondBar = false;
                    split.AddGraphView(etoIPPGraphView);

                    setupGraphViewsInSplitter(split, wg, waterGraphTab);
                }
            }
        #endregion

        #region salinityGraph
            if (tabpage.Tag.Equals("salinityGraph"))
            {
                if (profile.GetSalinitySensors().Length == 0)
                {
                    // mostrar link editor para profile agua
                    salinityGraphTab.Controls.Add(buildNoSensorsForGraphEditorLink(profile));
                }
                else
                {
                    salinityGraphTab.Controls.Clear();
                    salinitySplit = new SplitBasedWorkAreaSplitter();
                    SplitBasedWorkAreaSplitter split = salinitySplit;

                    split.Name = "split";

                    if (!database.isOpen)
                        database.OpenFullPath(profile.DataBasePath);

                    salinityGraph = new SalinityGraph(profile, database);
                    SalinityGraph sg = salinityGraph;

                    if (etoIPPGraph == null)
                        etoIPPGraph = new EtoIPPGraph(profile, database);
                    EtoIPPGraph etoIPPG = etoIPPGraph;
                    GraphView etoIPPGraphView = etoIPPG.GetETOIPPBarGraph();
                    ((EToIPPGraphView)etoIPPGraphView).ShowSecondBar = false;
                    split.AddGraphView(etoIPPGraphView);

                    setupGraphViewsInSplitter(split, sg, salinityGraphTab);
                }
            }
        #endregion

        #region temperatureGraph
            if (tabpage.Tag.Equals("temperatureGraph"))
            {
                if (profile.GetTempSensors().Length == 0)
                {
                    // mostrar link editor para profile agua
                    temperatureGraphTab.Controls.Add(buildNoSensorsForGraphEditorLink(profile));
                }
                else
                {
                    temperatureGraphTab.Controls.Clear();
                    temperatureSplit = new SplitBasedWorkAreaSplitter();
                    SplitBasedWorkAreaSplitter split = temperatureSplit;

                    split.Name = "split";

                    if (!database.isOpen)
                        database.OpenFullPath(profile.DataBasePath);

                    temperatureGraph = new TemperatureGraph(profile, database);
                    TemperatureGraph tg = temperatureGraph;

                    if (etoIPPGraph == null)
                        etoIPPGraph = new EtoIPPGraph(profile, database);
                    EtoIPPGraph etoIPPG = etoIPPGraph;
                    GraphView etoIPPGraphView = etoIPPG.GetETOIPPBarGraph();
                    ((EToIPPGraphView)etoIPPGraphView).ShowSecondBar = false;
                    split.AddGraphView(etoIPPGraphView);

                    setupGraphViewsInSplitter(split, tg, temperatureGraphTab);
                }
            }
        #endregion

        #region waterSainityGraph
            if (tabpage.Tag.Equals("waterSalinityGraph"))
            {
                /*
                if (profile.GetTempSensors().Length == 0)
                {
                    // mostrar link editor para profile agua
                    temperatureGraphTab.Controls.Add(buildNoSensorsForGraphEditorLink(profile));
                }
                else
                {*/
                    waterSalinityGraphTab.Controls.Clear();
                    // crear controles para gráfico
                    waterSalinitySplit = new SplitBasedWorkAreaSplitter();
                    SplitBasedWorkAreaSplitter split = waterSalinitySplit;
                    split.Name = "split";

                    if (!database.isOpen) 
                        database.OpenFullPath(profile.DataBasePath);

                    waterSalinityGraph = new WaterSalinityGraph(profile, database);
                    WaterSalinityGraph wsg = waterSalinityGraph;

                    if (etoIPPGraph == null)
                        etoIPPGraph = new EtoIPPGraph(profile, database);
                    EtoIPPGraph etoIPPG = etoIPPGraph;
                    GraphView etoIPPGraphView = etoIPPG.GetETOIPPBarGraph();
                    ((EToIPPGraphView)etoIPPGraphView).ShowSecondBar = false;
                    split.AddGraphView(etoIPPGraphView);

                    /*
                    etoGraph = new EToGraph(profile, database);
                    EToGraph etoG = etoGraph;


                    ippGraph = new IPPGraph(profile, database);
                    IPPGraph ippG = ippGraph;
                     * */

                    GraphView waterStackedGraph = wsg.GetWaterStackedGraph(StackedStyles.Stacked);

                    GraphView salinityStackedGraph = wsg.GetSalinityStackedGraph(StackedStyles.Stacked);


                    split.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    split.Dock = DockStyle.Fill;
                    waterSalinityGraphTab.Controls.Add(split);

                    //GraphView etoGraphView = etoG.GetBarGraph();
                    //GraphView ippGraphView = ippG.GetBarGraph();

                    // FIXME: en proceso
                    //((EToGraphView)etoGraphView).BarColor = Color.Gray;
                    //split.AddGraphView(etoGraphView);

                    //this.ShowEto();
                    //showEto = true;

                    //((IPPGraphView)ippGraphView).BarColor = Color.Blue;
                    //split.AddGraphView(ippGraphView);
                    split.AddGraphView(waterStackedGraph);
                    split.AddGraphView(salinityStackedGraph);
                    //this.HideIPP();
                    //showIPP = true;

                    split.SynchXAxisTo(waterStackedGraph);
                    split.AddSynchedVRuler();
                    split.AddSynchedVRuler();

                    //GraphPane stackedZedPane = stackedGraph.GetGraphControl().GraphPane;
                    //GraphPane summedZedPane = summedGraph.GetGraphControl().GraphPane;

                    //this.OrganizeEtoSummedStackedGraphViews();
                //}
            }

        #endregion

            this.ShowEto();
            //this.HideIPP();
            this.OrganizeEtoSummedStackedGraphViews();
        }

        private void setupGraphViewsInSplitter(SplitBasedWorkAreaSplitter split, Graph g, TabPage targetTabPage)
        {
            GraphView summedGraph = g.GetSummedGraph();
            GraphView stackedGraph = g.GetStackedGraph(StackedStyles.CommonY);
            split.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            split.Dock = DockStyle.Fill;
            targetTabPage.Controls.Add(split);
            split.AddGraphView(summedGraph);
            split.AddGraphView(stackedGraph);
            split.SynchXAxisTo(summedGraph);
            split.AddSynchedVRuler();
            split.AddSynchedVRuler();
        }

        private void OrganizeEtoSummedStackedGraphViews()
        {
            SplitBasedWorkAreaSplitter split = null;

            if (tabControl3.SelectedTab.Tag.Equals("waterGraph") && waterSplit != null)
                split = waterSplit;

            if (tabControl3.SelectedTab.Tag.Equals("salinityGraph") && salinitySplit != null)
                split = salinitySplit;

            if (tabControl3.SelectedTab.Tag.Equals("temperatureGraph") && temperatureSplit != null)
                split = temperatureSplit;

            if (tabControl3.SelectedTab.Tag.Equals("waterSalinityGraph") && waterSalinitySplit != null)
                split = waterSalinitySplit;

            if (split != null)
            {
                int containerHeight = getContainerHeight(split);
                int totalHeight = this.getTotalHeight(split);

                split.Containers[0].SplitterDistance = (int)Math.Round(totalHeight * 0.26, 0);
                split.Containers[1].SplitterDistance = (int)Math.Round(totalHeight * 0.37, 0);
                split.Containers[2].SplitterDistance = (int)Math.Round(totalHeight * 0.37, 0);
            }

        }

        private GraphView getStackedGraph(Graph graph, StackedStyles style)
        {
            GraphView stackedGraph = graph.GetStackedGraph(style);
            return stackedGraph;
        }

        private void swapGraphView(SplitBasedWorkAreaSplitter split, GraphView ngv, GraphView ogv)
        {
                            

                ngv.GraphPane.Tag = "new";
                ogv.GraphPane.Tag = "old";
                

                List<VRuler> tempRulers = new List<VRuler>(0);


                

                foreach (VRuler gv1_vruler in ogv.GraphPane.Rulers.VRulers)
                {
                    foreach (List<Ruler> rulers in split.SynchedRulers)
                    {
                        if ((rulers.Find(match => match.Equals(gv1_vruler)) != null))
                        {
                            tempRulers.Add(gv1_vruler);
                        }
                    }
                }

                split.ReplaceGraphView(ngv, ogv);
                for (int i = 0; i < tempRulers.Count; i++)
                {
                    Ruler r = ngv.GraphPane.AddVRuler();
                    r.OnSetPosition += new Ruler.SetPositionEventHanlder(split.r_OnSetPosition);
                    split.SynchedRulers[i].Add(r);
                }
            }

        public void ChangeStackedGraphStyle(StackedStyles style)
        {
            //database.OpenFullPath(profile.DataBasePath);
            TabPage graphTab = tabControl3.SelectedTab;

            #region waterGraph
            if (graphTab.Tag.Equals("waterGraph"))
            {
                SplitBasedWorkAreaSplitter split = (SplitBasedWorkAreaSplitter)(graphTab.Controls["split"]);
                GraphView ngv = getStackedGraph(waterGraph, style);
                GraphView ogv = split.GetGraphView(2);
                swapGraphView(split, ngv, ogv);
                
                
            }
                
            #endregion

            #region salinityGraph
            if (graphTab.Tag.Equals("salinityGraph"))
            {
                SplitBasedWorkAreaSplitter split = (SplitBasedWorkAreaSplitter)(graphTab.Controls["split"]);
                GraphView ngv = getStackedGraph(salinityGraph, style);
                GraphView ogv = split.GetGraphView(2);
                swapGraphView(split, ngv, ogv);
            }

            #endregion

            #region temperatureGraph
            if (graphTab.Tag.Equals("temperatureGraph"))
            {
                SplitBasedWorkAreaSplitter split = (SplitBasedWorkAreaSplitter)(graphTab.Controls["split"]);
                GraphView ngv = getStackedGraph(temperatureGraph, style);
                GraphView ogv = split.GetGraphView(2);
                swapGraphView(split, ngv, ogv);
            }

            #endregion
            onWorkSpaceEvent();
            //database.Close();
            
        }

  

        private void tabControl3_Selected(object sender, TabControlEventArgs e)
        {
            // FIXME: revisar este método

            //if (Program.ProfileSensorsChanged) // redibujar sólo si los sensores han cambiado
            //{
                // primero, verificar si hay sensores para cada gráfico
                if (e.TabPage.Name == "waterGraphTab" && sensorsChangedFor["waterGraph"])
                {
                    redrawTabPage(waterGraphTab);
                    sensorsChangedFor["waterGraph"] = false;
                }

                if (e.TabPage.Name == "salinityGraphTab" && sensorsChangedFor["salinityGraph"])
                {
                    redrawTabPage(salinityGraphTab);
                    sensorsChangedFor["salinityGraph"] = false;
                }

                if (e.TabPage.Name == "temperatureGraphTab" &&  sensorsChangedFor["temperatureGraph"])
                {
                    redrawTabPage(temperatureGraphTab);
                    sensorsChangedFor["temperatureGraph"] = false;
                }

                if (e.TabPage.Name == "waterSalinityGraphTab" && sensorsChangedFor["waterSalinityGraph"])
                {
                    redrawTabPage(waterSalinityGraphTab);
                    sensorsChangedFor["waterSalinityGraph"] = false;
                }

                Program.ProfileSensorsChanged = false;
            //}
        }

        private Panel buildNoSensorsForGraphEditorLink(LoggerProfile profile)
        {
            FlowLayoutPanel container = new FlowLayoutPanel();
            System.Windows.Forms.Label noSensorsText = new System.Windows.Forms.Label();
            Button openEditorButton = new Button();
            openEditorButton.Tag = profile;
            openEditorButton.Click += new EventHandler(openEditorButton_Click);
            openEditorButton.Text = Program.GetString("ProfileEditorLink");
            noSensorsText.Text = Program.GetString("ProfileNoSensorsForGraph") + profile.LoggerID;
            noSensorsText.AutoSize = true;
            noSensorsText.Anchor = AnchorStyles.Left;
            openEditorButton.Anchor = AnchorStyles.Left;
            container.Controls.Add(noSensorsText);
            container.Controls.Add(openEditorButton);
            container.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            container.Dock = DockStyle.Fill;
            return container;
        }

        void openEditorButton_Click(object sender, EventArgs e)
        {
            // FIXME: que el tag sea un objeto que proporcione información sobre
            // el profile al que pertenece el boton y el tab al que pertenece
            ProfileEditor editor = new ProfileEditor((LoggerProfile)((Button)sender).Tag);
            editor.ShowDialog();
            redrawTabPage((TabPage)((Panel)((Button)sender).Parent).Parent);
        }

        private void GraphWorkspace_Resize(object sender, EventArgs e)
        {
            //tabControl3.Width = ((GraphWorkspace)sender).Parent.Width;
            //this.Dock = DockStyle.None;
            //this.Dock = DockStyle.Fill;
        }

        public void OrganizeActiveGraphGraphView()
        {
            if (tabControl3.SelectedTab.Tag.Equals("waterGraph"))
            {

                OrganizeGraphViews(waterSplit);
            }
        }


        private int getTotalHeight(SplitBasedWorkAreaSplitter splitter)
        {
            return tabControl3.SelectedTab.Height;
        }

        private int getContainerHeight(SplitBasedWorkAreaSplitter splitter)
        {
            int visibleGraphs = 0;
            int spacersDistance = 0;

            foreach (SplitContainer container in splitter.Containers)
            {
                if (!container.Panel1Collapsed)
                {
                    visibleGraphs++;
                    spacersDistance += container.SplitterWidth;
                }
            }

            return tabControl3.SelectedTab.Height / visibleGraphs;

        }
        public void OrganizeGraphViews(SplitBasedWorkAreaSplitter splitter)
        {
            int containerHeight = getContainerHeight(splitter);

            foreach (SplitContainer container in splitter.Containers)
            {
                if (!container.Panel1Collapsed)
                {
                    container.SplitterDistance = containerHeight;
                }
            }

        }


        internal void ShowDefaultGraph()
        {
            redrawTabPage(waterGraphTab);
        }

        public GraphView GetActiveStackedGraphView()
        {
            if (tabControl3.SelectedTab.Tag.Equals("waterGraph") && waterSplit != null)
            {
                return waterSplit.GetGraphView(2);
            }

            if (tabControl3.SelectedTab.Tag.Equals("salinityGraph") && salinitySplit != null)
            {
                return salinitySplit.GetGraphView(2);
            }

            if (tabControl3.SelectedTab.Tag.Equals("temperatureGraph") && temperatureSplit != null)
            {
                return temperatureSplit.GetGraphView(2);
            }

/*            if (tabControl3.SelectedTab.Tag.Equals("waterGraph"))
            {
                return waterSplit.GetGraphView(2);
            }*/

            return null;
        }


        public void ResynchAllXAxisInWorkspace() {
            if (waterSplit != null)
            {
                waterSplit.SynchXAxisTo(waterSplit.Synchronizer);
            }

            if (salinitySplit != null)
            {
                salinitySplit.SynchXAxisTo(salinitySplit.Synchronizer);
            }

            if (temperatureSplit != null)
            {
                temperatureSplit.SynchXAxisTo(temperatureSplit.Synchronizer);
            }

            if (waterSalinitySplit != null)
            {
                waterSalinitySplit.SynchXAxisTo(waterSalinitySplit.Synchronizer);
            }

        }

        public GraphView GetActiveSummedGraphView()
        {
            if (tabControl3.SelectedTab.Tag.Equals("waterGraph"))
            {
                return waterSplit.GetGraphView(1);
            }

            if (tabControl3.SelectedTab.Tag.Equals("salinityGraph"))
            {
                return salinitySplit.GetGraphView(1);
            }

            if (tabControl3.SelectedTab.Tag.Equals("temperatureGraph"))
            {
                return temperatureSplit.GetGraphView(1);
            }


            return null;
        }

        public void ShowStackedWaterSalinityGraphs()
        {
            //database.OpenFullPath(profile.DataBasePath);
            TabPage graphTab = tabControl3.SelectedTab;

            #region waterSalinityGraphs
            if (graphTab.Tag.Equals("waterSalinityGraph"))
            {
                SplitBasedWorkAreaSplitter split = (SplitBasedWorkAreaSplitter)(graphTab.Controls["split"]);
                GraphView ngv;
                GraphView ogv;

                ngv = waterSalinityGraph.GetWaterStackedGraph(StackedStyles.Stacked);
                ogv = split.GetGraphView(2);
                swapGraphView(split, ngv, ogv);

                ngv = waterSalinityGraph.GetSalinityStackedGraph(StackedStyles.Stacked);
                ogv = split.GetGraphView(3);
                swapGraphView(split, ngv, ogv);

            }
            #endregion

        }

        public void ShowSummedWaterSalinityGraphs()
        {
            //database.OpenFullPath(profile.DataBasePath);
            TabPage graphTab = tabControl3.SelectedTab;

            #region waterSalinityGraphs
            if (graphTab.Tag.Equals("waterSalinityGraph"))
            {
                SplitBasedWorkAreaSplitter split = (SplitBasedWorkAreaSplitter)(graphTab.Controls["split"]);
                GraphView ngv;
                GraphView ogv;
                
                
                ngv = waterSalinityGraph.GetWaterSummedGraph();
                ogv = split.GetGraphView(2);
                SplitContainer toRemove = (SplitContainer)((SplitterPanel)ogv.GetGraphControl().Parent).Parent;
                Control spliterPanelParent  = toRemove.Parent;
                spliterPanelParent.Controls.Remove(toRemove);

                waterSalinitySplit.AddGraphView(ngv);


                ngv = waterSalinityGraph.GetSalinitySummedGraph();
                waterSalinitySplit.AddGraphView(ngv);
                //swapGraphView(split, ngv, ogv);                

                //split.AddGraphView(waterSalinityGraph.GetSalinitySummedGraph());                
            }

            //database.Close();
            #endregion

        }


        internal void Close()
        {
            if (database.isOpen)
                database.Close();
        }

        private void onWorkspaceTabChangedEvent()
        {
            if (WorkSpaceTabChangedEvent != null)
            {
                GraphWorkSpaceEventArgs eventArgs = new GraphWorkSpaceEventArgs();
                eventArgs.TabControl = tabControl3;
                WorkSpaceTabChangedEvent(this, eventArgs);
            }
        }

        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            onWorkspaceTabChangedEvent();
        }


    }

    public class GraphWorkSpaceEventArgs : EventArgs
    {
        private TabControl tabControl;

        public TabControl TabControl
        {
            get { return tabControl; }
            set { tabControl = value; }
        }
    }

    public delegate void WorkSpaceEventHandler(GraphWorkspace sender, GraphWorkSpaceEventArgs e);
}
