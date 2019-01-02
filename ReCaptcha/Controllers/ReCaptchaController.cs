using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RelaxationPortal.Attributes;
using RelaxationPortal.TagHelpers;

namespace RelaxationPortal.Controllers
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