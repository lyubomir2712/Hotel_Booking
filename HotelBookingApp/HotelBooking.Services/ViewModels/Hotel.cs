using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Services.ViewModels
{
    
        public class BenefitBadge
        {
            public string Text { get; set; }
            public string Identifier { get; set; }
            public string Explanation { get; set; }
            public string Variant { get; set; }
        }

        public class BoundingBox
        {
            public double NeLon { get; set; }
            public double NeLat { get; set; }
            public double SwLon { get; set; }
            public double SwLat { get; set; }
        }

        public class Checkin
        {
            public string UntilTime { get; set; }
            public string FromTime { get; set; }
        }

        public class Checkout
        {
            public string FromTime { get; set; }
            public string UntilTime { get; set; }
        }

        public class GrossPrice
        {
            public double Value { get; set; }
            public string Currency { get; set; }
        }

        public class MapPageFields
        {
            public List<PropertyAnnotation> PropertyAnnotations { get; set; }
            public BoundingBox BoundingBox { get; set; }
        }

        public class PriceBreakdown
        {
            public List<BenefitBadge> BenefitBadges { get; set; }
            public List<object> TaxExceptions { get; set; }
            public GrossPrice GrossPrice { get; set; }
            public StrikethroughPrice StrikethroughPrice { get; set; }
        }

        public class PropertyAnnotation
        {
            public double Longitude { get; set; }
            public double Latitude { get; set; }
        }

        public class Hotel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int MainPhotoId { get; set; }
            public string PhotoMainUrl { get; set; }
            public List<string> PhotoUrls { get; set; }
            public int Position { get; set; }
            public int RankingPosition { get; set; }
            public string CountryCode { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public double Price { get; set; }
            public string Currency { get; set; }
            public Checkin Checkin { get; set; }
            public Checkout Checkout { get; set; }
            public string CheckoutDate { get; set; }
            public string CheckinDate { get; set; }
            public double? ReviewScore { get; set; } = 0;
            public string ReviewScoreWord { get; set; }
            public int ReviewCount { get; set; }
            public int QualityClass { get; set; }
            public bool IsFirstPage { get; set; }
            public int AccuratePropertyClass { get; set; }
            public int PropertyClass { get; set; }
            public int Ufi { get; set; }
            public string WishlistName { get; set; }
            public int OptOutFromGalleryChanges { get; set; }
            public string Stars { get; set; }

            public int? has_swimming_pool { get; set; }
            public int? has_free_parking { get; set; }
            public int hotel_include_breakfast { get; set; }
    }

        public class Root
        {
            public int Count { get; set; }
            public MapPageFields MapPageFields { get; set; }
            public List<Hotel> Results { get; set; }
        }

        public class StrikethroughPrice
        {
            public double Value { get; set; }
            public string Currency { get; set; }
        }

    
}
