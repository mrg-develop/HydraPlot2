namespace HydraPlot2
{
    partial class GraphWorkspace
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.waterGraphTab = new System.Windows.Forms.TabPage();
            this.salinityGraphTab = new System.Windows.Forms.TabPage();
            this.temperatureGraphTab = new System.Windows.Forms.TabPage();
            this.waterSalinityGraphTab = new System.Windows.Forms.TabPage();
            this.tabControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.waterGraphTab);
            this.tabControl3.Controls.Add(this.salinityGraphTab);
            this.tabControl3.Controls.Add(this.temperatureGraphTab);
            this.tabControl3.Controls.Add(this.waterSalinityGraphTab);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(0, 0);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(347, 390);
            this.tabControl3.TabIndex = 1;
            this.tabControl3.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl3_Selected);
            this.tabControl3.SelectedIndexChanged += new System.EventHandler(this.tabControl3_SelectedIndexChanged);
            // 
            // waterGraphTab
            // 
            this.waterGraphTab.Location = new System.Drawing.Point(4, 22);
            this.waterGraphTab.Name = "waterGraphTab";
            this.waterGraphTab.Padding = new System.Windows.Forms.Padding(3);
            this.waterGraphTab.Size = new System.Drawing.Size(339, 364);
            this.waterGraphTab.TabIndex = 0;
            this.waterGraphTab.Text = "Contenido de Agua";
            this.waterGraphTab.UseVisualStyleBackColor = true;
            // 
            // salinityGraphTab
            // 
            this.salinityGraphTab.Location = new System.Drawing.Point(4, 22);
            this.salinityGraphTab.Name = "salinityGraphTab";
            this.salinityGraphTab.Size = new System.Drawing.Size(339, 364);
            this.salinityGraphTab.TabIndex = 1;
            this.salinityGraphTab.Text = "Conductividad Eléctrica";
            this.salinityGraphTab.UseVisualStyleBackColor = true;
            // 
            // temperatureGraphTab
            // 
            this.temperatureGraphTab.Location = new System.Drawing.Point(4, 22);
            this.temperatureGraphTab.Name = "temperatureGraphTab";
            this.temperatureGraphTab.Size = new System.Drawing.Size(339, 364);
            this.temperatureGraphTab.TabIndex = 2;
            this.temperatureGraphTab.Text = "Temperatura";
            this.temperatureGraphTab.UseVisualStyleBackColor = true;
            // 
            // waterSalinityGraphTab
            // 
            this.waterSalinityGraphTab.Location = new System.Drawing.Point(4, 22);
            this.waterSalinityGraphTab.Name = "waterSalinityGraphTab";
            this.waterSalinityGraphTab.Size = new System.Drawing.Size(339, 364);
            this.waterSalinityGraphTab.TabIndex = 3;
            this.waterSalinityGraphTab.Text = "Contenido de Agua y Conductividad Eléctrica";
            this.waterSalinityGraphTab.UseVisualStyleBackColor = true;
            // 
            // GraphWorkspace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl3);
            this.Name = "GraphWorkspace";
            this.Size = new System.Drawing.Size(347, 390);
            this.Resize += new System.EventHandler(this.GraphWorkspace_Resize);
            this.tabControl3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage salinityGraphTab;
        private System.Windows.Forms.TabPage temperatureGraphTab;
        private System.Windows.Forms.TabPage waterGraphTab;
        private System.Windows.Forms.TabPage waterSalinityGraphTab;
    }
}
