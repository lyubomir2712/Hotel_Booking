using HotelBooking.Services.Contracts;
using HotelBooking.Services.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using static HotelBooking.Services.ViewModels.Hotel;
using HotelBooking.Services.StarsService;

namespace HotelBooking.Services.ApiModule
{
    public class ApiService : IApiService
    {
        public async Task<List<Hotel>?> GetHotelsByLocation(string apiUrl, string newApiUrl, ApiDataViewModel model)
        {
            var client = new HttpClient();


            client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "b067df4ec3msh7add19d4e4747fbp12bc39jsna54fc447bbf1");
            client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "booking-com.p.rapidapi.com");


            var response = await client.GetAsync(apiUrl + $"?name={model.City}&locale=en-us");

            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            var hotels = JsonConvert.DeserializeObject<List<LocationId>>(responseJson);

            List<string> destIds = new List<string>();
            foreach (var obj in hotels)
            {
                string destId = obj.Dest_id;
                destIds.Add(destId);
            }

            string formattedCheckinDate = model.CheckinDate.ToString("yyyy-MM-dd");
            string formattedCheckoutDate = model.CheckoutDate.ToString("yyyy-MM-dd");

            var responseString =
                $"?order_by=price&adults_number={model.AdultsNumber}&checkin_date={formattedCheckinDate}&filter_by_currency=USD&locale=en-us&checkout_date={formattedCheckoutDate}&units=metric&room_number={model.RoomsNumber}&dest_type=city";
            if (model.ChildrenNumber > 0)
            {
                responseString += ($"&children_number={model.ChildrenNumber}");
            }

            List<Hotel> newHotels = new List<Hotel>();

            foreach (var id in destIds)
            {
                var newResponse = await client.GetAsync(newApiUrl + responseString + $"&dest_id={id}");
                newResponse.EnsureSuccessStatusCode();
                var newResponseJson = await newResponse.Content.ReadAsStringAsync();
                
                JToken responseToken = JToken.Parse(newResponseJson);
                var hotel = responseToken.SelectToken("result");
                if (hotel == null || !hotel.HasValues) continue;



                foreach (var h in hotel)
                {
                    var hotelName = h.Value<string>("hotel_name");
                    var hotelPhotoMainUrl = h.Value<string>("main_photo_url");
                    var hotelPrice = h.Value<double>("min_total_price");
                    var reviewScoreWord = h.Value<string>("review_score_word");
                    var reviewScore = h.Value<double?>("review_score");
                    var reviewsCount = h.Value<int?>("review_nr");
                    
                    if (hotelName != null && hotelPhotoMainUrl != null && hotelPrice > model.MinPrice && hotelPrice <= model.MaxPrice)
                    {
                        var newHotel = new Hotel
                        {
                            Name = hotelName,
                            PhotoMainUrl = hotelPhotoMainUrl,
                            ReviewScore = reviewScore,
                            ReviewScoreWord = reviewScoreWord,
                            Price = hotelPrice,
                            ReviewsCount = reviewsCount,
                            StartAt = formattedCheckinDate,
                            EndAt = formattedCheckoutDate,
                        };

                        newHotels.Add(newHotel);
                    }

                }

            }
            return newHotels;
        }

    }

}
