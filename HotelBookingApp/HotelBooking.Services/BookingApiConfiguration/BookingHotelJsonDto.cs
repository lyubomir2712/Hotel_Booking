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

        [JsonProperty("hotel_id")]
        public int HotelId { get; set; }

        [JsonProperty("country_trans")]
        public string Country { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("latitude")]
        public double? Latitude { get; set; }

        [JsonProperty("longitude")]
        public double? Longitude { get; set; }

        [JsonProperty("distance_to_cc")]
        public double? DistanceToCenter { get; set; }

        [JsonProperty("room_name")]
        public string RoomName { get; set; }

        [JsonProperty("bed_configuration")]
        public string BedConfiguration { get; set; }

        [JsonProperty("room_surface_in_m2")]
        public double? RoomSurface { get; set; }

        [JsonProperty("distance_to_cc_formatted")]
        public string DistanceToCityCenter { get; set; }
}