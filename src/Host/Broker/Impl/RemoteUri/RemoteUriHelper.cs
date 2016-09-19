﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Common.Core;
using Microsoft.R.Host.Protocol;

namespace Microsoft.R.Host.Broker.RemoteUri {
    public class RemoteUriHelper {
        public static async Task HandlerAsync(HttpContext context) {
            var url = context.Request.Headers[CustomHttpHeaders.RTVSRequestedURL];

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = context.Request.Method;

            ClientWebSocket clientWebsocket = null;
            if (context.WebSockets.IsWebSocketRequest) {
                UriBuilder ub = new UriBuilder(url) { Scheme = "ws" };
                clientWebsocket = new ClientWebSocket();
                await clientWebsocket.ConnectAsync(ub.Uri, CancellationToken.None);
                var serverWebSocket = await context.WebSockets.AcceptWebSocketAsync(clientWebsocket.SubProtocol);
                await WebSocketHelper.SendReceiveAsync(serverWebSocket, clientWebsocket, CancellationToken.None);
            } else {
                SetRequestHeaders(request, context.Request.Headers);

                if (context.Request.ContentLength > 0) {
                    using (Stream reqStream = await request.GetRequestStreamAsync()) {
                        await context.Request.Body.CopyToAsync(reqStream);
                        await reqStream.FlushAsync();
                    }
                }

                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                SetResponseHeaders(response, context.Response);

                using (Stream respStream = response.GetResponseStream()) {
                    await respStream.CopyToAsync(context.Response.Body);
                    await context.Response.Body.FlushAsync();
                }

                response.Close();
            }
        }

        private static void SetRequestHeaders(HttpWebRequest request, IHeaderDictionary requestHeaders) {
            // copy headers to avoid messing with original request headers
            Dictionary<string, string> headers = new Dictionary<string, string>();
            foreach (var pair in requestHeaders) {
                headers.Add(pair.Key, string.Join(", ", pair.Value));
            }

            string valueAccept;
            if (headers.TryGetValue("Accept", out valueAccept)) {
                request.Accept = valueAccept;
                headers.Remove("Accept");
            }

            string valueConnection;
            if (headers.TryGetValue("Connection", out valueConnection)) {
                if (valueConnection.EqualsIgnoreCase("keep-alive")) {
                    request.KeepAlive = true;
                } else if (valueConnection.EqualsIgnoreCase("close")) {
                    request.KeepAlive = false;
                } else {
                    request.Connection = valueConnection;
                }
                headers.Remove("Connection");
            }

            string valueContentLength;
            if (headers.TryGetValue("Content-Length", out valueContentLength)) {
                request.ContentLength = valueContentLength.ToLongOrDefault();
                headers.Remove("Content-Length");
            }

            string valueContentType;
            if (headers.TryGetValue("Content-Type", out valueContentType)) {
                request.ContentType = valueContentType;
                headers.Remove("Content-Type");
            }

            string valueExcept;
            if (headers.TryGetValue("Expect", out valueExcept)) {
                request.Expect = valueExcept;
                headers.Remove("Expect");
            }

            string valueDate;
            if (headers.TryGetValue("Date", out valueDate)) {
                request.Date = valueDate.ToDateTimeOrDefault();
                headers.Remove("Date");
            }

            string valueHost;
            if (headers.TryGetValue("Host", out valueHost)) {
                request.Host = valueHost;
                headers.Remove("Host");
            }

            string valueIfModifiedSince;
            if (headers.TryGetValue("If-Modified-Since", out valueIfModifiedSince)) {
                request.IfModifiedSince = valueIfModifiedSince.ToDateTimeOrDefault();
                headers.Remove("If-Modified-Since");
            }

            string valueRange;
            if (headers.TryGetValue("Range", out valueRange)) {
                // TODO: AddRange
                headers.Remove("Range");
            }

            string valueReferer;
            if (headers.TryGetValue("Referer", out valueReferer)) {
                request.Referer = valueReferer;
                headers.Remove("Referer");
            }

            string valueTransferEncoding;
            if (headers.TryGetValue("Transfer-Encoding", out valueTransferEncoding)) {
                request.SendChunked = true;
                request.TransferEncoding = valueTransferEncoding;
                headers.Remove("Transfer-Encoding");
            }

            string valueUserAgent;
            if (headers.TryGetValue("User-Agent", out valueUserAgent)) {
                request.UserAgent = valueUserAgent;
                headers.Remove("User-Agent");
            }

            foreach (var pair in headers) {
                request.Headers.Add(pair.Key, pair.Value);
            }
        }

        private static void SetResponseHeaders(HttpWebResponse response, HttpResponse httpResponse) {
            // copy headers to avoid messing with original response headers
            Dictionary<string, string> headers = new Dictionary<string, string>();
            foreach (var key in response.Headers.AllKeys) {
                headers.Add(key, response.Headers[key]);
            }

            httpResponse.ContentLength = response.ContentLength;
            httpResponse.ContentType = response.ContentType;
            httpResponse.StatusCode = (int)response.StatusCode;

            if (headers.ContainsKey("Content-Length")) {
                headers.Remove("Content-Length");
            }

            if (headers.ContainsKey("Content-Type")) {
                headers.Remove("Content-Type");
            }

            foreach (var pair in headers) {
                httpResponse.Headers.Add(pair.Key, pair.Value);
            }
        }
    }
}