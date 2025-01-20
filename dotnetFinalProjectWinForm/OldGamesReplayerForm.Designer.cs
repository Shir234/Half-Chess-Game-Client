namespace dotnetFinalProjectWinForm
{
    partial class OldGamesReplayerForm
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
            this.SuspendLayout();
            // 
            // OldGamesReplayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 1042);
            this.Name = "OldGamesReplayerForm";
            this.Text = "OldGamesReplayerForm";
            this.Load += new System.EventHandler(this.OldGamesReplayerForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OldGamesReplayerForm_Paint);
            this.ResumeLayout(false);

        }

        #endregion
    }
}