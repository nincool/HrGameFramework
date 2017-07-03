//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace Hr.Utility
{
    /// <summary>
    /// 类型相关的实用函数。
    /// </summary>
    public static class HrType
    {
        private readonly static string[] AssemblyNames = { "Assembly-CSharp" };

        /// <summary>
        /// 获取指定基类的所有子类的名称。
        /// </summary>
        /// <param name="typeBase">基类类型。</param>
        /// <returns>指定基类的所有子类的名称。</returns>
        public static string[] GetTypeNames(System.Type typeBase)
        {
            List<string> typeNames = new List<string>();

            foreach (string assemblyName in AssemblyNames)
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(assemblyName);
                if (assembly == null)
                {
                    continue;
                }

                System.Type[] types = assembly.GetTypes();
                foreach (System.Type type in types)
                {
                    if (type.IsClass && !type.IsAbstract && typeBase.IsAssignableFrom(type))
                    {
                        typeNames.Add(type.FullName);
                    }
                }
            }

            typeNames.Sort();

            return typeNames.ToArray();
        }

        public static string[] GetAssemblyNamespace()
        {
            HashSet<string> setNamespace = new HashSet<string>();

            foreach (string assemblyName in AssemblyNames)
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(assemblyName);
                if (assembly == null)
                {
                    continue;
                }

                System.Type[] types = assembly.GetTypes();
                foreach (System.Type type in types)
                {
                    if (type.IsClass && !type.IsAbstract)
                    {
                        setNamespace.Add(type.Namespace);
                    }
                }
            }

            return setNamespace.ToArray();
        }
    }
}
