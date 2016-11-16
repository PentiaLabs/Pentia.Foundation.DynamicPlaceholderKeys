using System.Web;
using System.Web.Mvc;
using Pentia.Foundation.DynamicPlaceholderKeys.Model;
using Sitecore.Data;
using Sitecore.Mvc;
using Sitecore.Mvc.Presentation;

namespace Pentia.Foundation.DynamicPlaceholderKeys
{
  public static class DynamicPlaceholderKeysExtensions
  {
    public static HtmlString DynamicPlaceholder(this HtmlHelper helper, string basePlaceholderKey)
    {
      return helper.Sitecore().Placeholder(DynamicPlaceholderKeyRepository.Get(basePlaceholderKey, new ID(RenderingContext.Current.Rendering.UniqueId)));
    }
  }
}