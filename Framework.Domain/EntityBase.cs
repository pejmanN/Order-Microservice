using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain
{
    public abstract class EntityBase<TKey>
    {
        public TKey Id { get; protected set; }

        protected EntityBase() { }
        protected EntityBase(TKey id)
        {
            Id = id;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != this.GetType()) return false;

            var entity = (EntityBase<TKey>)obj;
            return this.Id.Equals(entity.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
