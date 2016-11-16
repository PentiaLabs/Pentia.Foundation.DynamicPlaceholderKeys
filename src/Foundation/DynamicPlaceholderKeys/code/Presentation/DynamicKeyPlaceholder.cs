#region Copyright (c) Pentia, 1997 - 2015. All rights reserved (R).

// *************************************************************************
// *                                                                       *
// * http://www.pentia.net/                                                *
// *                                                                       *
// * E-Mail: info@pentia.net                                               *
// *                                                                       *
// * Copyright(C) Pentia, 1997 - 2015. All rights reserved (R).            *
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

using System.Web.UI;
using Pentia.Foundation.DynamicPlaceholderKeys.Model;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Layouts;
using Sitecore.Web.UI;
using Sitecore.Web.UI.WebControls;

#endregion

[assembly: TagPrefix("PT.Framework.DynamicPlaceholderKeys.Presentation", "PT")]

namespace Pentia.Foundation.DynamicPlaceholderKeys.Presentation
{
  [ToolboxData("<{0}:DynamicKeyPlaceholder runat=\"server\" Key=\"PlaceholderKey\" />")]
  public class DynamicKeyPlaceholder : WebControl, IExpandable
  {
    protected string Placeholderkey = Placeholder.DefaultPlaceholderKey;
    protected Placeholder Placeholder { get; set; }

    public string Key
    {
      get { return Placeholderkey; }
      set { Placeholderkey = value.ToLower(); }
    }

    #region IExpandable Members

    public void Expand()
    {
      EnsureChildControls();
    }

    #endregion

    protected override void CreateChildControls()
    {
      var dynamicKey = DynamicPlaceholderKeyRepository.Get(Placeholderkey, new ID(UniqueID));
      Tracer.Debug("DynamicKeyPlaceholder: Adding dynamic placeholder with Key " + dynamicKey);
      Placeholder = new Placeholder
      {
        Key = dynamicKey
      };
      Controls.Add(Placeholder);
      Placeholder.Expand();
    }

    protected override void DoRender(HtmlTextWriter output)
    {
      RenderChildren(output);
    }
  }
}