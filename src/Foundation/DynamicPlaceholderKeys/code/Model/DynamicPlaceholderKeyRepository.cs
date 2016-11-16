using Sitecore.Data;

namespace Pentia.Foundation.DynamicPlaceholderKeys.Model
{
  public class DynamicPlaceholderKeyRepository
  {
    public static string Get(string basePlaceholderKey, ID renderingUniqueId)
    {
      return basePlaceholderKey + renderingUniqueId;
    }
  }
}

