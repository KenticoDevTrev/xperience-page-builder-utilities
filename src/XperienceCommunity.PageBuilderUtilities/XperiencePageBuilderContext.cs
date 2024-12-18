using System;
using CMS.Websites.Routing;
using Kentico.PageBuilder.Web.Mvc;

namespace XperienceCommunity.PageBuilderUtilities
{
    public class XperiencePageBuilderContext(IWebsiteChannelContext websiteChannelContext,
        IPageBuilderDataContextRetriever pageBuilderDataContextRetriever) : IPageBuilderContext
    {
        private readonly IWebsiteChannelContext websiteChannelContext = websiteChannelContext ?? throw new ArgumentNullException(nameof(websiteChannelContext));
        private readonly IPageBuilderDataContextRetriever pageBuilderDataContextRetriever = pageBuilderDataContextRetriever ?? throw new ArgumentNullException(nameof(pageBuilderDataContextRetriever));

        /// <inheritdoc />
        public bool IsPreviewMode => websiteChannelContext.IsPreview;

        /// <inheritdoc />
        public bool IsLivePreviewMode => IsPreviewMode && !IsEditMode;

        /// <inheritdoc />
        public bool IsEditMode
        {
            get
            {
                try
                {
                    return pageBuilderDataContextRetriever.Retrieve().EditMode;
                }
                catch (Exception)
                {
                }
                return false;
            }
        }

        /// <inheritdoc />
        public bool IsLiveMode => !IsPreviewMode;

        /// <inheritdoc />
        public PageBuilderMode Mode =>
            IsLiveMode
                ? PageBuilderMode.Live
                : IsLivePreviewMode
                    ? PageBuilderMode.LivePreview
                    : PageBuilderMode.Edit;

        /// <inheritdoc />
        public string ModeName() => Enum.GetName(typeof(PageBuilderMode), Mode) ?? "";
    }
}
