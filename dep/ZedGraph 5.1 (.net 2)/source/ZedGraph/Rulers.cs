using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ZedGraph
{
    public class Rulers
    {
        private List<HRuler> hRulers;
        private List<VRuler> vRulers;

        public List<VRuler> VRulers {
            get { return vRulers; }
        }

        public List<HRuler> HRulers
        {
            get { return hRulers; }
        }

        public Rulers()
        {
            hRulers = new List<HRuler>(0);
            vRulers = new List<VRuler>(0);
        }
        
        public void AddRuler(HRuler hr)
        {
            hRulers.Add(hr);
        }

        public void AddRuler(VRuler vr)
        {
            vRulers.Add(vr);
        }


        public void RemoveRuler(HRuler hr) {
            hRulers.Remove(hr);
        }

        public void RemoveRuler(VRuler vr)
        {
            vRulers.Remove(vr);
        }

        public void Draw(Graphics g, GraphPane pane)
        {
            foreach (HRuler hr in hRulers)
            {
                hr.Draw(g, pane);
            }

            foreach (VRuler vr in vRulers)
            {
                vr.Draw(g, pane);
            }
        }

        public Ruler GetRulerInstanceFromMouse(Point mousePt)
        {
            //Console.WriteLine(mousePt.X + " " + mousePt.Y + "(" + gXpos + " " + gYpos);

            foreach (HRuler hr in hRulers)
            {
                if ((mousePt.Y >= hr.GYPos && mousePt.Y <= (hr.GYPos + hr.Height)) && (mousePt.X == hr.GXPos))
                {
                    return hr;
                }
            }

            foreach (VRuler vr in vRulers)
            {
                if ((mousePt.Y >= vr.GYPos && mousePt.Y <= (vr.GYPos + vr.Height)) && (mousePt.X >= vr.GXPos - 3 && mousePt.X <= vr.GXPos + 3))
                {
                    return vr;
                }
            }

            return null;
        }


    }
}
