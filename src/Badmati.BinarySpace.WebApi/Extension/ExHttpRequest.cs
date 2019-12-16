﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.IO;

namespace Badmati.BinarySpace.WebApi.Extension
{
    public static class ExHttpRequest
    {
        public static string ToReadAsyncJson(this HttpRequest request)
        {
            try
            {
                // Allows using several time the stream in ASP.Net Core
                request.EnableRewind();
                // Rewind, so the core is not lost when it looks the body for the request
                request.Body.Position = 0;
                var result = new StreamReader(request.Body).ReadToEnd();
                return result;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
