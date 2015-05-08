using System;
using Domain.Interfaces.Models;

namespace Domain.Models
{
    public class Contact : IContact
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

    }
}