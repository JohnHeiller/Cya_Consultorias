using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AppWeb.CyaConsultorias.Models;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppWeb.CyaConsultorias.Utilitaries;
using System;

namespace AppWeb.CyaConsultorias.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;

        public HomeController(ILogger<HomeController> logger, IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            ConfiguracionModel configuracionModel = new ConfiguracionModel
            {
                ServicesModel = new List<ServicesModel>()
            };
            ServicesModel servicesModel = new ServicesModel
            {
                Sections = new List<SectionModel>(),
                Details = new List<DetailModel>()
            };
            SectionModel sectionModel = new SectionModel();
            DetailModel detailModel = new DetailModel();
            servicesModel.Sections.Add(sectionModel);
            servicesModel.Details.Add(detailModel);
            configuracionModel.ServicesModel.Add(servicesModel);
            var test = JsonConvert.SerializeObject(configuracionModel);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/About")]
        public IActionResult About()
        {
            return View();
        }

        [Route("/Services/{section}")]
        public IActionResult Services(string section)
        {
            ViewData["serviceSectionView"] = section;
            return View();
        }

        private Blog GetBlogList()
        {
            Blog infoBlog = new Blog();
            using (StreamReader jsonStream = System.IO.File.OpenText(Constantes.BlogsJsonPath))
            {
                var json = jsonStream.ReadToEnd();
                infoBlog = JsonConvert.DeserializeObject<Blog>(json);
            }
            infoBlog.ValidarFecha();

            #region Obtencion de datos para filtros de Blogs
            var categorias = infoBlog.Blogs.GroupBy(x => x.Categoria).Select(y => new { Categoria = y.Key, Cantidad = y.Count() }).OrderBy(x => x.Categoria).ToList();
            infoBlog.Categorias = new List<(string Categoria, int Cantidad)>();
            foreach (var item in categorias)
            {
                infoBlog.Categorias.Add((item.Categoria, item.Cantidad));
            }
            var autores = infoBlog.Blogs.GroupBy(x => x.Autor).Select(y => new { Autor = y.Key, Cantidad = y.Count() }).OrderBy(x => x.Autor).ToList();
            infoBlog.Autores = new List<(string Autor, int Cantidad)>();
            foreach (var item in autores)
            {
                infoBlog.Autores.Add((item.Autor, item.Cantidad));
            }
            #endregion Obtencion de datos para filtros de Blogs

            return infoBlog;
        }

        [Route("/Blog/{pagina}/{autor}/{categoria}")]
        public IActionResult Blog(int pagina, string autor = null, string categoria = null)
        {
            pagina = pagina < 1 ? 1 : pagina;
            Blog infoBlog = GetBlogList();

            #region Valida filtros por Autor o Categoria
            if (!string.IsNullOrWhiteSpace(autor) && infoBlog.Autores.Exists(x => x.Autor.Contains(autor)))
            {
                infoBlog.FiltroAutor = autor;
                infoBlog.Blogs = infoBlog.Blogs.Where(x => x.Autor.ToUpper().Contains(autor.ToUpper())).ToList();
            }
            else
            {
                infoBlog.FiltroAutor = null;
            }
            if (!string.IsNullOrWhiteSpace(categoria) && infoBlog.Categorias.Exists(x => x.Categoria.Contains(categoria)))
            {
                infoBlog.FiltroCategoria = categoria;
                infoBlog.Blogs = infoBlog.Blogs.Where(x => x.Categoria.ToUpper().Contains(categoria.ToUpper())).ToList();
            }
            else
            {
                infoBlog.FiltroCategoria = null;
            }
            #endregion Valida filtros por Autor o Categoria

            #region Valida paginacion de Blogs
            if (infoBlog.Blogs.Count() > 3)
            {
                infoBlog.CantidadPaginas = (((double)infoBlog.Blogs.Count() / (double)3) > ((int)infoBlog.Blogs.Count() / (int)3)) ? (int)(infoBlog.Blogs.Count() / 3 + 1) : (int)(infoBlog.Blogs.Count() / 3);
                infoBlog.Blogs = infoBlog.Blogs.OrderByDescending(x => x.DateTime).Skip((pagina - 1) * 3).Take(3).ToList();
            }
            else
            {
                infoBlog.CantidadPaginas = 1;
                infoBlog.Blogs = infoBlog.Blogs.OrderByDescending(x => x.DateTime).ToList();
            }
            infoBlog.PaginaActual = pagina;
            #endregion Valida paginacion de Blogs
            
            return View(infoBlog);
        }

        private Categorias GetCategoriasList() 
        {
            Categorias categorias = new Categorias();
            using (StreamReader jsonStream = System.IO.File.OpenText(Constantes.CategoriasJsonPath))
            {
                var json = jsonStream.ReadToEnd();
                categorias = JsonConvert.DeserializeObject<Categorias>(json);
            }
            categorias.CategoriasList = categorias.CategoriasList.OrderBy(x => x.Nombre).ToList();

            return categorias;
        }

        private bool UpdateBlogList(Blog infoBlog)
        {
            string blogJson = JsonConvert.SerializeObject(infoBlog);
            using (var jsonStream = new StreamWriter(Constantes.BlogsJsonPath, false))
            {
                jsonStream.WriteLine(blogJson);
                jsonStream.Close();
            }

            return true;
        }

        [Route("/NewBlog")]
        public IActionResult NewBlog()
        {
            Categorias categorias = GetCategoriasList();
            var categoriasDropDown = categorias.CategoriasList
                                        .Select(x => new { x.Codigo, x.Nombre })
                                        .ToList();
            ViewData["categoriaSelect"] = new SelectList(categoriasDropDown,"Codigo", "Nombre");

            return View();
        }

        [Route("/NewBlog")]
        [HttpPost]
        public IActionResult NewBlog(BlogModel blog)
        {
            #region Valida modelo de nuevo Blog
            if (string.IsNullOrWhiteSpace(blog.Resumen))
            {
                var sentence = blog.Cuerpo.Split(".").FirstOrDefault();
                if(!string.IsNullOrWhiteSpace(sentence) && sentence.Trim().Count() < 200)
                {
                    blog.Resumen = sentence;
                }
                else if(!string.IsNullOrWhiteSpace(sentence))
                {
                    blog.Resumen = sentence.Substring(0, 180).Trim() + "...";
                }
                else
                {
                    blog.Resumen = blog.Cuerpo.Substring(0, 180).Trim() + "...";
                }
            }
            blog.FechaRegistro = blog.DateTime.Value.ToShortDateString();
            blog.DiaFecha = blog.DateTime.Value.ToString("dd");
            blog.MesFecha = blog.DateTime.Value.ToString("MMM");

            Categorias categorias = GetCategoriasList();
            var categoriaSelected = categorias.CategoriasList.Where(x => x.Codigo.Contains(blog.Categoria)).FirstOrDefault();
            if (categoriaSelected == null || string.IsNullOrWhiteSpace(categoriaSelected.Codigo))
            {
                categoriaSelected = categorias.CategoriasList.First();
            }
            blog.ImagenPath = categoriaSelected.ImagenPath;
            blog.Categoria = categoriaSelected.Nombre;
            #endregion Valida modelo de nuevo Blog

            #region Actualiza lista de Blog
            var blogs = GetBlogList();
            blogs.Blogs.Add(blog);
            UpdateBlogList(blogs);
            #endregion Actualiza lista de Blog

            TempData["NewBlogSaved"] = "¡Se ha registrado el nuevo item de Blog de " + blog.Autor + "!";
            
            return Redirect("/NewBlog");
        }

        [Route("/Customer")]
        public IActionResult Customer()
        {
            ViewData["identificationTypeSelect"] = new SelectList(Constantes.IdentificationTypes, "Value", "Text");
            ViewData["certificationTypeSelect"] = new SelectList(Constantes.CertificationTypes, "Value", "Text");
            return View();
        }

        [Route("/NewCertification")]
        [HttpPost]
        public async Task<IActionResult> NewContactMessage(CustomerModel data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.CertificationType) || string.IsNullOrWhiteSpace(data.IdentificationNumber))
            {
                return Json(false);
            }
            string certificationText = Constantes.CertificationTypes.Find(x => x.Value == data.CertificationType).Text;
            string subject = string.Concat(
                data.CertificationType, " - ",
                certificationText, " ", 
                data.IdentificationNumber);
            string email = !string.IsNullOrWhiteSpace(data.Email) ? data.Email : "No registra";
            string message = string.Format(
                Constantes.BodyEmailCertification, 
                DateTime.Today.ToShortDateString(), 
                data.IdentificationType, 
                data.IdentificationNumber, 
                data.Phone, 
                email, 
                certificationText, 
                data.CertificationType);
            await _emailSender
                    .SendEmailAsync(true, subject, message)
                    .ConfigureAwait(false);

            return Json(true);
        }

        [Route("/Contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [Route("/NewContactMessage")]
        [HttpPost]
        public async Task<IActionResult> NewContactMessage(ContactModel data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.Subject) || string.IsNullOrWhiteSpace(data.Message))
            {
                return Json(false);
            }
            string subject = string.Concat(
                "PQR", " - ",
                data.Subject);
            string email = !string.IsNullOrWhiteSpace(data.Email) ? data.Email : "No registra";
            string message = string.Format(
                Constantes.BodyEmailContact,
                DateTime.Today.ToShortDateString(),
                data.FirstName,
                data.LastName,
                data.Phone,
                email,
                data.Message);
            await _emailSender
                    .SendEmailAsync(false, subject, message)
                    .ConfigureAwait(false);

            return Json(true);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
