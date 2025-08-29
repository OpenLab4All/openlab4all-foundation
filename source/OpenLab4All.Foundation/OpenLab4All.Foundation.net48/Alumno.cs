using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLab4All.Foundation.net48
{
  internal class Alumno
  {
    public int IdAlumno { get; set; }
    public string Grado { get; set; }
    public string ApellidoPaterno { get; set; }
    public string ApellidoMaterno { get; set; }
    public string Nombres { get; set; }

    public Alumno() { }

    public Alumno(DataRow dr)
    {
      IdAlumno = dr.Field<int>("id_alumno");
      Grado = dr.Field<string>("des_grado");
      ApellidoPaterno = dr.Field<string>("ape_paterno");
      ApellidoMaterno = dr.Field<string>("ape_materno");
      Nombres = dr.Field<string>("nombre");
    }
  }
}
