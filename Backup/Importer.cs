using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace HydraPlot2
{


    public class ImporterReport
    {

        private string currentFileName;

        public string CurrentFileName
        {
            get { return currentFileName; }
        }

        private int inserted;

        public int Inserted 
        {
            get { return inserted; }
        }

        private int duplicated;

        public int Duplicated
        {
            get { return duplicated; }
        }

        private int invalid;

        public int Invalid
        {
            get { return invalid; }
        }

        private int total;

        public int Total
        {
            get { return total; }
        }

        public ImporterReport(int inserted, int duplicated, int invalid, int total, string currentFileName)
        {
            this.inserted = inserted;
            this.duplicated = duplicated;
            this.invalid = invalid;
            this.total = total;
            this.currentFileName = currentFileName;
        }
    }

    public class ImporterLineReport : ImporterReport
    {
        private int currentLine;

        public int CurrentLine
        {
            get { return currentLine; }
        }

        private int totalLines;

        public int TotalLines
        {
            get { return totalLines; }
        }

        public ImporterLineReport(int inserted, int duplicated, int invalid, int total, string currentFileName, int currentLine, int totalLines)
            : base(inserted, duplicated, invalid, total, currentFileName)
        {
            this.currentLine = currentLine;
            this.totalLines = totalLines;
        }
    }

    public class ImporterFileReport : ImporterReport
    {


        private int currentFileNum;

        public int CurrentFileNum
        {
            get { return currentFileNum; }
        }

        private int CurrentProcessedFileNum;

        public int MyProperty
        {
            get { return CurrentProcessedFileNum; }
            set { CurrentProcessedFileNum = value; }
        }

        private int totalFileNum;

        public int TotalFileNum
        {
            get { return totalFileNum; }
        }


        public ImporterFileReport(int inserted, int duplicated, int invalid, int total, string currentFileName, int currentFileNum, int totalFileNum)
            : base(inserted, duplicated, invalid, total, currentFileName)
        {
            this.currentFileNum = currentFileNum;
            this.totalFileNum = totalFileNum;
            
        }
    }

    public delegate void ImportFinishedEventHandler(Importer sender, ImporterReport report);
    public delegate void LineProcessedEventHandler(Importer sender, ImporterLineReport report);
    public delegate void FileProcessedEventHandler(Importer sender, ImporterFileReport report);
    public delegate void FileProcessingEventHandler(Importer sender, ImporterFileReport report);

    public class Importer
    {
        public event ImportFinishedEventHandler ImportFinished;
        public event LineProcessedEventHandler LineProcessed;
        public event FileProcessedEventHandler FileProcessed;
        public event FileProcessingEventHandler FileProcessing;

        private Database db;
        private FileStream file;
        
        private int M_inserted;
        private int M_duplicated;
        private int M_invalid;
        private int M_total;

        private int S_inserted;
        private int S_duplicated;
        private int S_invalid;
        private int S_total;

        private bool cancelAllOperations = false;

        public bool Cancelled
        {
            get { return cancelAllOperations; }
        }

        public Importer(Database db)
        {
            this.db = db;
        }

        private void OpenDatFile(string filename)
        {
            // TODO try catch block
            file = new FileStream(filename, FileMode.Open);            
        }

        private void CloseDatFile()
        {
            if (file != null)
            {
                file.Close();
                file = null;
            }
        }

        private int getTotalLines(StreamReader r)
        {
            int lineCount = 0;
            while (r.ReadLine() != null)
            {
                lineCount++;
            }
            return lineCount;
        }

        public void ImportSingleDatFile(string filename)
        {
            ImportSingleDatFile(filename, null);
        }

        // TODO: arreglar este método y agregar exceptions
        public void ImportSingleDatFile(string filename, string target_probe_id)
        {
            S_duplicated = 0;
            S_inserted = 0;
            S_invalid = 0;
            S_total = 0;


            bool firstLine = true;
            string selected_db = "";
            FileInfo fi = new FileInfo(filename);

            OpenDatFile(filename);            

            if (file != null)
            {
                // obtener el total de lineas

                int currentLine = 0;
                StreamReader r = new StreamReader(file);
                int totalLines = getTotalLines(r);
                file.Seek(0, SeekOrigin.Begin);

                string line;


                while ((line = r.ReadLine()) != null)
                {
                    System.Threading.Thread.Sleep(1);
                    int dup = 0;
                    int ins = 0;
                    int inv = 0;

                    if (line.Length == 56)
                    {

                        S_total++;
                        //Console.WriteLine(line.Length);
                        string probe_id;
                        if (target_probe_id == null)
                        {
                            probe_id = line.Substring(0, 20).Trim(); // saca el id desde el dat
                        }
                        else
                        {
                            probe_id = target_probe_id;
                        }

                        if (firstLine)
                        {
                            if (target_probe_id == null) {
                            selected_db = probe_id;

                            firstLine = false;
                            if (!db.Exists(probe_id))
                            {
                                db.CreateDatabase(probe_id);
                            }

                            if (!db.isOpen)
                            {
                                db.Open(probe_id);
                            }
                                }

                        }

                        if (probe_id.Equals(selected_db) || target_probe_id != null)
                        {
                            string probe = line.Substring(0, 20).Trim();
                            if (target_probe_id != null && probe == target_probe_id)
                            {
                                string dat_fecha = line.Substring(29, 6).Trim();
                                string dat_hora = line.Substring(38, 4).Trim();
                                string canal = line.Substring(20, 9).Trim();
                                string lectura = line.Substring(42, 14).Trim();


                                try
                                {
                                    db.AddRow(dat_fecha, dat_hora, canal, lectura);
                                    S_inserted++;
                                    ins = 1;
                                }
                                catch (DatabaseException e)
                                {
                                    switch (e.DatabaseError)
                                    {
                                        case DatabaseErrorTypes.PK_VIOLATION:
                                            S_duplicated++;
                                            dup = 1;
                                            break;
                                        case DatabaseErrorTypes.DATA_INVALID:
                                            S_invalid++;
                                            inv = 1;
                                            break;
                                        case DatabaseErrorTypes.NOT_DEFINED:
                                            S_invalid++;
                                            inv = 1;
                                            break;
                                        default:
                                            S_invalid++;
                                            inv = 1;
                                            break;
                                    }
                                }
                            }
                        }
                        else // cambio de probe_id cuando ya estaba definida la db
                        {
                            S_invalid++;
                            inv = 1;
                        }
                    }
                    else
                    {
                        // if line length != 56
                        S_invalid++;
                        inv = 1;
                    }
                    // aqui puede ir un evento
                    currentLine++;
                    if (LineProcessed != null)
                    {
                        LineProcessed(this, new ImporterLineReport(ins, dup, inv, 1, fi.Name, currentLine, totalLines));
                    }
                    if (cancelAllOperations)
                    {
                        CloseDatFile();
                        return;
                    }
                }
            }

            CloseDatFile();
        }

        public void ImportMultipleDatFiles(string[] filenames)
        {
            ImportMultipleDatFiles(filenames, null);
        }

        public void ImportMultipleDatFiles(string[] filenames, string probe_id)
        {
            M_duplicated = 0;
            M_inserted = 0;
            M_invalid = 0;
            M_total = 0;

            int currentFileNum = 0;
            int totalFileNum = filenames.Length;

            foreach (string filename in filenames)
            {


                FileInfo fi = new FileInfo(filename);

                if (FileProcessing != null)
                {
                    FileProcessing(this, new ImporterFileReport(0,0,0,0, fi.Name, currentFileNum, totalFileNum));
                }



                if (probe_id == null)
                {
                    ImportSingleDatFile(filename);
                }
                else
                {
                    ImportSingleDatFile(filename, probe_id);
                }
                M_duplicated += S_duplicated;
                M_inserted += S_inserted;
                M_invalid += S_invalid;
                M_total += S_total;
                currentFileNum++;
                if (cancelAllOperations)
                    return;

                if (FileProcessed != null)
                {
                    FileProcessed(this, new ImporterFileReport(M_inserted, M_duplicated, M_invalid, M_total, fi.Name, currentFileNum, totalFileNum));
                }
            }

            if (ImportFinished != null)
                ImportFinished(this, GetMultiImportReport());
        }

        public void CancellAllOperations()
        {
            cancelAllOperations = true;
        }


        public ImporterReport GetSingleImportReport()
        {
            return new ImporterReport(S_inserted, S_duplicated, S_invalid, S_total, "");
        }

        public ImporterReport GetMultiImportReport()
        {
            return new ImporterReport(M_inserted, M_duplicated, M_invalid, M_total, "");
        }
    }
}
