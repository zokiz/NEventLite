﻿using System;
using System.Collections.Generic;
using NEventLite.Domain;
using NEventLite.Storage;

namespace NEventLite_Storage_Providers.InMemory
{
    public class InMemorySnapshotStorageProvider:ISnapshotStorageProvider{

        private readonly Dictionary<Guid,NEventLite.Snapshot.Snapshot> _items = new Dictionary<Guid,NEventLite.Snapshot.Snapshot>();

        public NEventLite.Snapshot.Snapshot GetSnapshot<T>(Guid aggregateId) where T : AggregateRoot
        {
            if (_items.ContainsKey(aggregateId))
            {
                return _items[aggregateId];
            }
            else
            {
                return null;
            }
        }

        public void SaveSnapshot<T>(NEventLite.Snapshot.Snapshot snapshot) where T : AggregateRoot
        {
            if (_items.ContainsKey(snapshot.AggregateId))
            {
               _items[snapshot.AggregateId] = snapshot;
            }
            else
            {
               _items.Add(snapshot.AggregateId,snapshot);
            }
        }
    }
}
