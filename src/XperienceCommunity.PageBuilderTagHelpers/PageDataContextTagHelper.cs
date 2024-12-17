using System;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace XperienceCommunity.PageBuilderTagHelpers
{
    /// <summary>
    /// <see cref="ITagHelper"/> implementation targeting &lt;page-data-context&gt; elements that conditionally renders
    /// content based on the existance of the <see cref="IWebPageDataContextRetriever" /> retrieved from 
    /// <see cref="IWebPageDataContextRetriever"/> and the value of the <see cref="Initialized" /> attribute of the Tag Helper.
    /// </summary>
    [HtmlTargetElement("page-data-context")]
    public class PageDataContextTagHelper(IWebPageDataContextRetriever webPageDataContextRetriever) : TagHelper
    {
        private readonly IWebPageDataContextRetriever webPageDataContextRetriever = webPageDataContextRetriever ?? throw new ArgumentNullException(nameof(webPageDataContextRetriever));

        /// <inheritdoc />
        public override int Order => 0;

        /// <summary>
        /// Determines whether or not the inner content of the Tag Helper is displayed.
        /// If the <see cref="IWebPageDataContextRetriever" /> is populated for the current request and <see cref="Initialized" />
        /// is true or the <see cref="IWebPageDataContextRetriever" /> is not populated and <see cref="Initialized" /> is false, the
        /// content will be displayed. Otherwise the content will not be displayed.
        /// </summary>
        public bool Initialized { get; set; } = true;

        /// <inheritdoc />
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(context);

            ArgumentNullException.ThrowIfNull(output);

            // Always strip the outer tag name as we never want <page-data-context> to render
            output.TagName = null;

            if (webPageDataContextRetriever.TryRetrieve(out var data))
            {
                if (data is not null && Initialized)
                {
                    return;
                }

                output.SuppressOutput();
            }

            if (!Initialized)
            {
                return;
            }

            output.SuppressOutput();
        }
    }
}
