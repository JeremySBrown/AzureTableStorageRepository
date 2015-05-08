using System;

namespace Domain.Interfaces.Models
{
    public interface IContact
    {
        Guid Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string EmailAddress { get; set; }
    }
}