﻿using System.Collections.Generic;
using System.Linq;

namespace SearchApi.Core.OpenTracing
{
    public class OpenTracingPipeSpecification : IPipeSpecification<ConsumeContext>, IPipeSpecification<PublishContext>
    {
        public IEnumerable<ValidationResult> Validate()
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public void Apply(IPipeBuilder<ConsumeContext> builder)
        {
            builder.AddFilter(new OpenTracingConsumeFilter());
        }

        public void Apply(IPipeBuilder<PublishContext> builder)
        {
            builder.AddFilter(new OpenTracingPublishFilter());
        }
    }
}