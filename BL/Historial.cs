using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Historial
    {
        public static ML.Result Add(ML.Historial historial)
        {          
            ML.Result result = new();
            try
            {
                using (DL.DrosasSicossContext context = new())
                {
                    int query = context.Database.ExecuteSqlRaw($"HistorialAdd '{historial.Numero}', {historial.Resultado}, " +
                        $"{historial.Usuario.IdUsuario}");

                    //Validar si hay filas afectadas
                    if (query > 0)
                    {
                        result.Correct = true;
                    }//if
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error al registrar el Historial.";
                    }//else                  
                }//using SQL Connection
            }//try

            catch (Exception ex)
            {
                result.Ex = ex;
                result.Correct = false;
                result.ErrorMessage = "Ocurrio un error al registrar el Historial";
                throw;
            }//catch
            return result;
        }//Add
                   
        public static ML.Result GetByIdUsuario(int idUsuario)
        {            
            ML.Result result = new ML.Result();
            
            try
            {
                using (DL.DrosasSicossContext context = new DL.DrosasSicossContext())
                {
                    var queryGetAll = context.Historials.FromSqlRaw($"HistorialGetByIdUsuario {idUsuario}");

                    ML.Historial historial = new ML.Historial();
                    if (queryGetAll != null)
                    {
                        result.Objects = new List<object>();

                        foreach (var item in queryGetAll)
                        {
                            historial = new ML.Historial();

                            historial.IdHistorial = item.IdHistorial;
                            historial.Numero = item.Numero;
                            historial.Resultado = item.Resultado.Value;
                            historial.FechaHora = item.FechaHora.Value;
                            historial.Usuario = new ML.Usuario();
                            historial.Usuario.IdUsuario = item.IdUsuario.Value;

                            result.Objects.Add(historial);
                        }//foreach
                        result.Correct = true;
                    }//if                                                   
                }//using context
            }//try
            catch (Exception ex)
            {
                result.Ex = ex;
                result.Correct = false;
                result.ErrorMessage = "Error al mostrar los registros!";
                throw;
            }
            return result;      
        }//GetByIdUsuario

        public static ML.Result DeleteByIdUsuario(int idUsuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.DrosasSicossContext context = new())
                {
                    var query = context.Database.ExecuteSqlRaw($"HistorialDeleteByIdUsuario {idUsuario}");

                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                    else 
                    { 
                        result.Correct = false; 
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = ex.Message;
                throw;
            }
            return result;
        }//DeleteByIdUsuario

        public static ML.Result DeleteById(int idHistorial)
        {
            ML.Result result = new();

            try
            {
                using (DL.DrosasSicossContext context = new())
                {
                    var query = context.Database.ExecuteSqlRaw($"HistorialDeleteById {idHistorial}");

                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = ex.Message;
                throw;
            }
            return result;
        }//DeleteById

        public static int Calcular(string nums)
        {                     
            ML.Result result = new();
            int superdigito = 0;
            
            int[] array = new int[nums.Length];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = int.Parse(nums.Substring(i, 1));       
            }
            while (array[0] != null || array[1] != null || array[1] != 0) 
            {
                superdigito = 0;
                foreach (int i in array)
                {
                    superdigito += i;
                }
                nums = superdigito.ToString();
                if (nums.Length == 1)
                {
                    break;
                }
                array = new int[nums.Length];
                array[0] = superdigito;
                if (superdigito < 10)
                {
                    break;
                }
                else
                {
                    //nums = superdigito.ToString();
                    for (int i = 0; i < nums.Length; i++)
                    {
                        array[i] = int.Parse(nums.Substring(i, 1));
                    }
                }

            }
            
            return superdigito;
        }
    }//Class
}//NS
