using HotelBooking.Data.Entities;
using HotelBooking.Data.Entities.Request;
using HotelBooking.Data.Entities.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Web;

namespace HotelBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingApiController : ControllerBase
    {
 
        [HttpGet]

        public async Task<List<Hotel>> GetHotelsInCity([FromQuery]string model)

        {

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-rapidapi-host", "booking-com.p.rapidapi.com");
            client.DefaultRequestHeaders.Add("x-rapidapi-key", "b067df4ec3msh7add19d4e4747fbp12bc39jsna54fc447bbf1");


            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["name"] = model;
            queryString["locale"] = "en-gb";
            //queryString["sort_by"] = "popularity";
            //queryString["checkin_date"] = model.CheckinDate.ToString();
            //queryString["checkout_date"] = model.CheckoutDate.ToString();
            //queryString["filter_by_currency"] = "USD";
            //queryString["price_filter_min"] = model.MinPrice.ToString();
            //queryString["price_filter_max"] = model.MaxPrice.ToString();

            //if (model.HasPool)
            //{
            //    queryString["facilities"] += "Pool,";
            //}
            //if (hasParking)
            //{
            //    queryString["facilities"] += "Parking,";
            //}
            //if (hasFitness)
            //{
            //    queryString["facilities"] += "Fitness,";
            //}
            //if (hasInternet)
            //{
            //    queryString["facilities"] += "Internet,";
            //}
            //if (hasRestaurant)
            //{
            //    queryString["facilities"] += "Restaurant,";
            //}

            var urlBuilder = new UriBuilder("https://booking-com.p.rapidapi.com/v1/hotels/locations");
            urlBuilder.Query = queryString.ToString();


            var response = await client.GetAsync(urlBuilder.Uri);
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            var hotels = JsonConvert.DeserializeObject<List<Hotel>>(responseJson);


            return hotels;
        }
    }
}






