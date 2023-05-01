using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace ArticuloNegocio
{
    public class Negocio
    {
        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;



            try
            {
                conexion.ConnectionString = "server =.\\SQLEXPRESS; database = CATALOGO_DB; integrated security = true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "select Codigo,Nombre, a.Descripcion,c.Descripcion Categoria,ImagenUrl,Precio,m.Descripcion marca,a.IdCategoria,a.IdMarca,a.Id from ARTICULOS a , CATEGORIAS c, MARCAS m where a.IdCategoria = c.Id and m.Id = a.IdMarca";
                comando.Connection = conexion;
                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)lector["Id"];
                    aux.codigoArticulo = (string)lector["Codigo"];
                    aux.nombre = (string)lector["nombre"];
                    aux.descripcion = (string)lector["Descripcion"];
                    aux.url_imagen = (string)lector["ImagenUrl"];
                    aux.categoria = new CATEGORIA();
                    aux.categoria.id = (int)lector["IdCategoria"];
                    aux.categoria.descripcion = (string)lector["Categoria"];
                    aux.marca = new MARCA();
                    aux.marca.id = (int)lector["IdMarca"];
                    aux.marca.descripcion = (string)lector["marca"];
                    aux.precio = lector.GetDecimal(5);

                    lista.Add(aux);
                }

                conexion.Close();


                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            

            

        }
        public void modificar(Articulo modi)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update ARTICULOS set Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, IdMarca = @idMarca,IdCategoria = @idcategoria, ImagenUrl = @urlimagen,Precio = @precio where Id = @id");
                datos.setearParametro("@codigo", modi.codigoArticulo);
                datos.setearParametro("@nombre",modi.nombre );
                datos.setearParametro("@descripcion", modi.descripcion);
                datos.setearParametro("@idMarca", modi.marca.id);
                datos.setearParametro("@idcategoria", modi.categoria.id);
                datos.setearParametro("@urlimagen", modi.url_imagen);
                datos.setearParametro("@precio", modi.precio);
                datos.setearParametro("@id", modi.Id);
                datos.ejecutarAccion();
                

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void agregar(Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("insert into ARTICULOS (Codigo,Nombre, Descripcion,ImagenUrl,Precio,IdMarca,IdCategoria)values(@codigo,@nombre,@descripcion,@urlimagen,@precio,@idMarca,@idCategoria)");
                datos.setearParametro("@codigo",nuevo.codigoArticulo);
                datos.setearParametro("@nombre",nuevo.nombre);
                datos.setearParametro("@descripcion", nuevo.descripcion);
                datos.setearParametro("@urlimagen", nuevo.url_imagen);
                datos.setearParametro("@precio", nuevo.precio);
                datos.setearParametro("@idMarca", nuevo.marca.id);
                datos.setearParametro("@idCategoria", nuevo.categoria.id);
                datos.ejecutarAccion();
                

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

        }
        public void eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("delete from ARTICULOS where Id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public List<Articulo>  filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "select Codigo,Nombre, a.Descripcion,c.Descripcion Categoria,ImagenUrl,Precio,m.Descripcion marca,a.IdCategoria,a.IdMarca,a.Id from ARTICULOS a , CATEGORIAS c, MARCAS m where a.IdCategoria = c.Id and m.Id = a.IdMarca and ";
                if (campo == "Nombre")

                {

                    switch (criterio)
                    {
                        case "Empieza con":
                            consulta += "Nombre like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "Nombre like '%" + filtro + "'";
                            break;
                        case "Contiene":
                            consulta += "Nombre like '%" + filtro + "%' ";

                            break;
                        default:
                            break;
                    }
                }
                else if (campo == "Codigo")
                {
                    
                        switch (criterio)
                    {
                        case "Empieza con":
                            consulta += "a.Codigo like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "a.Codigo like '%" + filtro + "'";
                            break;
                        case "Contiene":
                            consulta += "a.Codigo like '%" + filtro + "%' ";

                            break;
                        default:
                            break;
                    }
                }

                    
                  
                       
                datos.setearConsulta(consulta);
                 datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.codigoArticulo = (string)datos.Lector["Codigo"];
                    aux.nombre = (string)datos.Lector["nombre"];
                    aux.descripcion = (string)datos.Lector["Descripcion"];
                    aux.url_imagen = (string)datos.Lector["ImagenUrl"];
                    aux.categoria = new CATEGORIA();
                    aux.categoria.id = (int)datos.Lector["IdCategoria"];
                    aux.categoria.descripcion = (string)datos.Lector["Categoria"];
                    aux.marca = new MARCA();
                    aux.marca.id = (int)datos.Lector["IdMarca"];
                    aux.marca.descripcion = (string)datos.Lector["marca"];
                    aux.precio = datos.Lector.GetDecimal(5);

                    lista.Add(aux);
                }




                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
