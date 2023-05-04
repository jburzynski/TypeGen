using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TypeGen.Core.Extensions;
using TypeGen.Core.Metadata;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.Core.Generator.Services
{
    internal class TsJsonContractResolver : DefaultContractResolver
    {
        private readonly IMetadataReaderFactory _metadataReaderFactory;
        private readonly GeneratorOptions _generatorOptions;

        public TsJsonContractResolver(IMetadataReaderFactory metadataReaderFactory, GeneratorOptions generatorOptions)
        {
            _metadataReaderFactory = metadataReaderFactory;
            _generatorOptions = generatorOptions;
        }

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            var exportableMembers = objectType.GetTsExportableMembers(_metadataReaderFactory.GetInstance()).ToList();
            return exportableMembers;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var nameAttribute = _metadataReaderFactory.GetInstance().GetAttribute<TsMemberNameAttribute>(member);
            property.PropertyName = nameAttribute?.Name ?? _generatorOptions.PropertyNameConverters.Convert(member.Name, member);
            return property;
        }
    }
}
