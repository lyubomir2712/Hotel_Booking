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
        
        [JsonProperty("distance_to_cc_formatted")]
        public string DistanceToCityCenter { get; set; }
        
        [JsonProperty("is_free_cancellable")]
        public bool? IsFreeCancellable { get; set; }

        [JsonProperty("is_no_prepayment_block")]
        public bool? IsNoPrepayment { get; set; }

        [JsonProperty("hotel_include_breakfast")]
        public bool? IncludesBreakfast { get; set; }

        [JsonProperty("has_free_parking")]
        public bool? HasFreeParking { get; set; }
        
        [JsonProperty("accommodation_type_name")]
        public string? AccommodationType { get; set; }

        [JsonProperty("is_beach_front")]
        public bool? IsBeachFront { get; set; }
}