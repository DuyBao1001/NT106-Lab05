using Bai5;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Bai5
{
    public partial class SignupForm : Form
    {
        private const string SIGNUP_URL = "https://nt106.uitiot.vn/api/v1/user/signup";
        private readonly ApiHandler _apiHandler;

        public SignupForm(ApiHandler apiHandler)
        {
            InitializeComponent();
            _apiHandler = apiHandler;
            btnSubmit.Click += BtnSubmit_Click;
            btnClear.Click += BtnClear_Click;
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtEmail.Clear();
            txtFirstname.Clear();
            txtLastname.Clear();
            txtPhone.Clear();
            dtpBirthday.Value = DateTime.Now;
            rdoMale.Checked = true;
        }

        private async void BtnSubmit_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra dữ liệu bắt buộc 
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ các trường bắt buộc (Username, Password, Email).", "Lỗi");
                return;
            }

            // 2. Kiểm tra lỗi DỮ LIỆU CỤ THỂ (Ngày sinh không được trong tương lai)
            if (dtpBirthday.Value.Date >= DateTime.Today.Date)
            {
                MessageBox.Show("Ngày sinh phải là một ngày trong quá khứ.", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                await PerformSignUpAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi hệ thống: {ex.Message}", "Lỗi");
            }
        }

        private async Task PerformSignUpAsync()
        {
            // 1. TẠO CẤU TRÚC JSON ĐÚNG CHO API (Khắc phục lỗi "Field required")
            var dataToSend = new
            {
                // CÁC TRƯỜNG Ở CẤP ROOT (GỒM EMAIL, USERNAME, PASSWORD)
                username = txtUsername.Text,
                password = txtPassword.Text,
                email = txtEmail.Text,

                // CÁC TRƯỜNG THÔNG TIN CÁ NHÂN LỒNG TRONG user_info
                user_info = new
                {
                    firstname = txtFirstname.Text,
                    lastname = txtLastname.Text,
                    phone = txtPhone.Text,
                    // language = txtLanguage.Text, // Bỏ comment nếu có control này
                    sex = (rdoMale.Checked) ? "Male" : "Female",
                    date_of_birth = dtpBirthday.Value.ToString("yyyy-MM-dd") // Chuẩn hóa định dạng
                }
            };

            // Khởi tạo HttpClient
            HttpClient client = _apiHandler.GetClient();
            string jsonContent = JsonConvert.SerializeObject(dataToSend);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                // 2. Gửi yêu cầu HTTP POST
                HttpResponseMessage response = await client.PostAsync(SIGNUP_URL, httpContent);
                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // === ĐĂNG KÝ THÀNH CÔNG ===
                    MessageBox.Show("Đăng ký tài khoản thành công! Vui lòng đăng nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else // Xử lý lỗi (Status Code 4xx, 5xx)
                {
                    // === LOGIC XỬ LÝ LỖI LINH HOẠT (KHẮC PHỤC LỖI UNEXPECTED TOKEN STARTARRAY) ===
                    string errorMessage = $"Đăng ký thất bại. Mã trạng thái: {(int)response.StatusCode}";

                    try
                    {
                        // 1. Sử dụng JToken.Parse để phân tích cú pháp linh hoạt
                        JToken errorToken = JToken.Parse(responseString);

                        // Cố gắng tìm trường lỗi chi tiết, thường là 'detail' hoặc root nếu đó là mảng
                        JToken detailToken = errorToken["detail"] ?? errorToken.Root;

                        if (detailToken is JArray)
                        {
                            // TRƯỜNG HỢP 1: MẢNG LỖI (Validation Error - Gây ra lỗi StartArray ban đầu)
                            var errorList = detailToken.ToObject<List<ApiErrorDetail>>();

                            if (errorList != null && errorList.Any())
                            {
                                errorMessage = errorList.First().GetDisplayMessage();
                            }
                            else
                            {
                                errorMessage = "Phản hồi lỗi là mảng rỗng hoặc không đúng cấu trúc chi tiết.";
                            }
                        }
                        else if (detailToken is JObject)
                        {
                            // TRƯỜNG HỢP 2: ĐỐI TƯỢNG LỖI ĐƠN
                            ErrorModel errorObject = detailToken.ToObject<ErrorModel>();
                            errorMessage = errorObject.Detail ?? "Lỗi đối tượng không xác định.";
                        }
                        else if (detailToken is JValue)
                        {
                            // TRƯỜNG HỢP 3: Giá trị đơn thuần
                            errorMessage = detailToken.ToString();
                        }
                        else
                        {
                            errorMessage = $"Phản hồi lỗi có cấu trúc không xác định: {responseString.Substring(0, Math.Min(responseString.Length, 50))}...";
                        }
                    }
                    catch (JsonException ex)
                    {
                        // Bắt lỗi Deserialization
                        errorMessage = $"Lỗi hệ thống: Không thể đọc cấu trúc lỗi API. Chi tiết Json: {ex.Message} (Phản hồi không phải JSON tiêu chuẩn)";
                    }
                    catch (Exception ex)
                    {
                        // Lỗi chung khi xử lý lỗi
                        errorMessage = $"Lỗi hệ thống không xác định trong quá trình xử lý lỗi: {ex.Message}";
                    }

                    MessageBox.Show($"Đăng ký thất bại. Lỗi: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                // Bắt lỗi kết nối mạng hoặc lỗi timeout
                MessageBox.Show($"Lỗi kết nối: Không thể kết nối tới máy chủ API. Chi tiết: {ex.Message}", "Lỗi Mạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Bắt lỗi hệ thống không xác định
                MessageBox.Show($"Đã xảy ra lỗi hệ thống không xác định: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}