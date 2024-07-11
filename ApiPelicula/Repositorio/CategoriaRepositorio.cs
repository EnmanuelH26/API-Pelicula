using ApiPelicula.Data;
using ApiPelicula.Model;
using ApiPelicula.Repositorio.IRepositorio;

namespace ApiPelicula.Repositorio
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly AplicationDbContext _db; //instancia del contexto


        //construcor de la clase
        public CategoriaRepositorio(AplicationDbContext db)
        {
            _db = db; //para acceder a cualquiera de las entidades
        }
        public bool ActualizarCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now; //para darle la fecha actual

            //
            var categoriaExistente = _db.Categoria.Find(categoria.Id); //esto es para encontrar si la categoria existe en la db


            if (categoriaExistente != null)
            {
                _db.Entry(categoriaExistente).CurrentValues.SetValues(categoria);
            }
            else
            {
                _db.Categoria.Update(categoria); //actualizamos la bd con el metodo update y le pasamos la categoria

            }

            return GuardarCategoria(); //metodo guardar
        }

        public bool BorrarCategoria(Categoria categoria)
        {
            _db.Categoria.Remove(categoria); //Removemos el con el metodo remove y le pasamos la categoria
            return GuardarCategoria();
        }

        public bool CrearCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now; //para darle la fecha actual
            _db.Categoria.Add(categoria); //Agregamos la bd con el metodo Add y le pasamos la categoria
            return GuardarCategoria(); //metodo guardar
        }

        public bool ExisteCategoria(int categoriaId)
        {
            return _db.Categoria.Any(c => c.Id == categoriaId); // valiamos si existe el id en la db y si existe(con el metodo Any)
                                                                // devuelve un true
        }

        public bool ExisteCategoria(string nombre)
        {
            return _db.Categoria.Any(c => c.NombreCategoria.ToLower().Trim() == nombre.ToLower().Trim()); //valiamos si el nombre existe
                                                                                                          //en la bd conel metodo Any y con
                                                                                                          //el metodo Trim eliminamos
                                                                                                          //los espacios en blanco
        }

        public Categoria GetCategoria(int categoriaId)
        {
            return _db.Categoria.FirstOrDefault(c => c.Id == categoriaId); //aqui busca la primera categoria que coincida. el metodo FirstOrDefault hace que busques un id en espesifico
        }

        public ICollection<Categoria> GetCategorias()
        {
            return _db.Categoria.OrderBy(c => c.Id).ToList(); //aqui lo ordenamos por nombre de categoria y lo mandamos como una lista ya que el ICollection es para listas
        }

        public bool GuardarCategoria()
        {
            return _db.SaveChanges() > 0 ? true : false; // va a guradar los cambios siempre y cuando
                                                         // reciba algo mayor a 0 es decir si se guardan
                                                         // los cambios en el metodo AcualizarCategoria, recibira un 1 de lo contrario dara 0
        }
    }
}
