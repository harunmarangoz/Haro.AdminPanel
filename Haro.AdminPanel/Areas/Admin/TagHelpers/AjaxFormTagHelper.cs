// Decompiled with JetBrains decompiler
// Type: Haro.AdminTagHelper.Form.Ajax.AjaxFormTagHelper
// Assembly: Haro.AdminTagHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4F814C32-95AC-4E62-892D-EBBAA38E1D9F
// Assembly location: C:\Users\harun\Downloads\aracTavsiye\aracTavsiye\Haro.AdminTagHelper.dll

using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Haro.AdminPanel.Areas.Admin.TagHelpers
{
    [HtmlTargetElement("ajax-form")]
    public class AjaxFormTagHelper : FormTagHelper
    {
        public AjaxFormTagHelper(IHtmlGenerator generator) : base(generator)
        {
        }

        [HtmlAttributeName("begin")] public string Begin { get; set; } = "ajaxFormBegin";
        [HtmlAttributeName("success")] public string Success { get; set; } = "ajaxFormSuccess";
        [HtmlAttributeName("failure")] public string Fail { get; set; } = "ajaxFormFail";
        [HtmlAttributeName("complete")] public string Complete { get; set; } = "ajaxFormComplete";

        [HtmlAttributeName("submit-button")] public bool SubmitButton { get; set; } = true;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("data-ajax-method", this.Method);
            output.Attributes.Add("enctype", "multipart/form-data");
            await base.ProcessAsync(context, output);
            output.TagName = "form";
            output.Attributes.Add("data-ajax", "true");
            output.Attributes.Add("data-ajax-begin", Begin);
            output.Attributes.Add("data-ajax-failure", Fail);
            output.Attributes.Add("data-ajax-success", Success);
            output.Attributes.Add("data-ajax-complete", Complete);
            if (SubmitButton)
            {
                var tag = new TagBuilder("button");
                tag.Attributes.Add("type", "submit");
                tag.Attributes.Add("class", "btn btn-success");
                tag.InnerHtml.Append("Kaydet");
                output.PostContent.AppendHtml(tag);
            }
        }
    }
}