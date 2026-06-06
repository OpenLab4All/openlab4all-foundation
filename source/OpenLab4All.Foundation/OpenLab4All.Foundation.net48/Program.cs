using OpenLab4All.Foundation.Data.Mapping;
using OpenLab4All.Foundation.Data.SqlServer;
using System;

namespace OpenLab4All.Foundation.net48
{
  internal class Program
  {
    static void Main(string[] args)
    {
      AppContext.SetSwitch("Microsoft.Data.SqlClient.UseManagedNetworkingOnWindows", true);


      var cnx = SqlServerConnectionString.Parse("Data Source=mssql.srv.net.pe;Initial Catalog=test;User Id=sa;Password=EswchqK3PfwbjzjMCgJj;");
      cnx.TrustServerCertificate = true;
      var a = cnx.Build();


      var cnn = new SqlDataAccess(cnx);

      var res = cnn.ExecuteProcedure("[dbo].[sp_alumno_listar]");

      if (res.Success)
      {
        Console.WriteLine(res.Message);
        Console.WriteLine(res.Data);
      }
      else
      {
        Console.WriteLine(res.Message);
      }

      var res2 = cnn.ExecuteProcedure(
          "[dbo].[sp_alumno_listar]"
          , ds => DataSetMapper.MapList<Alumno>(ds)
        );

      if (res2.Success)
      {
        Console.WriteLine(res.Message);
        Console.WriteLine(res.Data);
      }
      else
      {
        Console.WriteLine(res.Message);
      }

      var res3 = cnn.ExecuteProcedure(
          "[dbo].[sp_alumno_listar]"
          , ds => DataSetMapper.MapSingle<Alumno>(ds)
        );

      if (res2.Success)
      {
        Console.WriteLine(res.Message);
        Console.WriteLine(res.Data);
      }
      else
      {
        Console.WriteLine(res.Message);
      }

      Console.ReadLine();
    }
  }
}
