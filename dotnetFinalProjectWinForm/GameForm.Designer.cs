namespace dotnetFinalProjectWinForm
{
    partial class GameForm
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
            this.whosTurn = new System.Windows.Forms.Label();
            this.counter = new System.Windows.Forms.Label();
            this.DrawOrPlayBtn = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // whosTurn
            // 
            this.whosTurn.AutoSize = true;
            this.whosTurn.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.whosTurn.Location = new System.Drawing.Point(12, 9);
            this.whosTurn.Name = "whosTurn";
            this.whosTurn.Size = new System.Drawing.Size(0, 29);
            this.whosTurn.TabIndex = 0;
            this.whosTurn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // counter
            // 
            this.counter.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.counter.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.counter.ForeColor = System.Drawing.Color.Black;
            this.counter.Location = new System.Drawing.Point(243, 9);
            this.counter.Name = "counter";
            this.counter.Size = new System.Drawing.Size(275, 50);
            this.counter.TabIndex = 1;
            this.counter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DrawOrPlayBtn
            // 
            this.DrawOrPlayBtn.Location = new System.Drawing.Point(221, 31);
            this.DrawOrPlayBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DrawOrPlayBtn.Name = "DrawOrPlayBtn";
            this.DrawOrPlayBtn.Size = new System.Drawing.Size(82, 44);
            this.DrawOrPlayBtn.TabIndex = 2;
            this.DrawOrPlayBtn.Text = "button1";
            this.DrawOrPlayBtn.UseVisualStyleBackColor = true;
            this.DrawOrPlayBtn.Click += new System.EventHandler(this.DrawOrPlayBtn_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(51, 9);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(19, 10);
            this.button2.TabIndex = 3;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 1017);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.DrawOrPlayBtn);
            this.Controls.Add(this.counter);
            this.Controls.Add(this.whosTurn);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "GameForm";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label whosTurn;
        private System.Windows.Forms.Label counter;
        private System.Windows.Forms.Button DrawOrPlayBtn;
        private System.Windows.Forms.Button button2;
    }
}

