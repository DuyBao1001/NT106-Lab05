namespace Bai5
{
    partial class DishControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblContributor = new System.Windows.Forms.Label(); // Khai báo lblContributor
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.picFoodImage = new System.Windows.Forms.PictureBox();
            this.pnlContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFoodImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlContainer
            // 
            this.pnlContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlContainer.Controls.Add(this.btnDelete);
            this.pnlContainer.Controls.Add(this.lblContributor); // Thêm vào Controls
            this.pnlContainer.Controls.Add(this.lblAddress);
            this.pnlContainer.Controls.Add(this.lblPrice);
            this.pnlContainer.Controls.Add(this.lblName);
            this.pnlContainer.Controls.Add(this.picFoodImage);
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(550, 110);
            this.pnlContainer.TabIndex = 0;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Red;
            this.btnDelete.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnDelete.Location = new System.Drawing.Point(450, 10);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(85, 30);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "Xóa";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Visible = false;
            // 
            // lblContributor
            // 
            this.lblContributor.AutoSize = true;
            this.lblContributor.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblContributor.Location = new System.Drawing.Point(120, 85); // Vị trí mẫu
            this.lblContributor.Name = "lblContributor";
            this.lblContributor.Size = new System.Drawing.Size(81, 19);
            this.lblContributor.TabIndex = 4;
            this.lblContributor.Text = "Đóng góp: N/A";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(120, 60);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(51, 16);
            this.lblAddress.TabIndex = 3;
            this.lblAddress.Text = "Địa chỉ:";
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrice.ForeColor = System.Drawing.Color.Blue;
            this.lblPrice.Location = new System.Drawing.Point(120, 40);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(37, 16);
            this.lblPrice.TabIndex = 2;
            this.lblPrice.Text = "Giá:";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(120, 15);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(107, 23);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Tên Món Ăn";
            // 
            // picFoodImage
            // 
            this.picFoodImage.Location = new System.Drawing.Point(10, 10);
            this.picFoodImage.Name = "picFoodImage";
            this.picFoodImage.Size = new System.Drawing.Size(90, 90);
            this.picFoodImage.TabIndex = 0;
            this.picFoodImage.TabStop = false;
            // 
            // DishControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlContainer);
            this.Name = "DishControl";
            this.Size = new System.Drawing.Size(550, 110);
            this.pnlContainer.ResumeLayout(false);
            this.pnlContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFoodImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlContainer;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.PictureBox picFoodImage;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblContributor; // KHAI BÁO BIẾN CHO CONTROLS
        private System.Windows.Forms.Button btnDelete;
    }
}