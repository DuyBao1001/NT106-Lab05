using Bai5;
using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;

namespace Bai5
{
    public partial class RandomFoodPopupForm : Form
    {
        private readonly Dish _randomDish;
        public RandomFoodPopupForm(Dish dish)
        {
            InitializeComponent();
            _randomDish = dish;
            this.Load += RandomFoodPopupForm_Load;
        }

        private void RandomFoodPopupForm_Load(object sender, EventArgs e)
        {
            if (_randomDish != null)
            {
                BindFoodData(_randomDish);
            }
            else
            {
                lblDishName.Text = "Không tìm thấy món ăn ngẫu nhiên!";
            }
        }

        private void BindFoodData(Dish food)
        {
            this.Text = $"Ăn {food.Name} đi!";
            lblDishName.Text = food.Name;
            lblPriceValue.Text = food.Price.ToString("N0") + " VNĐ";
            lblAddressValue.Text = food.Address;
            lblDescriptionValue.Text = food.Description;

            if (!string.IsNullOrEmpty(food.ImageUrl))
            {
                LoadImageAsync(food.ImageUrl);
            }
        }

        private async void LoadImageAsync(string imageUrl)
        {
            string encodedUrl = Uri.EscapeUriString(imageUrl);

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var imageBytes = await client.GetByteArrayAsync(encodedUrl);

                    using (var ms = new MemoryStream(imageBytes))
                    {
                        picFoodImage.Image = Image.FromStream(ms);
                        picFoodImage.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                }
                catch (HttpRequestException)
                {
                    picFoodImage.Image = null;
                }
                catch (Exception)
                {
                    picFoodImage.Image = null;
                }
            }
        }
    }
}