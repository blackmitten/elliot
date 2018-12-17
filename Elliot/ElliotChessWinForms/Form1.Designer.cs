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
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBoxWaitToProceed = new System.Windows.Forms.CheckBox();
            this.buttonInstructToMove = new System.Windows.Forms.Button();
            this.labelWhosTurn = new System.Windows.Forms.Label();
            this.boardControl1 = new Blackmitten.Elliot.WinForms.BoardControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.labelPlayers = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelPlayers);
            this.panel1.Controls.Add(this.checkBoxWaitToProceed);
            this.panel1.Controls.Add(this.buttonInstructToMove);
            this.panel1.Controls.Add(this.labelWhosTurn);
            this.panel1.Controls.Add(this.boardControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 407);
            this.panel1.TabIndex = 1;
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
            // panel2
            // 
            this.panel2.Controls.Add(this.listBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 407);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 159);
            this.panel2.TabIndex = 2;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(800, 159);
            this.listBox1.TabIndex = 0;
            // 
            // labelPlayers
            // 
            this.labelPlayers.AutoSize = true;
            this.labelPlayers.Location = new System.Drawing.Point(431, 228);
            this.labelPlayers.Name = "labelPlayers";
            this.labelPlayers.Size = new System.Drawing.Size(35, 13);
            this.labelPlayers.TabIndex = 4;
            this.labelPlayers.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 566);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Blackmitten.Elliot.WinForms.BoardControl boardControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label labelWhosTurn;
        private System.Windows.Forms.Button buttonInstructToMove;
        private System.Windows.Forms.CheckBox checkBoxWaitToProceed;
        private System.Windows.Forms.Label labelPlayers;
    }
}

