using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Threading;


namespace HydraPlot2
{
    

    public class ImporterWorker : BackgroundWorker
    {

        public class ImporterWorkerReport
        {
            public ImporterFileReport FileReport;
            public ImporterLineReport LineReport;

            public ImporterWorkerReport(ImporterFileReport fReport, ImporterLineReport lReport)
            {
                this.FileReport = fReport;
                this.LineReport = lReport;
            }
        }

        public string[] Files
        {
            get { return files; }
        }


        private string probeId;
        public string ProbeId
        {
            get { return probeId; }
        }



        string[] files;
        Importer importer;
        ImporterMonitor monitor;

        Semaphore semaphore = new Semaphore(0, 1);
        Object thisLock = new Object();
        bool pause = false;

        public ImporterWorker(string[] path, Importer i, ImporterMonitor m)
            : base()
        {
            new ImporterWorker(path, null, i, m);
        }

        public ImporterWorker(string[] path, string ProbeId, Importer i, ImporterMonitor m) : base()
        {
            this.WorkerReportsProgress = true;
            this.WorkerSupportsCancellation = true;
            probeId = ProbeId;
            files = path;
            importer = i;
            monitor = m;
            monitor.RequestCancel += new RequestCancelEventHandler(monitor_RequestCancel);

            importer.LineProcessed += new LineProcessedEventHandler(importer_LineProcessed);
            importer.FileProcessed += new FileProcessedEventHandler(importer_FileProcessed);
            importer.FileProcessing += new FileProcessingEventHandler(importer_FileProcessing);
            this.DoWork += new DoWorkEventHandler(ImporterWorker_DoWork);
            this.ProgressChanged += new ProgressChangedEventHandler(ImporterWorker_ProgressChanged);
            this.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ImporterWorker_RunWorkerCompleted);
        }

        void monitor_RequestCancel(ImporterMonitor monitor, bool proceed)
        {
            lock (thisLock)
            {
                //pause = true;

                // el thread de backgroundworker no se bloqueará hasta que no termine de procesar la linea en curso
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("¿Desea cancelar la importación?", "Importando...", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    this.CancelAsync();
                }
                else
                {
                    // en este punto el thread deberia estar bloqueado por el semaforo
                    //semaphore.Release();
                }
            }
        }

        void ImporterWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                monitor.SetIdleMode();
                monitor.SetCaption("Importación cancelada");
                //monitor.ShowReportLink();
                monitor.SetOperationStatus("Importación cancelada", System.Drawing.Color.Red);
            }
            else
            {
                monitor.SetIdleMode();
                monitor.SetCaption("Importación finalizada");
                //monitor.ShowReportLink();
                monitor.SetOperationStatus("Importación Finalizada", System.Drawing.Color.Green);
            }
        }

        


        #region Esto debería correr en el thread del worker     
        
        void importer_FileProcessing(Importer sender, ImporterFileReport report)
        {
            ImporterWorkerReport wReport = new ImporterWorkerReport(report, null);
            this.ReportProgress(0, wReport);
        }

        void importer_FileProcessed(Importer sender, ImporterFileReport report)
        {
            ImporterWorkerReport wReport = new ImporterWorkerReport(report, null);
            this.ReportProgress(1, wReport);
        }


        void importer_LineProcessed(Importer sender, ImporterLineReport report)
        {
            ImporterWorkerReport wReport = new ImporterWorkerReport(null, report);
            this.ReportProgress(0, wReport);
            lock (thisLock) {
                if (this.CancellationPending)
                {
                    importer.CancellAllOperations();                    
                    
                }
            };

            
        }


        void ImporterWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Thread.Sleep(1000);
            //monitor.SetRunningMode();

            ImporterWorker iw = sender as ImporterWorker;

            
            importer.ImportMultipleDatFiles(iw.Files, probeId);
            if (importer.Cancelled)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
            //this.OnRunWorkerCompleted(new RunWorkerCompletedEventArgs(null, null, true));
        }
        #endregion

        // Esto debería estar corriendo en el thread que llamo al BackgroundWorker
        void ImporterWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ImporterWorkerReport r = e.UserState as ImporterWorkerReport;            
            monitor.UpdateStatus(r, e.ProgressPercentage);
            //Console.WriteLine(e.ProgressPercentage);
        }
  


    }
}
