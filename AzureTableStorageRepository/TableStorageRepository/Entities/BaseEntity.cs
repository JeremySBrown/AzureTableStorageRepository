using Microsoft.WindowsAzure.Storage.Table;
using TableStorageRepository.Helpers;

namespace TableStorageRepository.Entities
{
    public abstract class BaseEntity<T, U> : TableEntity
        where T : class, U, new() 
    {
        protected BaseEntity(U model)
        {
            var baseModel = model as T;
            if (baseModel != null)
            {
                SelfMap(baseModel);
            }
        }

        protected BaseEntity(U model, string partitionKey) : this(model)
        {
            this.PartitionKey = partitionKey;
        } 
        
        protected virtual void SelfMap(T source)
        {
            ModelHelper.CopyProperties(source, this);
            SetRowKey();
        }

        public virtual T ToDomainClass()
        {
            var temp = new T();
            ModelHelper.CopyProperties(this, temp);
            return temp;
        }

        protected abstract void SetRowKey();
    }
}