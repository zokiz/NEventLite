﻿using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using NEventLite.Events;
using NEventLite.Event_Bus;
using NEventLite.Logger;
using NEventLite.Repository;
using NEventLite.Storage;
using NEventLite_Example.Domain;
using NEventLite_Example.Event_Bus;
using NEventLite_Example.Logging;
using NEventLite_Example.Repository;
using NEventLite_Example.Storage;
using NEventLite_Storage_Providers.InMemory;

namespace NEventLite_Example.Util
{
    public class DependencyResolver
    {
        private IContainer Container { get; }

        public DependencyResolver()
        {
            // Create your builder.
            var builder = new ContainerBuilder();

            //-------- Event Stores ------------

            //Event store connection settings are in EventstoreEventStorageProvider class
            //If you don't have eventstore installed comment our the line below
            //builder.RegisterType<MyEventstoreEventStorageProvider>().As<IEventStorageProvider>().SingleInstance();

            builder.RegisterType<InMemoryEventStorageProvider>().As<IEventStorageProvider>().PreserveExistingDefaults().SingleInstance();

            //----------------------------------

            //-------- Snapshot Stores ----------

            //Event store connection settings are in EventstoreConnection class
            //If you don't have eventstore installed comment out the line below
            //builder.RegisterType<MyEventstoreSnapshotStorageProvider>().As<ISnapshotStorageProvider>().SingleInstance();

            //Redis connection settings are in RedisConnection class
            //builder.RegisterType<MyRedisSnapshotStorageProvider>().As<ISnapshotStorageProvider>().SingleInstance();

            builder.RegisterType<InMemorySnapshotStorageProvider>().As<ISnapshotStorageProvider>().PreserveExistingDefaults().SingleInstance();

            //----------------------------------

            //Event Bus
            builder.RegisterType<InMemoryEventBus>().As<IEventBus>().SingleInstance();

            //Logging
            builder.RegisterType<ConsoleLogger>().As<ILogger>();

            //This will resolve and bind storage types to a concrete repository of <T> as needed
            builder.RegisterGeneric(typeof(Repository<>)).Named("handler",typeof(IRepository<>)).SingleInstance();

            //This will bind the decorator
            builder.RegisterGenericDecorator(typeof(MyRepositoryDecorator<>),typeof(IRepository<>),fromKey: "handler");

            //Register NoteRepository
            builder.RegisterType<NoteRepository>();

            Container = builder.Build();
        }

        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

    }
}
