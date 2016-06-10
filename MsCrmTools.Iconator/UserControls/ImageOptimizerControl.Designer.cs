namespace MsCrmTools.Iconator.UserControls
{
    partial class ImageOptimizerControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.chkSelected = new System.Windows.Forms.CheckBox();
            this.pbOriginalImage = new System.Windows.Forms.PictureBox();
            this.pbOptimizedImage = new System.Windows.Forms.PictureBox();
            this.lblOriginalImage = new System.Windows.Forms.Label();
            this.lblOptimizedImage = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbOriginalImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOptimizedImage)).BeginInit();
            this.SuspendLayout();
            // 
            // chkSelected
            // 
            this.chkSelected.AutoSize = true;
            this.chkSelected.Location = new System.Drawing.Point(13, 25);
            this.chkSelected.Name = "chkSelected";
            this.chkSelected.Size = new System.Drawing.Size(22, 21);
            this.chkSelected.TabIndex = 0;
            this.chkSelected.UseVisualStyleBackColor = true;
            // 
            // pbOriginalImage
            // 
            this.pbOriginalImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(81)))));
            this.pbOriginalImage.Location = new System.Drawing.Point(200, 3);
            this.pbOriginalImage.Name = "pbOriginalImage";
            this.pbOriginalImage.Size = new System.Drawing.Size(64, 64);
            this.pbOriginalImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbOriginalImage.TabIndex = 1;
            this.pbOriginalImage.TabStop = false;
            // 
            // pbOptimizedImage
            // 
            this.pbOptimizedImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(81)))));
            this.pbOptimizedImage.Location = new System.Drawing.Point(481, 3);
            this.pbOptimizedImage.Name = "pbOptimizedImage";
            this.pbOptimizedImage.Size = new System.Drawing.Size(64, 64);
            this.pbOptimizedImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbOptimizedImage.TabIndex = 2;
            this.pbOptimizedImage.TabStop = false;
            // 
            // lblOriginalImage
            // 
            this.lblOriginalImage.AutoSize = true;
            this.lblOriginalImage.Location = new System.Drawing.Point(83, 25);
            this.lblOriginalImage.Name = "lblOriginalImage";
            this.lblOriginalImage.Size = new System.Drawing.Size(111, 20);
            this.lblOriginalImage.TabIndex = 3;
            this.lblOriginalImage.Text = "Original Image";
            // 
            // lblOptimizedImage
            // 
            this.lblOptimizedImage.AutoSize = true;
            this.lblOptimizedImage.Location = new System.Drawing.Point(346, 25);
            this.lblOptimizedImage.Name = "lblOptimizedImage";
            this.lblOptimizedImage.Size = new System.Drawing.Size(129, 20);
            this.lblOptimizedImage.TabIndex = 4;
            this.lblOptimizedImage.Text = "Optimized Image";
            // 
            // ImageOptimizerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblOptimizedImage);
            this.Controls.Add(this.lblOriginalImage);
            this.Controls.Add(this.pbOptimizedImage);
            this.Controls.Add(this.pbOriginalImage);
            this.Controls.Add(this.chkSelected);
            this.Name = "ImageOptimizerControl";
            this.Size = new System.Drawing.Size(550, 80);
            ((System.ComponentModel.ISupportInitialize)(this.pbOriginalImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOptimizedImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkSelected;
        private System.Windows.Forms.PictureBox pbOriginalImage;
        private System.Windows.Forms.PictureBox pbOptimizedImage;
        private System.Windows.Forms.Label lblOriginalImage;
        private System.Windows.Forms.Label lblOptimizedImage;
    }
}
