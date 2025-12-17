using System;
using System.Collections.Generic;

namespace Bai4
{
    public class Movie
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string DetailUrl { get; set; }
        public int Price { get; set; } = 40000;
    }
    public class OrderInfo
    {
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string MovieName { get; set; }
        public List<string> Seats { get; set; }
        public long TotalPrice { get; set; }
        public DateTime BookingTime { get; set; }
    }
}