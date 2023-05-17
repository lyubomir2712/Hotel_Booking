using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Services.Contracts
{
    public interface IApiConnectionService
    {
        public HttpClient ApiConnection { get;}
        public void EstablisConnection();

       
    }
}
