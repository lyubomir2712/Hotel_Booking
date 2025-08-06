using HotelBooking.Models.BaseModels;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Models.AppModels;

public class AdminPanelBookings : BaseModel
{
    public int ClientId { get; set; }
    public string ClientFirstName { get; set; }
    public string ClientLastName { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public int Price { get; set; }
    public int HotelModelId { get; set; }

    public HotelModel HotelModel { get; set; }

}