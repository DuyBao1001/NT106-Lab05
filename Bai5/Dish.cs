using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bai5
{

    public class Dish
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("ten_mon_an")]
        public string Name { get; set; }

        [JsonProperty("gia")]
        public int Price { get; set; }

        [JsonProperty("dia_chi")]
        public string Address { get; set; }

        [JsonProperty("mo_ta")]
        public string Description { get; set; }

        [JsonProperty("hinh_anh")]
        public string ImageUrl { get; set; }

        [JsonProperty("nguoi_dong_gop")]
        public string ContributorName { get; set; }

        [JsonProperty("user_id")]
        public int ContributorId { get; set; }

        [JsonProperty("category_id")]
        public int CategoryId { get; set; }
    }
}