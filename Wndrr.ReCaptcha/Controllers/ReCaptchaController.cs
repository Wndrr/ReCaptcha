using System;
using Microsoft.AspNetCore.Mvc;
using Wndrr.ReCaptcha.Attributes;
using Wndrr.ReCaptcha.TagHelpers;

namespace Wndrr.ReCaptcha.Controllers
{
    public class ReCaptchaController : Controller
    {
        [HttpPost]
        [ReCaptcha]
        [Route("GetRecaptchaCachedValue")]
        public ContentResult Get(Guid reCaptchaCacheGuid, string reCaptchaToken)
        {
            var content = ReCaptchaContentCache.CachedContent[reCaptchaCacheGuid];
            return Content(content);
        }
    }
}