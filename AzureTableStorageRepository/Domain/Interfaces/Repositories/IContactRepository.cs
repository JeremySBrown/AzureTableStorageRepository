using System;
using System.Collections;
using System.Collections.Generic;
using Domain.Interfaces.Models;

namespace Domain.Interfaces.Repositories
{
    public interface IContactRepository
    {
        IContact Find(Guid id);
        IList<IContact> FindAll();
        void Create(IContact contact);
        void Update(IContact contact);
        bool Delete(Guid id);

    }
}