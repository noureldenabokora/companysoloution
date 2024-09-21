using Demo.DAL.Models;

namespace routeone.Helpers
{
    public interface IEmailSettings
    {
        public void SendEmail(Email email);
    }
}
