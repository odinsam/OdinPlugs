using System.Collections.Generic;

namespace OdinPlugs.OdinMAF.OdinEF.EFCore
{
    public class OdinUpdateObject
    {
        public static T UpdateObject<T, D>(T updateObject, D sourceObject, params string[] OtherFields)
        {
            List<string> fields = new List<string>();
            foreach (var item in OtherFields)
            {
                fields.Add(item);
            }
            foreach (var pr in updateObject.GetType().GetProperties())
            {

                if (!fields.Contains(pr.Name))
                    if (sourceObject.GetType().GetProperty(pr.Name) != null)
                        pr.SetValue(updateObject, sourceObject.GetType().GetProperty(pr.Name).GetValue(sourceObject));
            }
            return updateObject;
        }
    }
}