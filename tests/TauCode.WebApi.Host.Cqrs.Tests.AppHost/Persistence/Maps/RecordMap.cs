using FluentNHibernate.Mapping;
using TauCode.WebApi.Host.Cqrs.Tests.Core.Domain;

namespace TauCode.WebApi.Host.Cqrs.Tests.AppHost.Persistence.Maps
{
    public class RecordMap : ClassMap<Record>
    {
        public RecordMap()
        {
            this.Id(x => x.Id);
            this.Map(x => x.Code);
            this.Map(x => x.Name);
        }
    }
}
