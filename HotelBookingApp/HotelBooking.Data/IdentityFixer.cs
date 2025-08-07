using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data;

public static class IdentityFixer
{
    private const string FixSql = @"
            DECLARE @max INT;
            SELECT @max = ISNULL(MAX(Id), 0) FROM dbo.Bookings;
            DECLARE @current INT;
            SELECT @current = IDENT_CURRENT('dbo.Bookings');
            IF (@current < @max)
                DBCC CHECKIDENT ('dbo.Bookings', RESEED, @max);";

    /// <summary>Reseeds Bookings.Id if the identity value lags behind MAX(Id).</summary>
    public static Task FixBookingsIdentityAsync(this BookingDbContext db) =>
        db.Database.ExecuteSqlRawAsync(FixSql);
}