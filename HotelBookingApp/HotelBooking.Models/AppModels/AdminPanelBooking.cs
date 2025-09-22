using System;
using HotelBooking.Models.BaseModels;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Models.AppModels;

public class AdminPanelBooking : BaseModel
{
    public int ClientId { get; set; }
    public string ClientFirstName { get; set; }
    public string ClientLastName { get; set; }
    public string ClientEmail { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public double Price { get; set; }

    public int AdultsNumber { get; set; } = 1;

    public int? ChildrenNumber { get; set; }

    public int RoomsNumber { get; set; } = 1;
    public int HotelModelId { get; set; }
    public HotelModel HotelModel { get; set; }

}