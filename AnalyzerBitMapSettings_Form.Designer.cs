namespace DSPMediaPlayer
{
    partial class AnalyzerBitMapSettings_Form
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
            this.Analyzer_bitmap_params = new System.Windows.Forms.GroupBox();
            this.Analyzer_PitchSkip_ComboBox = new System.Windows.Forms.ComboBox();
            this.Analyzer_PSNR_TextBox = new System.Windows.Forms.RichTextBox();
            this.Pitch_Skip_Label = new System.Windows.Forms.Label();
            this.Region_PSNR_Threshold_Label = new System.Windows.Forms.Label();
            this.Cancel_button = new System.Windows.Forms.Button();
            this.OK_button = new System.Windows.Forms.Button();
            this.CR_Background_color_Label = new System.Windows.Forms.Label();
            this.Regions_background_level = new System.Windows.Forms.GroupBox();
            this.CR_BackgroundColor_TextBox = new System.Windows.Forms.RichTextBox();
            this.CB_BackgroundColor_TextBox = new System.Windows.Forms.RichTextBox();
            this.CB_Background_color_label = new System.Windows.Forms.Label();
            this.Analyzer_bitmap_params.SuspendLayout();
            this.Regions_background_level.SuspendLayout();
            this.SuspendLayout();
            // 
            // Analyzer_bitmap_params
            // 
            this.Analyzer_bitmap_params.Controls.Add(this.Analyzer_PitchSkip_ComboBox);
            this.Analyzer_bitmap_params.Controls.Add(this.Analyzer_PSNR_TextBox);
            this.Analyzer_bitmap_params.Controls.Add(this.Pitch_Skip_Label);
            this.Analyzer_bitmap_params.Controls.Add(this.Region_PSNR_Threshold_Label);
            this.Analyzer_bitmap_params.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Analyzer_bitmap_params.Location = new System.Drawing.Point(28, 43);
            this.Analyzer_bitmap_params.Name = "Analyzer_bitmap_params";
            this.Analyzer_bitmap_params.Size = new System.Drawing.Size(329, 119);
            this.Analyzer_bitmap_params.TabIndex = 0;
            this.Analyzer_bitmap_params.TabStop = false;
            this.Analyzer_bitmap_params.Text = "Analyzer bitmap params:";
            // 
            // Analyzer_PitchSkip_ComboBox
            // 
            this.Analyzer_PitchSkip_ComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Analyzer_PitchSkip_ComboBox.FormattingEnabled = true;
            this.Analyzer_PitchSkip_ComboBox.Location = new System.Drawing.Point(199, 71);
            this.Analyzer_PitchSkip_ComboBox.Name = "Analyzer_PitchSkip_ComboBox";
            this.Analyzer_PitchSkip_ComboBox.Size = new System.Drawing.Size(78, 24);
            this.Analyzer_PitchSkip_ComboBox.TabIndex = 15;
            this.Analyzer_PitchSkip_ComboBox.SelectedIndexChanged += new System.EventHandler(this.Analyzer_PitchSkip_ComboBox_SelectedIndexChanged);
            // 
            // Analyzer_PSNR_TextBox
            // 
            this.Analyzer_PSNR_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Analyzer_PSNR_TextBox.Location = new System.Drawing.Point(199, 31);
            this.Analyzer_PSNR_TextBox.Multiline = false;
            this.Analyzer_PSNR_TextBox.Name = "Analyzer_PSNR_TextBox";
            this.Analyzer_PSNR_TextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.Analyzer_PSNR_TextBox.Size = new System.Drawing.Size(78, 26);
            this.Analyzer_PSNR_TextBox.TabIndex = 12;
            this.Analyzer_PSNR_TextBox.Text = "";
            // 
            // Pitch_Skip_Label
            // 
            this.Pitch_Skip_Label.AutoSize = true;
            this.Pitch_Skip_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Pitch_Skip_Label.Location = new System.Drawing.Point(25, 71);
            this.Pitch_Skip_Label.Name = "Pitch_Skip_Label";
            this.Pitch_Skip_Label.Size = new System.Drawing.Size(69, 13);
            this.Pitch_Skip_Label.TabIndex = 1;
            this.Pitch_Skip_Label.Text = "Pitch Skip:";
            // 
            // Region_PSNR_Threshold_Label
            // 
            this.Region_PSNR_Threshold_Label.AutoSize = true;
            this.Region_PSNR_Threshold_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Region_PSNR_Threshold_Label.Location = new System.Drawing.Point(25, 31);
            this.Region_PSNR_Threshold_Label.Name = "Region_PSNR_Threshold_Label";
            this.Region_PSNR_Threshold_Label.Size = new System.Drawing.Size(149, 13);
            this.Region_PSNR_Threshold_Label.TabIndex = 0;
            this.Region_PSNR_Threshold_Label.Text = "Region PSNR Threshold:";
            // 
            // Cancel_button
            // 
            this.Cancel_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancel_button.Location = new System.Drawing.Point(187, 346);
            this.Cancel_button.Name = "Cancel_button";
            this.Cancel_button.Size = new System.Drawing.Size(75, 23);
            this.Cancel_button.TabIndex = 5;
            this.Cancel_button.Text = "Cancel";
            this.Cancel_button.UseVisualStyleBackColor = true;
            this.Cancel_button.Click += new System.EventHandler(this.Cancel_button_Click);
            // 
            // OK_button
            // 
            this.OK_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OK_button.Location = new System.Drawing.Point(84, 346);
            this.OK_button.Name = "OK_button";
            this.OK_button.Size = new System.Drawing.Size(75, 23);
            this.OK_button.TabIndex = 4;
            this.OK_button.Text = "OK";
            this.OK_button.UseVisualStyleBackColor = true;
            this.OK_button.Click += new System.EventHandler(this.OK_button_Click);
            // 
            // CR_Background_color_Label
            // 
            this.CR_Background_color_Label.AutoSize = true;
            this.CR_Background_color_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CR_Background_color_Label.Location = new System.Drawing.Point(25, 71);
            this.CR_Background_color_Label.Name = "CR_Background_color_Label";
            this.CR_Background_color_Label.Size = new System.Drawing.Size(128, 13);
            this.CR_Background_color_Label.TabIndex = 1;
            this.CR_Background_color_Label.Text = "CR Background color";
            // 
            // Regions_background_level
            // 
            this.Regions_background_level.Controls.Add(this.CR_BackgroundColor_TextBox);
            this.Regions_background_level.Controls.Add(this.CB_BackgroundColor_TextBox);
            this.Regions_background_level.Controls.Add(this.CR_Background_color_Label);
            this.Regions_background_level.Controls.Add(this.CB_Background_color_label);
            this.Regions_background_level.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Regions_background_level.Location = new System.Drawing.Point(28, 198);
            this.Regions_background_level.Name = "Regions_background_level";
            this.Regions_background_level.Size = new System.Drawing.Size(329, 119);
            this.Regions_background_level.TabIndex = 6;
            this.Regions_background_level.TabStop = false;
            this.Regions_background_level.Text = "Regions background level:";
            // 
            // CR_BackgroundColor_TextBox
            // 
            this.CR_BackgroundColor_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CR_BackgroundColor_TextBox.Location = new System.Drawing.Point(199, 71);
            this.CR_BackgroundColor_TextBox.Multiline = false;
            this.CR_BackgroundColor_TextBox.Name = "CR_BackgroundColor_TextBox";
            this.CR_BackgroundColor_TextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.CR_BackgroundColor_TextBox.Size = new System.Drawing.Size(78, 26);
            this.CR_BackgroundColor_TextBox.TabIndex = 13;
            this.CR_BackgroundColor_TextBox.Text = "";
            // 
            // CB_BackgroundColor_TextBox
            // 
            this.CB_BackgroundColor_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CB_BackgroundColor_TextBox.Location = new System.Drawing.Point(199, 31);
            this.CB_BackgroundColor_TextBox.Multiline = false;
            this.CB_BackgroundColor_TextBox.Name = "CB_BackgroundColor_TextBox";
            this.CB_BackgroundColor_TextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.CB_BackgroundColor_TextBox.Size = new System.Drawing.Size(78, 26);
            this.CB_BackgroundColor_TextBox.TabIndex = 12;
            this.CB_BackgroundColor_TextBox.Text = "";
            // 
            // CB_Background_color_label
            // 
            this.CB_Background_color_label.AutoSize = true;
            this.CB_Background_color_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CB_Background_color_label.Location = new System.Drawing.Point(25, 31);
            this.CB_Background_color_label.Name = "CB_Background_color_label";
            this.CB_Background_color_label.Size = new System.Drawing.Size(131, 13);
            this.CB_Background_color_label.TabIndex = 0;
            this.CB_Background_color_label.Text = "CB Background color:";
            // 
            // AnalyzerBitMapSettings_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 394);
            this.Controls.Add(this.Regions_background_level);
            this.Controls.Add(this.Cancel_button);
            this.Controls.Add(this.OK_button);
            this.Controls.Add(this.Analyzer_bitmap_params);
            this.Name = "AnalyzerBitMapSettings_Form";
            this.Text = "AnalyzerBitMapSettings_Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AnalyzerBitMapSettings_Form_FormClosing);
            this.Analyzer_bitmap_params.ResumeLayout(false);
            this.Analyzer_bitmap_params.PerformLayout();
            this.Regions_background_level.ResumeLayout(false);
            this.Regions_background_level.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox Analyzer_bitmap_params;
        private System.Windows.Forms.Label Pitch_Skip_Label;
        private System.Windows.Forms.Label Region_PSNR_Threshold_Label;
        public System.Windows.Forms.RichTextBox Analyzer_PSNR_TextBox;
        public System.Windows.Forms.ComboBox Analyzer_PitchSkip_ComboBox;
        private System.Windows.Forms.Button Cancel_button;
        private System.Windows.Forms.Button OK_button;
        private System.Windows.Forms.Label CR_Background_color_Label;
        private System.Windows.Forms.GroupBox Regions_background_level;
        public System.Windows.Forms.RichTextBox CB_BackgroundColor_TextBox;
        private System.Windows.Forms.Label CB_Background_color_label;
        public System.Windows.Forms.RichTextBox CR_BackgroundColor_TextBox;
    }
}