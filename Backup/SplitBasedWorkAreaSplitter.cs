using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace HydraPlot2
{
    public class SplitBasedWorkAreaSplitter : Panel, IWorkAreaSplitter 
    {
        private List<SplitContainer> containers;
        private List<GraphView> graphViews;
        private GraphView sinchronizer;
        private List<List<Ruler>> synchedRulers;

        public GraphView Synchronizer
        {
            get { return sinchronizer; }
        }

        public List<SplitContainer> Containers
        {
            get
            {
                return containers;
            }
        }

        public SplitBasedWorkAreaSplitter()
        {
            containers = new List<SplitContainer>(0);
            graphViews = new List<GraphView>(0);
            sinchronizer = null;
            synchedRulers = new List<List<Ruler>>(0);
        }

        public List<GraphView> GraphViews
        {
            get { return graphViews; }
        }

        public void ReplaceGraphView(GraphView ngv, GraphView ogv)
        {
            SplitContainer splitterToRemove = (SplitContainer)((SplitterPanel)(ogv.GetGraphControl().Parent)).Parent;
            containers.Remove(splitterToRemove);
            Control splitterToRemoveParent = splitterToRemove.Parent;
            splitterToRemoveParent.Controls.Remove(splitterToRemove);
            

            // remove rulers


            //SplitContainer s = containers[index];

            //int i = this.containers[0].Panel2.Controls.IndexOf(containers[index]);

            //this.containers[0].Panel2.Controls.RemoveAt(i);
            
            //containers.RemoveAt(index);
            
            //s.Panel1.Controls.RemoveAt(0); //eliminando el zed
            /*ZedGraphControl zed = gv.GetGraphControl();
            zed.Tag = "test";
            s.Panel1.Controls.Add(zed);
             * */
            GraphView toRemove = ogv;
            Rulers rs = toRemove.GraphPane.Rulers;
            
            // ubicar las rulers del graphview y removerlas y de los synched
            foreach (HRuler hruler in rs.HRulers)
            {
                foreach (List<Ruler> sr in synchedRulers)
                {
                    if (sr.Find(ruler => ruler.Equals(hruler)) != null)
                    {
                        sr.Remove(hruler);
                    }
                }
            }

            foreach (VRuler vruler in rs.VRulers)
            {
                foreach (List<Ruler> sr in synchedRulers)
                {
                    if (sr.Find(ruler => ruler.Equals(vruler)) != null)
                    {
                        sr.Remove(vruler);
                    }
                }
            }

            rs.HRulers.Clear();
            rs.VRulers.Clear();

            graphViews.Remove(ogv);
            AddGraphView(ngv);
            SynchXAxis(sinchronizer.GetGraphControl(), ngv.GetGraphControl());
        
            int d = this.containers[0].SplitterDistance;
            this.containers[0].SplitterDistance = d + 1;
            this.containers[0].SplitterDistance = d;
        }

        public GraphView GetGraphView(int index)
        {
            return graphViews[index];
        }

        public void SetGraphView(int index, GraphView gv)
        {
            graphViews[index] = gv;
        }

        public void AddGraphView(GraphView gv)
        {
            SplitContainer s = new SplitContainer();
            graphViews.Add(gv);


            //s.Panel1MinSize = 100;

            if (containers.Count == 0)
            {
                this.Controls.Add(s);
            } else {
                SplitContainer p = containers[containers.Count-1];
                p.Panel2Collapsed = false;
                //p.SplitterDistance = 100;
                p.Panel2.Controls.Add(s);
            }

                containers.Add(s);
                s.Dock = DockStyle.Fill;
                try
                {
                    s.Orientation = Orientation.Horizontal;
                }
                catch (Exception e)
                {
                    // FIXME manejo de la distribución de splitters
                    Console.WriteLine(e.Message);
                }
                ZedGraphControl zed = gv.GetGraphControl();
                
                zed.ZoomEvent += new ZedGraphControl.ZoomEventHandler(zed_ZoomEvent);
                //zed.GotFocus += new EventHandler(zed_GotFocus);
                //zed.LostFocus += new EventHandler(zed_LostFocus);
                //zed.Click += new EventHandler(zed_Click);
                
                s.Panel1.Controls.Add(zed);
                s.Panel2Collapsed = true; // DOH?
                
                gv.Render();

            /*
                if (sinchronizer == null)
                {
                    sinchronizer = gv;
                }
                else
                {
                    SynchXAxis(sinchronizer.GetGraphControl(), gv.GetGraphControl());
                }           
             */

                gv.GetGraphControl().Invalidate();
            }

        void zed_Click(object sender, EventArgs e)
        {
            ZedGraphControl zed = (ZedGraphControl)sender;
            zed.Focus();
        }

        void zed_LostFocus(object sender, EventArgs e)
        {
            ZedGraphControl zed = (ZedGraphControl)sender;
            Border b = zed.GraphPane.Border;
            b.Width = 1;
            b.IsVisible = true;


        }

        void zed_GotFocus(object sender, EventArgs e)
        {
            ZedGraphControl zed = (ZedGraphControl)sender;
            Border b = zed.GraphPane.Border;
            b.Style = System.Drawing.Drawing2D.DashStyle.Solid;
           
            b.Width = 2;
            b.IsVisible = true;
            b.Color = System.Drawing.Color.Black;
        }

        public void SynchXAxisTo(GraphView targetGV)
        {
            ZedGraphControl target = targetGV.GetGraphControl();
            sinchronizer = targetGV;
            foreach (GraphView gv in graphViews)
            {
                ZedGraphControl zed = gv.GetGraphControl();
                if (!target.Equals(zed))
                {
                    SynchXAxis(target, zed);
                }

            }

            foreach (GraphView gv in graphViews)
            {
                ZedGraphControl zed = gv.GetGraphControl();
                //zed.AxisChange();
                //zed.Invalidate();
                zed.Refresh();
            }

        }

        

        private void SynchXAxis(ZedGraphControl sender, ZedGraphControl target)
        {
            foreach (GraphPane pane in target.MasterPane.PaneList)
            {
                //Console.WriteLine("Panes:" + sender.MasterPane.PaneList.Count);
                //sender.GraphPane.XAxis.Scale.Clone(zed.GraphPane.XAxis);
                //zed.AxisChange();
                //System.Console.WriteLine("Gotcha " + i.ToString());
                //sender.GraphPane.XAxis.Scale.
                ZedGraphControl.Synchronize(sender.GraphPane.XAxis, pane.XAxis);
            }

        }

        private void SynchWidth(ZedGraphControl sender, ZedGraphControl target)
        {
            foreach (GraphPane pane in target.MasterPane.PaneList)
            {
                
            }
        }

        void zed_ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
        {
            //System.Console.WriteLine("Zoom/Pan");
            //int i = 0;


                double min = sender.GraphPane.XAxis.Scale.Min;
                double max = sender.GraphPane.XAxis.Scale.Max;
                double delta = max - min;
                Console.WriteLine("MIN: " + min + " MAX: " + max);
                Console.WriteLine("DELTA: " + delta);
                Console.WriteLine("BaseTic: " + sender.GraphPane.XAxis.Scale.BaseTic);
                Console.WriteLine("MajorStep: " + sender.GraphPane.XAxis.Scale.MajorStep + " MajorUnit: " + sender.GraphPane.XAxis.Scale.MajorUnit.ToString());
                Console.WriteLine("MinorStep: " + sender.GraphPane.XAxis.Scale.MinorStep + " MinorUnit: " + sender.GraphPane.XAxis.Scale.MinorUnit.ToString());

                GraphPane target = sender.GraphPane;

                if (target.XAxis.Scale.MajorUnit == DateUnit.Day && delta <= 8.0)
                {
                    //target.XAxis.Scale.MajorUnit = DateUnit.Hour;
                    //target.XAxis.Scale.MajorStep = 24;
                    //target.XAxis.Scale.MinorUnit = DateUnit.Day;
                    //target.XAxis.Scale.MajorStep = 0.2;
                    //target.XAxis.Scale.BaseTic = 0.0;
                    //zed.GraphPane.XAxis.Scale.BaseTic = 1.0 / 24.0;

                }
                else
                {
                    //zed.GraphPane.XAxis.Scale.MinorUnit = DateUnit.Day;
                    //zed.GraphPane.XAxis.Scale.BaseTic = 1;

                    //target.XAxis.Scale.MinorUnit = DateUnit.Day;
                    //target.XAxis.Scale.BaseTic = PointPair.Missing;

                }
            

            foreach (GraphView gv in graphViews)
            {
                ZedGraphControl zed = gv.GetGraphControl();

                

                if (sender != zed)
                {

                    SynchXAxis(sender, zed);
                    //zed.AxisChange();
                    //zed.Invalidate();
                    zed.Refresh();
                }
                //i++;
            }
        }

        public void AddSynchedVRuler()
        {
            //Console.WriteLine(graphViews.Count);
            List<Ruler> rulers = new List<Ruler>(0); // FIXME
            foreach (GraphView gv in graphViews)
            {
                foreach (GraphPane gp in gv.PaneList)
                {
                    //GraphPane gp = gv.GraphPane;
                    Ruler r = gp.AddVRuler();
                    r.OnSetPosition += new Ruler.SetPositionEventHanlder(r_OnSetPosition);
                    rulers.Add(r);
                }
            }
            synchedRulers.Add(rulers);
        }

        public void RemoveRulerFromSynched(Ruler target)
        {
            foreach (List<Ruler> rulers in synchedRulers)
            {
                if (rulers.Find( match => match.Equals(target)) != null) {
                    rulers.Remove(target);
                }
            }      
        }

        internal void r_OnSetPosition(Ruler sender)
        {
            foreach (List<Ruler> rulers in synchedRulers)
            {
                if (rulers.Find(ruler => ruler.Equals(sender)) != null)
                {
                    //Console.WriteLine("Ruler found synching");
                    foreach (Ruler r in rulers)
                    {
                        if (sender != r)
                        {
                            r.GXPos = sender.GXPos;
                            //r.GYPos = sender.GYPos; FIXME para otro tipo de rulers no verticales
                        }
                                        
                    }
                    continue;
                }
                
            }
            RedrawRulers(sender);
        }

        public void RedrawRulers(Ruler ignore)
        {
            foreach (GraphView gv in graphViews) {
                Rulers rs = gv.GraphPane.Rulers;
                foreach (Ruler r in rs.VRulers)
                {
                    if (r != ignore)
                    {
                        gv.GetGraphControl().Invalidate(r.OldRect);
                        gv.GetGraphControl().Invalidate(r.Rect);
                    }
                }
            }
        }

        public List<List<Ruler>> SynchedRulers
        {
            get { return synchedRulers; }
        }


        public void HideContainer(int index)
            
        {
            containers[index].Panel1Collapsed = true;
        }

        public void ShowContainer(int index)
        {
            containers[index].Panel1Collapsed = false;
        }

        }


    

    }
