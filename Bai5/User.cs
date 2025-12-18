using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Bai5
{
    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("sex")]
        public string Sex { get; set; }

    }
}