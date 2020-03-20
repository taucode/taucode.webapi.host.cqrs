using System.Linq;
using NHibernate;
using TauCode.WebApi.Host.Cqrs.Tests.Core.Domain;

namespace TauCode.WebApi.Host.Cqrs.Tests.AppHost.Persistence.Repositories
{
    public class NHibernateRecordRepository : IRecordRepository
    {
        private readonly ISession _session;

        public NHibernateRecordRepository(ISession session)
        {
            _session = session;
        }

        public Record Get(string id)
        {
            return _session
                .Query<Record>()
                .SingleOrDefault(x => x.Id == id);
        }

        public void Save(Record record)
        {
            _session.SaveOrUpdate(record);
        }

        public bool Delete(string id)
        {
            var record = _session.Query<Record>().SingleOrDefault(x => x.Id == id);
            if (record != null)
            {
                _session.Delete(record);
            }

            return record != null;
        }
    }
}
