namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ILabService
    {
        public Task<ServiceDefault<LabInsert>> CreateLabAsync (LabInsert labInsert);
    }

}

