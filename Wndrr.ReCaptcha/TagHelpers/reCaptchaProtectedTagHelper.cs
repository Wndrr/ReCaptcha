using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Wndrr.ReCaptcha.TagHelpers
{
    [HtmlTargetElement(Attributes = "captcha")]
    public class ReCaptchaProtectedTagHelper : TagHelper
    {
        public bool Captcha { get; set; }
        [HtmlAttributeName("captcha-trigger-type")]
        public CaptchaTriggerType TriggerType { get; set; } = CaptchaTriggerType.Auto;
        [HtmlAttributeName("captcha-value")]
        public string Value { get; set; }

        [HtmlAttributeName("placeholder-class")]
        public string Classes { get; set; } = "btn btn-outline-info m-auto";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();
            var savedContent = childContent.GetContent();

            var guid = Guid.NewGuid();
            ReCaptchaContentCache.CachedContent.Add(guid, savedContent);

            var jsFriendlyGuid = guid.ToString().Replace('-', '_');

            output.Content.Clear();
            output.Content.AppendHtml($@"
<script src='https://www.google.com/recaptcha/api.js?render=6LeIoIUUAAAAAEAkT_rE9ub8rxlMSsx5_mU_mD0B'></script>
<form id='{jsFriendlyGuid}' action='/GetRecaptchaCachedValue' )>
    <input type='hidden' value='{guid}' name='reCaptchaCacheGuid'/>
    <input type='hidden' name='reCaptchaToken'/>
</form>
<span class='{Classes}' onclick='reCaptcha_{jsFriendlyGuid}(event)'>
{Value}
</span>
<script>
    function reCaptcha_{jsFriendlyGuid}(event) {{

                grecaptcha.ready(function () {{
                    grecaptcha.execute('6LeIoIUUAAAAAEAkT_rE9ub8rxlMSsx5_mU_mD0B', {{ action: 'send' }})
                    .then(function (token) {{
                        $('form#{jsFriendlyGuid} > input[name=reCaptchaToken]').val(token);

                        var form = $('form#{jsFriendlyGuid}');
                            var url = form.attr('action');

                            $.ajax({{
                                   type: 'POST',
                                   url: url,
                                   data: form.serialize(), // serializes the form's elements.
                                   success: function(data)
                                   {{
                                        form.parent().html(data);
                                   }}
                                 }});
                    }});
                }});
            }}
</script>

");
            if (TriggerType == CaptchaTriggerType.Auto)
            {
                output.Content.AppendHtml($@"
<script>
document.addEventListener(""DOMContentLoaded"", function() {{
    reCaptcha_{jsFriendlyGuid}();
}});
</script>
");
            }

        }
    }

    public enum CaptchaTriggerType
    {
        Button,
        Auto
    }

    public static class ReCaptchaContentCache
    {
        public static Dictionary<Guid, string> CachedContent { get; set; } = new Dictionary<Guid, string>();
    }
}
