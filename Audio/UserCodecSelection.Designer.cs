namespace DSPMediaPlayer
{
    partial class UserCodecSelection
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
            this.OK_button = new System.Windows.Forms.Button();
            this.CodecSelectionComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OK_button
            // 
            this.OK_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OK_button.Location = new System.Drawing.Point(41, 102);
            this.OK_button.Name = "OK_button";
            this.OK_button.Size = new System.Drawing.Size(270, 23);
            this.OK_button.TabIndex = 3;
            this.OK_button.Text = "OK";
            this.OK_button.UseVisualStyleBackColor = true;
            this.OK_button.Click += new System.EventHandler(this.OK_button_Click);
            // 
            // CodecSelectionComboBox
            // 
            this.CodecSelectionComboBox.FormattingEnabled = true;
            this.CodecSelectionComboBox.Location = new System.Drawing.Point(41, 58);
            this.CodecSelectionComboBox.Name = "CodecSelectionComboBox";
            this.CodecSelectionComboBox.Size = new System.Drawing.Size(270, 21);
            this.CodecSelectionComboBox.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(273, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "We didn\'t recognize a valid codec, Please select codec:";
            // 
            // UserCodecSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 162);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CodecSelectionComboBox);
            this.Controls.Add(this.OK_button);
            this.Name = "UserCodecSelection";
            this.Text = "Please select a Codec";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OK_button;
        private System.Windows.Forms.ComboBox CodecSelectionComboBox;
        private System.Windows.Forms.Label label1;
    }
}