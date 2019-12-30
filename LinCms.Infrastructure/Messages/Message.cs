using System;
using LinCms.Infrastructure.Extensions;
using LinCms.Core;
using LinCms.Core.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LinCms.Infrastructure.Messages
{
    public abstract class Message : IMessage
    {
        private object? _message;

        public abstract int Code { get; }

        public abstract ResultCode ErrorCode { get; set; }

        public virtual object Msg
        {
            get => _message ?? ErrorCode.GetDisplayName();
            set => _message = value;
        }

        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(new
            {               
                Code,
                ErrorCode,
                Msg
            }, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy {ProcessDictionaryKeys = true}
                },
                Formatting = Formatting.Indented,
                DateFormatString = "yyyy-MM-dd"
            });
        }
    }
}
