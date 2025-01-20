namespace dotnetFinalProjectWinForm
{
    partial class MainMenuForm
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
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.TimerListCombo = new System.Windows.Forms.ComboBox();
            this.playerDetails = new System.Windows.Forms.Label();
            this.AllGames = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.AutoSize = true;
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button2.Location = new System.Drawing.Point(453, 249);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(203, 75);
            this.button2.TabIndex = 14;
            this.button2.Text = "Start Game";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = new System.Drawing.Font("Bahnschrift SemiBold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(453, 145);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 34);
            this.label1.TabIndex = 11;
            this.label1.Text = "Set game timer:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.AntiqueWhite;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Font = new System.Drawing.Font("Bahnschrift Condensed", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.SaddleBrown;
            this.label5.Location = new System.Drawing.Point(77, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 0);
            this.label5.TabIndex = 9;
            this.label5.Text = "Please Log-In";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.MistyRose;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label6.Font = new System.Drawing.Font("Bahnschrift Condensed", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Firebrick;
            this.label6.Location = new System.Drawing.Point(7, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(669, 106);
            this.label6.TabIndex = 8;
            this.label6.Text = "Welcome to Half Chess Game";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TimerListCombo
            // 
            this.TimerListCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TimerListCombo.FormattingEnabled = true;
            this.TimerListCombo.Location = new System.Drawing.Point(453, 199);
            this.TimerListCombo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TimerListCombo.Name = "TimerListCombo";
            this.TimerListCombo.Size = new System.Drawing.Size(203, 24);
            this.TimerListCombo.TabIndex = 15;
            // 
            // playerDetails
            // 
            this.playerDetails.BackColor = System.Drawing.Color.AntiqueWhite;
            this.playerDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.playerDetails.Font = new System.Drawing.Font("Bahnschrift SemiLight Condensed", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playerDetails.ForeColor = System.Drawing.Color.Black;
            this.playerDetails.Location = new System.Drawing.Point(7, 129);
            this.playerDetails.Name = "playerDetails";
            this.playerDetails.Size = new System.Drawing.Size(357, 260);
            this.playerDetails.TabIndex = 16;
            this.playerDetails.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AllGames
            // 
            this.AllGames.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.AllGames.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.AllGames.Location = new System.Drawing.Point(453, 346);
            this.AllGames.Name = "AllGames";
            this.AllGames.Size = new System.Drawing.Size(203, 40);
            this.AllGames.TabIndex = 17;
            this.AllGames.Text = "Show All Games";
            this.AllGames.UseVisualStyleBackColor = true;
            this.AllGames.Click += new System.EventHandler(this.AllGames_Click);
            // 
            // MainMenuForm
            // 
            this.AcceptButton = this.button2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 410);
            this.Controls.Add(this.AllGames);
            this.Controls.Add(this.playerDetails);
            this.Controls.Add(this.TimerListCombo);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainMenuForm";
            this.Text = "MainMenuForm";
            this.Load += new System.EventHandler(this.MainMenuForm_Load);
            this.Shown += new System.EventHandler(this.MainMenuForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox TimerListCombo;
        private System.Windows.Forms.Label playerDetails;
        private System.Windows.Forms.Button AllGames;
    }
}