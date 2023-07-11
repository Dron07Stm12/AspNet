using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Threading.Tasks;




namespace Platform.Platform
{
    public class ConsentMiddleware
    {
        private RequestDelegate request;

        public ConsentMiddleware(RequestDelegate request)
        {
            this.request = request; 
        }

        public Task Invoke(HttpContext http) {

            if (http.Request.Path == "/consent")
            {
                ITrackingConsentFeature feature = http.Features.Get<ITrackingConsentFeature>();
                if (! feature.HasConsent)
                {
                    feature.GrantConsent();
                }
                else
                {
                    feature.WithdrawConsent();   
                }

                return http.Response.WriteAsync(feature.HasConsent ? "Consent Granted(true)\n" : "Consent Withdrawn(false)\n");
            }

            return request(http);
        }
    }
}
