using System.Data.Entity;

namespace CarFixed.DS.DAL
{
    public class CarFixedBaseDR<T> : GenericDataRepository<T> where T : class
    {
        internal override DbContext CreateContext()
        {
            CarFixedEntities entities = new CarFixedEntities();
            entities.Configuration.ProxyCreationEnabled = false;

            return entities;
        }
    }
}
