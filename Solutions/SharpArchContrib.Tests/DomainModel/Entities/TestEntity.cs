namespace Tests.DomainModel.Entities
{
    using SharpArch.Domain.DomainModel;

    public class TestEntity : Entity
    {
        [DomainSignature]
        public virtual string Name { get; set; }
    }
}