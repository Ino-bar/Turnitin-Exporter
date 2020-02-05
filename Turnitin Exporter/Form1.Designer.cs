namespace Turnitin_Exporter
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
            this.ChooseFolderButton = new System.Windows.Forms.Button();
            this.ChooseFolderLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ChooseFolderButton
            // 
            this.ChooseFolderButton.Location = new System.Drawing.Point(23, 48);
            this.ChooseFolderButton.Name = "ChooseFolderButton";
            this.ChooseFolderButton.Size = new System.Drawing.Size(75, 23);
            this.ChooseFolderButton.TabIndex = 0;
            this.ChooseFolderButton.Text = "Choose";
            this.ChooseFolderButton.UseVisualStyleBackColor = true;
            this.ChooseFolderButton.Click += new System.EventHandler(this.ChooseFolderButton_Click);
            // 
            // ChooseFolderLabel
            // 
            this.ChooseFolderLabel.AutoSize = true;
            this.ChooseFolderLabel.Location = new System.Drawing.Point(20, 20);
            this.ChooseFolderLabel.Name = "ChooseFolderLabel";
            this.ChooseFolderLabel.Size = new System.Drawing.Size(131, 13);
            this.ChooseFolderLabel.TabIndex = 1;
            this.ChooseFolderLabel.Text = "Choose file folder location:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 450);
            this.Controls.Add(this.ChooseFolderLabel);
            this.Controls.Add(this.ChooseFolderButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ChooseFolderButton;
        private System.Windows.Forms.Label ChooseFolderLabel;
    }
}

