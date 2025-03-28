namespace Finaktiva.Domain.Common
{
    public class Entity
    {
        public int Id { get; set; }
        public Guid? CreateById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool IsActive { get; set; } = true;

        public void SetCreate(Guid createById)
        {
            CreateById = createById;
            CreatedDate = DateTime.UtcNow;
        }
        public void SetUpdate(Guid updateById = new Guid())
        {
            LastModifiedBy = updateById;
            LastModifiedDate = DateTime.UtcNow;
        }

        public Entity(bool isActive)
        {
            IsActive = isActive;
        }

        public Entity()
        {
        }
    }
}
