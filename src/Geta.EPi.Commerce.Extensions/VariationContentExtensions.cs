﻿using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using EPiServer.XForms.WebControls;

namespace Geta.EPi.Commerce.Extensions
{
    public static class VariationContentExtensions
    {
#pragma warning disable 649
        private static Injected<UrlResolver> _urlResolver;
        private static Injected<ILinksRepository> _linksRepository;
#pragma warning restore 649

        public static string GetUrl(this VariationContent variant)
        {
            var productLink = variant.GetParentProducts(_linksRepository.Service).FirstOrDefault();
            if (productLink == null)
            {
                return string.Empty;
            }
            var urlBuilder = new UrlBuilder(_urlResolver.Service.GetUrl(productLink));

            if (variant.Code != null)
            {
                urlBuilder.QueryCollection.Add("variationId", variant.Code);
            }
            return urlBuilder.ToString();
        }

        /// <summary>
        /// Get the parent products
        /// </summary>
        /// <param name="variationContent">The variation content</param>
        /// <param name="linksRepository">The link repository</param>
        /// <returns>Collection of product references</returns>
        public static IEnumerable<ContentReference> GetProducts(this VariationContent variationContent, ILinksRepository linksRepository)
        {
            return linksRepository.GetRelationsByTarget<ProductVariation>(variationContent.ContentLink).Select(r => r.Source);
        }

        /// <summary>
        /// Get the parent products
        /// </summary>
        /// <param name="variationContent">The variation content</param>
        /// <returns>Collection of product references</returns>
        public static IEnumerable<ContentReference> GetProducts(this VariationContent variationContent)
        {
            return variationContent.GetProducts(_linksRepository.Service);
        }
    }
}