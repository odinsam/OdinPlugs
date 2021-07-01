using System;
using System.Xml;
namespace OdinPlugs.Wx
{
    public class SafeXmlDocument : XmlDocument
    {
        public SafeXmlDocument()
        {
            this.XmlResolver = null;
        }
    }
}
