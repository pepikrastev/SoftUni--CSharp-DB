using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MiniORM
{
	// TODO: Create your ChangeTracker class here.
    internal class ChangeTracker<T>
        where T : class, new()
    {
        private readonly List<T> allEntities;

        private readonly List<T> added;

        private readonly List<T> removed;

        public ChangeTracker(IEnumerable<T> entities) 
        {
             this.added = new List<T>();
             this.removed = new List<T>();

             this.allEntities = CloneEntities(entities);
        }

        public IReadOnlyCollection<T> AllEntities =>
            this.allEntities.AsReadOnly();

        public IReadOnlyCollection<T> Added =>
            this.added.AsReadOnly();

        public IReadOnlyCollection<T> Removed =>
            this.removed.AsReadOnly();

        public void Add(T item) => this.added.Add(item);

        public void Remove(T item) => this.removed.Add(item);

        public IEnumerable<T> GetModifiedEntities(DbSet<T> dbSet)
        {
            List<T> modifiedEntitites = new List<T>();

            PropertyInfo[] primaryKeys = typeof(T)
                .GetProperties()
                .Where(pi => pi.HasAttribute<KeyAttribute>())
                .ToArray();

            foreach (T proxyEntity in this.AllEntities)
            {
                object[] primaryKeyValues = GetPrimaryKeyValues(primaryKeys, proxyEntity)
                    .ToArray();

                T entity = dbSet
                    .Entities
                    .Single(e => GetPrimaryKeyValues(primaryKeys, e).SequenceEqual(primaryKeyValues));

                bool isModified = IsModified(proxyEntity, entity);

                if (isModified)
                {
                    modifiedEntitites.Add(entity);
                }
            }

            return modifiedEntitites;
        }

        private static bool IsModified(T proxyEntity, T realEntity)
        {
            PropertyInfo[] monitoredProperties = typeof(T)
                .GetProperties()
                .Where(pi => DbContext.AllowedSqlTypes.Contains(pi.PropertyType))
                .ToArray();

            PropertyInfo[] modifiedProperties = monitoredProperties
                .Where(pi => !Equals(pi.GetValue(realEntity), pi.GetValue(proxyEntity)))
                .ToArray();

            bool isModified =  modifiedProperties.Any();
            return isModified;
        }

        private static IEnumerable<object> GetPrimaryKeyValues(IEnumerable<PropertyInfo> primaryKey, T entity)
        {
            return primaryKey.Select(pi => pi.GetValue(entity));
        }

        private static List<T> CloneEntities(IEnumerable<T> entities)
        {
            List<T> clonedEntities = new List<T>();

            PropertyInfo[] propertiesToClone = typeof(T)
                .GetProperties()
                .Where(pi => DbContext.AllowedSqlTypes.Contains(pi.PropertyType))
                .ToArray();

            foreach (T entity in entities)
            {
                T clonedEntity = Activator.CreateInstance<T>();

                foreach (PropertyInfo property in propertiesToClone)
                {
                    object value = property
                        .GetValue(entity);

                    property.SetValue(clonedEntity, value);
                }

                clonedEntities.Add(clonedEntity);
            }

            return clonedEntities;
        }
    }
}