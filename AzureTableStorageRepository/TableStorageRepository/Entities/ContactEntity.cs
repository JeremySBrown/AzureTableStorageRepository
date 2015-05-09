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
            throw new NotImplementedException();
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
    }
}