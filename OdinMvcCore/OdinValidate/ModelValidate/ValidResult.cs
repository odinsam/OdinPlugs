using System.Collections.Generic;

namespace OdinPlugs.OdinMvcCore.OdinValidate.ModelValidate
{
    public class ValidResult
    {
        public List<ErrorMember> ErrorMembers { get; set; }
        public bool IsVaild { get; set; }
    }
}