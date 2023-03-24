using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class CalcularController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            int idUsuario = (int)HttpContext.Session.GetInt32("IdUsuario");
            if (HttpContext.Session.GetString("Numero") != null && HttpContext.Session.GetInt32("Resultado") != null)
            {
                ViewBag.Numero = HttpContext.Session.GetString("Numero");
                ViewBag.Resultado = (int)HttpContext.Session.GetInt32("Resultado");
            }            
            
            ViewBag.IdUsuario = idUsuario;
            ML.Result result = BL.Historial.GetByIdUsuario(idUsuario);
            return View(result);
        }
        [HttpPost]
        public ActionResult SuperDigito(string numero)
        {            
            int idUsuario = (int)HttpContext.Session.GetInt32("IdUsuario");            
            HttpContext.Session.SetString("Numero", numero);
            int resultado = BL.Historial.Calcular(numero);
            HttpContext.Session.SetInt32("Resultado", resultado);
            ML.Historial historial = new();
            historial.Numero = numero;
            historial.Usuario = new ML.Usuario();
            historial.Usuario.IdUsuario = idUsuario;
            historial.Resultado = resultado;
            BL.Historial.Add(historial);
            ViewBag.IdUsuario = idUsuario;
            return RedirectToAction("Index");         
        }

        public ActionResult DeleteById(int idHistorial)
        {
            ML.Result result = BL.Historial.DeleteById(idHistorial);
            if (result.Correct)
            {
                ViewBag.Message = "Registro eliminado";
                return PartialView("Modal");
            }
            else
            {
                ViewBag.Message = "Error al eliminar";
                return PartialView("Modal");
            }          
        }

        public ActionResult DeleteByIdUsuario()
        {
            int idUsuario = (int)HttpContext.Session.GetInt32("IdUsuario");
            BL.Historial.DeleteByIdUsuario(idUsuario);

            return RedirectToAction("Index");
        }
    }
}
