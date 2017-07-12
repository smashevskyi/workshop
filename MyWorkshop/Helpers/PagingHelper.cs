using MyWorkshop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MyWorkshop.Helpers
{
    public static class PagingHelper
    {
        // modified code from A.Friman's book
        public static MvcHtmlString PageLinks(
        this HtmlHelper html,
        PagingInfo pagingInfo,
        Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder li = new TagBuilder("li");   // <li>
                li.AddCssClass("page-item");            // <li class="page-item">
                if (i == pagingInfo.CurrentPage)        // or
                    li.AddCssClass("active");           // <li class="page-item active">

                TagBuilder a = new TagBuilder("a");     // <a>
                a.AddCssClass("page-link");             // <a class="page-link">
                a.MergeAttribute("href", pageUrl(i));   // <a href="url" class="page-link">
                a.InnerHtml = i.ToString();             // <a href="url" class="page-link"> 1..n </a>

                li.InnerHtml = a.ToString();            // <li class="page-item"><a></a></li>

                result.Append(li.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}