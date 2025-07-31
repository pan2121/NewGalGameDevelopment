using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class InterfaceFinder
{
    // 示例：查找所有实现IManager接口的类
    public static List<Type> FindClassesImplementingInterface<TInterface>()
    {
        return FindClassesImplementingInterface(typeof(TInterface));
    }

    public static List<Type> FindClassesImplementingInterface(Type interfaceType)
    {
        // 验证是否为接口类型
        if (!interfaceType.IsInterface)
        {
            Debug.LogError($"{interfaceType.Name} 不是接口！");
            return new List<Type>();
        }

        var results = new List<Type>();

        // 遍历所有已加载的程序集
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            try
            {
                var types = assembly.GetTypes()
                    .Where(t =>
                        !t.IsAbstract && // 排除抽象类
                        !t.IsInterface && // 排除接口本身
                        interfaceType.IsAssignableFrom(t)); // 核心检查: 是否实现接口

                results.AddRange(types);
            }
            catch (ReflectionTypeLoadException e)
            {
                Debug.LogWarning($"无法加载程序集 {assembly.FullName} 中的某些类型: {e.LoaderExceptions.FirstOrDefault()?.Message}");
            }
        }

        return results;
    }
}

public static class shili
{
    [RuntimeInitializeOnLoadMethod]
    private static void InitManager()
    {
        var interactables = InterfaceFinder.FindClassesImplementingInterface<IManager>();
        foreach (var type in interactables)
        {
            if (type.GetProperty("Instance")?.GetValue(null) is IManager manager)
            {
                manager.Init(); // 通过单例访问
            }
        }

    }
}