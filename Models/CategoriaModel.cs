using System;
using System.Collections.Generic;

namespace AppWeb.CyaConsultorias.Models
{
    public class CategoriaModel
    {
        public string Codigo { get; set; }
        public string ImagenPath { get; set; }
        public string Nombre { get; set; }
    }

    public class Categorias
    {
        public List<CategoriaModel> CategoriasList { get; set; }
    }

}
