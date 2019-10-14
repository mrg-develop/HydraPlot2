using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace HydraPlot2
{
    class ToolStripManager
    {
        private ToolStrip ts;

        public ToolStripManager(Form1 mainForm)
        {
            ts = mainForm.GetToolStrip;
        }

        public void SetClosedState()
        {
            ts.Items["openButton"].Enabled = true;
            ts.Items["importButton"].Enabled = true;
            ts.Items["apilarButton"].Enabled = false;
            ts.Items["mostrarETO"].Enabled = false;
            ts.Items["mostrarIPP"].Enabled = false;
            ts.Items["fullXButton"].Enabled = false;
            ts.Items["fullYButton"].Enabled = false;
            ts.Items["masXButton"].Enabled = false;
            ts.Items["menosXButton"].Enabled = false;
            ts.Items["masYButton"].Enabled = false;
            ts.Items["menosYButton"].Enabled = false;
            
        }

        public void SetOpenedState()
        {
            ts.Items["openButton"].Enabled = true;
            ts.Items["importButton"].Enabled = true;
            ts.Items["apilarButton"].Enabled = true;
            ts.Items["mostrarETO"].Enabled = true;
            ts.Items["mostrarIPP"].Enabled = false;
            ts.Items["fullXButton"].Enabled = true;
            ts.Items["fullYButton"].Enabled = true;
            ts.Items["masXButton"].Enabled = true;
            ts.Items["menosXButton"].Enabled = true;
            ts.Items["masYButton"].Enabled = true;
            ts.Items["menosYButton"].Enabled = true;

        }


        internal void SetETOIPPPanelActive()
        {
            ToolStripButton b = (ToolStripButton)ts.Items["mostrarETO"];
            b.Enabled = true;
            b.Checked = true;
            ts.Items["mostrarIPP"].Enabled = true;

        }

        internal void SetETOIPPPanelInactive()
        {
            ts.Items["mostrarETO"].Enabled = true;
            ts.Items["mostrarIPP"].Enabled = false;

        }
    }
}
