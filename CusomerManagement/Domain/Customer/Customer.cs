using Framework.Domain;
using Shared.Contracts;

namespace CusomerManagement.Domain.Customer
{
    public class Customer : AggregateRootBase<long>
    {
        public Customer(long id, string name, bool disabled, decimal credit)
        {
            Id = id;
            Name = name;
            Disabled = disabled;
            Credit = credit;
        }

        public string Name { get; private set; }
        public bool Disabled { get; private set; }
        public decimal Credit { get; private set; }

        public void Enable()
        {
            if (Disabled)
            {
                Disabled = false;
                Publish(new CustomerEnabled(this.Id));
            }
        }

        public void Disable()
        {
            if (!Disabled)
            {
                Disabled = true;
                Publish(new CustomerDisabled(this.Id));
            }
        }
    }
}
