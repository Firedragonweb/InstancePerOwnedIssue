using Autofac;
using Autofac.Features.OwnedInstances;
using NUnit.Framework;

namespace InstancePerOwnedIssues
{
    public class Tests
    {
        [Test]
        public void SingleInstanceTest()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<Service>().AsSelf().SingleInstance();
            builder.RegisterType<RootA>().Keyed<IRoot>("A");
            builder.RegisterType<RootB>().Keyed<IRoot>("B");
            IContainer container = builder.Build();
            Owned<IRoot> ownedRoot = container.ResolveKeyed<Owned<IRoot>>("A");
            Service service = container.Resolve<Service>();
            Assert.That(ReferenceEquals(ownedRoot.Value.Dependency, service), Is.True);
        }
        [Test]
        public void InstancePerLifetimeTest()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<Service>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<RootA>().Keyed<IRoot>("A");
            builder.RegisterType<RootB>().Keyed<IRoot>("B");
            IContainer container = builder.Build();
            Owned<IRoot> ownedRoot = container.ResolveKeyed<Owned<IRoot>>("A");
            Service service = container.Resolve<Service>();
            Assert.That(ReferenceEquals(ownedRoot.Value.Dependency, service), Is.False);
        }
        [Test]
        public void InstancePerOwnedTest()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<Service>().AsSelf().InstancePerOwned<IRoot>();
            builder.RegisterType<RootA>().Keyed<IRoot>("A");
            builder.RegisterType<RootB>().Keyed<IRoot>("B");
            IContainer container = builder.Build();
            Owned<IRoot> ownedRoot = container.ResolveKeyed<Owned<IRoot>>("A");
            Assert.That(ownedRoot.Value.Dependency, Is.Not.Null);
        }
    }

    public class Service
    {

    }

    public interface IRoot
    {
        Service Dependency { get; }
    }

    public class RootA : IRoot
    {
        public RootA(Service dependency)
        {
            Dependency = dependency;
        }

        /// <inheritdoc />
        public Service Dependency { get; }
    }

    public class RootB : IRoot
    {
        public RootB(Service dependency)
        {
            Dependency = dependency;
        }

        /// <inheritdoc />
        public Service Dependency { get; }
    }

}