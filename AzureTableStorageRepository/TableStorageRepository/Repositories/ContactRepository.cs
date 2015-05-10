using System;
using System.Collections.Generic;
using Domain.Interfaces.Models;
using Domain.Interfaces.Repositories;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using TableStorageRepository.Entities;

namespace TableStorageRepository.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly string _clientAccount;
        private readonly string _partitionKey;
        private CloudStorageAccount _storageAccount;
        private CloudTableClient _tableClient;
        private CloudTable _table;

        public ContactRepository(string clientAccount, string partitionKey)
        {
            _clientAccount = clientAccount;
            _partitionKey = partitionKey;

            InitTableStoreage();
        }

        public IContact Find(Guid id)
        {
            string rowKey = string.Format("Contact_{0}", id);
            TableOperation retrieveOperation = TableOperation.Retrieve<ContactEntity>(_partitionKey, rowKey);
            TableResult tableResult = _table.Execute(retrieveOperation);
            if (retrieveOperation != null)
            {
                var customerEntity = tableResult.Result as ContactEntity;
                if (customerEntity != null)
                {
                    return customerEntity.ToDomainClass();
                }
            }

            return null;
        }

        public IList<IContact> FindAll()
        {
            var query = new TableQuery<ContactEntity>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, _partitionKey),
                    TableOperators.And,
                    TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThan, "Contact_"),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThanOrEqual,
                            "Contact_ffffffff-ffff-ffff-ffff-ffffffffffff")
                        )
                    ));

            var contacts = new List<IContact>();
            foreach (ContactEntity contactEntity in _table.ExecuteQuery(query))
            {
                contacts.Add(contactEntity.ToDomainClass());
            }

            return contacts;
        }

        public void Create(IContact contact)
        {
            contact.Id = Guid.NewGuid();
            Update(contact);
        }

        public void Update(IContact contact)
        {
            var contactEntity = new ContactEntity(contact, _partitionKey);
            TableOperation saveOperation = TableOperation.InsertOrReplace(contactEntity);
            _table.Execute(saveOperation);
        }

        public bool Delete(Guid id)
        {
            string rowKey = string.Format("Contact_{0}", id);
            TableOperation retrieveOperation = TableOperation.Retrieve<ContactEntity>(_partitionKey, rowKey);
            TableResult tableResult = _table.Execute(retrieveOperation);
            if (retrieveOperation != null)
            {
                var contactEntity = tableResult.Result as ContactEntity;
                if (contactEntity != null)
                {
                    TableOperation deleteOperation = TableOperation.Delete(contactEntity);
                    _table.Execute(deleteOperation);
                    return true;
                }
            }

            return false;
        }

        private void InitTableStoreage()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            _tableClient = _storageAccount.CreateCloudTableClient();
            _table = _tableClient.GetTableReference(_clientAccount);
            _table.CreateIfNotExists();

            
        }
    }
}