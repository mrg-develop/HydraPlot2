using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace HydraPlot2
{

    public delegate void Panel2ManagerEvent(Panel2Manager sender, EventArgs e);

    public sealed class Panel2Manager
    {
        //private SplitterPanel panel2;
        private Panel panel2;
        private GraphWorkspace workspace;
        private Panel noSensorsPanel;

        public Panel2Manager(Panel panel2Instance)
        {
            panel2 = panel2Instance;
            
        }

        public event Panel2ManagerEvent GraphWorkspaceLoaded;
        public event Panel2ManagerEvent GraphWorkspaceDisposed;
        public event Panel2ManagerEvent GraphWorkspaceLayoutEvent;
        public event Panel2ManagerEvent GraphWorkspaceTabChanged;

        private void onGraphWorkspaceLayoutEvent()
        {
            if (GraphWorkspaceLayoutEvent != null)
            {
                GraphWorkspaceLayoutEvent(this, new EventArgs());
            }
        }

        private void onGraphWorkspaceTabChanged() {
            if (GraphWorkspaceTabChanged != null)
                GraphWorkspaceTabChanged(this, new EventArgs());
        }

        private void onShowGraphWorkspace()
        {
            if (GraphWorkspaceLoaded != null)
            {
                GraphWorkspaceLoaded(this, new EventArgs());
            }
        }

        private void onGraphWorkspaceUnload()
        {
            if (GraphWorkspaceDisposed != null)
            {
                GraphWorkspaceDisposed(this, new EventArgs());
            }
        }


        public void ShowGraphWorkspace(LoggerProfile profile)
        {
            if (profile == null)
            {
                if (workspace != null)
                {
                    panel2.Controls.Remove(workspace);
                    workspace.Dispose();
                    workspace = null;
                    onGraphWorkspaceUnload();
                }

                if (noSensorsPanel != null)
                {
                    panel2.Controls.Remove(noSensorsPanel);
                    noSensorsPanel.Dispose();
                    noSensorsPanel = null;
                }

                noSensorsPanel = new FlowLayoutPanel();
                noSensorsPanel.Dock = DockStyle.Fill;
                Label noSensorsText = new Label();
                noSensorsText.Text = Program.GetString("ProfileNoSensorsText");
                noSensorsPanel.Controls.Add(noSensorsText);
                Button openEditorButton = new Button();
                openEditorButton.Tag = profile;
                openEditorButton.Click += new EventHandler(openEditorButton_Click);
                openEditorButton.Text = Program.GetString("ProfileEditorLink");
                noSensorsPanel.Controls.Add(openEditorButton);
                panel2.Controls.Add(noSensorsPanel);
                noSensorsPanel.Dock = DockStyle.Fill;
                // mostrar el profile editor link        
            }
            else // tenemos perfil, mostrar gráfico para cada tab o bien editor
                 // si no hay sensores para el gráfico en particular
            {
                if (noSensorsPanel != null)
                {
                    panel2.Controls.Remove(noSensorsPanel);
                    noSensorsPanel.Dispose();
                    noSensorsPanel = null;
                }

                if (workspace != null)
                {
                    panel2.Controls.Remove(workspace);
                    workspace.Dispose();
                    workspace = null;
                    onGraphWorkspaceUnload();
                }

                workspace = new GraphWorkspace(profile, new Database());
                workspace.WorkSpaceLayoutEvent += new WorkSpaceEventHandler(workspace_WorkSpaceLayoutEvent);
                workspace.WorkSpaceTabChangedEvent += new WorkSpaceEventHandler(workspace_WorkSpaceTabChangedEvent);

                workspace.Load += new EventHandler(workspace_Load);
                workspace.ShowDefaultGraph();
                onShowGraphWorkspace();
                
                

                //workspace.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                //workspace.Dock = DockStyle.Fill;
                //workspace.AutoSize = true;
                panel2.Controls.Add(workspace);
                Program.SetMainFormTitle(profile.DatabaseName);
            }            
        }

        void workspace_WorkSpaceTabChangedEvent(GraphWorkspace sender, GraphWorkSpaceEventArgs e)
        {
            if (workspace.IsEtoHidden)
            {
                workspace.HideEto();
            }
            else
            {
                workspace.ShowEto();
            }

            if (workspace.isShowIPPInEtoGraph)
            {
                workspace.ShowIPPInEtoGraph();
            }
            else
            {
                workspace.HideIPPInEtoGraph();
            }
            onGraphWorkspaceTabChanged();
        }

        void workspace_WorkSpaceLayoutEvent(GraphWorkspace sender, EventArgs e)
        {
            onGraphWorkspaceLayoutEvent();
        }

        void workspace_Disposed(object sender, EventArgs e)
        {
            onGraphWorkspaceUnload();
        }

        void workspace_Load(object sender, EventArgs e)
        {
            workspace.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            workspace.Dock = DockStyle.Fill;
            
        }

        void openEditorButton_Click(object sender, EventArgs e)
        {
            ProfileEditor editor = new ProfileEditor((LoggerProfile)((Button)sender).Tag);
            editor.ShowDialog();
            //redrawTabPage((TabPage)((Panel)((Button)sender).Parent).Parent);

        }


        public void HideGraphWorkspace()
        {
            if (noSensorsPanel != null)
            {
                panel2.Controls.Remove(noSensorsPanel);
                noSensorsPanel.Dispose();
                noSensorsPanel = null;
            }

            if (workspace != null)
            {
                panel2.Controls.Remove(workspace);
                workspace.Dispose();
                workspace = null;
                onGraphWorkspaceUnload();
            }
            
        }

        public GraphWorkspace GetWorkspace()
        {
            return this.workspace;
        }


        internal void Exit()
        {
            workspace.Close();
        }
    }
}
