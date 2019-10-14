using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HydraPlot2
{

    public delegate void RequestCancelEventHandler(ImporterMonitor monitor, bool proceed);

    public partial class ImporterMonitor : Form
    {
        public event RequestCancelEventHandler RequestCancel;

        bool isRunningMode = false;

        private int insertados = 0;
        private int duplicados = 0;
        private int invalidos = 0;
        private int total = 0;

        public ImporterMonitor()
        {
            InitializeComponent();
            button1.Click += new EventHandler(button1_Click);
  
        }

        public void UpdateStatus(ImporterWorker.ImporterWorkerReport wReport, int fileEvent)
        {
            if (wReport.LineReport != null)
            {
                ImporterLineReport report = wReport.LineReport;

                insertados += report.Inserted;
                duplicados += report.Duplicated;
                invalidos += report.Invalid;
                total += report.Total;

                lblInsertados.Text = insertados.ToString();
                lblDuplicados.Text = duplicados.ToString();
                lblInvalidos.Text = invalidos.ToString();
                lblTotal.Text = total.ToString();      
          
                int p = Convert.ToInt32(( (float)report.CurrentLine / (float)report.TotalLines) * 100.0f);
                pbLine.Value = p;
                lblLineProgress.Text = "Archivo actual: " + report.CurrentFileName + " (" +p.ToString() + "%)";
            }

            if (wReport.FileReport != null)
            {
                ImporterFileReport report = wReport.FileReport;
                if (fileEvent == 0)
                {
                    lblFileProgress.Text = "Progreso global: procesando archivo " + (report.CurrentFileNum + 1) + " de " + report.TotalFileNum;
                }
                else
                {
                    int p = Convert.ToInt32(((float)(report.CurrentFileNum) / (float)report.TotalFileNum) * 100.0f);
                    pbFile.Value = p;
                }
            }
        }

        public void SetRunningMode()
        {
            isRunningMode = true;
            button1.Text = "Cancelar";
            SetOperationStatus("Importando...", System.Drawing.Color.Green);
            //button1.Image = HydraPlot.Properties.Resources.cancel;
        }

        void button1_Click(object sender, EventArgs e)
        {

            if (isRunningMode)
            {
                bool proceed = false;
                if (RequestCancel != null)
                    RequestCancel(this, proceed);
            }
            else
            {
                Close();
            }
        }

        public void SetIdleMode()
        {
            isRunningMode = false;
            button1.Text = "Continuar";
            //button1.Image = HydraPlot.Properties.Resources.ok;
        }

        public void SetCaption(string text)
        {
            this.Text = text;
        }

        public void ShowReportLink()
        {
            linkReport.Visible = true;
        }

        public void SetOperationStatus(string text, Color c)
        {
            lblOperationStatus.Text = text;
            lblOperationStatus.ForeColor = c;
        }
    }
}
