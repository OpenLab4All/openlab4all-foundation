using System;
using System.Collections.Generic;

namespace OpenLab4All.Foundation.Data.Abstractions
{
  public interface IConnectionString
  {
    string ProviderName { get; }
    string Build();
    string BuildRedacted();
    IReadOnlyDictionary<string, object> GetKeyValues();
  }
}
