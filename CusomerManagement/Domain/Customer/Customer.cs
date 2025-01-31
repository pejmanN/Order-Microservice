using Framework.Domain;
using Shared.Contracts;

namespace CusomerManagement.Domain.Customer
{
    public class Customer : AggregateRootBase<Guid>
    {
        public Customer(Guid id, string name, bool disabled, decimal credit)
        {
            Id = id;
            Name = name;
            Disabled = disabled;
            Credit = credit;
        }

        public Customer(string name, bool disabled, decimal credit)
        {
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

        public void Debit(decimal amount)
        {
            if (this.Credit < amount)
            {
                throw new Exception("The debit process cannot be completed");
            }

            Credit -= amount;
        }

        public void CreditAccount(decimal amount)
        {
            Credit += amount;
        }
    }
}
