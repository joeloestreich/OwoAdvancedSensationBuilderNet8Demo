namespace OwoAdvancedSensationBuilder.Demo {
    partial class Form1 {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            btnOpen = new Button();
            btnStart = new Button();
            btnDebug = new Button();
            SuspendLayout();
            // 
            // btnOpen
            // 
            btnOpen.Location = new Point(257, 12);
            btnOpen.Name = "btnOpen";
            btnOpen.Size = new Size(266, 87);
            btnOpen.TabIndex = 0;
            btnOpen.Text = "Press This";
            btnOpen.UseVisualStyleBackColor = true;
            btnOpen.Click += btnOpen_Click;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(257, 105);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(266, 87);
            btnStart.TabIndex = 1;
            btnStart.Text = "Then After Starting the Video This";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnDebug
            // 
            btnDebug.Location = new Point(639, 368);
            btnDebug.Name = "btnDebug";
            btnDebug.Size = new Size(149, 70);
            btnDebug.TabIndex = 2;
            btnDebug.Text = "Debug";
            btnDebug.UseVisualStyleBackColor = true;
            btnDebug.Click += btnDebug_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnDebug);
            Controls.Add(btnStart);
            Controls.Add(btnOpen);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button btnOpen;
        private Button btnStart;
        private Button btnDebug;
    }
}
