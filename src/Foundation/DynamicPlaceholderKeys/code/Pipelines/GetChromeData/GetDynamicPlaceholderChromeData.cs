#region Copyright (c) Pentia, 1997 - 2014. All rights reserved (R).

// *************************************************************************
// *                                                                       *
// * http://www.pentia.net/                                                *
// *                                                                       *
// * E-Mail: info@pentia.net                                               *
// *                                                                       *
// * Copyright(C) Pentia, 1997 - 2014. All rights reserved (R).            *
// *                                                                       *
// * LEGAL NOTICE: This is unpublished proprietary source code of Pentia.  *
// * The contents of this file are protected by copyright laws and         *
// * international copyright treaties, as well as other intellectual       *
// * property laws and treaties. These contents may not be extracted,      *
// * copied, modified or redistributed either as a whole or part thereof   *
// * in any form, and may not be used directly in, or to assist with, the  *
// * creation of derivative works of any nature without prior written      *
// * permission from Pentia. The above copyright notice does not           *
// * evidence any actual or intended publication of this file.             *
// *                                                                       *
// *************************************************************************

#endregion

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