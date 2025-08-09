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


            var response = await client.GetAsync(apiUrl + $"?name={model.City}&locale={model.Locale}");

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



            List<Hotel> newHotels = new List<Hotel>();

            foreach (var id in destIds)
            {
                var newResponse = await client.GetAsync(newApiUrl + $"?order_by=price&adults_number=2&checkin_date={formattedCheckinDate}&filter_by_currency=USD&dest_id={id}&locale={model.Locale}&checkout_date={formattedCheckoutDate}&units=metric&room_number=1&dest_type=city");
                response.EnsureSuccessStatusCode();
                var newResponseJson = await newResponse.Content.ReadAsStringAsync();
                
                JToken responseToken = JToken.Parse(newResponseJson);
                var hotel = responseToken.SelectToken("result");



                foreach (var h in hotel)
                {
                    var hotelName = h.Value<string>("hotel_name");
                    var hotelPhotoMainUrl = h.Value<string>("main_photo_url");
                    var hotelPrice = h.Value<double>("min_total_price");
                    var reviewScoreWord = h.Value<string>("review_score_word");
                    var reviewScore = h.Value<double?>("review_score");

                    StarsService.StarsService starService = new StarsService.StarsService();
                    string stars = starService.StarService(reviewScore);



                    if (hotelName != null && hotelPhotoMainUrl != null && hotelPrice > model.MinPrice && hotelPrice <= model.MaxPrice)
                    {
                        var newHotel = new Hotel
                        {
                            Name = hotelName,
                            PhotoMainUrl = hotelPhotoMainUrl,
                            ReviewScore = reviewScore,
                            ReviewScoreWord = reviewScoreWord,
                            Price = hotelPrice,
                            Stars = stars,
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
