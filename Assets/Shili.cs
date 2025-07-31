using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class InterfaceFinder
{
    // ʾ������������ʵ��IManager�ӿڵ���
    public static List<Type> FindClassesImplementingInterface<TInterface>()
    {
        return FindClassesImplementingInterface(typeof(TInterface));
    }

    public static List<Type> FindClassesImplementingInterface(Type interfaceType)
    {
        // ��֤�Ƿ�Ϊ�ӿ�����
        if (!interfaceType.IsInterface)
        {
            Debug.LogError($"{interfaceType.Name} ���ǽӿڣ�");
            return new List<Type>();
        }

        var results = new List<Type>();

        // ���������Ѽ��صĳ���
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            try
            {
                var types = assembly.GetTypes()
                    .Where(t =>
                        !t.IsAbstract && // �ų�������
                        !t.IsInterface && // �ų��ӿڱ���
                        interfaceType.IsAssignableFrom(t)); // ���ļ��: �Ƿ�ʵ�ֽӿ�

                results.AddRange(types);
            }
            catch (ReflectionTypeLoadException e)
            {
                Debug.LogWarning($"�޷����س��� {assembly.FullName} �е�ĳЩ����: {e.LoaderExceptions.FirstOrDefault()?.Message}");
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
                manager.Init(); // ͨ����������
            }
        }

    }
}