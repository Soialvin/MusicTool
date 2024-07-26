using Microsoft.AspNetCore.Http;
using MusicTool.Models.DTO;

namespace MusicTool.DataAccess.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
