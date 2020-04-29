namespace Minesweeper
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
            this.DifficultyTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SizeTxt = new System.Windows.Forms.TextBox();
            this.GoBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DifficultyTxt
            // 
            this.DifficultyTxt.Location = new System.Drawing.Point(374, 223);
            this.DifficultyTxt.Name = "DifficultyTxt";
            this.DifficultyTxt.Size = new System.Drawing.Size(100, 20);
            this.DifficultyTxt.TabIndex = 0;
            this.DifficultyTxt.TextChanged += new System.EventHandler(this.DifficultyTxt_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(374, 204);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Difficulty";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(256, 204);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Size";
            // 
            // SizeTxt
            // 
            this.SizeTxt.Location = new System.Drawing.Point(256, 223);
            this.SizeTxt.Name = "SizeTxt";
            this.SizeTxt.Size = new System.Drawing.Size(100, 20);
            this.SizeTxt.TabIndex = 2;
            this.SizeTxt.TextChanged += new System.EventHandler(this.SizeTxt_TextChanged);
            // 
            // GoBtn
            // 
            this.GoBtn.Location = new System.Drawing.Point(332, 249);
            this.GoBtn.Name = "GoBtn";
            this.GoBtn.Size = new System.Drawing.Size(75, 23);
            this.GoBtn.TabIndex = 4;
            this.GoBtn.Text = "Go";
            this.GoBtn.UseVisualStyleBackColor = true;
            this.GoBtn.Click += new System.EventHandler(this.GoBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.GoBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SizeTxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DifficultyTxt);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox DifficultyTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SizeTxt;
        private System.Windows.Forms.Button GoBtn;
    }
}