using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace HydraPlot2
{
    class WorkAreaContainer : TableLayoutPanel
    {
        private int currentRow = 0;
        private int rowCount = 0;

        public void AddGraph(GraphView gv)
        {
            ZedGraphControl zg = gv.GetGraphControl(); 

            /*
            if (currentRow == 0) { // la primera fila no está usada
                Controls.Add(zg, 0, currentRow);
                
            }*/
            Controls.Add(zg, 0, currentRow);
            currentRow++; // actualizamos la cuenta
            rowCount++;

            // resize
            RowStyles.Clear();
            gv.Render();
        }

        public int Rows {
            get
            {
                return rowCount;
            }
        }

    }
}
