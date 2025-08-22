using System.Text;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;
using HotelBooking.Services.Contracts.EmailServicesContracts;
using HotelBooking.Services.Contracts.HotelsServicesContracts;

namespace HotelBooking.Services.EmailServices
{
    public class CheckoutEmailService : ICheckoutEmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly IEmailTemplatePathProviderService _emailTemplatePathProviderService;
        private readonly IGetEmailTemplateFromPathService _getEmailTemplateFromPathService;
        private readonly IGetEmailTemplateHtmlWithParametersService _getEmailTemplateHtmlWithParametersService;

        public CheckoutEmailService(
            IEmailSender emailSender,
            IEmailTemplatePathProviderService emailTemplatePathProviderService,
            IGetEmailTemplateFromPathService getEmailTemplateFromPathService,
            IGetEmailTemplateHtmlWithParametersService getEmailTemplateHtmlWithParametersService)
        {
            _emailSender = emailSender;
            _emailTemplatePathProviderService = emailTemplatePathProviderService;
            _getEmailTemplateFromPathService = getEmailTemplateFromPathService;
            _getEmailTemplateHtmlWithParametersService = getEmailTemplateHtmlWithParametersService;
        }

        public async Task SendCheckoutSummaryAsync(IUnitOfWork unitOfWork, UserModel currentUser, List<BookingModel> bookings)
        {
            if (currentUser == null || bookings == null || bookings.Count == 0)
                return;

            var emailReceiver = currentUser.Email;
            if (string.IsNullOrWhiteSpace(emailReceiver))
                throw new InvalidOperationException("User does not have a valid email address.");

            var subject = "Successfully Checked Out Bookings";
            var templatePath = _emailTemplatePathProviderService.CheckoutBookingsEmailTemplatePath;
            var template = await _getEmailTemplateFromPathService.GetEmailTemplateFromPath(templatePath);

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<div style=\"font-family:Arial, sans-serif;\">");
            stringBuilder.AppendLine("  <h2>Booking summary</h2>");
            stringBuilder.AppendLine("  <div>");

            foreach (var booking in bookings)
            {
                var hotel = await unitOfWork.Repository<HotelModel>()
                    .FirstOrDefaultAsync(h => h.Id == booking.HotelModelId);

                var renderedEmailBookingHtml = _getEmailTemplateHtmlWithParametersService
                    .GetEmailTemplateHtmlWithParameters(template, currentUser, booking, hotel);

                stringBuilder.AppendLine(renderedEmailBookingHtml);
                stringBuilder.AppendLine("<hr style=\"margin:16px 0; border:none; border-top:1px solid #ddd;\">");
            }

            stringBuilder.AppendLine("  </div>");
            stringBuilder.AppendLine("  <p style=\"font-size:12px;color:#666;\">This email provides an overview of your recent checkout.</p>");
            stringBuilder.AppendLine("</div>");

            await _emailSender.SendAsync(emailReceiver, subject, stringBuilder.ToString());
        }
    }
}