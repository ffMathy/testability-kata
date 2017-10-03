using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestabilityKata
{
    public class TestabilityKataAutofacConfiguration: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(
                    typeof(TestabilityKataAutofacConfiguration).Assembly)
                .AsImplementedInterfaces();

            base.Load(builder);
        }
    }
}
