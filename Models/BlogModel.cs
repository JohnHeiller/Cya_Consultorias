using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppWeb.CyaConsultorias.Models
{
    public class BlogModel
    {
        public string ImagenPath { get; set; }
        public string DiaFecha { get; set; }
        public string MesFecha { get; set; }
        [NotMapped]
        public DateTime? DateTime { get; set; }
        public string FechaRegistro { get; set; }
        public string Titulo { get; set; }
        public string Resumen { get; set; }
        public string Cuerpo { get; set; }
        public string Categoria { get; set; }
        public string Autor { get; set; }
    }

    public class Blog
    {
        public List<BlogModel> Blogs { get; set; }
        [NotMapped]
        public List<(string Categoria, int Cantidad)> Categorias { get; set; }
        [NotMapped]
        public List<(string Autor, int Cantidad)> Autores { get; set; }
        [NotMapped]
        public int CantidadPaginas { get; set; }
        [NotMapped]
        public int PaginaActual { get; set; }
        [NotMapped]
        public string FiltroAutor { get; set; }
        [NotMapped]
        public string FiltroCategoria { get; set; }
        public void ValidarFecha()
        {
            foreach (var blog in Blogs)
            {
                if (!string.IsNullOrWhiteSpace(blog.FechaRegistro))
                {
                    blog.DateTime = DateTime.Parse(blog.FechaRegistro);
                    blog.DiaFecha = blog.DateTime.Value.ToString("dd");
                    blog.MesFecha = blog.DateTime.Value.ToString("MMM");
                }
            }
        }
    }
}
