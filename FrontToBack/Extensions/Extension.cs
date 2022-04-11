using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Extensions
{
    public static class Extension
    {
        public static bool IsValidType(this IFormFile file, string type)
        {
            return file.ContentType.Contains(type);
        }
        public static bool IsValidSize(this IFormFile file, int kb)
        {
            return file.Length / 1024 <= kb;
        }
    }
}
