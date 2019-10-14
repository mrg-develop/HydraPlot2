namespace HydraPlot2
{
    partial class ImporterMonitor
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pbFile = new System.Windows.Forms.ProgressBar();
            this.linkReport = new System.Windows.Forms.LinkLabel();
            this.pbLine = new System.Windows.Forms.ProgressBar();
            this.lblFileProgress = new System.Windows.Forms.Label();
            this.lblLineProgress = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblInvalidos = new System.Windows.Forms.Label();
            this.lblDuplicados = new System.Windows.Forms.Label();
            this.lblInsertados = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lblOperationStatus = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbFile
            // 
            this.pbFile.Location = new System.Drawing.Point(12, 61);
            this.pbFile.Name = "pbFile";
            this.pbFile.Size = new System.Drawing.Size(266, 34);
            this.pbFile.TabIndex = 0;
            // 
            // linkReport
            // 
            this.linkReport.AutoSize = true;
            this.linkReport.Location = new System.Drawing.Point(12, 303);
            this.linkReport.Name = "linkReport";
            this.linkReport.Size = new System.Drawing.Size(60, 13);
            this.linkReport.TabIndex = 4;
            this.linkReport.TabStop = true;
            this.linkReport.Text = "Ver informe";
            this.linkReport.Visible = false;
            // 
            // pbLine
            // 
            this.pbLine.Location = new System.Drawing.Point(12, 133);
            this.pbLine.Name = "pbLine";
            this.pbLine.Size = new System.Drawing.Size(266, 12);
            this.pbLine.TabIndex = 5;
            // 
            // lblFileProgress
            // 
            this.lblFileProgress.Location = new System.Drawing.Point(12, 45);
            this.lblFileProgress.Name = "lblFileProgress";
            this.lblFileProgress.Size = new System.Drawing.Size(266, 13);
            this.lblFileProgress.TabIndex = 6;
            this.lblFileProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLineProgress
            // 
            this.lblLineProgress.Location = new System.Drawing.Point(12, 117);
            this.lblLineProgress.Name = "lblLineProgress";
            this.lblLineProgress.Size = new System.Drawing.Size(266, 13);
            this.lblLineProgress.TabIndex = 7;
            this.lblLineProgress.Text = "0%";
            this.lblLineProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(11, 168);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(266, 104);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Estadísticas";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.78724F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.21276F));
            this.tableLayoutPanel1.Controls.Add(this.lblTotal, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblInvalidos, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblDuplicados, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblInsertados, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(57, 13);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(148, 78);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // lblInvalidos
            // 
            this.lblInvalidos.AutoSize = true;
            this.lblInvalidos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInvalidos.Location = new System.Drawing.Point(76, 38);
            this.lblInvalidos.Name = "lblInvalidos";
            this.lblInvalidos.Size = new System.Drawing.Size(69, 19);
            this.lblInvalidos.TabIndex = 5;
            this.lblInvalidos.Text = "0";
            this.lblInvalidos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDuplicados
            // 
            this.lblDuplicados.AutoSize = true;
            this.lblDuplicados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDuplicados.Location = new System.Drawing.Point(76, 19);
            this.lblDuplicados.Name = "lblDuplicados";
            this.lblDuplicados.Size = new System.Drawing.Size(69, 19);
            this.lblDuplicados.TabIndex = 3;
            this.lblDuplicados.Text = "0";
            this.lblDuplicados.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblInsertados
            // 
            this.lblInsertados.AutoSize = true;
            this.lblInsertados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInsertados.Location = new System.Drawing.Point(76, 0);
            this.lblInsertados.Name = "lblInsertados";
            this.lblInsertados.Size = new System.Drawing.Size(69, 19);
            this.lblInsertados.TabIndex = 1;
            this.lblInsertados.Text = "0";
            this.lblInsertados.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Insertados:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(197, 297);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 24);
            this.button1.TabIndex = 3;
            this.button1.Text = "Cancelar";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // lblOperationStatus
            // 
            this.lblOperationStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOperationStatus.Location = new System.Drawing.Point(13, 9);
            this.lblOperationStatus.Name = "lblOperationStatus";
            this.lblOperationStatus.Size = new System.Drawing.Size(265, 27);
            this.lblOperationStatus.TabIndex = 10;
            this.lblOperationStatus.Text = "label2";
            this.lblOperationStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(3, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 19);
            this.label3.TabIndex = 2;
            this.label3.Text = "Duplicados:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(3, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 19);
            this.label5.TabIndex = 4;
            this.label5.Text = "Inválidos:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Blue;
            this.label7.Location = new System.Drawing.Point(3, 57);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 21);
            this.label7.TabIndex = 6;
            this.label7.Text = "Total:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotal.Location = new System.Drawing.Point(76, 57);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(69, 21);
            this.lblTotal.TabIndex = 7;
            this.lblTotal.Text = "0";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ImporterMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 333);
            this.ControlBox = false;
            this.Controls.Add(this.lblOperationStatus);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblLineProgress);
            this.Controls.Add(this.lblFileProgress);
            this.Controls.Add(this.pbLine);
            this.Controls.Add(this.linkReport);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pbFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimizeBox = false;
            this.Name = "ImporterMonitor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Importando datos...";
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pbFile;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.LinkLabel linkReport;
        private System.Windows.Forms.ProgressBar pbLine;
        private System.Windows.Forms.Label lblFileProgress;
        private System.Windows.Forms.Label lblLineProgress;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblInvalidos;
        private System.Windows.Forms.Label lblDuplicados;
        private System.Windows.Forms.Label lblInsertados;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblOperationStatus;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;

    }
}