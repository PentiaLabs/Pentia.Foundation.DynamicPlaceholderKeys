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

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetPlaceholderRenderings;

#endregion

namespace Pentia.Foundation.DynamicPlaceholderKeys.Pipelines.GetPlaceholderRenderings
{
  public class GetDynamicKeyAllowedRenderings : GetAllowedRenderings
  {
    public const string DynamicKeyRegex = @"(.+){[\d\w]{8}\-([\d\w]{4}\-){3}[\d\w]{12}}";

    public new void Process(GetPlaceholderRenderingsArgs args)
    {
      Assert.IsNotNull(args, "args");

      var placeholderKey = args.PlaceholderKey;
      var regex = new Regex(DynamicKeyRegex);
      var match = regex.Match(placeholderKey);

      if (!match.Success || match.Groups.Count <= 0)
      {
        return;
      }

      placeholderKey = match.Groups[1].Value;

      var placeholderItem = GetPlaceholderItem(args, placeholderKey);

      var renderings = GetRenderings(args, placeholderItem);

      if (renderings == null)
        return;

      if (args.PlaceholderRenderings == null)
      {
        args.PlaceholderRenderings = new List<Item>();
      }
      args.PlaceholderRenderings.AddRange(renderings);
    }

    private IEnumerable<Item> GetRenderings(GetPlaceholderRenderingsArgs args, Item placeholderItem)
    {
      if (placeholderItem == null) 
        return null;

      bool allowedControlsSpecified;
      args.HasPlaceholderSettings = true;
      var renderings = GetRenderings(placeholderItem, out allowedControlsSpecified);
      if (!allowedControlsSpecified) 
        return renderings;

      args.CustomData["allowedControlsSpecified"] = true;
      args.Options.ShowTree = false;
      return renderings;
    }

    private static Item GetPlaceholderItem(GetPlaceholderRenderingsArgs args, string placeholderKey)
    {
      if (ID.IsNullOrEmpty(args.DeviceId))
        return Client.Page.GetPlaceholderItem(placeholderKey, args.ContentDatabase, args.LayoutDefinition);

      using (new DeviceSwitcher(args.DeviceId, args.ContentDatabase))
      {
        return Client.Page.GetPlaceholderItem(placeholderKey, args.ContentDatabase, args.LayoutDefinition);
      }
    }
  }
}