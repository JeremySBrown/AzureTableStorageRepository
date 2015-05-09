using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TableStorageRepository.Helpers
{
    public static class ModelHelper
    {
        
        public static void CopyProperties<TSource, TDestination>(TSource sourceModel, TDestination destinationModel) 
            where TSource : class 
            where TDestination : class
        {
            if (sourceModel == null || destinationModel == null)
            {
                return;
            }

            PropertyInfo[] sourceProperties = GetProperties(sourceModel);
            PropertyInfo[] destinationProperties = GetProperties(destinationModel);

            foreach (PropertyInfo sourceProperty in sourceProperties)
            {
                string sourcePropertyName = sourceProperty.Name;
                string sourcePropertyType = sourceProperty.PropertyType.FullName;

                PropertyInfo detinationProperty =
                    destinationProperties.FirstOrDefault(dp => dp.Name == sourcePropertyName
                                                               && dp.PropertyType.FullName == sourcePropertyType 
                                                               && dp.CanWrite);

                if (detinationProperty != null)
                {
                    object sourceValue = sourceProperty.GetValue(sourceModel, null);
                    detinationProperty.SetValue(destinationModel, sourceValue, null);
                }
            }
        }

        public static void CopyList<TSource, TDestination>(IList<TSource> sourceList, IList<TDestination> destinationList)
            where TSource : class
            where TDestination : class 
        {
            foreach (TSource source in sourceList)
            {
                TDestination destinationModel = Activator.CreateInstance<TDestination>();
                CopyProperties(source, destinationModel);
                destinationList.Add(destinationModel);
            }
            
        }

        public static TDestination MapTo<TSource, TDestination>(TSource sourceModel)
            where TSource : class
            where TDestination : class
        {
            if (sourceModel == null)
            {
                return null;
            }

            TDestination destinationModel = Activator.CreateInstance<TDestination>();

            CopyProperties(sourceModel, destinationModel);

            return destinationModel;
        }

        public static IList<TDestination> MapToList<TSource, TDestination>(IList<TSource> sourceList)
            where TSource : class
            where TDestination : class
        {
            IList<TDestination> destinationList = new List<TDestination>();

            foreach (TSource source in sourceList)
            {
                destinationList.Add(MapTo<TSource, TDestination>(source));
            }

            return destinationList;
        }

        private static PropertyInfo[] GetProperties<T>(T model) where T: class
        {
            return model.GetType().GetProperties();
        }
    }
}