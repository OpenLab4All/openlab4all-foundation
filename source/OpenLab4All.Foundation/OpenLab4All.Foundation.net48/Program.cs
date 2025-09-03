using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenLab4All.Foundation.Data.SqlServer;
using OpenLab4All.Foundation.Data.Mapping;
using OpenLab4All.Foundation.Core;

namespace OpenLab4All.Foundation.net48
{
  internal class Program
  {
    static void Main(string[] args)
    {
      AppContext.SetSwitch("Microsoft.Data.SqlClient.UseManagedNetworkingOnWindows", true);


      var cnx = SqlServerConnectionString.Parse("xxx");
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
