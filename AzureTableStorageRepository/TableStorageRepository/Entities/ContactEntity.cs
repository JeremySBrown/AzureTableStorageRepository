using System;
using Domain.Interfaces.Models;
using Domain.Models;

namespace TableStorageRepository.Entities
{
    public class ContactEntity : BaseEntity<Contact, IContact>, IContact
    {
        public ContactEntity():base(null)
        {
            
        }

        public ContactEntity(IContact model) : base(model)
        {
        }

        public ContactEntity(IContact model, string partitionKey) : base(model, partitionKey)
        {
        }

        protected override void SetRowKey()
        {
            RowKey = string.Format("Contact_{0}", Id);
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }
    }
}