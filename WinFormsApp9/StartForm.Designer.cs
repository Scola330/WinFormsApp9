namespace WinFormsApp9
{
    partial class StartForm
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
            StartButton = new Button();
            SuspendLayout();
            // 
            // StartButton
            // 
            StartButton.BackColor = Color.Black;
            StartButton.Cursor = Cursors.Cross;
            StartButton.ForeColor = Color.White;
            StartButton.Location = new Point(135, 256);
            StartButton.Margin = new Padding(0);
            StartButton.Name = "StartButton";
            StartButton.Size = new Size(516, 107);
            StartButton.TabIndex = 0;
            StartButton.Text = "START";
            StartButton.UseVisualStyleBackColor = false;
            StartButton.Click += StartButton_Click;
            // 
            // StartForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(800, 450);
            Controls.Add(StartButton);
            Cursor = Cursors.Cross;
            Name = "StartForm";
            Text = "StartForm";
            ResumeLayout(false);
        }

        #endregion

        private Button StartButton;
    }
}