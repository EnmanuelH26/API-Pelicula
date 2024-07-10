using ApiPelicula.Data;
using ApiPelicula.Model;
using ApiPelicula.Repositorio.IRepositorio;

namespace ApiPelicula.Repositorio
{
    public class PeliculaRepositorio : IPeliculaRepositorio
    {
        private readonly AplicationDbContext _db; //instancia del contexto


        //construcor de la clase
        public PeliculaRepositorio(AplicationDbContext db)
        {
            _db = db; //para acceder a cualquiera de las entidades
        }
        public bool ActualizarCategoria(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now; //para darle la fecha actual
            
            _db.Pelicula.Update(pelicula); //actualizamos la bd con el metodo update y le pasamos la categoria
            return GuardarPelicula(); //metodo guardar
        }

        public bool BorrarPelicula(Pelicula pelicula)
        {
            _db.Pelicula.Remove(pelicula); //Removemos el con el metodo remove y le pasamos la categoria
            return GuardarPelicula();
        }

        public bool CrearPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now; //para darle la fecha actual
            _db.Pelicula.Add(pelicula); //Agregamos la bd con el metodo Add y le pasamos la categoria
            return GuardarPelicula(); //metodo guardar
        }

        public bool ExistePelicula(int peliculaId)
        {
            return _db.Pelicula.Any(c => c.Id == peliculaId); // valiamos si existe el id en la db y si existe(con el metodo Any)
                                                                // devuelve un true
        }

        public bool ExistePelicula(string nombre)
        {
            return _db.Pelicula.Any(c => c.NombrePelicula.ToLower().Trim() == nombre.ToLower().Trim()); //valiamos si el nombre existe
                                                                                                          //en la bd conel metodo Any y con
                                                                                                          //el metodo Trim eliminamos
                                                                                                          //los espacios en blanco
        }

        public Pelicula GetPelicula(int peliculaId)
        {
            return _db.Pelicula.FirstOrDefault(c => c.Id == peliculaId); //aqui busca la primera categoria que coincida. el metodo FirstOrDefault hace que busques un id en espesifico
        }

        public ICollection<Pelicula> GetCategorias()
        {
            return _db.Pelicula.OrderBy(c => c.NombrePelicula).ToList(); //aqui lo ordenamos por nombre de categoria y lo mandamos como una lista ya que el ICollection es para listas
        }

        public bool GuardarPelicula()
        {
            return _db.SaveChanges() > 0 ? true : false; // va a guradar los cambios siempre y cuando
                                                         // reciba algo mayor a 0 es decir si se guardan
                                                         // los cambios en el metodo AcualizarCategoria, recibira un 1 de lo contrario dara 0
        }

        public ICollection<Pelicula> GetPeliculas()
        {
            throw new NotImplementedException();
        }

        public ICollection<Pelicula> GetPeliculasEnCategoria(int catId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Pelicula> BuscarPelicula(string buscarPelicula)
        {
            throw new NotImplementedException();
        }

        public bool ActualizarPelicula(Pelicula pelicula)
        {
            throw new NotImplementedException();
        }
    }
}
