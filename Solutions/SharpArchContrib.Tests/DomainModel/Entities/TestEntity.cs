namespace Tests.DomainModel.Entities
{
    using SharpArch.Core.DomainModel;

    public class TestEntity : Entity
    {
        [DomainSignature]
        public virtual string Name { get; set; }
    }
}