﻿namespace OwoAdvancedSensationBuilder.Demo.DemoSections {
    partial class ExperienceSection {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            openAdvancedFormBtn = new Button();
            btnDebug = new Button();
            btnStart = new Button();
            btnOpen = new Button();
            SuspendLayout();
            // 
            // openAdvancedFormBtn
            // 
            openAdvancedFormBtn.Location = new Point(9, 126);
            openAdvancedFormBtn.Name = "openAdvancedFormBtn";
            openAdvancedFormBtn.Size = new Size(75, 70);
            openAdvancedFormBtn.TabIndex = 11;
            openAdvancedFormBtn.Text = "Advanced";
            openAdvancedFormBtn.UseVisualStyleBackColor = true;
            openAdvancedFormBtn.Click += openAdvancedFormBtn_Click;
            // 
            // btnDebug
            // 
            btnDebug.Location = new Point(636, 126);
            btnDebug.Name = "btnDebug";
            btnDebug.Size = new Size(149, 70);
            btnDebug.TabIndex = 10;
            btnDebug.Text = "Debug";
            btnDebug.UseVisualStyleBackColor = true;
            btnDebug.Click += btnDebug_Click;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(247, 96);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(266, 87);
            btnStart.TabIndex = 9;
            btnStart.Text = "Then After Starting the Video This";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnOpen
            // 
            btnOpen.Location = new Point(247, 3);
            btnOpen.Name = "btnOpen";
            btnOpen.Size = new Size(266, 87);
            btnOpen.TabIndex = 8;
            btnOpen.Text = "Press This";
            btnOpen.UseVisualStyleBackColor = true;
            btnOpen.Click += btnOpen_Click;
            // 
            // ExperienceSection
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(openAdvancedFormBtn);
            Controls.Add(btnDebug);
            Controls.Add(btnStart);
            Controls.Add(btnOpen);
            Name = "ExperienceSection";
            Size = new Size(790, 203);
            ResumeLayout(false);
        }

        #endregion

        private Button openAdvancedFormBtn;
        private Button btnDebug;
        private Button btnStart;
        private Button btnOpen;
    }
}
