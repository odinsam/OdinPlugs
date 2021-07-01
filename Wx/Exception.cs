using System;
using System.Collections.Generic;
using System.Web;

namespace OdinPlugs.Wx
{
    public class WxPayException : Exception
    {
        public WxPayException(string msg) : base(msg)
        {

        }
    }
}