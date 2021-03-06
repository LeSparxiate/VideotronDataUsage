﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VideotronDataUsage.Classes
{
    /// <summary>
    /// Used to perform HTTP requests. (POST/GET/...)
    /// </summary>
    public class HttpRequest
    {
        /// <summary>
        /// Perform a Get request
        /// </summary>
        /// <param name="link">Request link</param>
        /// <param name="parameters">Parameters</param>
        /// <param name="headers">Header parameters</param>
        /// <param name="h_data">Header values</param>
        /// <returns>Result of the request</returns>
        public static HttpWebResponse GetRequest(string link, string[] parameters = null, string[] headers = null, string[] h_data = null)
        {
            var postData = "";

            var request = (HttpWebRequest)WebRequest.Create(link);

            if (parameters != null)
            {
                for (int i = 0; i < parameters.Count(); i++)
                {
                    if (i > 0)
                        postData += "&";
                    postData += parameters[i];
                }
            }

            var data = Encoding.ASCII.GetBytes(postData);
            request.ContentLength = data.Length;

            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";

            if (headers != null && h_data != null)
            {
                for (int i = 0; i < headers.Count(); i++)
                {
                    request.Headers.Add(headers[i], h_data[i]);
                }
            }

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                return response;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
