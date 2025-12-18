using HtmlAgilityPack;
using Microsoft.Web.WebView2.WinForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Bai4
{
    public partial class Form1 : Form
    {
        private List<Movie> moviesList = new List<Movie>();
        private Movie currentMovie = null;
        private HashSet<string> currentSelectedSeats = new HashSet<string>();

        private Dictionary<string, HashSet<string>> soldSeatsDB = new Dictionary<string, HashSet<string>>();
        private Dictionary<string, Button> seatButtons = new Dictionary<string, Button>();

        private const string FILE_MOVIES = "movies.json";
        private const string FILE_ORDERS = "orders.json";
        private const string FILE_OUTPUT_STATS = "output.txt";
        private const int TOTAL_SEATS_PER_MOVIE = 40;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            GenerateSeatMap();
            LoadOrderHistory();
            await LoadOrCrawlMovies();
        }


        private async Task SendMailConfirm(OrderInfo order, Movie movie)
        {
            string fromEmail = "tcdbao.1001@gmail.com";
            string password = "risaqxfcngunrvsd"; 

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Beta Cinemas Ticket System", fromEmail));
            message.To.Add(new MailboxAddress(order.CustomerName, order.Email));
            message.Subject = $"[Vé Điện Tử] Xác nhận đặt vé: {movie.Name}";

            var builder = new BodyBuilder();

            string listGhe = string.Join(", ", order.Seats); 

            string htmlContent = $@"
            <html>
            <body style='font-family: Arial, sans-serif; margin: 0; padding: 0;'>
                <div style='
                    background-image: url(""{movie.ImageUrl}""); 
                    background-size: cover; 
                    background-position: center; 
                    padding: 60px 20px;
                    text-align: center;'>
                    
                    <div style='
                        background-color: rgba(255, 255, 255, 0.95); 
                        max-width: 600px; 
                        margin: 0 auto; 
                        border-radius: 12px; 
                        padding: 30px; 
                        box-shadow: 0 5px 15px rgba(0,0,0,0.5);'>
                        
                        <h2 style='color: #e74c3c; margin-top: 0; text-transform: uppercase;'>XÁC NHẬN ĐẶT VÉ THÀNH CÔNG</h2>
                        <p style='font-size: 16px;'>Xin chào <strong>{order.CustomerName}</strong>,</p>
                        <p>Vé của bạn đã được thanh toán thành công. Dưới đây là chi tiết:</p>
                        
                        <table style='width: 100%; border-collapse: collapse; margin: 20px 0;'>
                            <tr style='border-bottom: 1px solid #ddd;'>
                                <td style='padding: 12px; text-align: left; color: #555;'>Phim:</td>
                                <td style='padding: 12px; text-align: right; font-weight: bold; font-size: 18px;'>{order.MovieName}</td>
                            </tr>
                            <tr style='border-bottom: 1px solid #ddd;'>
                                <td style='padding: 12px; text-align: left; color: #555;'>Vị trí ghế:</td>
                                <td style='padding: 12px; text-align: right; font-weight: bold; color: #e74c3c; font-size: 18px;'>{listGhe}</td>
                            </tr>
                            <tr style='border-bottom: 1px solid #ddd;'>
                                <td style='padding: 12px; text-align: left; color: #555;'>Thời gian đặt:</td>
                                <td style='padding: 12px; text-align: right;'>{order.BookingTime:dd/MM/yyyy HH:mm}</td>
                            </tr>
                            <tr>
                                <td style='padding: 12px; text-align: left; color: #555;'><strong>Tổng thanh toán:</strong></td>
                                <td style='padding: 12px; text-align: right; font-weight: bold; font-size: 20px; color: #2c3e50;'>{order.TotalPrice:N0} VNĐ</td>
                            </tr>
                        </table>

                        <div style='margin-top: 25px; border-top: 2px dashed #bbb; padding-top: 20px;'>
                            <p style='font-style: italic; color: #555; font-size: 14px;'>
                                ""Beta Cinemas - Rạp phim giá rẻ dành cho sinh viên!""<br/>
                                Chúc bạn có những giây phút xem phim vui vẻ.
                            </p>
                        </div>
                    </div>
                    
                    <div style='margin-top: 20px; color: white; text-shadow: 1px 1px 3px #000;'>
                        <small>Email được gửi tự động từ hệ thống Lab 5 NT106.</small>
                    </div>
                </div>
            </body>
            </html>";

            builder.HtmlBody = htmlContent;
            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                    await client.AuthenticateAsync(fromEmail, password);

                    await client.SendAsync(message);

                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi kết nối MailKit: " + ex.Message);
                }
            }
        }

        private async void btnOrder_Click(object sender, EventArgs e)
        {
            if (currentMovie == null || currentSelectedSeats.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn phim và ít nhất 1 ghế!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập tên và email khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnOrder.Enabled = false;
            btnOrder.Text = "Đang xử lý...";
            lblStatus.Text = "Đang gửi email xác nhận...";

            long total = currentSelectedSeats.Count * currentMovie.Price;
            OrderInfo order = new OrderInfo
            {
                CustomerName = txtName.Text,
                Email = txtEmail.Text.Trim(),
                MovieName = currentMovie.Name,
                Seats = currentSelectedSeats.ToList(),
                TotalPrice = total,
                BookingTime = DateTime.Now
            };

            try
            {
                await SendMailConfirm(order, currentMovie);

                SaveOrder(order);
                if (!soldSeatsDB.ContainsKey(currentMovie.Name))
                    soldSeatsDB[currentMovie.Name] = new HashSet<string>();

                foreach (var s in currentSelectedSeats)
                    soldSeatsDB[currentMovie.Name].Add(s);

                UpdateStatisticsFile();

                string msg = $"Đặt vé thành công!\nEmail xác nhận đã gửi tới: {order.Email}";
                MessageBox.Show(msg, "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);

                currentSelectedSeats.Clear();
                txtName.Clear();
                txtEmail.Clear();
                UpdateSeatMapStatus();
                UpdateInfoPanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gửi mail thất bại: " + ex.Message + "\nVé chưa được đặt.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnOrder.Enabled = true;
                btnOrder.Text = "XÁC NHẬN ĐẶT VÉ";
                lblStatus.Text = "Sẵn sàng.";
            }
        }



        private async Task LoadOrCrawlMovies()
        {
            if (File.Exists(FILE_MOVIES))
            {
                try
                {
                    string json = File.ReadAllText(FILE_MOVIES);
                    moviesList = JsonConvert.DeserializeObject<List<Movie>>(json);
                    if (moviesList != null && moviesList.Count > 0)
                    {
                        RenderMovies();
                        lblStatus.Text = $"Đã tải {moviesList.Count} phim từ cache.";
                        progressBar1.Value = 100;
                        return;
                    }
                }
                catch { }
            }
            await CrawlDataFromWeb();
        }

        private async Task CrawlDataFromWeb()
        {
            string url = "https://betacinemas.vn/phim.htm";
            lblStatus.Text = "Đang kết nối tới Beta Cinemas...";
            progressBar1.Value = 20;

            try
            {
                HtmlWeb web = new HtmlWeb();
                var doc = await Task.Run(() => web.Load(url));
                progressBar1.Value = 50;

                var movieNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'col-lg-4') or contains(@class, 'film-item')]");

                moviesList.Clear();
                if (movieNodes != null)
                {
                    foreach (var node in movieNodes)
                    {
                        var linkNode = node.SelectSingleNode(".//a[contains(@href, 'chi-tiet-phim')]");
                        var imgNode = node.SelectSingleNode(".//img");
                        var nameNode = node.SelectSingleNode(".//h3/a") ?? node.SelectSingleNode(".//h3");

                        if (linkNode != null && imgNode != null)
                        {
                            string rawLink = linkNode.GetAttributeValue("href", "");
                            string fullUrl = rawLink.StartsWith("http") ? rawLink : "https://betacinemas.vn/" + rawLink.TrimStart('/');
                            string imgUrl = imgNode.GetAttributeValue("src", "");
                            string name = nameNode?.InnerText.Trim();

                            if (!string.IsNullOrEmpty(name) && !moviesList.Any(m => m.DetailUrl == fullUrl))
                            {
                                moviesList.Add(new Movie { Name = name, ImageUrl = imgUrl, DetailUrl = fullUrl });
                            }
                        }
                    }
                }
                progressBar1.Value = 80;
                File.WriteAllText(FILE_MOVIES, JsonConvert.SerializeObject(moviesList, Formatting.Indented));
                RenderMovies();
                lblStatus.Text = $"Hoàn tất! Tìm thấy {moviesList.Count} phim.";
                progressBar1.Value = 100;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Lỗi kết nối mạng.";
                MessageBox.Show("Lỗi Crawl: " + ex.Message);
            }
        }

        private void RenderMovies()
        {
            flowListMovies.Controls.Clear();
            foreach (var movie in moviesList)
            {
                Panel pnl = new Panel { Width = 390, Height = 120, Margin = new Padding(5), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Cursor = Cursors.Hand };

                PictureBox pb = new PictureBox { Size = new Size(80, 110), Location = new Point(5, 5), SizeMode = PictureBoxSizeMode.StretchImage };
                try { pb.Load(movie.ImageUrl); } catch { }

                Label lblName = new Label { Text = movie.Name, Font = new Font("Segoe UI", 11, FontStyle.Bold), Location = new Point(95, 10), AutoSize = true, MaximumSize = new Size(280, 0) };

                Label lblDetail = new Label { Text = "Xem chi tiết...", Font = new Font("Segoe UI", 9, FontStyle.Underline), ForeColor = Color.Blue, Location = new Point(95, 80), AutoSize = true };

                pnl.Controls.Add(pb); pnl.Controls.Add(lblName); pnl.Controls.Add(lblDetail);

                void OnClick(object s, EventArgs e) => SelectMovie(movie);
                pnl.Click += OnClick;
                pb.Click += OnClick;
                lblName.Click += OnClick;
                lblDetail.Click += OnClick;

                flowListMovies.Controls.Add(pnl);
            }
        }

        private void SelectMovie(Movie movie)
        {
            currentMovie = movie;
            lblMovieName.Text = "Phim: " + movie.Name;
            currentSelectedSeats.Clear();
            UpdateInfoPanel();
            UpdateSeatMapStatus();

            try { OpenWebDetail(movie); } catch { }
        }

        private void OpenWebDetail(Movie movie)
        {
            Form webForm = new Form { Text = movie.Name, Size = new Size(1000, 600), StartPosition = FormStartPosition.CenterScreen };
            WebView2 wv = new WebView2 { Dock = DockStyle.Fill };
            webForm.Controls.Add(wv);
            webForm.Show();
            wv.Source = new Uri(movie.DetailUrl);
        }

        private void GenerateSeatMap()
        {
            pnlSeats.Controls.Clear();

            Label lblScreen = new Label
            {
                Text = "MÀN HÌNH",
                BackColor = Color.Gray,
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Dock = DockStyle.Top, 
                Height = 50
            };
            pnlSeats.Controls.Add(lblScreen);

            TableLayoutPanel tbl = new TableLayoutPanel();
            tbl.RowCount = 5;
            tbl.ColumnCount = 6; 
            tbl.AutoSize = true;

            char[] rows = { 'A', 'B', 'C', 'D', 'E' };
            for (int r = 0; r < rows.Length; r++)
            {
                for (int c = 1; c <= tbl.ColumnCount; c++)
                {
                    string seatID = $"{rows[r]}{c}";
                    Button btn = new Button
                    {
                        Text = seatID,
                        Width = 50, 
                        Height = 50,
                        Tag = seatID,
                        BackColor = Color.White,
                        Margin = new Padding(5)
                    };
                    btn.Click += BtnSeat_Click;
                    seatButtons[seatID] = btn;
                    tbl.Controls.Add(btn, c - 1, r);
                }
            }

            pnlSeats.Controls.Add(tbl);


            int x = (pnlSeats.Width - tbl.Width) / 2;
            tbl.Location = new Point(x, 80);
        }

        private void BtnSeat_Click(object sender, EventArgs e)
        {
            if (currentMovie == null) { MessageBox.Show("Vui lòng chọn phim trước!"); return; }
            Button btn = sender as Button;
            string id = btn.Tag.ToString();

            if (currentSelectedSeats.Contains(id))
            {
                currentSelectedSeats.Remove(id);
                btn.BackColor = Color.White;
            }
            else
            {
                currentSelectedSeats.Add(id);
                btn.BackColor = Color.Yellow;
            }
            UpdateInfoPanel();
        }

        private void UpdateSeatMapStatus()
        {
            if (currentMovie == null) return;
            HashSet<string> soldList = soldSeatsDB.ContainsKey(currentMovie.Name) ? soldSeatsDB[currentMovie.Name] : new HashSet<string>();

            foreach (var kvp in seatButtons)
            {
                if (soldList.Contains(kvp.Key))
                {
                    kvp.Value.BackColor = Color.Red;
                    kvp.Value.Enabled = false;
                }
                else
                {
                    kvp.Value.BackColor = currentSelectedSeats.Contains(kvp.Key) ? Color.Yellow : Color.White;
                    kvp.Value.Enabled = true;
                }
            }
        }

        private void UpdateInfoPanel()
        {
            lblSeatCount.Text = $"Ghế đã chọn: {currentSelectedSeats.Count}";
            if (currentMovie != null)
                lblTotalMoney.Text = $"{currentSelectedSeats.Count * currentMovie.Price:N0} đ";
        }

        private void SaveOrder(OrderInfo newOrder)
        {
            List<OrderInfo> allOrders = new List<OrderInfo>();
            if (File.Exists(FILE_ORDERS)) try { allOrders = JsonConvert.DeserializeObject<List<OrderInfo>>(File.ReadAllText(FILE_ORDERS)); } catch { }
            allOrders.Add(newOrder);
            File.WriteAllText(FILE_ORDERS, JsonConvert.SerializeObject(allOrders, Formatting.Indented));
        }

        private void LoadOrderHistory()
        {
            if (File.Exists(FILE_ORDERS))
            {
                try
                {
                    var orders = JsonConvert.DeserializeObject<List<OrderInfo>>(File.ReadAllText(FILE_ORDERS));
                    if (orders != null)
                    {
                        foreach (var order in orders)
                        {
                            if (!soldSeatsDB.ContainsKey(order.MovieName)) soldSeatsDB[order.MovieName] = new HashSet<string>();
                            foreach (var seat in order.Seats) soldSeatsDB[order.MovieName].Add(seat);
                        }
                    }
                }
                catch { }
            }
        }

        private void UpdateStatisticsFile()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(FILE_OUTPUT_STATS))
                {
                    foreach (var movie in moviesList)
                    {
                        int sold = soldSeatsDB.ContainsKey(movie.Name) ? soldSeatsDB[movie.Name].Count : 0;
                        sw.WriteLine($"{movie.Name}: {sold} vé");
                    }
                }
            }
            catch { }
        }

        private void lblTotalMoney_Click(object sender, EventArgs e) { }
    }
}