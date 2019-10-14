using System;
using System.Collections.Generic;
using System.Text;
using ZedGraph;
using System.Drawing;

namespace HydraPlot2
{
    public abstract class GraphView
    {


        protected GraphData gd;
        protected ZedGraphControl zed;
        protected Dictionary<string, PointPairList> series;
        protected List<Color> palette;
        protected int paletteIndex;
        

        protected float defaultX;
        protected float defaultY;
        protected float marginBottom;
        protected float marginRight;
        protected List<Sensor> sensorList;

        public List<Sensor> SensorList
        {
            get { return sensorList; }           
        }

        protected void applyStandardGraphFormat(GraphPane target)
        {
            //zed.IsAutoScrollRange = true;
            setAxisTitles();
            target.XAxis.Type = AxisType.Date;
            target.Title.IsVisible = false;
            target.XAxis.Title.IsVisible = false;
            target.YAxis.Title.IsVisible = true;
            target.XAxis.Scale.FontSpec.Family = "Arial";
            target.YAxis.Scale.FontSpec.Family = "Arial";
            target.YAxis.Scale.FontSpec.Size = 11;
            target.XAxis.Scale.FontSpec.Size = 11;
            target.XAxis.MajorGrid.IsVisible = true;
            target.XAxis.MajorGrid.DashOn = 3.0F;
            target.XAxis.Color = Color.Black;
            target.YAxis.Color = Color.Black;
            
            target.IsFontsScaled = false;
            //target.YAxis.Scale.Min = 0.18;
            //target.YAxis.Scale.Max = 0.30;
            target.IsIgnoreMissing = false;
            target.Legend.FontSpec.Size = 11;
            target.YAxis.Scale.Format = "F2";
            //target.Chart.IsRectAuto = false; // DEBUG




            //target.XAxis.Scale.MinorStep = 1;
            //target.XAxis.Scale.MajorStep = 1;
            target.XAxis.Scale.MinorUnit = DateUnit.Hour;
            target.XAxis.Scale.MajorUnit = DateUnit.Day;
            target.XAxis.Scale.MinorStepAuto = true;
            target.XAxis.Scale.MajorStepAuto = true;
            //target.XAxis.Scale.BaseTic = 6.0;
            

        }


        public GraphData GraphDataObject {
            get 
            { 
                return gd; 
            }
            set
            {
                gd = value;
            }
        }

        public GraphView(GraphData gd)
        {
            this.gd = gd;
            defaultX = 60;
            defaultY = 30;
            marginBottom = 30;
            marginRight = 40;
            
            series = new Dictionary<string, PointPairList>(0);
            this.gd = gd;
            this.sensorList = gd.GetSensorList();
            paletteIndex = 0;
            palette = new List<Color>(0);
            palette.Add(Color.Red);
            palette.Add(Color.Blue);
            palette.Add(Color.Green);


            zed = new ZedGraphControl();
            
            applyStandardGraphFormat(zed.GraphPane);
            zed.IsEnableVZoom = false;
            zed.IsEnableHZoom = true;
            zed.IsShowHScrollBar = false;
            zed.PanButtons = System.Windows.Forms.MouseButtons.Right;
            zed.PanModifierKeys = System.Windows.Forms.Keys.None;
            zed.IsShowPointValues = false;
            zed.IsShowContextMenu = false;
            zed.Dock = System.Windows.Forms.DockStyle.Fill;
            zed.MouseWheel += new System.Windows.Forms.MouseEventHandler(zed_MouseWheel);
            zed.IsZoomOnMouseCenter = false;
            zed.GraphPane.YAxis.Scale.MaxAuto = true;
            zed.GraphPane.YAxis.Scale.MinAuto = true;
            zed.PointToolTip.UseAnimation = false;
            zed.PointToolTip.UseFading = false;
            



            // zed.Resize += new EventHandler(zed_Resize);

            zed.PointValueEvent += new ZedGraphControl.PointValueHandler(zed_PointValueEvent);
            //zed.PointValueFormat = "a";

            zed.Resize += new EventHandler(zed_Resize);
            

        }

        private void setAxisTitles()
        {
            string labelText;

            switch (gd.GraphDataType)
            {
                case GraphData.GraphDataTypes.WATERSENSOR:
                    labelText = "Contenido de Agua(% v/v)";
                    break;
                case GraphData.GraphDataTypes.SALINITYSENSOR:
                    labelText = "C.E. masa de suelo (dS/m)";
                    break;
                case GraphData.GraphDataTypes.TEMPERATURESENSOR:
                    labelText = "Temperatura suelo (°C)";
                    break;
                case GraphData.GraphDataTypes.ETO:
                    labelText = "ETo (mm)";
                    break;
                case GraphData.GraphDataTypes.IPP:
                    labelText = "ETo / IPP (mm)";
                    break;
                case GraphData.GraphDataTypes.UNDEFINED:
                    labelText = "eje y";
                    break;
                default:
                    labelText = "eje y";
                    break;
            }

                AxisLabel label = new AxisLabel(labelText, "Arial", 11, Color.Black, false, false, false);
                label.FontSpec.Angle = 180;
                
                zed.GraphPane.YAxis.Title = label;
                zed.GraphPane.YAxis.Title.FontSpec.Border.IsVisible = false;
            

            
        }

        void zed_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            RectangleF yscrollRect = new RectangleF(5, zed.GraphPane.Chart.Rect.Top, zed.GraphPane.Chart.Rect.Left, zed.GraphPane.Chart.Rect.Height);
            RectangleF xscrollRect = new RectangleF(zed.GraphPane.Chart.Rect.Left, zed.GraphPane.Chart.Rect.Bottom, zed.GraphPane.Chart.Rect.Width, zed.GraphPane.Rect.Bottom - zed.GraphPane.Chart.Rect.Bottom);
            Console.WriteLine(e.Location.ToString());
            ZoomState oldstate = null;
            ZoomState newstate = null;
            if (yscrollRect.Contains(e.X, e.Y)) 
            {
                oldstate = new ZoomState(zed.GraphPane, ZoomState.StateType.Zoom);
                Console.WriteLine("Y Scroll " + e.Delta);
                if (e.Delta > 0)
                {
                    zed.ZoomScale(zed.GraphPane.YAxis, 0.9, 0, false);
                }
                else if (e.Delta < 0)
                {
                    zed.ZoomScale(zed.GraphPane.YAxis, 1.1, 0, false);
                }
                newstate = new ZoomState(zed.GraphPane, ZoomState.StateType.Zoom);
                zed.AxisChange();
                zed.Invalidate();
                zed.FireZoomEvent(zed, oldstate, newstate);
                /*
                if (e.Delta > 0)
                {
                    zed.GraphPane.YAxis.Scale.Min = zed.GraphPane.YAxis.Scale.Min + (e.Delta/1000);
                    zed.Invalidate();
                }
                else if (e.Delta < 0)
                {
                    zed.GraphPane.YAxis.Scale.Min = zed.GraphPane.YAxis.Scale.Min + (e.Delta/1000);
                    zed.Invalidate();
                }*/
            } else if (xscrollRect.Contains(e.X, e.Y))
            {
                oldstate = new ZoomState(zed.GraphPane, ZoomState.StateType.Zoom);
                Console.WriteLine("X Scroll " + e.Delta);
                if (e.Delta > 0)
                {
                    zed.ZoomScale(zed.GraphPane.XAxis, 0.9, 0, false);
                    
                }
                else if (e.Delta < 0)
                {
                    zed.ZoomScale(zed.GraphPane.XAxis, 1.1, 0, false);
                }
                newstate = new ZoomState(zed.GraphPane, ZoomState.StateType.Zoom);
                zed.AxisChange();
                zed.Invalidate();
                zed.FireZoomEvent(zed, oldstate, newstate);
            }
        }

        protected virtual void zed_Resize(object sender, EventArgs e)
        {
            Console.WriteLine("Zedcontrol in GraphView Resized");
            if (zed.GraphPane != null)
            {
                if (zed.GraphPane.Chart.IsRectAuto == false)
                {

                    float height = 0;
                    float width = 0;
                    
                    foreach (GraphPane pane in zed.MasterPane.PaneList)
                    {
                        

                        height = pane.Rect.Height - defaultY - marginBottom;
                        width = pane.Rect.Width - defaultX - marginRight;

                        pane.Chart.Rect = new RectangleF(pane.Rect.X + defaultX, pane.Rect.Y + defaultY, width, height);
                        

                    }
                }
            }
        }

        string zed_PointValueEvent(ZedGraphControl sender, GraphPane pane, CurveItem curve, int iPt)
        {
            double biased = curve.Points[iPt].Y;
            double unbiased = biased - curve.Points.Bias;
            string X = ((XDate)curve.Points[iPt].X).ToString("g");
            
            
            return X + " " + unbiased.ToString();
        }


        protected virtual void fillPointPairList(string channel)
        {
            PointPairList p = new PointPairList();

            Dictionary<DateTime, Decimal?> data = gd.GetData(channel);

            foreach (KeyValuePair<DateTime, Decimal?> d in data)
            {
                if (d.Value != null)
                {
                    p.Add(new XDate(d.Key), (double)d.Value);
                }
                else
                {
                    p.Add(new XDate(d.Key), double.NaN);
                }
            }

            // FIXME revisar esto

            // aqui intervenid para ocultar o mostar series
    
            bool add = false;

            foreach (Sensor s in sensorList)
            {
                if (s.Name.Equals(channel) && s.Selected == true)
                {
                    add = true;
                }
            }
            if (add)
            {
                series.Add(channel, p);
            }

        }

        protected Color getLineColor()
        {
            if (paletteIndex == palette.Count)
                paletteIndex = 0;
            Color c = palette[paletteIndex];
            paletteIndex++;
            return c;
        }

        public ZedGraphControl GetGraphControl()
        {          
            return zed;
        }

        public void Refresh()
        {
            series.Clear();
            foreach (GraphPane p in zed.MasterPane.PaneList)
            {
                p.CurveList.Clear();
            }
            Render();            
            zed.Refresh();
        }

        public abstract void Render();

        public void SaveToImage()
        {
            zed.SaveAsBitmap();
        }

        public GraphPane GraphPane
        {
            get { return zed.GraphPane; }
        }

        public PaneList PaneList
        {
            get { return zed.MasterPane.PaneList; }
        }
    }
}
