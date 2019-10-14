using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ZedGraph
{
    public class VRuler: Ruler
    {
        private int dataBoxWidth = 100;
        private int dataBoxHeight = 14;

        public VRuler(float x, float y):base(x,y)
        {
        }

        public override event Ruler.SetPositionEventHanlder OnSetPosition;

        /*public float XPos { 
            get { return xpos; }
            set { xpos = value; } 
        }*/

        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        public override void Draw(System.Drawing.Graphics g, GraphPane pane)
        {
            g.DrawLine(new Pen(Color.Blue), gXpos, gYpos, gXpos, gYpos + pane.Chart.Rect.Height); // tengo una liena por pane
            //gXpos = pane.Chart.Rect.X + xpos;
            //gYpos = pane.Chart.Rect.Y;
            height = pane.Chart.Rect.Height;

            Point[] upArrow = { new Point((int)gXpos, (int)gYpos), new Point((int)gXpos - 5, (int)gYpos - 5), new Point((int)gXpos + 5, (int)gYpos - 5) };
            //Point[] downArrow = { new Point((int)gXpos, (int)(gYpos+height)), new Point((int)gXpos - 5, (int)(gYpos + height + 5)), new Point((int)gXpos + 5, (int)(gYpos + height + 5)) };
            g.FillPolygon(Brushes.Blue, upArrow);
            //g.FillPolygon(Brushes.Blue, downArrow);
            buildDatabox(g, pane);
            buildDeltaBoxes(g, pane);
        }

        // build 

        private void buildDatabox(System.Drawing.Graphics g, GraphPane pane)
        {
            //pane.GetValuedXForRuler(this);
            float minpix = pane.XAxis.Scale._minPix;
            float maxpix = pane.XAxis.Scale._maxPix;
            double current = pane.XAxis.Scale.ReverseTransform(gXpos);
            string dateLabel = XDate.ToString(current, "ddd dd MM yyyy HH:mm");
            
            //g.DrawString(minpix.ToString() + " " + maxpix.ToString(), new Font("Arial", 8), Brushes.Black, new PointF(gXpos - 10, gYpos - 17));
            double xdelta = pane.GetXDelta(this);
            string xdeltaString;
            if (xdelta == 0)
            {
                xdeltaString = "";
            }
            else if (xdelta < 1)
            {
                xdeltaString = "Δ " + String.Format("{0:0.0}", xdelta*24) + " h";
            }
            else 
            {
                xdeltaString = "Δ " + String.Format("{0:0.0}", xdelta) + " d";
            }
            SizeF s = g.MeasureString(dateLabel + " " + xdeltaString, new Font("Arial", 8));
            dataBoxWidth = (int)Math.Round(s.Width,0);
            int xpos = (int)Math.Round(gXpos);
            int ypos = (int)Math.Round(gYpos);
            g.DrawRectangle(new Pen(Color.Black), xpos - 10, ypos - 17, dataBoxWidth, dataBoxHeight);
            g.FillRectangle(Brushes.Wheat, xpos - 9, ypos - 16, dataBoxWidth-1, dataBoxHeight-1);
            g.DrawString(dateLabel + " " + xdeltaString, new Font("Arial", 8), Brushes.Black, new PointF(xpos - 9, ypos - 16));
        }

        private void buildDeltaBoxes(System.Drawing.Graphics g, GraphPane pane)
        {
            List<Ruler.RulerIntersection> yIntersections = pane.GetYIntersections(this);
            int xpos; 
            int ypos;
            int deltaBoxWidth = 30;
            int deltaBoxHeight = 12;
            double ydelta = 0;
            string ydeltaString;

            foreach (RulerIntersection intersection in yIntersections)
            {
            
                if (intersection.yValue != null)
                {                    
                    xpos = (int)Math.Round(gXpos);
                    ypos = (int)Math.Round(intersection.gYpos);
                    ydelta = pane.GetYDelta(this, intersection.curve);
                    if (ydelta == 0)
                    {
                        ydeltaString = "";
                    }
                    else if (ydelta > 0)
                    {
                        ydeltaString = "Δ +" + String.Format("{0:0.000}", ydelta);
                    }
                    else
                    {
                        ydeltaString = "Δ -" + String.Format("{0:0.000}", ydelta);
                    }
                    string deltaString = String.Format("{0:0.000}", intersection.yValue) + " " + ydeltaString;
                    SizeF s = g.MeasureString(deltaString, new Font("Arial", 7.5f));
                    deltaBoxWidth = (int)Math.Round(s.Width, 0);
                    g.DrawRectangle(new Pen(Color.Black),xpos, ypos, deltaBoxWidth, deltaBoxHeight);
                    g.FillRectangle(Brushes.Wheat, xpos + 1, ypos + 1, deltaBoxWidth-1, deltaBoxHeight-1);
                    g.DrawString(deltaString, new Font("Arial", 7.5f), new SolidBrush(intersection.color), new PointF(xpos, ypos));
                }
            }
        }


        public override void SetNewPos(float x, float y)
        {
            base.SetNewPos(x, y);
            oldGXpos = gXpos;
            gXpos = x;


            OnSetPosition(this);
        }

        public override Rectangle Rect
        {
            //get { return new Rectangle((int)this.GXPos - 5, (int)this.GYPos - 5, 10, (int)this.Height + 10); }
            get { return new Rectangle((int)this.GXPos - 10, (int)this.GYPos - 18, (int)dataBoxWidth+8, (int)this.Height + 18); }
        }

        public override Rectangle OldRect
        {
            //get { return new Rectangle((int)this.GXPos - 10, (int)this.GYPos - 15, 20, (int)this.Height + 15); }
            get { return new Rectangle((int)this.oldGXpos - 10, (int)this.oldGYpos - 18, (int)dataBoxWidth+8, (int)this.Height + 18); }
        }
    }
}
