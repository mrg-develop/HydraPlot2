using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace HydraPlot2
{
    
    public class GraphData
    {

        public enum GraphDataTypes
        {
            UNDEFINED,
            WATERSENSOR,
            SALINITYSENSOR,
            TEMPERATURESENSOR,
            ETO,
            IPP
        }

        private const decimal volumeConst = 40.2909258M;

        private Database database;

        public Database Database   
        {
            get { return database; }
        }

        private string[] channels;

        public string[] Channels
        {
            get { return channels; }
        }

        private GraphDataTypes dataType;

        public GraphDataTypes GraphDataType {
            get { return dataType; }
        }

        public GraphData(Database db, string[] channels, GraphDataTypes dataType)
        {
            //reader = db.GetChannelData(channel);
            database = db;
            this.channels = channels;
            this.dataType = dataType;
        }

        private DateTime buildKey(DateTime fecha, DateTime hora)
        {
            fecha = DateTime.Parse(fecha.ToShortDateString());
            // FIXME
#if DEBUG
            //Console.WriteLine("");
            //Console.WriteLine(fecha.ToString());
            //fecha = fecha.AddHours(hora.Hour);
            //Console.WriteLine(fecha.ToString());

            //Console.WriteLine(key.ToString());
#else
                //fecha = fecha.AddHours(hora.Hour);
                //DateTime key = fecha.AddMinutes(hora.Minute);
#endif

            DateTime key = fecha.AddMinutes(hora.Minute);
            key = key.AddHours(hora.Hour);
            return key;
        }

        private Dictionary<DateTime, Decimal?> GetDailyData(string channel)
        {
            Dictionary<DateTime, Decimal?> data = new Dictionary<DateTime, Decimal?>(0);
            IDataReader reader = database.GetDailyData(channel);
            while (reader.Read())
            {

                DateTime fecha = reader.GetDateTime(0);

                DateTime hora = new DateTime(1,1,1,12,0,0);

                DateTime key = buildKey(fecha, hora);

                if (reader.IsDBNull(1))
                {
                    data.Add(key, null);
                }
                else
                {
                    data.Add(key, reader.GetDecimal(1));
                }
            }

            return data;
        }

        public Dictionary<DateTime, Decimal?> GetData(string channel) {

            

            IDataReader reader;
            if (channel.Equals("ETo"))
            {
                return GetDailyData("ETo");
            }
            else if (channel.Equals("IPP"))
            {
                return GetDailyData("IPP");
            }
            else
            {
                reader = database.GetChannelData(channel);
            }

            Dictionary<DateTime, Decimal?> data = new Dictionary<DateTime, Decimal?>(0);

            while (reader.Read())
            {
                
                DateTime fecha = reader.GetDateTime(0);

                DateTime hora = reader.GetDateTime(1);

                DateTime key = buildKey(fecha, hora);

                if (reader.IsDBNull(2))
                {
                    data.Add(key, null);
                }
                else
                {
                    if (this.GraphDataType == GraphDataTypes.WATERSENSOR)
                    {
                        data.Add(key, reader.GetDecimal(2) *  100.0M);
                    }
                    else if (this.GraphDataType == GraphDataTypes.SALINITYSENSOR)
                    {
                        data.Add(key, reader.GetDecimal(2) * 10.0M);
                    }
                    else
                    {
                        data.Add(key, reader.GetDecimal(2));
                    }
                }
            }

            return data;
        }

        public SortedDictionary<DateTime, Decimal?> GetPonderatedSummedData(string[] channels)
        {
            SortedDictionary<DateTime, Decimal?> data = new SortedDictionary<DateTime, Decimal?>();

            foreach (string channel in channels)
            {
                IDataReader reader = database.GetChannelData(channel);
                while (reader.Read())
                {

                    DateTime fecha = reader.GetDateTime(0);

                    DateTime hora = reader.GetDateTime(1);

                    DateTime key = buildKey(fecha, hora);

                    if (reader.IsDBNull(2))
                    {
                        if (data.ContainsKey(key))
                        {
                            //data[key] += 0; // FIXME: comportamiento para null gaps
                        }
                        else
                        {
                            data.Add(key, null); // FIXME: comportamiento para null gaps
                        }
                    }
                    else
                    {
                        decimal value = reader.GetDecimal(2);
                        if (this.dataType == GraphDataTypes.WATERSENSOR)
                            value = value * 100.0M;
                        else if (this.dataType == GraphDataTypes.SALINITYSENSOR)
                            value = value * 10.0M;

                        if (data.ContainsKey(key))
                        {
                            data[key] += value;
                        }
                        else
                        {
                            data.Add(key, value);
                        }

                    }

                }
            }


            List<DateTime> keys = new List<DateTime>(data.Keys);

            foreach (DateTime key in keys)
            {
                data[key] = data[key] / channels.Length;
            }



            return data;
        }

        public List<Sensor> GetSensorList()
        {
            List<Sensor> sensors = new List<Sensor>(0);

            foreach (string channel in channels)
            {
                sensors.Add(new Sensor(channel, true, 0));
            }

            return sensors;
        }

        public SortedDictionary<DateTime, Decimal?> GetWaterPonderatedSummedData(string[] channels)
        {
            SortedDictionary<DateTime, Decimal?> data = new SortedDictionary<DateTime, Decimal?>();

            foreach (string channel in channels)
            {
                IDataReader reader = database.GetChannelData(channel);
                while (reader.Read())
                {

                    DateTime fecha = reader.GetDateTime(0);

                    DateTime hora = reader.GetDateTime(1);

                    DateTime key = buildKey(fecha, hora);

                    if (reader.IsDBNull(2))
                    {
                        if (data.ContainsKey(key))
                        {
                            //data[key] += 0; // FIXME: comportamiento para null gaps
                        }
                        else
                        {
                            data.Add(key, null); // FIXME: comportamiento para null gaps
                        }
                    }
                    else
                    {
                        if (data.ContainsKey(key))
                        {
                            data[key] += reader.GetDecimal(2);
                        }
                        else
                        {
                            data.Add(key, reader.GetDecimal(2));
                        }
                          
                    }

                }
            }


            List<DateTime> keys = new List<DateTime>(data.Keys);

            foreach (DateTime key in keys)
            {
                data[key] = data[key] / channels.Length;
            }


            
            return data;

        }
    }
}
