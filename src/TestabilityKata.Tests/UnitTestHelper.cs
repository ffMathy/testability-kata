using Autofac;
using FluffySpoon.Testing.Autofake;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestabilityKata.Tests
{
    public class UnitTestHelper
    {
        public IContainer CreateIocContainerFor<TClassBeingTested>()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new TestabilityKataAutofacConfiguration());

            var autofaker = new Autofaker();
            autofaker.UseNSubstitute();
            autofaker.UseAutofac(containerBuilder);

            autofaker.RegisterFakesForConstructorParameterTypesOf<TClassBeingTested>();

            return containerBuilder.Build();
        }
    }
}
