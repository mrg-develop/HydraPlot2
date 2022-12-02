using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ZedGraph
{
    public abstract class Ruler
    {
        public struct RulerIntersection
        {
            public string label;
            public float gXpos;
            public float gYpos;
            public double? yValue;
            public Color color;
            public CurveItem curve;
        }

        protected float gXpos;
        protected float gYpos;
        protected float oldGXpos;
        protected float oldGYpos;
        protected float width;
        protected float height;

        public delegate void SetPositionEventHanlder(Ruler sender);
        public virtual event SetPositionEventHanlder OnSetPosition;


        public Ruler(float x, float y)
        {
            gXpos = x;
            oldGXpos = x;
            gYpos = y;
            oldGYpos = y;
        }

        public float GYPos {
            get { return gYpos; }
            set {
                oldGYpos = gYpos;
                gYpos = value;
            }
        }

        public float GXPos
        {
            get { return gXpos; }
            set {
                oldGXpos = gXpos;
                gXpos = value; 
            }
        }

        public float Height
        {
            get { return height; }
        }

        public abstract void Draw(Graphics g, GraphPane pane);

        public virtual void SetNewPos(float x, float y)
        {
            //Console.WriteLine("new pos!");
            oldGXpos = gXpos;
            oldGYpos = gYpos;
            
        }

        public bool IsMouseOverRuler(Point mousePt)
        {
            //Console.WriteLine(mousePt.X + " " + mousePt.Y + "(" + gXpos + " " + gYpos);

            if ( (mousePt.Y >= gYpos && mousePt.Y <= (gYpos + height) ) && (mousePt.X >= gXpos-3 && mousePt.X <= gXpos+3 ))
            {
                return true;
            }
            return false;
        }

        public abstract Rectangle Rect
        {
            get;
        }

        public abstract Rectangle OldRect
        {
            get;
        }
        

    }
}
