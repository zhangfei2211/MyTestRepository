using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Hosting;
using System.Web.Optimization;

namespace WebSite.App_Start
{
    public class BundleTransform : IBundleTransform

    {
        public void Process(BundleContext context, BundleResponse response)
        {
            foreach (var file in response.Files)
            {
                using (var fileStream = File.OpenRead(HostingEnvironment.MapPath(file.IncludedVirtualPath)))
                {
                    /// 此处选择使用MD5计算方式
                    var algorithm = new MD5CryptoServiceProvider();
                    var hashBytes = algorithm.ComputeHash(fileStream);
                    var version = string.Join(null, hashBytes.Select(value => value.ToString("x2")));
                    //var version = HttpServerUtility.UrlTokenEncode(hashBytes);
                    file.IncludedVirtualPath = string.Concat(file.IncludedVirtualPath, $"?v={version}");
                }
            }
        }
    }
}