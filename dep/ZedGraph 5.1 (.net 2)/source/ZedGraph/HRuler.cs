using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ZedGraph
{
    public class HRuler: Ruler
    {

        public HRuler(float x, float y):base(x,y)
        {
            
        }
        public override void Draw(System.Drawing.Graphics g, GraphPane pane)
        {
            g.DrawLine(new Pen(Color.Blue), pane.Chart.Rect.X, pane.Chart.Rect.Y, pane.Chart.Rect.X, pane.Chart.Rect.Y + 100); // tengo una liena por pane
        }


                public override void SetNewPos(float x, float y)
                {
                    throw new NotImplementedException();
                }

                public override Rectangle Rect
                {
                    get { throw new NotImplementedException(); }
                }

                public override Rectangle OldRect
                {
                    get { throw new NotImplementedException(); }
                }
    }

}
