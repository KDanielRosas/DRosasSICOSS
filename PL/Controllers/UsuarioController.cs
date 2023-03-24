using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }//Login(get)

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            ML.Result result = BL.Usuario.GetByUserName(username, password);
            ML.Usuario usuario = (ML.Usuario)result.Object;
            int idUsuario = usuario.IdUsuario;

            if (result.Correct)
            {
                if (usuario.Password == password)
                {
                    HttpContext.Session.SetInt32("IdUsuario", idUsuario);
                    return RedirectToAction("Index", "Calcular");
                }//if
                else
                {
                    ViewBag.Message = "Contraseña incorrecta";
                    return PartialView("Modal1");
                }//if
            }//if
            else
            {
                ViewBag.Message = "El usuario no existe";
                return PartialView("Modal1");
            }//else           
        }//Login(post)

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }//SignUp(get)

        [HttpPost]
        public ActionResult SignUp(string userName, string password) 
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                ViewBag.Message = "No se permiten ingresar campos vacios.";
                return PartialView("SignUpFail");
            }
            else
            {
                ML.Result result = BL.Usuario.Add(userName, password);
                if (result.Correct)
                {
                    ViewBag.Message = "Registro exitoso";
                    return PartialView("SignUpSuccess");
                }
                else
                {
                    ViewBag.Message = "El userName ya existe, ingrese uno diferente.";
                    return PartialView("SignUpFail");
                }
            }            
        }//SignUp(post)

        public ActionResult LogOut()
        {
            HttpContext.Session.Remove("IdUsuario");
            HttpContext.Session.Remove("Numero");
            HttpContext.Session.Remove("Resultado");
            HttpContext.Session.Remove("Resultado");

            return View("Login");
        }//LogOut

        [HttpPost]
        public JsonResult CheckUserName(string userName)
        {
            ML.Result result = BL.Usuario.CheckUserName(userName);
            
            return Json(result);
        }
    }//UsuarioController
}//NS

