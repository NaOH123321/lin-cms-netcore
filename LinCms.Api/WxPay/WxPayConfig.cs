using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinCms.Api.WxPay
{
    public class WxPayConfig
    {
        private static volatile IWxPayConfig config;
        private static object syncRoot = new object();

        public static IWxPayConfig GetConfig()
        {
            if (config == null)
            {
                lock (syncRoot)
                {
                    if (config == null)
                        config = new DemoConfig();
                }
            }

            return config;
        }
    }
}

