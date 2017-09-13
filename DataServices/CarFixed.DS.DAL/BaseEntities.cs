using System.Data.Entity;

namespace CarFixed.DS.DAL
{
    public class BaseEntities : DbContext
    {
        public BaseEntities() : base() { }

        public BaseEntities(string nameOrConnectionString) : base(nameOrConnectionString) { }
    }
}
