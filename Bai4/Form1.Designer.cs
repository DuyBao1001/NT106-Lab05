using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Bai4
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            progressBar1 = new ProgressBar();
            lblStatus = new Label();
            flowListMovies = new FlowLayoutPanel();
            pnlSeats = new Panel();
            groupBoxInfo = new GroupBox();
            label1 = new Label();
            txtName = new TextBox();
            btnOrder = new Button();
            lblTotalMoney = new Label();
            lblSeatCount = new Label();
            lblMovieName = new Label();
            lblEmail = new Label();
            txtEmail = new TextBox();
            groupBoxInfo.SuspendLayout();
            SuspendLayout();
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(12, 15);
            progressBar1.Margin = new Padding(3, 4, 3, 4);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(1208, 29);
            progressBar1.TabIndex = 0;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblStatus.ForeColor = Color.Blue;
            lblStatus.Location = new Point(12, 48);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(113, 20);
            lblStatus.TabIndex = 1;
            lblStatus.Text = "Đang sẵn sàng...";
            // 
            // flowListMovies
            // 
            flowListMovies.AutoScroll = true;
            flowListMovies.BackColor = Color.WhiteSmoke;
            flowListMovies.BorderStyle = BorderStyle.FixedSingle;
            flowListMovies.Location = new Point(12, 88);
            flowListMovies.Margin = new Padding(3, 4, 3, 4);
            flowListMovies.Name = "flowListMovies";
            flowListMovies.Size = new Size(420, 837);
            flowListMovies.TabIndex = 2;
            // 
            // pnlSeats
            // 
            pnlSeats.BackColor = Color.White;
            pnlSeats.BorderStyle = BorderStyle.FixedSingle;
            pnlSeats.Location = new Point(445, 88);
            pnlSeats.Margin = new Padding(3, 4, 3, 4);
            pnlSeats.Name = "pnlSeats";
            pnlSeats.Size = new Size(500, 837);
            pnlSeats.TabIndex = 3;
            // 
            // groupBoxInfo
            // 
            groupBoxInfo.Controls.Add(label1);
            groupBoxInfo.Controls.Add(txtName);
            groupBoxInfo.Controls.Add(btnOrder);
            groupBoxInfo.Controls.Add(lblTotalMoney);
            groupBoxInfo.Controls.Add(lblSeatCount);
            groupBoxInfo.Controls.Add(lblMovieName);
            groupBoxInfo.Controls.Add(lblEmail);
            groupBoxInfo.Controls.Add(txtEmail);
            groupBoxInfo.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBoxInfo.Location = new Point(951, 88);
            groupBoxInfo.Margin = new Padding(3, 4, 3, 4);
            groupBoxInfo.Name = "groupBoxInfo";
            groupBoxInfo.Padding = new Padding(3, 4, 3, 4);
            groupBoxInfo.Size = new Size(270, 725);
            groupBoxInfo.TabIndex = 4;
            groupBoxInfo.TabStop = false;
            groupBoxInfo.Text = "THÔNG TIN ĐẶT VÉ";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(15, 50);
            label1.Name = "label1";
            label1.Size = new Size(114, 20);
            label1.TabIndex = 5;
            label1.Text = "Tên khách hàng:";
            // 
            // txtName
            // 
            txtName.Location = new Point(19, 81);
            txtName.Margin = new Padding(3, 4, 3, 4);
            txtName.Name = "txtName";
            txtName.Size = new Size(230, 30);
            txtName.TabIndex = 4;
            // 
            // btnOrder
            // 
            btnOrder.BackColor = Color.DodgerBlue;
            btnOrder.Cursor = Cursors.Hand;
            btnOrder.FlatStyle = FlatStyle.Flat;
            btnOrder.ForeColor = Color.White;
            btnOrder.Location = new Point(23, 624);
            btnOrder.Margin = new Padding(3, 4, 3, 4);
            btnOrder.Name = "btnOrder";
            btnOrder.Size = new Size(230, 62);
            btnOrder.TabIndex = 3;
            btnOrder.Text = "XÁC NHẬN ĐẶT VÉ";
            btnOrder.UseVisualStyleBackColor = false;
            btnOrder.Click += btnOrder_Click;
            // 
            // lblTotalMoney
            // 
            lblTotalMoney.AutoSize = true;
            lblTotalMoney.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalMoney.ForeColor = Color.Red;
            lblTotalMoney.Location = new Point(23, 522);
            lblTotalMoney.Name = "lblTotalMoney";
            lblTotalMoney.Size = new Size(142, 31);
            lblTotalMoney.TabIndex = 2;
            lblTotalMoney.Text = "Tổng tiền: 0";
            lblTotalMoney.Click += lblTotalMoney_Click;
            // 
            // lblSeatCount
            // 
            lblSeatCount.AutoSize = true;
            lblSeatCount.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSeatCount.Location = new Point(23, 357);
            lblSeatCount.Name = "lblSeatCount";
            lblSeatCount.Size = new Size(126, 23);
            lblSeatCount.TabIndex = 1;
            lblSeatCount.Text = "Ghế đã chọn: 0";
            // 
            // lblMovieName
            // 
            lblMovieName.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblMovieName.ForeColor = Color.DarkSlateGray;
            lblMovieName.Location = new Point(15, 218);
            lblMovieName.Name = "lblMovieName";
            lblMovieName.Size = new Size(234, 78);
            lblMovieName.TabIndex = 0;
            lblMovieName.Text = "Phim: (Vui lòng chọn phim bên trái)";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 9F);
            lblEmail.Location = new Point(15, 125);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(49, 20);
            lblEmail.TabIndex = 6;
            lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(19, 156);
            txtEmail.Margin = new Padding(3, 4, 3, 4);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(230, 30);
            txtEmail.TabIndex = 7;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 941);
            Controls.Add(groupBoxInfo);
            Controls.Add(pnlSeats);
            Controls.Add(flowListMovies);
            Controls.Add(lblStatus);
            Controls.Add(progressBar1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Lab 4 - Web Scraping Beta Cinemas";
            Load += Form1_Load;
            groupBoxInfo.ResumeLayout(false);
            groupBoxInfo.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.FlowLayoutPanel flowListMovies;
        private System.Windows.Forms.Panel pnlSeats;
        private System.Windows.Forms.GroupBox groupBoxInfo;
        private System.Windows.Forms.Label lblMovieName;
        private System.Windows.Forms.Label lblSeatCount;
        private System.Windows.Forms.Label lblTotalMoney;
        private System.Windows.Forms.Button btnOrder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
    }
}