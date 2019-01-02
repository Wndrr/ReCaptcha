using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RelaxationPortal.Attributes
{
    public class ReCaptchaAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            using (var httpClient = new HttpClient())
            {
                var secret = "6LeIoIUUAAAAAEWYXavuE4x_Fu2iRoZOSd_Okbf6";
                var googleVerifyUrl = "https://www.google.com/recaptcha/api/siteverify";

                if (context.ActionArguments["reCaptchaToken"] is string response)
                {

                    var values = new Dictionary<string, string>
                    {
                        { "secret", secret},
                        { "response", response}

                    };

                    var content = new FormUrlEncodedContent(values);
                    var wsCallResponseString = httpClient
                        .PostAsync(googleVerifyUrl, content)
                        .Result
                        .Content
                        .ReadAsStringAsync()
                        .Result;
                    var trucResponse = ReCaptchaV3Response.FromJson(wsCallResponseString);

                    if(trucResponse.Success == false || trucResponse.Score < 0.5)
                        throw new UnauthorizedAccessException();
                }
            }

            base.OnActionExecuting(context);
        }
    }
}