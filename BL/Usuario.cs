using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Usuario
    {
        public static ML.Result Add(string userName, string password)
        {
            ML.Result result = new();

            try
            {
                using (DL.DrosasSicossContext context = new())
                {     
                    ML.Result check = CheckUserName(userName);

                    if (check.Correct)
                    {
                        int query = context.Database.ExecuteSqlRaw($"UsuarioAdd '{userName}', '{password}'");

                        //Validar si hay filas afectadas
                        if (query > 0)
                        {
                            result.Correct = true;
                        }//if
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "Error al registrar el Usuario.";
                        }//else                  
                    }
                    else
                    {
                        result.Correct = false;
                    }
                    
                }//using SQL Connection
            }//try

            catch (Exception ex)
            {
                result.Ex = ex;
                result.Correct = false;
                result.ErrorMessage = "Ocurrio un error al registrar el Usuario";
                throw;
            }//

            return result;
        }//Add

        public static ML.Result GetByUserName(string userName, string password)
        {
            ML.Result result = new();
            ML.Usuario usuario = new();
            try
            {
                using (DL.DrosasSicossContext context = new())
                {
                    var query = context.Usuarios.FromSqlRaw
                        ($"UsuarioGetByUserName '{userName}', '{password}'").AsEnumerable().FirstOrDefault();

                    if (query != null)
                    {
                        result.Correct = true;
                        result.Object = new object();
                        usuario.IdUsuario = query.IdUsuario;
                        usuario.UserName = query.UserName;
                        usuario.Password = query.Password;

                        result.Object = usuario;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
                throw;
            }
            return result;
        }//UsuarioGetByUserName

        public static ML.Result CheckUserName(string userName)
        {
            ML.Result result = new();
            try
            {
                using (DL.DrosasSicossContext context = new())
                {
                    var query = context.Usuarios.FromSqlRaw($"UsuarioCheck '{userName}'").AsEnumerable().FirstOrDefault();

                    if (query == null)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                    //if (query.UserName == userName)
                    //{
                    //    ML.Usuario usuario = new ML.Usuario();
                    //    usuario.IdUsuario = query.IdUsuario;
                    //    usuario.UserName = query.UserName;
                    //    usuario.Password = query.Password;
                    //    result.Object = usuario;
                    //    result.Correct = false;
                    //}
                    //else
                    //{
                    //    result.Correct = true;
                    //}
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                throw;
            }
            return result;
        }
    }//Class
}//NS