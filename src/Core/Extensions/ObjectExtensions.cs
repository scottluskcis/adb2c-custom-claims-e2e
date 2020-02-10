﻿using Newtonsoft.Json;

namespace Core.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object value, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(value, formatting);
        }
    }
}
