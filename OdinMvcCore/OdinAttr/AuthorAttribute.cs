using System;

namespace OdinPlugs.OdinMvcCore.OdinAttr
{
    public class AuthorAttribute : Attribute
    {
        public string AuthorName { get; set; }

        public AuthorAttribute(string authorName)
        {
            AuthorName = authorName;
        }
    }
}