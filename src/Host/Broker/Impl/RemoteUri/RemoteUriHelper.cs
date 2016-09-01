﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace Microsoft.R.Host.Broker.RemoteUri {
    public class RemoteUriHelper {
        public static async Task HandlerAsync(HttpContext context) {
            string url = null;
            string method = null;
            Dictionary<string, string> headers = null;
            Stream content = null;

            BinaryReader reader = new BinaryReader(context.Request.Body);
            url = reader.ReadString();
            method = reader.ReadString();
            headers = JsonConvert.DeserializeObject<Dictionary<string, string>>(reader.ReadString());

            HttpClient client = new HttpClient();
            Uri uri = new Uri(url);

            HttpMethod httpMethod = new HttpMethod(method);
            HttpRequestMessage req = new HttpRequestMessage(httpMethod, uri);

            foreach (var key in headers.Keys) {
                var val = headers[key];
                req.Headers.Add(key, val);
            }

            if (httpMethod == HttpMethod.Post) {
                content = new MemoryStream();
                long length = context.Request.Body.Length - context.Request.Body.Position;
                var buffer = reader.ReadBytes((int)length);
                await content.WriteAsync(buffer, 0, buffer.Length);
                await content.FlushAsync();
                content.Position = 0;

                req.Content = new StreamContent(content);
            }

            // send request to the local server
            var httpResponse = await client.SendAsync(req);
            var respHeaders = new Dictionary<string, string>();

            foreach (var pair in httpResponse.Headers) {
                StringBuilder value = new StringBuilder("");
                foreach (var val in pair.Value) {
                    value.Append(val);
                    value.Append(",");
                }
                respHeaders.Add(pair.Key, value.ToString());
            }

            BinaryWriter writer = new BinaryWriter(context.Response.Body);
            writer.Write(JsonConvert.SerializeObject(respHeaders));
            await httpResponse.Content.CopyToAsync(context.Response.Body);
        }
    }
}
