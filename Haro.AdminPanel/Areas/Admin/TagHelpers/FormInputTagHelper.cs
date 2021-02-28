using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haro.AdminPanel.Models.Enums;
using Haro.AdminPanel.Utilities.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Haro.AdminPanel.Areas.Admin.TagHelpers
{
    [HtmlTargetElement("form-input")]
    public class FormInputTagHelper : TagHelper
    {
        [HtmlAttributeName("for")] public ModelExpression For { get; set; }

        [HtmlAttributeName("name")] public string Name { get; set; }
        [HtmlAttributeName("value")] public string Value { get; set; }
        [HtmlAttributeName("label")] public string Label { get; set; }
        [HtmlAttributeName("type")] public ColumnType ColumnType { get; set; }
        [HtmlAttributeName("id")] public string Id { get; set; }
        [HtmlAttributeName("select-items")] public List<SelectListItem> SelectItems { get; set; }
        [HtmlAttributeName("multiple")] public bool Multiple { get; set; }
        [HtmlAttributeName("disabled")] public bool Disabled { get; set; }
        [ViewContext] [HtmlAttributeNotBound] public ViewContext ViewContext { get; set; }
        public IHtmlGenerator Generator { get; }

        public FormInputTagHelper(IHtmlGenerator generator) : base()
        {
            Generator = generator;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            string str;
            if (string.IsNullOrEmpty(Id)) Id = Name;
            if (string.IsNullOrEmpty(Name)) Name = "";
            if (For == null) throw new Exception("For not bound");
            if (SelectItems != null)
            {
                object model = For.Model;
                if (model != null) str = model.ToString();
                else str = null;
                SelectItems.ForEach(x =>
                {
                    if (x.Value == Value) x.Selected = true;
                });
            }

            output = await WrapResult(output, context);
            if (ColumnType == ColumnType.Hidden)
            {
                output.SetAttribute("style", "display:none;");
            }
        }

        private async Task<TagHelperOutput> WrapResult(TagHelperOutput output, TagHelperContext context)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "form-group");

            output.Content.AppendHtml(await CreateLabelElement(context));
            output.Content.AppendHtml(await Create(context));
            return output;
        }

        private async Task<TagHelperOutput> CreateLabelElement(TagHelperContext context)
        {
            LabelTagHelper labelTagHelper = new LabelTagHelper(Generator);
            labelTagHelper.ViewContext = ViewContext;
            labelTagHelper.For = For;
            var tagHelperOutput = TagHelperExtension.CreateTagHelperOutput("label");
            await labelTagHelper.ProcessAsync(context, tagHelperOutput);
            return tagHelperOutput;
        }

        private async Task<TagHelperOutput> Create(TagHelperContext context)
        {
            TagHelperOutput output;
            switch (ColumnType)
            {
                case ColumnType.Text:
                case ColumnType.Image:
                case ColumnType.MultipleImage:
                case ColumnType.Number:
                case ColumnType.Bool:
                case ColumnType.Password:
                case ColumnType.Hidden:
                    output =  await CreateInput(context);
                    break;
                case ColumnType.TextArea:
                case ColumnType.Editor:
                    output =  await CreateTextArea(context);
                    break;
                case ColumnType.SelectList:
                case ColumnType.MultipleSelectList:
                    output = await CreateSelect(context);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (Disabled)
            {
                output.SetAttribute("disabled","true");
            }
            return output;
        }

        private async Task<TagHelperOutput> CreateTextArea(TagHelperContext context)
        {
            var tagHelper = new TextAreaTagHelper(Generator);
            tagHelper.For = For;
            tagHelper.ViewContext = ViewContext;
            var tagHelperOutput = TagHelperExtension.CreateTagHelperOutput("textarea");
            await tagHelper.ProcessAsync(context, tagHelperOutput);
            return tagHelperOutput;
        }

        private async Task<TagHelperOutput> CreateSelect(TagHelperContext context)
        {
            SelectItems ??= new List<SelectListItem>();
            var tagHelper = new SelectTagHelper(Generator);
            tagHelper.ViewContext = ViewContext;
            tagHelper.For = For;
            tagHelper.Items = SelectItems;
            TagHelperOutput tagHelperOutput = TagHelperExtension.CreateTagHelperOutput("select");
            if (Multiple) tagHelperOutput.Attributes.Add("multiple", "multiple");
            tagHelperOutput.SetAttribute("class", "form-control");
            await tagHelper.ProcessAsync(context, tagHelperOutput);
            return tagHelperOutput;
        }

        private async Task<TagHelperOutput> CreateInput(TagHelperContext context)
        {
            InputTagHelper inputTagHelper = new InputTagHelper(Generator);
            inputTagHelper.ViewContext = ViewContext;
            inputTagHelper.For = For;
            TagHelperOutput tagHelperOutput = TagHelperExtension.CreateTagHelperOutput("input");
            await inputTagHelper.ProcessAsync(context, tagHelperOutput);
            tagHelperOutput.SetAttribute("type", GetInputTypeName(ColumnType));
            tagHelperOutput.SetAttribute("class", "form-control");
            return tagHelperOutput;
        }

        private string GetInputTypeName(ColumnType type)
        {
            switch (type)
            {
                case ColumnType.Text:
                case ColumnType.Image:
                case ColumnType.MultipleImage:
                    return "text";
                case ColumnType.Bool:
                    return "checkbox";
                case ColumnType.Number:
                    return "number";
                case ColumnType.Password:
                    return "password";
                case ColumnType.Hidden:
                    return "hidden";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}