using System;
using Blog.Core.Entities.Interfaces.BasicEntity;

namespace Blog.Core.Entities
{
    public class Entity:IEntity
    {
        public Guid Id { get ; set; }

        public Entity()
        {
            if (Id == Guid.Empty)
                Id = Guid.NewGuid();
        }
    }
}