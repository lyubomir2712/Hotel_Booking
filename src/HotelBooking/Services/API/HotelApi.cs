using System.Net.Http.Headers;
using static System.Net.WebRequestMethods;

namespace HotelBooking.Services.API
{
    public class HotelApi
    {
        //HttpClient client = new HttpClient();
        //HttpRequestMessage request = new HttpRequestMessage
        //{
        //    Method = HttpMethod.Get,
        //    RequestUri = new Uri("https://booking-com.p.rapidapi.com/v1/metadata/exchange-rates?currency=AED&locale=en-gb"),
        //    Headers = {
        //        { "X-RapidAPI-Key", "b067df4ec3msh7add19d4e4747fbp12bc39jsna54fc447bbf1" },
        //        { "X-RapidAPI-Host", "booking-com.p.rapidapi.com" },
        //    },
        //};


        //public async void Response()
        //{
        //    using (var response = await client.SendAsync(request))
        //    {
        //        response.EnsureSuccessStatusCode();
        //        var body = await response.Content.ReadAsStringAsync();
        //        Console.WriteLine(body);
        //    }
        //}



        
    HttpClient client = new HttpClient();
    HttpRequestMessage request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://booking-com.p.rapidapi.com/v2/hotels/details?hotel_id=1676161&currency=BGN&locale=bg&checkout_date=2023-09-28&checkin_date=2023-09-27"),
            Headers =
        {
            { "X-RapidAPI-Key", "b067df4ec3msh7add19d4e4747fbp12bc39jsna54fc447bbf1" },
            { "X-RapidAPI-Host", "booking-com.p.rapidapi.com" },
        },
        };

        public async void Response()
        {
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
            }
        }
        
    } 
}
