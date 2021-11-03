using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppWeb.CyaConsultorias.Utilitaries
{
    public class Constantes
    {
        public const string BlogsJsonPath = @"wwwroot\json\blogs.json";
        public const string CategoriasJsonPath = @"wwwroot\json\categorias.json";
        public const string BodyEmailCertification = "Se ha recibido una Solicitud de Certificación con fecha {0}. \n\nUsuario(a): {1} {2},\nTeléfono: {3},\nCorreo electrónico: {4},\n\nEsto para un(a) {5} ({6}).";
        public const string BodyEmailContact = "Se ha recibido un mensaje de contacto PQR con fecha {0}. \n\nNombre: {1} {2},\nTeléfono: {3},\nCorreo electrónico: {4},\n\nCuya descripción es: {5}.";

        public static List<SelectListItem> IdentificationTypes = new List<SelectListItem>()
        {
            new SelectListItem{ Value = "CC", Text = "Cédula de Ciudadanía"},
            new SelectListItem{ Value = "CE", Text = "Cédula de Extranjería"},
            new SelectListItem{ Value = "NIP", Text = "Número de Ientificación Personal"},
            new SelectListItem{ Value = "NIT", Text = "Número de Identificación Tributaria"},
            new SelectListItem{ Value = "PAP", Text = "Pasaporte"}
        };

        public static List<SelectListItem> CertificationTypes = new List<SelectListItem>()
        {
            new SelectListItem{ Value = "CCD", Text = "Certificado Consolidado de Deuda"},
            new SelectListItem{ Value = "CPT", Text = "Certificado Consolidado de Pagos a Terceros"},
            new SelectListItem{ Value = "CSA", Text = "Certificado de Socios y Accionistas"},
            new SelectListItem{ Value = "CEL", Text = "Certificado Laboral"},
            new SelectListItem{ Value = "CET", Text = "Certificado Tributario"}
        };
    }
}
