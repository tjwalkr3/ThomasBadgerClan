namespace BadgerClan.Shared;
using System.Runtime.Serialization;
using System.ServiceModel;

[DataContract]
public class MoveRequest
{
    [DataMember(Order = 1)]
    public int PlayStyle { get; set; }
}

[DataContract]
public class MoveResponse
{
    [DataMember(Order = 1)]
    public bool Success { get; set; }
}

[ServiceContract]
public interface IGrpcMoveService
{
    [OperationContract]
    public Task<MoveResponse> ChangeStrategy(MoveRequest request);
}
