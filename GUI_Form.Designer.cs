namespace DSPMediaPlayer
{
    partial class DSPMediaPlayer_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DSPMediaPlayer_Form));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.threadsInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pcapFileHandlerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.codecTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.h264ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.h263ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vp8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCleanFrameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveRawFrameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveImageafterDecoderDisplayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuEncode = new System.Windows.Forms.ToolStripMenuItem();
            this.codecTypeToolStripMenuEncodeCodecType = new System.Windows.Forms.ToolStripMenuItem();
            this.h264ToolStripMenuEncode = new System.Windows.Forms.ToolStripMenuItem();
            this.h263ToolStripMenuEncode = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.view11ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extraInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.overlayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorSpacesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showYToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showUToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rOIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableDisableROIGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.analyzerBitMapSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.audioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.linearSampleRateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Linear8000 = new System.Windows.Forms.ToolStripMenuItem();
            this.Linear16000 = new System.Windows.Forms.ToolStripMenuItem();
            this.Linear32000 = new System.Windows.Forms.ToolStripMenuItem();
            this.Linear48000 = new System.Windows.Forms.ToolStripMenuItem();
            this.Linear64000 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.versionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CodecTypeLabel = new System.Windows.Forms.Label();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.errorLogTextBox = new System.Windows.Forms.TextBox();
            this.ExtraInfoTreeView = new System.Windows.Forms.TreeView();
            this.versionLabel = new System.Windows.Forms.Label();
            this.Stop_button = new System.Windows.Forms.Button();
            this.Forward_button = new System.Windows.Forms.Button();
            this.Rewind_button = new System.Windows.Forms.Button();
            this.Pause_button = new System.Windows.Forms.Button();
            this.Play_button = new System.Windows.Forms.Button();
            this.Browse_Button = new System.Windows.Forms.Button();
            this.Display_pictureBox = new System.Windows.Forms.PictureBox();
            this.imagePanel1 = new DSPMediaPlayer.ImagePanel();
            this.waveControl_UI = new DSPMediaPlayer.Audio.WaveControl_UI();
            this.openSharpFramesFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Display_pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(132, 34);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(510, 43);
            this.richTextBox1.TabIndex = 28;
            this.richTextBox1.Text = "";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.overlayToolStripMenuItem,
            this.audioToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1344, 24);
            this.menuStrip1.TabIndex = 34;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.threadsInfoToolStripMenuItem,
            this.pcapFileHandlerToolStripMenuItem,
            this.saveCleanFrameToolStripMenuItem,
            this.saveRawFrameToolStripMenuItem,
            this.saveImageafterDecoderDisplayToolStripMenuItem,
            this.toolStripMenuEncode});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // threadsInfoToolStripMenuItem
            // 
            this.threadsInfoToolStripMenuItem.Name = "threadsInfoToolStripMenuItem";
            this.threadsInfoToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.threadsInfoToolStripMenuItem.Text = "ThreadsInfo";
            this.threadsInfoToolStripMenuItem.Click += new System.EventHandler(this.threadsInfoToolStripMenuItem_Click);
            // 
            // pcapFileHandlerToolStripMenuItem
            // 
            this.pcapFileHandlerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.codecTypeToolStripMenuItem});
            this.pcapFileHandlerToolStripMenuItem.Name = "pcapFileHandlerToolStripMenuItem";
            this.pcapFileHandlerToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.pcapFileHandlerToolStripMenuItem.Text = "Pcap File Handler";
            // 
            // codecTypeToolStripMenuItem
            // 
            this.codecTypeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.h264ToolStripMenuItem,
            this.h263ToolStripMenuItem,
            this.vp8ToolStripMenuItem});
            this.codecTypeToolStripMenuItem.Name = "codecTypeToolStripMenuItem";
            this.codecTypeToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.codecTypeToolStripMenuItem.Text = "CodecType:";
            // 
            // h264ToolStripMenuItem
            // 
            this.h264ToolStripMenuItem.Name = "h264ToolStripMenuItem";
            this.h264ToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.h264ToolStripMenuItem.Text = "H264";
            this.h264ToolStripMenuItem.Click += new System.EventHandler(this.h264ToolStripMenuItem_Click);
            // 
            // h263ToolStripMenuItem
            // 
            this.h263ToolStripMenuItem.Name = "h263ToolStripMenuItem";
            this.h263ToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.h263ToolStripMenuItem.Text = "H263";
            this.h263ToolStripMenuItem.Click += new System.EventHandler(this.h263ToolStripMenuItem_Click);
            // 
            // vp8ToolStripMenuItem
            // 
            this.vp8ToolStripMenuItem.Name = "vp8ToolStripMenuItem";
            this.vp8ToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.vp8ToolStripMenuItem.Text = "VP8";
            this.vp8ToolStripMenuItem.Click += new System.EventHandler(this.vp8ToolStripMenuItem_Click);
            // 
            // saveCleanFrameToolStripMenuItem
            // 
            this.saveCleanFrameToolStripMenuItem.Name = "saveCleanFrameToolStripMenuItem";
            this.saveCleanFrameToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.saveCleanFrameToolStripMenuItem.Text = "Save Clean Frame (after RTP)";
            this.saveCleanFrameToolStripMenuItem.Click += new System.EventHandler(this.saveCleanFrameToolStripMenuItem_Click);
            // 
            // saveRawFrameToolStripMenuItem
            // 
            this.saveRawFrameToolStripMenuItem.Name = "saveRawFrameToolStripMenuItem";
            this.saveRawFrameToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.saveRawFrameToolStripMenuItem.Text = "Save Raw Frame (after decoder)";
            this.saveRawFrameToolStripMenuItem.Click += new System.EventHandler(this.saveRawFrameToolStripMenuItem_Click);
            // 
            // saveImageafterDecoderDisplayToolStripMenuItem
            // 
            this.saveImageafterDecoderDisplayToolStripMenuItem.Name = "saveImageafterDecoderDisplayToolStripMenuItem";
            this.saveImageafterDecoderDisplayToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.saveImageafterDecoderDisplayToolStripMenuItem.Text = "Save image (after decoder & display)";
            this.saveImageafterDecoderDisplayToolStripMenuItem.Click += new System.EventHandler(this.saveImageafterDecoderDisplayToolStripMenuItem_Click);
            // 
            // toolStripMenuEncode
            // 
            this.toolStripMenuEncode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.codecTypeToolStripMenuEncodeCodecType});
            this.toolStripMenuEncode.Name = "toolStripMenuEncode";
            this.toolStripMenuEncode.Size = new System.Drawing.Size(258, 22);
            this.toolStripMenuEncode.Text = "Encode";
            // 
            // codecTypeToolStripMenuEncodeCodecType
            // 
            this.codecTypeToolStripMenuEncodeCodecType.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.h264ToolStripMenuEncode,
            this.h263ToolStripMenuEncode});
            this.codecTypeToolStripMenuEncodeCodecType.Name = "codecTypeToolStripMenuEncodeCodecType";
            this.codecTypeToolStripMenuEncodeCodecType.Size = new System.Drawing.Size(134, 22);
            this.codecTypeToolStripMenuEncodeCodecType.Text = "CodecType";
            this.codecTypeToolStripMenuEncodeCodecType.Click += new System.EventHandler(this.codecTypeToolStripMenuItem1_Click);
            // 
            // h264ToolStripMenuEncode
            // 
            this.h264ToolStripMenuEncode.Name = "h264ToolStripMenuEncode";
            this.h264ToolStripMenuEncode.Size = new System.Drawing.Size(101, 22);
            this.h264ToolStripMenuEncode.Text = "H264";
            this.h264ToolStripMenuEncode.Click += new System.EventHandler(this.CodecH264EncodeTypeToolStripMenuItem_Click);
            // 
            // h263ToolStripMenuEncode
            // 
            this.h263ToolStripMenuEncode.Name = "h263ToolStripMenuEncode";
            this.h263ToolStripMenuEncode.Size = new System.Drawing.Size(101, 22);
            this.h263ToolStripMenuEncode.Text = "H263";
            this.h263ToolStripMenuEncode.Click += new System.EventHandler(this.CodecH263EncodeTypeToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.view11ToolStripMenuItem,
            this.extraInfoToolStripMenuItem,
            this.infoTextToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // view11ToolStripMenuItem
            // 
            this.view11ToolStripMenuItem.Name = "view11ToolStripMenuItem";
            this.view11ToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.view11ToolStripMenuItem.Text = "View 1:1";
            this.view11ToolStripMenuItem.Click += new System.EventHandler(this.view11ToolStripMenuItem_Click);
            // 
            // extraInfoToolStripMenuItem
            // 
            this.extraInfoToolStripMenuItem.Name = "extraInfoToolStripMenuItem";
            this.extraInfoToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.extraInfoToolStripMenuItem.Text = "Extra Info";
            this.extraInfoToolStripMenuItem.Click += new System.EventHandler(this.extraInfoToolStripMenuItem_Click);
            // 
            // infoTextToolStripMenuItem
            // 
            this.infoTextToolStripMenuItem.Name = "infoTextToolStripMenuItem";
            this.infoTextToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.infoTextToolStripMenuItem.Text = "Info text";
            this.infoTextToolStripMenuItem.Click += new System.EventHandler(this.infoTextToolStripMenuItem_Click);
            // 
            // overlayToolStripMenuItem
            // 
            this.overlayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.colorSpacesToolStripMenuItem,
            this.rOIToolStripMenuItem});
            this.overlayToolStripMenuItem.Name = "overlayToolStripMenuItem";
            this.overlayToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.overlayToolStripMenuItem.Text = "Overlay";
            // 
            // colorSpacesToolStripMenuItem
            // 
            this.colorSpacesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showYToolStripMenuItem,
            this.showUToolStripMenuItem,
            this.showVToolStripMenuItem});
            this.colorSpacesToolStripMenuItem.Name = "colorSpacesToolStripMenuItem";
            this.colorSpacesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.colorSpacesToolStripMenuItem.Text = "Color Spaces";
            // 
            // showYToolStripMenuItem
            // 
            this.showYToolStripMenuItem.Name = "showYToolStripMenuItem";
            this.showYToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.showYToolStripMenuItem.Text = "Show Y";
            this.showYToolStripMenuItem.Click += new System.EventHandler(this.showYToolStripMenuItem_Click);
            // 
            // showUToolStripMenuItem
            // 
            this.showUToolStripMenuItem.Name = "showUToolStripMenuItem";
            this.showUToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.showUToolStripMenuItem.Text = "Show U";
            this.showUToolStripMenuItem.Click += new System.EventHandler(this.showUToolStripMenuItem_Click);
            // 
            // showVToolStripMenuItem
            // 
            this.showVToolStripMenuItem.Name = "showVToolStripMenuItem";
            this.showVToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.showVToolStripMenuItem.Text = "Show V";
            this.showVToolStripMenuItem.Click += new System.EventHandler(this.showVToolStripMenuItem_Click);
            // 
            // rOIToolStripMenuItem
            // 
            this.rOIToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableDisableROIGridToolStripMenuItem,
            this.analyzerBitMapSettingsToolStripMenuItem,
            this.openSharpFramesFolderToolStripMenuItem});
            this.rOIToolStripMenuItem.Name = "rOIToolStripMenuItem";
            this.rOIToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.rOIToolStripMenuItem.Text = "ROI";
            // 
            // enableDisableROIGridToolStripMenuItem
            // 
            this.enableDisableROIGridToolStripMenuItem.Name = "enableDisableROIGridToolStripMenuItem";
            this.enableDisableROIGridToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.enableDisableROIGridToolStripMenuItem.Text = "Enable\\Disable ROI grid";
            this.enableDisableROIGridToolStripMenuItem.Click += new System.EventHandler(this.enableDisableROIGridToolStripMenuItem_Click);
            // 
            // analyzerBitMapSettingsToolStripMenuItem
            // 
            this.analyzerBitMapSettingsToolStripMenuItem.Name = "analyzerBitMapSettingsToolStripMenuItem";
            this.analyzerBitMapSettingsToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.analyzerBitMapSettingsToolStripMenuItem.Text = "Analyzer bit map settings";
            this.analyzerBitMapSettingsToolStripMenuItem.Click += new System.EventHandler(this.analyzerBitMapSettingsToolStripMenuItem_Click);
            // 
            // audioToolStripMenuItem
            // 
            this.audioToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.linearSampleRateToolStripMenuItem});
            this.audioToolStripMenuItem.Name = "audioToolStripMenuItem";
            this.audioToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.audioToolStripMenuItem.Text = "Audio";
            // 
            // linearSampleRateToolStripMenuItem
            // 
            this.linearSampleRateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Linear8000,
            this.Linear16000,
            this.Linear32000,
            this.Linear48000,
            this.Linear64000});
            this.linearSampleRateToolStripMenuItem.Name = "linearSampleRateToolStripMenuItem";
            this.linearSampleRateToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.linearSampleRateToolStripMenuItem.Text = "Linear sample rate";
            // 
            // Linear8000
            // 
            this.Linear8000.Name = "Linear8000";
            this.Linear8000.Size = new System.Drawing.Size(104, 22);
            this.Linear8000.Text = "8000";
            this.Linear8000.Click += new System.EventHandler(this.Linear8000_Click);
            // 
            // Linear16000
            // 
            this.Linear16000.Name = "Linear16000";
            this.Linear16000.Size = new System.Drawing.Size(104, 22);
            this.Linear16000.Text = "16000";
            this.Linear16000.Click += new System.EventHandler(this.Linear16000_Click);
            // 
            // Linear32000
            // 
            this.Linear32000.Name = "Linear32000";
            this.Linear32000.Size = new System.Drawing.Size(104, 22);
            this.Linear32000.Text = "32000";
            this.Linear32000.Click += new System.EventHandler(this.Linear32000_Click);
            // 
            // Linear48000
            // 
            this.Linear48000.Name = "Linear48000";
            this.Linear48000.Size = new System.Drawing.Size(104, 22);
            this.Linear48000.Text = "48000";
            this.Linear48000.Click += new System.EventHandler(this.Linear48000_Click);
            // 
            // Linear64000
            // 
            this.Linear64000.Name = "Linear64000";
            this.Linear64000.Size = new System.Drawing.Size(104, 22);
            this.Linear64000.Text = "64000";
            this.Linear64000.Click += new System.EventHandler(this.Linear64000_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.versionToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // versionToolStripMenuItem
            // 
            this.versionToolStripMenuItem.Name = "versionToolStripMenuItem";
            this.versionToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.versionToolStripMenuItem.Text = "Version";
            this.versionToolStripMenuItem.Click += new System.EventHandler(this.versionToolStripMenuItem_Click);
            // 
            // CodecTypeLabel
            // 
            this.CodecTypeLabel.AutoSize = true;
            this.CodecTypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CodecTypeLabel.Location = new System.Drawing.Point(1020, 35);
            this.CodecTypeLabel.Name = "CodecTypeLabel";
            this.CodecTypeLabel.Size = new System.Drawing.Size(0, 17);
            this.CodecTypeLabel.TabIndex = 42;
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoLabel.Location = new System.Drawing.Point(35, 100);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(0, 16);
            this.InfoLabel.TabIndex = 43;
            // 
            // errorLogTextBox
            // 
            this.errorLogTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.errorLogTextBox.Location = new System.Drawing.Point(53, 578);
            this.errorLogTextBox.Multiline = true;
            this.errorLogTextBox.Name = "errorLogTextBox";
            this.errorLogTextBox.ReadOnly = true;
            this.errorLogTextBox.Size = new System.Drawing.Size(992, 71);
            this.errorLogTextBox.TabIndex = 44;
            // 
            // ExtraInfoTreeView
            // 
            this.ExtraInfoTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ExtraInfoTreeView.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExtraInfoTreeView.Location = new System.Drawing.Point(1051, 148);
            this.ExtraInfoTreeView.Name = "ExtraInfoTreeView";
            this.ExtraInfoTreeView.Size = new System.Drawing.Size(281, 501);
            this.ExtraInfoTreeView.TabIndex = 45;
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(1051, 38);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(0, 13);
            this.versionLabel.TabIndex = 46;
            // 
            // Stop_button
            // 
            this.Stop_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Stop_button.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.Stop_button.Image = ((System.Drawing.Image)(resources.GetObject("Stop_button.Image")));
            this.Stop_button.Location = new System.Drawing.Point(740, 38);
            this.Stop_button.Name = "Stop_button";
            this.Stop_button.Size = new System.Drawing.Size(40, 34);
            this.Stop_button.TabIndex = 40;
            this.Stop_button.UseVisualStyleBackColor = true;
            this.Stop_button.Click += new System.EventHandler(this.Stop_button_Click);
            // 
            // Forward_button
            // 
            this.Forward_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Forward_button.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.Forward_button.Image = global::DSPMediaPlayer.Properties.Resources.forward1;
            this.Forward_button.Location = new System.Drawing.Point(914, 38);
            this.Forward_button.Name = "Forward_button";
            this.Forward_button.Size = new System.Drawing.Size(42, 34);
            this.Forward_button.TabIndex = 39;
            this.Forward_button.UseVisualStyleBackColor = true;
            this.Forward_button.Click += new System.EventHandler(this.Forward_button_Click);
            // 
            // Rewind_button
            // 
            this.Rewind_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Rewind_button.Image = global::DSPMediaPlayer.Properties.Resources.rewind1;
            this.Rewind_button.Location = new System.Drawing.Point(859, 38);
            this.Rewind_button.Name = "Rewind_button";
            this.Rewind_button.Size = new System.Drawing.Size(40, 34);
            this.Rewind_button.TabIndex = 38;
            this.Rewind_button.UseVisualStyleBackColor = true;
            this.Rewind_button.Click += new System.EventHandler(this.Rewind_button_Click);
            // 
            // Pause_button
            // 
            this.Pause_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Pause_button.Image = global::DSPMediaPlayer.Properties.Resources.pause1;
            this.Pause_button.Location = new System.Drawing.Point(797, 38);
            this.Pause_button.Name = "Pause_button";
            this.Pause_button.Size = new System.Drawing.Size(45, 34);
            this.Pause_button.TabIndex = 37;
            this.Pause_button.UseVisualStyleBackColor = true;
            this.Pause_button.Click += new System.EventHandler(this.Pause_button_Click);
            // 
            // Play_button
            // 
            this.Play_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Play_button.Image = ((System.Drawing.Image)(resources.GetObject("Play_button.Image")));
            this.Play_button.Location = new System.Drawing.Point(681, 38);
            this.Play_button.Name = "Play_button";
            this.Play_button.Size = new System.Drawing.Size(44, 34);
            this.Play_button.TabIndex = 36;
            this.Play_button.UseVisualStyleBackColor = true;
            this.Play_button.Click += new System.EventHandler(this.Play_button_Click);
            // 
            // Browse_Button
            // 
            this.Browse_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Browse_Button.Image = ((System.Drawing.Image)(resources.GetObject("Browse_Button.Image")));
            this.Browse_Button.Location = new System.Drawing.Point(21, 34);
            this.Browse_Button.Name = "Browse_Button";
            this.Browse_Button.Size = new System.Drawing.Size(56, 43);
            this.Browse_Button.TabIndex = 29;
            this.Browse_Button.UseVisualStyleBackColor = true;
            this.Browse_Button.Click += new System.EventHandler(this.Browse_Button_Click);
            // 
            // Display_pictureBox
            // 
            this.Display_pictureBox.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Display_pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Display_pictureBox.Location = new System.Drawing.Point(44, 452);
            this.Display_pictureBox.Name = "Display_pictureBox";
            this.Display_pictureBox.Size = new System.Drawing.Size(124, 72);
            this.Display_pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Display_pictureBox.TabIndex = 35;
            this.Display_pictureBox.TabStop = false;
            // 
            // imagePanel1
            // 
            this.imagePanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.imagePanel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.imagePanel1.CanvasSize = new System.Drawing.Size(600, 400);
            this.imagePanel1.Image = null;
            this.imagePanel1.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            this.imagePanel1.Location = new System.Drawing.Point(53, 148);
            this.imagePanel1.Name = "imagePanel1";
            this.imagePanel1.Size = new System.Drawing.Size(992, 413);
            this.imagePanel1.TabIndex = 0;
            this.imagePanel1.Zoom = 1F;
            // 
            // waveControl_UI
            // 
            this.waveControl_UI.Location = new System.Drawing.Point(53, 148);
            this.waveControl_UI.Name = "waveControl_UI";
            this.waveControl_UI.Size = new System.Drawing.Size(992, 413);
            this.waveControl_UI.TabIndex = 47;
            // 
            // openSharpFramesFolderToolStripMenuItem
            // 
            this.openSharpFramesFolderToolStripMenuItem.Name = "openSharpFramesFolderToolStripMenuItem";
            this.openSharpFramesFolderToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.openSharpFramesFolderToolStripMenuItem.Text = "Open SharpFrames Folder";
            this.openSharpFramesFolderToolStripMenuItem.Click += new System.EventHandler(this.openSharpFramesFolderToolStripMenuItem_Click);
            // 
            // DSPMediaPlayer_Form
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1344, 677);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.ExtraInfoTreeView);
            this.Controls.Add(this.errorLogTextBox);
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.CodecTypeLabel);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.Stop_button);
            this.Controls.Add(this.Forward_button);
            this.Controls.Add(this.Rewind_button);
            this.Controls.Add(this.Pause_button);
            this.Controls.Add(this.Play_button);
            this.Controls.Add(this.Browse_Button);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.imagePanel1);
            this.Controls.Add(this.waveControl_UI);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1250, 650);
            this.Name = "DSPMediaPlayer_Form";
            this.Text = "DSP Media Player";
            this.Load += new System.EventHandler(this.DSPMediaPlayer_Form_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DSPMediaPlayer_Form_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Display_pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Browse_Button;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
       
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem; 
        private System.Windows.Forms.ToolStripMenuItem view11ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        public System.Windows.Forms.PictureBox Display_pictureBox;
        private System.Windows.Forms.Button Stop_button;
        private System.Windows.Forms.Button Forward_button;
        private System.Windows.Forms.Button Rewind_button;
        private System.Windows.Forms.Button Pause_button;
        private System.Windows.Forms.Button Play_button;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.Label CodecTypeLabel;
        public System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.ToolStripMenuItem threadsInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pcapFileHandlerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem versionToolStripMenuItem;
        public ImagePanel imagePanel1;
        private System.Windows.Forms.ToolStripMenuItem codecTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem h264ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem h263ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vp8ToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem saveCleanFrameToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem saveRawFrameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem overlayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorSpacesToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem showYToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem showUToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem showVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuEncode;
        private System.Windows.Forms.ToolStripMenuItem codecTypeToolStripMenuEncodeCodecType;
        private System.Windows.Forms.ToolStripMenuItem h264ToolStripMenuEncode;
        private System.Windows.Forms.ToolStripMenuItem h263ToolStripMenuEncode;
        private System.Windows.Forms.TextBox errorLogTextBox;
        private System.Windows.Forms.TreeView ExtraInfoTreeView;
        private System.Windows.Forms.Label versionLabel;
        public System.Windows.Forms.ToolStripMenuItem saveImageafterDecoderDisplayToolStripMenuItem;
        private DSPMediaPlayer.Audio.WaveControl_UI waveControl_UI;
        private System.Windows.Forms.ToolStripMenuItem audioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem linearSampleRateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Linear8000;
        private System.Windows.Forms.ToolStripMenuItem Linear16000;
        private System.Windows.Forms.ToolStripMenuItem Linear32000;
        private System.Windows.Forms.ToolStripMenuItem Linear48000;
        private System.Windows.Forms.ToolStripMenuItem Linear64000;
        private System.Windows.Forms.ToolStripMenuItem extraInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infoTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rOIToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem enableDisableROIGridToolStripMenuItem;		
        private System.Windows.Forms.ToolStripMenuItem analyzerBitMapSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSharpFramesFolderToolStripMenuItem;
		
      

    }
}

