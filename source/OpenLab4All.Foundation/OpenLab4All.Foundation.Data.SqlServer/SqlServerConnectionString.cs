using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using OpenLab4All.Foundation.Data.Abstractions;

namespace OpenLab4All.Foundation.Data.SqlServer
{
  public sealed class SqlServerConnectionString : IConnectionString
  {
    private readonly SqlConnectionStringBuilder _builder;

    public SqlServerConnectionString()
    {
      _builder = new SqlConnectionStringBuilder
      {
        DataSource = "(local)",
        InitialCatalog = "master",
        IntegratedSecurity = false,
        Pooling = true,
        MinPoolSize = 1,
        MaxPoolSize = 5,
        ApplicationName = "OpenLab4All",
        Encrypt = true,
        TrustServerCertificate = false
      };
    }

    public static SqlServerConnectionString Parse(string raw)
    {
      if (string.IsNullOrWhiteSpace(raw))
        throw new ArgumentException("The connection string cannot be null or empty.", nameof(raw));

      var b = new SqlConnectionStringBuilder(raw);
      return new SqlServerConnectionString(b);
    }

    private SqlServerConnectionString(SqlConnectionStringBuilder builder)
    {
      _builder = new SqlConnectionStringBuilder(builder.ConnectionString);
    }

    public string Server { get => _builder.DataSource; set => _builder.DataSource = value ?? "(local)"; }
    public string Database { get => _builder.InitialCatalog; set => _builder.InitialCatalog = value ?? "master"; }
    public bool IntegratedSecurity { get => _builder.IntegratedSecurity; set => _builder.IntegratedSecurity = value; }
    public string UserId { get => _builder.UserID; set => _builder.UserID = value ?? string.Empty; }
    public string Password { get => _builder.Password; set => _builder.Password = value ?? string.Empty; }
    public bool Pooling { get => _builder.Pooling; set => _builder.Pooling = value; }
    public int MinPoolSize { get => _builder.MinPoolSize; set => _builder.MinPoolSize = Math.Max(0, value); }
    public int MaxPoolSize { get => _builder.MaxPoolSize; set => _builder.MaxPoolSize = Math.Max(0, value); }
    public int TimeoutSeconds { get => _builder.ConnectTimeout; set => _builder.ConnectTimeout = Math.Max(0, value); }
    public string ApplicationName { get => _builder.ApplicationName; set => _builder.ApplicationName = string.IsNullOrWhiteSpace(value) ? "OpenLab4All" : value; }

    public bool Encrypt { get => _builder.Encrypt; set => _builder.Encrypt = value; }
    public bool TrustServerCertificate { get => _builder.TrustServerCertificate; set => _builder.TrustServerCertificate = value; }
    public string HostNameInCertificate { get => _builder.ContainsKey("HostNameInCertificate") ? (string)_builder["HostNameInCertificate"] : string.Empty; set => _builder["HostNameInCertificate"] = value ?? string.Empty; }
    public string Authentication { get => _builder.ContainsKey("Authentication") ? (string)_builder["Authentication"] : string.Empty; set => _builder["Authentication"] = value ?? string.Empty; }
    public string ApplicationIntent { get => _builder.ApplicationIntent.ToString(); set => _builder.ApplicationIntent = (ApplicationIntent)Enum.Parse(typeof(ApplicationIntent), value, true); }

    public string ProviderName => "Microsoft.Data.SqlClient";
    public string Build() => _builder.ConnectionString;

    public string BuildRedacted()
    {
      var clone = new SqlConnectionStringBuilder(_builder.ConnectionString) { Password = "********" };
      return clone.ConnectionString;
    }

    public IReadOnlyDictionary<string, object> GetKeyValues()
    {
      var dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
      foreach (string key in _builder.Keys)
        dict[key] = _builder[key];
      return dict;
    }

    public SqlServerConnectionString WithServer(string server) { Server = server; return this; }
    public SqlServerConnectionString WithDatabase(string database) { Database = database; return this; }
    public SqlServerConnectionString WithCredentials(string user, string pwd) { IntegratedSecurity = false; UserId = user; Password = pwd; return this; }
    public SqlServerConnectionString WithIntegratedSecurity() { IntegratedSecurity = true; UserId = ""; Password = ""; return this; }
  }
}
