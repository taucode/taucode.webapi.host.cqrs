namespace TauCode.WebApi.Host.Cqrs.Tests.Core.Domain
{
    public interface IRecordRepository
    {
        Record Get(string id);
        void Save(Record record);
        bool Delete(string id);
    }
}
