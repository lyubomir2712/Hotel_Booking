using Newtonsoft.Json;

namespace HotelBooking.Services.BookingApiConfiguration;

internal class BookingHotelJsonDto
{ 
        [JsonProperty("hotel_name")] 
        public string? HotelName { get; set; }

        [JsonProperty("main_photo_url")]
        public string? MainPhotoUrl { get; set; }

        [JsonProperty("min_total_price")]
        public double? MinTotalPrice { get; set; }

        [JsonProperty("review_score")]
        public double? ReviewScore { get; set; }

        [JsonProperty("review_score_word")]
        public string? ReviewScoreWord { get; set; }

        [JsonProperty("review_nr")]
        public int? ReviewCount { get; set; }
}