using TauCode.Cqrs.Commands;
using TauCode.WebApi.Host.Cqrs.Tests.Core.Domain;

namespace TauCode.WebApi.Host.Cqrs.Tests.Core.Features.CreateRecord
{
    public class CreateRecordCommand : Command<RecordId>
    {
    }
}
