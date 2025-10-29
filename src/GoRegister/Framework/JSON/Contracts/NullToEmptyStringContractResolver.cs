using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GoRegister.Framework.JSON.Contracts
{
    public class NullToEmptyStringContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (property.PropertyType == typeof(string))
            {
                // Wrap value provider supplied by Json.NET.
                property.ValueProvider = new NullToEmptyStringValueProvider(property.ValueProvider);
            }

            return property;
        }

        sealed class NullToEmptyStringValueProvider : IValueProvider
        {
            private readonly IValueProvider Provider;

            public NullToEmptyStringValueProvider(IValueProvider provider)
            {
                Provider = provider ?? throw new ArgumentNullException("provider");
            }

            public object GetValue(object target) => Provider.GetValue(target) ?? "";

            public void SetValue(object target, object value) => Provider.SetValue(target, value);
        }
    }
}
