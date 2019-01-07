namespace BlackMitten.Elliot.Winforms
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxFen = new System.Windows.Forms.TextBox();
            this.labelMoveNumber = new System.Windows.Forms.Label();
            this.labelPlayers = new System.Windows.Forms.Label();
            this.checkBoxWaitToProceed = new System.Windows.Forms.CheckBox();
            this.buttonInstructToMove = new System.Windows.Forms.Button();
            this.labelWhosTurn = new System.Windows.Forms.Label();
            this.boardControl1 = new Blackmitten.Elliot.WinForms.BoardControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNewGame = new System.Windows.Forms.ToolStripButton();
            this.panel1.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBoxFen);
            this.panel1.Controls.Add(this.labelMoveNumber);
            this.panel1.Controls.Add(this.labelPlayers);
            this.panel1.Controls.Add(this.checkBoxWaitToProceed);
            this.panel1.Controls.Add(this.buttonInstructToMove);
            this.panel1.Controls.Add(this.labelWhosTurn);
            this.panel1.Controls.Add(this.boardControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(894, 476);
            this.panel1.TabIndex = 1;
            // 
            // textBoxFen
            // 
            this.textBoxFen.Location = new System.Drawing.Point(431, 249);
            this.textBoxFen.Name = "textBoxFen";
            this.textBoxFen.ReadOnly = true;
            this.textBoxFen.Size = new System.Drawing.Size(329, 20);
            this.textBoxFen.TabIndex = 6;
            // 
            // labelMoveNumber
            // 
            this.labelMoveNumber.AutoSize = true;
            this.labelMoveNumber.Location = new System.Drawing.Point(428, 211);
            this.labelMoveNumber.Name = "labelMoveNumber";
            this.labelMoveNumber.Size = new System.Drawing.Size(43, 13);
            this.labelMoveNumber.TabIndex = 5;
            this.labelMoveNumber.Text = "Move 1";
            // 
            // labelPlayers
            // 
            this.labelPlayers.AutoSize = true;
            this.labelPlayers.Location = new System.Drawing.Point(428, 175);
            this.labelPlayers.Name = "labelPlayers";
            this.labelPlayers.Size = new System.Drawing.Size(154, 13);
            this.labelPlayers.TabIndex = 4;
            this.labelPlayers.Text = "White human vs back machine";
            // 
            // checkBoxWaitToProceed
            // 
            this.checkBoxWaitToProceed.AutoSize = true;
            this.checkBoxWaitToProceed.Location = new System.Drawing.Point(431, 128);
            this.checkBoxWaitToProceed.Name = "checkBoxWaitToProceed";
            this.checkBoxWaitToProceed.Size = new System.Drawing.Size(102, 17);
            this.checkBoxWaitToProceed.TabIndex = 3;
            this.checkBoxWaitToProceed.Text = "Wait to proceed";
            this.checkBoxWaitToProceed.UseVisualStyleBackColor = true;
            // 
            // buttonInstructToMove
            // 
            this.buttonInstructToMove.Location = new System.Drawing.Point(431, 75);
            this.buttonInstructToMove.Name = "buttonInstructToMove";
            this.buttonInstructToMove.Size = new System.Drawing.Size(106, 35);
            this.buttonInstructToMove.TabIndex = 2;
            this.buttonInstructToMove.Text = "Proceed with move";
            this.buttonInstructToMove.UseVisualStyleBackColor = true;
            this.buttonInstructToMove.Click += new System.EventHandler(this.buttonInstructToMove_Click);
            // 
            // labelWhosTurn
            // 
            this.labelWhosTurn.AutoSize = true;
            this.labelWhosTurn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWhosTurn.Location = new System.Drawing.Point(427, 35);
            this.labelWhosTurn.Name = "labelWhosTurn";
            this.labelWhosTurn.Size = new System.Drawing.Size(110, 24);
            this.labelWhosTurn.TabIndex = 1;
            this.labelWhosTurn.Text = "Who\'s turn?";
            // 
            // boardControl1
            // 
            this.boardControl1.Board = null;
            this.boardControl1.Location = new System.Drawing.Point(3, 3);
            this.boardControl1.Name = "boardControl1";
            this.boardControl1.Size = new System.Drawing.Size(407, 404);
            this.boardControl1.TabIndex = 0;
            this.boardControl1.WaitingForBlackHuman = false;
            this.boardControl1.WaitingForWhiteHuman = false;
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.panel1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(894, 476);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(894, 515);
            this.toolStripContainer1.TabIndex = 2;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNewGame});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(79, 39);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripButtonNewGame
            // 
            this.toolStripButtonNewGame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNewGame.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNewGame.Image")));
            this.toolStripButtonNewGame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNewGame.Name = "toolStripButtonNewGame";
            this.toolStripButtonNewGame.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonNewGame.Text = "toolStripButton1";
            this.toolStripButtonNewGame.Click += new System.EventHandler(this.toolStripButtonNewGame_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 515);
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Blackmitten.Elliot.WinForms.BoardControl boardControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label labelWhosTurn;
        private System.Windows.Forms.Button buttonInstructToMove;
        private System.Windows.Forms.CheckBox checkBoxWaitToProceed;
        private System.Windows.Forms.Label labelPlayers;
        private System.Windows.Forms.Label labelMoveNumber;
        private System.Windows.Forms.TextBox textBoxFen;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonNewGame;
    }
}

