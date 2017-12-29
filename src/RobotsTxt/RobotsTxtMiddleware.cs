﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RobotsTxt {
    public class RobotsTxtMiddleware {
        private static readonly PathString _robotsTxtPath = new PathString("/robots.txt");
        private readonly RobotsTxtOptions _options;
        private readonly RequestDelegate _next;

        public RobotsTxtMiddleware(RequestDelegate next, RobotsTxtOptions options) {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context) {
            if(context.Request.Path == _robotsTxtPath) {
                await BuildRobotsTxt(context);
                return;
            }

            await _next.Invoke(context);
        }

        private async Task BuildRobotsTxt(HttpContext context) {
            var sb = _options.Build();

            using(var sw = new StreamWriter(context.Response.Body))
                await sw.WriteAsync(sb.ToString());
        }
    }
}