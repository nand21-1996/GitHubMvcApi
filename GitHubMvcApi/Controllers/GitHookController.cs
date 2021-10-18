﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebHooks;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Primitives;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace GitHubMvcApi.Controllers
{
    public class GitHookController : ControllerBase
    {
        private const string Sha1Prefix = "sha1=";

        [HttpPost]
        public async Task<IActionResult> Receive()
        {
            Request.Headers.TryGetValue("X-GitHub-Event", out StringValues eventName);
            Request.Headers.TryGetValue("X-Hub-Signature", out StringValues signature);
            Request.Headers.TryGetValue("X-GitHub-Delivery", out StringValues delivery);

            using (var reader = new StreamReader(Request.Body))
            {
                var txt = await reader.ReadToEndAsync();

                if (IsGithubPushAllowed(txt, eventName, signature))
                {
                    return Ok(eventName+" Works");
                }
            }

            return Unauthorized();
        }
        private bool IsGithubPushAllowed(string payload, string eventName, string signatureWithPrefix)
        {
            if (string.IsNullOrWhiteSpace(payload))
            {
                throw new ArgumentNullException(nameof(payload));
            }
            if (string.IsNullOrWhiteSpace(eventName))
            {
                throw new ArgumentNullException(nameof(eventName));
            }
            if (string.IsNullOrWhiteSpace(signatureWithPrefix))
            {
                throw new ArgumentNullException(nameof(signatureWithPrefix));
            }

            /* test if the eventName is ok if you want 
            if (!eventName.Equals("push", StringComparison.OrdinalIgnoreCase))
            {
                ...
            } */

            //if (signatureWithPrefix.StartsWith(Sha1Prefix, StringComparison.OrdinalIgnoreCase))
            //{
            //    var signature = signatureWithPrefix.Substring(Sha1Prefix.Length);
            //    var secret = Encoding.ASCII.GetBytes(_tokenOptions.Value.ServiceSecret);
            //    var payloadBytes = Encoding.ASCII.GetBytes(payload);

            //    using (var hmSha1 = new HMACSHA1(secret))
            //    {
            //        var hash = hmSha1.ComputeHash(payloadBytes);

            //        var hashString = ToHexString(hash);

            //        if (hashString.Equals(signature))
            //        {
            //            return true;
            //        }
            //    }
            //}

            return true;
        }


        public static string ToHexString(byte[] bytes)
        {
            var builder = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                builder.AppendFormat("{0:x2}", b);
            }

            return builder.ToString();
        }

      
    }
}
