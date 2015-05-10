using System;
using System.Collections.Generic;
using Domain.Interfaces.Models;
using Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TableStorageRepository.Entities;
using TableStorageRepository.Repositories;

namespace TableStorageRepository.Tests
{
    [TestClass]
    public class ContactEntityTests
    {
        [TestMethod]
        public void Contructor_WithBaseModel_SetProperties_RowKey()
        {
            var contact = new Contact
                          {
                              Id = Guid.NewGuid(),
                              FirstName = "Test",
                              LastName = "User01",
                              EmailAddress = "test.user01@somedomain.com",
                              DateCreated = DateTime.Now,
                              Active = true
                          };

            var contactEntity = new ContactEntity(contact);

            var expectedRowKey = string.Format("Contact_{0}", contact.Id);
            Assert.AreEqual(contact.FirstName, contactEntity.FirstName);
            Assert.AreEqual(contact.LastName, contactEntity.LastName);
            Assert.AreEqual(contact.EmailAddress, contactEntity.EmailAddress);
            Assert.AreEqual(contact.DateCreated, contactEntity.DateCreated);
            Assert.AreEqual(contact.Active, contactEntity.Active);
            Assert.AreEqual(expectedRowKey, contactEntity.RowKey);
        }

        [TestMethod]
        public void Constructor_AllowsParameterlessConstructor()
        {
            var customerEntity = new ContactEntity();

            Assert.AreEqual(customerEntity.Id, Guid.Empty);
            Assert.IsNull(customerEntity.FirstName);
            Assert.IsNull(customerEntity.LastName);
        }

        [TestMethod]
        public void ToDomainClass_Returns_NewObjectOfBaseClassType()
        {
            var contactA = new Contact
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User01",
                EmailAddress = "test.user01@somedomain.com",
                DateCreated = DateTime.Now,
                Active = true
            };

            var contactEntity = new ContactEntity(contactA);

            var contactB = contactEntity.ToDomainClass();

            Assert.AreEqual(contactA.FirstName, contactB.FirstName);
            Assert.AreEqual(contactA.LastName, contactB.LastName);
            Assert.AreEqual(contactA.EmailAddress, contactB.EmailAddress);
            Assert.AreEqual(contactA.DateCreated, contactB.DateCreated);
            Assert.AreEqual(contactA.Active, contactB.Active);
            
        }

        [TestMethod]
        public void CRUD_Tests()
        {
            var contactA = new Contact
            {
                FirstName = "Test",
                LastName = "User01",
                EmailAddress = "test.user01@somedomain.com",
                DateCreated = DateTime.UtcNow,
                Active = true
            };

            var contactRepository = new ContactRepository("MyTestTable", "MyTestPartitionKey");

            //Create
            contactRepository.Create(contactA);
            Assert.IsTrue(contactA.Id != Guid.Empty);

            // Read
            var contactB = contactRepository.Find(contactA.Id);

            Assert.AreEqual(contactA.FirstName, contactB.FirstName);
            Assert.AreEqual(contactA.LastName, contactB.LastName);
            Assert.AreEqual(contactA.EmailAddress, contactB.EmailAddress);
            Assert.AreEqual(contactA.DateCreated, contactB.DateCreated);
            Assert.AreEqual(contactA.Active, contactB.Active);

            // Update
            contactB.LastName = "Last Name Changed";
            contactRepository.Update(contactB);

            var contactC = contactRepository.Find(contactB.Id);
            Assert.IsNotNull(contactC);
            Assert.AreEqual(contactC.LastName, contactB.LastName);

            // Delete
            var deleteResult = contactRepository.Delete(contactC.Id);
            var contactD = contactRepository.Find(contactC.Id);

            Assert.IsTrue(deleteResult);
            Assert.IsNull(contactD);

        }

        [TestMethod]
        public void FindAll_ReturnsAllContacts()
        {
            var contactRepository = new ContactRepository("MyTestTable", "MyTestPartitionKey");
            

            for (int i = 0; i < 10; i++)
            {
                var contact = new Contact
                              {
                                  FirstName = "Test",
                                  LastName = string.Format("Contact{0}", i),
                                  DateCreated = DateTime.UtcNow,
                                  Active = true
                              };
                contactRepository.Create(contact);
            }

            var contacts = contactRepository.FindAll();

            Assert.IsTrue(contacts.Count >= 10);
        }

    }
}
