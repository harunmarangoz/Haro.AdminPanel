// Decompiled with JetBrains decompiler
// Type: Haro.AdminTagHelper.TagHelperExtension
// Assembly: Haro.AdminTagHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4F814C32-95AC-4E62-892D-EBBAA38E1D9F
// Assembly location: C:\Users\harun\Downloads\aracTavsiye\aracTavsiye\Haro.AdminTagHelper.dll

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Haro.AdminPanel.Utilities.Extensions
{
  public static class TagHelperExtension
  {
		public static void AppendHtml(this TagBuilder tag, IHtmlContent html)
		{
			tag.InnerHtml.AppendHtml(html);
		}

		public static void AppendHtml(this TagBuilder tag, string str)
		{
			tag.InnerHtml.AppendHtml(str);
		}

		public static TagHelperOutput CreateTagHelperOutput(string tagName)
		{
			return new TagHelperOutput(
				tagName,
				new TagHelperAttributeList(),
				(s, t) =>
				{
					return Task.Factory.StartNew<TagHelperContent>(
						() => new DefaultTagHelperContent());
				}
			);
		}

		public static bool HasAttribute(this TagHelperOutput tagHelperOutput, string str)
		{
			return tagHelperOutput.Attributes.Any(x => x.Name == str);
		}

		public static bool HasAttribute(this TagHelperOutput tagHelperOutput, Func<TagHelperAttribute, bool> func)
		{
			return (object)Enumerable.FirstOrDefault<TagHelperAttribute>(tagHelperOutput.Attributes, func) != (object)null;
		}

		public static void SetAttribute(this TagBuilder tag, string key, string value)
		{
			tag.Attributes.Add(key, value);
		}

		public static void SetAttribute(this TagHelperOutput tag, string key, string value)
		{
			tag.Attributes.Add(key, value);
		}

		private static void SetScript(this TagHelperOutput inputElement, ViewContext viewContext, string scr)
		{
			if (string.IsNullOrEmpty(scr))
			{
				return;
			}
			dynamic viewBag = ((dynamic)viewContext.ViewBag).RenderPartial != (dynamic)null;
			if ((!viewBag ? viewBag : viewBag & ((dynamic)viewContext.ViewBag).RenderPartial))
			{
				TagBuilder tagBuilder = new TagBuilder("script");
				tagBuilder.AppendHtml(scr);
				inputElement.PostContent.AppendHtml(tagBuilder);
				return;
			}
			IHtmlContent htmlString = new HtmlString(scr);
			string str = "Scripts";
			List<IHtmlContent> item = viewContext.TempData[str] as List<IHtmlContent>;
			if (item != null)
			{
				item.Add(htmlString);
			}
			else
			{
				List<IHtmlContent> list = new List<IHtmlContent>();
				list.Add(htmlString);
				item = list;
			}
			viewContext.TempData[str]= item;
		}

		public static TagBuilder WrapElement(this TagBuilder element, string tagName, string classValue = "")
		{
			TagBuilder tagBuilder = new TagBuilder(tagName);
			tagBuilder.AddCssClass(classValue);
			tagBuilder.InnerHtml.AppendHtml(element);
			return tagBuilder;
		}

		public static IHtmlContent WrapElements(List<IHtmlContent> elements, string tagName, string classValue = "")
		{
			TagBuilder tagBuilder = new TagBuilder(tagName);
			tagBuilder.AddCssClass(classValue);
			foreach (IHtmlContent element in elements)
			{
				tagBuilder.InnerHtml.AppendHtml(element);
			}
			return tagBuilder;
		}
	}
}
