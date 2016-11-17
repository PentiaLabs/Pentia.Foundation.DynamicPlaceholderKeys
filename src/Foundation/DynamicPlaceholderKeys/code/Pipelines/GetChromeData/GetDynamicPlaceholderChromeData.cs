#region Dependencies

using System;
using System.Text.RegularExpressions;
using Pentia.Foundation.DynamicPlaceholderKeys.Pipelines.GetPlaceholderRenderings;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetChromeData;
using Sitecore.Web.UI.PageModes;

#endregion

namespace Pentia.Foundation.DynamicPlaceholderKeys.Pipelines.GetChromeData
{
  public class GetDynamicPlaceholderChromeData : GetPlaceholderChromeData
  {
    public override void Process(GetChromeDataArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.IsNotNull(args.ChromeData, "Chrome Data");
      if (! args.ChromeType.Equals("placeholder", StringComparison.InvariantCultureIgnoreCase))
      {
        return;
      }

      var placeholderKey = (string) args.CustomData["placeHolderKey"];
      if (string.IsNullOrEmpty(placeholderKey))
        return;

      var regex = new Regex(GetDynamicKeyAllowedRenderings.DynamicKeyRegex);
      var match = regex.Match(placeholderKey);
      if (!match.Success || match.Groups.Count <= 0)
      {
        return;
      }

      var newPlaceholderKey = match.Groups[1].Value;
      if (args.Item == null) return;
      var layout = ChromeContext.GetLayout(args.Item);
      var item = Client.Page.GetPlaceholderItem(newPlaceholderKey, args.Item.Database, layout);
      if (item != null)
      {
        args.ChromeData.DisplayName = item.DisplayName;
      }
      if ((item != null) && !string.IsNullOrEmpty(item.Appearance.ShortDescription))
      {
        args.ChromeData.ExpandedDisplayName = item.Appearance.ShortDescription;
      }
    }
  }
}