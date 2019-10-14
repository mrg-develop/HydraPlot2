using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
//using System.Data.OleDb;
using System.Data.Odbc;
using System.IO;
using System.Globalization;

namespace HydraPlot2
{
    public class Database
    {
        //OleDbConnection conn;
        OdbcConnection conn;
        public string Name;
        public bool isOpen = false;

        private string path;

        public string Path
        {
            get
            {
                return path;
            }
        }

        public void Open(string dbName)
        {
            Close();
            //string source = "Provider=Microsoft.JET.OLEDB.4.0; data source=databases\\" + dbName.Trim() + ".mdb; Mode=16;";
            string source = "Driver={Microsoft Access Driver (*.mdb)};" +
                            "Dbq=.\\databases\\" + dbName.Trim() + ".mdb;" +
                            "Mode=SHARE";
            //conn = new OleDbConnection(source);
            conn = new OdbcConnection(source);
            conn.Open();
            this.Name = dbName;
            isOpen = true;
            path = conn.Database;
        }

        public void OpenFullPath(string path)
        {
            //string source = "Provider=Microsoft.JET.OLEDB.4.0; data source=databases\\" + dbName.Trim() + ".mdb; Mode=16;";
            string source = "Driver={Microsoft Access Driver (*.mdb)};" +
                            "Dbq=" + path + ";" +
                            "Mode=SHARE";
            //conn = new OleDbConnection(source);
            conn = new OdbcConnection(source);           
            conn.Open();
            this.Name = System.IO.Path.GetFileNameWithoutExtension(path);
            isOpen = true;
            this.path = path;
        }

        public void Close()
        {
            if (conn != null)
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();

                isOpen = false;
            }
        }

        // se salta aquellas filas que estén duplicadas y
        // verifica que el probe_id sea consistente (no inserta filas si el probe_id != al dbName)
        public void AddRow(string dat_fecha, string dat_hora, string canal, string lectura)
        {
            if (isOpen)
            {
                NumberFormatInfo f = new NumberFormatInfo();
                f.NumberDecimalSeparator = ".";
                f.NumberGroupSeparator = ",";
                f.NumberDecimalDigits = 2;

                try
                {
                    int year = 2000 + int.Parse(dat_fecha.Substring(0, 2));
                    int month = int.Parse(dat_fecha.Substring(2, 2));
                    int day = int.Parse(dat_fecha.Substring(4, 2));

                    int hour = int.Parse(dat_hora.Substring(0, 2));
                    int min = int.Parse(dat_hora.Substring(2, 2));

                    DateTime d = new DateTime(year, month, day, hour, min, 0);


                    Decimal v = Decimal.Parse(lectura, f);

                    //Console.WriteLine(d.ToShortDateString());
                    //Console.WriteLine(d.ToShortTimeString();


                    string cmdText = "INSERT INTO datos(fecha, hora, canal, lectura) VALUES (" +
                        "'{0}', '{1}', '{2}', {3})";
                    cmdText = String.Format(cmdText, d.ToShortDateString(), d.ToShortTimeString(), canal, v.ToString(f));
                    //Console.WriteLine(cmdText); 
                    //OleDbCommand command = new OleDbCommand(cmdText, conn);
                    OdbcCommand command = new OdbcCommand(cmdText, conn);
                    command.ExecuteNonQuery();
                }
                    /*
                catch (OleDbException e)
                    {
                        if (e.Errors[0].SQLState.Equals("3022"))
                        {
                            throw new DatabaseException("Error " + e.Errors[0].SQLState + " al intentar insertar una fila a la base de datos " + dbName +
                            ". OleDB entregó lo siguiente: " + e.Message, DatabaseErrorTypes.PK_VIOLATION);
                        }
                        else
                        {
                            throw new Exception("Error " + e.Errors[0].SQLState + " al intentar insertar una fila a la base de datos " + dbName +
                            ". OleDB entregó lo siguiente: " + e.Message);
                        }

                    }*/
                     catch (OdbcException e)
                    {
                        if (e.Errors[0].SQLState.Equals("23000"))
                        {
                            throw new DatabaseException("Error " + e.Errors[0].SQLState + " al intentar insertar una fila a la base de datos " + Name +
                            ". OleDB entregó lo siguiente: " + e.Message, DatabaseErrorTypes.PK_VIOLATION);
                        }
                        else
                        {
                            throw new Exception("Error " + e.Errors[0].SQLState + " al intentar insertar una fila a la base de datos " + Name +
                            ". OleDB entregó lo siguiente: " + e.Message);
                        }

                    }
                catch (Exception e)
                {
                    if (e is ArgumentNullException || e is FormatException || e is OverflowException)
                    {
                        throw new DatabaseException("Ha ocurrido un error de transformación de datos al intentar insertar datos a la base " +
                            Name + String.Format(". El input es: dat_fecha: {0}, dat_hora: {1}, canal: {2}, lectura: {3}", dat_fecha, dat_hora, canal, lectura), DatabaseErrorTypes.DATA_INVALID);
                    }
                    else
                    {
                        throw new DatabaseException("Ha ocurrido un error no definido en AddRow para la base de datos " + Name +
                            String.Format(". El input es: dat_fecha: {0}, dat_hora: {1}, canal: {2}, lectura: {3}", dat_fecha, dat_hora, canal, lectura), DatabaseErrorTypes.NOT_DEFINED);
                    }
                }
            }
            else
            {
                throw new Exception("La base de datos " + Name + " no se encuentra abierta.");
            }
        }

        // hay que pasarle la llave primaria
        public void DeleteRow()
        {
        }

        // elimina todas las filas de la base de datos
        public void Truncate()
        {
        }


        // crea una base de datos a partir de template.mdb con el nombre dbName
        public void CreateDatabase(string dbName)
        {
            if (!File.Exists("databases\\" + dbName + ".mdb")) 
            {
                File.Copy(Directory.GetCurrentDirectory() + "\\databases\\template.mdb",
                    Directory.GetCurrentDirectory() + "\\databases\\" + dbName + ".mdb");
                Open(dbName);
                DatabaseInfo di = GetDbInfo();
                di.ProbeId = dbName;
                SetDbInfo(di);
                Close();            
            }
        }

        // elimina una base de datos .mdb
        public void DeleteDatabase(string dbName)
        {
        }

        public DatabaseInfo GetDbInfo()
        {
            // fixme tirar excepcion mejor?
            if (isOpen)
            {
                //OleDbCommand command = new OleDbCommand("SELECT TOP 1 * FROM metadatos", conn);
                OdbcCommand command = new OdbcCommand("SELECT TOP 1 * FROM metadatos", conn);
                //OleDbDataReader reader = command.ExecuteReader();
                OdbcDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    DatabaseInfo di = new DatabaseInfo();
                    di.MajorVersion = reader.GetByte(0);
                    di.MinorVersion = reader.GetByte(1);
                    di.ProbeId = reader.GetString(2);
                    reader.Close();
                    return di;
                }
                else
                {
                    throw new Exception("Error al intentar leer DatabaseInfo desde " + Name);
                }
            }
            else
            {
                throw new Exception("La base de datos " + Name +  " no se encuentra abierta.");
            }
        }

        public void SetDbInfo(DatabaseInfo di)
        {
            if (isOpen)
            {
                string cmdText = String.Format("UPDATE metadatos SET version = {0:G} , revision = {1:G}, probe_id = '{2}' ",
                    di.MajorVersion, di.MinorVersion, di.ProbeId);
                //OleDbCommand command = new OleDbCommand(cmdText, conn);
                OdbcCommand command = new OdbcCommand(cmdText, conn);
                command.ExecuteNonQuery();
            }
            else
            {
                throw new Exception("La base de datos " + Name + " no se encuentra abierta.");
            }
        }


        public bool Exists(string dbName)
        {
            return File.Exists("databases\\" + dbName + ".mdb");
        }

        public string[] ChannelsAvailable
        {
            get {
                return getChannels();
            }
        }

        private string[] getChannels()
        {
            string cmdText = "SELECT DISTINCT canal FROM datos";
            OdbcCommand command = new OdbcCommand(cmdText, conn);
            OdbcDataReader r = command.ExecuteReader();

            List<string> channels = new List<string>(0);

            while (r.Read())
            {
                //Console.WriteLine(r.GetString(0));
                channels.Add(r.GetString(0));    
            }
            r.Close();

            return channels.ToArray();
        }

        // ojo hay que cerrar el reader después
        public OdbcDataReader GetChannelData(string channel)
        {
            string cmdText = "SELECT fecha, hora, lectura FROM datos WHERE canal = '" + channel + "' ORDER BY fecha ASC, hora ASC";
            OdbcCommand command = new OdbcCommand(cmdText, conn);
            return command.ExecuteReader();
        }

        public OdbcDataReader GetDailyData(string channel)
        {
            string cmdText = "SELECT fecha, SUM(mm) FROM " + channel + " GROUP BY fecha ORDER BY fecha ASC";
            OdbcCommand command = new OdbcCommand(cmdText, conn);
            return command.ExecuteReader();
        }

        
        public LoggerProfile GetLoggerProfile()
        {
            string cmdText = "SELECT SensorId, SensorType FROM profile GROUP BY SensorId, SensorType ORDER BY SensorId ASC";
            OdbcCommand command = new OdbcCommand(cmdText, conn);
            OdbcDataReader reader = command.ExecuteReader();

            List<string> w = new List<string>(0);
            List<string> s = new List<string>(0);
            List<string> t = new List<string>(0);

            while (reader.Read())
            {
                switch (reader.GetInt32(1)) {
                    case 0 : // water
                        w.Add(reader.GetString(0));
                        break;
                    case 1: // salinity
                        s.Add(reader.GetString(0));
                        break;
                    case 2: // temp
                        t.Add(reader.GetString(0));
                        break;
                }
            }

            LoggerProfile profile = new LoggerProfile(GetDbInfo().ProbeId, w.ToArray(), s.ToArray(), t.ToArray(), path);
            reader.Close();
            return profile;
        }

        public void ExecuteCommand(string cmdStr)
        {
            if (isOpen)
            {
                OdbcCommand cmd = new OdbcCommand(cmdStr, conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
