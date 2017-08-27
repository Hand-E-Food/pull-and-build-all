namespace PullAndBuildAll
{
    partial class MainForm
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
            this.outputPanel = new System.Windows.Forms.TableLayoutPanel();
            this.repositoryLabel = new System.Windows.Forms.Label();
            this.pullLabel = new System.Windows.Forms.Label();
            this.nugetLabel = new System.Windows.Forms.Label();
            this.platformLabel = new System.Windows.Forms.Label();
            this.buildLabel = new System.Windows.Forms.Label();
            this.outputPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // outputPanel
            // 
            this.outputPanel.AutoSize = true;
            this.outputPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.outputPanel.ColumnCount = 5;
            this.outputPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.outputPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.outputPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.outputPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.outputPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.outputPanel.Controls.Add(this.repositoryLabel, 0, 0);
            this.outputPanel.Controls.Add(this.pullLabel, 1, 0);
            this.outputPanel.Controls.Add(this.nugetLabel, 2, 0);
            this.outputPanel.Controls.Add(this.platformLabel, 3, 0);
            this.outputPanel.Controls.Add(this.buildLabel, 4, 0);
            this.outputPanel.Location = new System.Drawing.Point(12, 12);
            this.outputPanel.Margin = new System.Windows.Forms.Padding(12);
            this.outputPanel.Name = "outputPanel";
            this.outputPanel.RowCount = 1;
            this.outputPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.outputPanel.Size = new System.Drawing.Size(372, 30);
            this.outputPanel.TabIndex = 0;
            // 
            // repositoryLabel
            // 
            this.repositoryLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.repositoryLabel.AutoSize = true;
            this.repositoryLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.repositoryLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.repositoryLabel.Location = new System.Drawing.Point(3, 3);
            this.repositoryLabel.Margin = new System.Windows.Forms.Padding(3);
            this.repositoryLabel.Name = "repositoryLabel";
            this.repositoryLabel.Padding = new System.Windows.Forms.Padding(3);
            this.repositoryLabel.Size = new System.Drawing.Size(92, 24);
            this.repositoryLabel.TabIndex = 0;
            this.repositoryLabel.Text = "Repository";
            this.repositoryLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pullLabel
            // 
            this.pullLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pullLabel.AutoSize = true;
            this.pullLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pullLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pullLabel.Location = new System.Drawing.Point(101, 3);
            this.pullLabel.Margin = new System.Windows.Forms.Padding(3);
            this.pullLabel.Name = "pullLabel";
            this.pullLabel.Padding = new System.Windows.Forms.Padding(3);
            this.pullLabel.Size = new System.Drawing.Size(59, 24);
            this.pullLabel.TabIndex = 1;
            this.pullLabel.Text = "Pull";
            this.pullLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nugetLabel
            // 
            this.nugetLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nugetLabel.AutoSize = true;
            this.nugetLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nugetLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nugetLabel.Location = new System.Drawing.Point(166, 3);
            this.nugetLabel.Margin = new System.Windows.Forms.Padding(3);
            this.nugetLabel.Name = "nugetLabel";
            this.nugetLabel.Padding = new System.Windows.Forms.Padding(3);
            this.nugetLabel.Size = new System.Drawing.Size(59, 24);
            this.nugetLabel.TabIndex = 2;
            this.nugetLabel.Text = "NuGet";
            this.nugetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // platformLabel
            // 
            this.platformLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.platformLabel.AutoSize = true;
            this.platformLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.platformLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.platformLabel.Location = new System.Drawing.Point(231, 3);
            this.platformLabel.Margin = new System.Windows.Forms.Padding(3);
            this.platformLabel.Name = "platformLabel";
            this.platformLabel.Padding = new System.Windows.Forms.Padding(3);
            this.platformLabel.Size = new System.Drawing.Size(73, 24);
            this.platformLabel.TabIndex = 2;
            this.platformLabel.Text = "Platform";
            this.platformLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buildLabel
            // 
            this.buildLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buildLabel.AutoSize = true;
            this.buildLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.buildLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buildLabel.Location = new System.Drawing.Point(310, 3);
            this.buildLabel.Margin = new System.Windows.Forms.Padding(3);
            this.buildLabel.Name = "buildLabel";
            this.buildLabel.Padding = new System.Windows.Forms.Padding(3);
            this.buildLabel.Size = new System.Drawing.Size(59, 24);
            this.buildLabel.TabIndex = 3;
            this.buildLabel.Text = "Build";
            this.buildLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 54);
            this.Controls.Add(this.outputPanel);
            this.Name = "MainForm";
            this.Text = "Pull and Build All";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.outputPanel.ResumeLayout(false);
            this.outputPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel outputPanel;
        private System.Windows.Forms.Label repositoryLabel;
        private System.Windows.Forms.Label pullLabel;
        private System.Windows.Forms.Label nugetLabel;
        private System.Windows.Forms.Label platformLabel;
        private System.Windows.Forms.Label buildLabel;
    }
}

