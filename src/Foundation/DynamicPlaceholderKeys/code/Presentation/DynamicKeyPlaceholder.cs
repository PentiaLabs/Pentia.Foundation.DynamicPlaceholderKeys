#region Dependencies

using System.Web.UI;
using Pentia.Foundation.DynamicPlaceholderKeys.Model;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Layouts;
using Sitecore.Web.UI;
using Sitecore.Web.UI.WebControls;

#endregion

[assembly: TagPrefix("Pentia.Foundation.DynamicPlaceholderKeys.Presentation", "PT")]

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
