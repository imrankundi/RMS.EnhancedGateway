using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection;
namespace RMS.Component.Wmi
{
    public class WmiObjectCreator
    {

        public static IEnumerable<T> QueryObject<T>() where T : WmiBaseType
        {
            return QueryObject<T>(null);
        }
        public static IEnumerable<T> QueryObject<T>(string whereClause) where T : WmiBaseType
        {
            Type type = typeof(T);
            Assembly assem = type.Assembly;
            string className = type.FullName;
            List<T> ts = new List<T>();
            var obj = assem.CreateInstance(className);
            var baseObj = obj as WmiBaseType;
            var props = WmiClassProperties(baseObj.ClassInfo.Namespace, baseObj.ClassInfo.Name, whereClause);

            foreach (var collectioin in props)
            {
                var o = assem.CreateInstance(className);
                foreach (PropertyData prop in collectioin)
                {

                    PropertyInfo piInstance = type.GetProperty(prop.Name);


                    if (piInstance != null)
                    {
                        var pAttr = piInstance.GetCustomAttributes<WmiPropertyAttribute>();
                        if (pAttr.Count() == 0)
                        {
                            piInstance.SetValue(o, prop.Value);
                        }
                        else
                        {
                            var a = pAttr.FirstOrDefault();
                            if (!a.Ignore)
                                piInstance.SetValue(o, a.Name);
                        }
                    }
                }

                ts.Add((T)o);
            }


            return ts;
        }

        private static IEnumerable<PropertyDataCollection> WmiClassProperties(string namespaceName, string wmiClassName, string whereClause = null)
        {
            ManagementPath managementPath = new ManagementPath();
            managementPath.Path = namespaceName;
            ManagementScope managementScope = new ManagementScope(managementPath);
            string query = string.Format("SELECT * FROM {0}{1}", wmiClassName, (string.IsNullOrWhiteSpace(whereClause) ? "" : string.Format(" WHERE {0}", whereClause)));

            ObjectQuery objectQuery = new ObjectQuery(query);
            ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher(managementScope, objectQuery);
            ManagementObjectCollection objectCollection = objectSearcher.Get();
            foreach (ManagementObject managementObject in objectCollection)
            {
                PropertyDataCollection props = managementObject.Properties;
                yield return props;
            }
        }
    }
}
