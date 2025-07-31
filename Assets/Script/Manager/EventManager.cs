using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventManager : MonoBehaviour, IManager
{
    private static EventManager instance;
    private Dictionary<string, List<Action<EventArgs>>> eventDic;

    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("EventManager").AddComponent<EventManager>();
                DontDestroyOnLoad(instance); // 确保在场景切换时不销毁
            }
            return instance;
        }
    }
    // 私有化构造函数
    private EventManager()
    {
        eventDic = new Dictionary<string, List<Action<EventArgs>>>();
    }
    public void Init()
    {

    }
    /// <summary>
    /// 事件注册
    /// </summary>
    /// <param name="str"></param>
    /// <param name="actions"></param>
    public void AddEvent(string str, params Action<EventArgs>[] actions)
    {
        if (!eventDic.ContainsKey(str))
        {
            eventDic[str] = new List<Action<EventArgs>>();
        }

        var eventList = eventDic[str];
        if (actions == null)
        {
            return;
        }
        foreach (var action in actions)
        {
            if (!eventList.Contains(action))
            {
                eventList.Add(action);
            }
            else
            {
                Debug.Log("添加重复的函数" + str);
            }
        }
    }
    /// <summary>
    /// 事件注销
    /// </summary>
    /// <param name="str"></param>
    /// <param name="actions"></param>
    public void RemoveEvent(string str, params Action<EventArgs>[] actions)
    {
        if (!eventDic.ContainsKey(str))
        {
            Debug.LogError("没有该事件，注销失败" + str);
            return;
        }

        var eventList = eventDic[str];
        foreach (var action in actions)
        {
            if (eventList.Contains(action))
            {
                eventList.Remove(action);
            }
        }
    }

    public void RemoveListener(string str)
    {
        if (eventDic.ContainsKey(str))
        {
            eventDic.Remove(str);
        }
    }

    public void ResetEvent()
    {
        eventDic.Clear();
    }
    /// <summary>
    /// 事件广播
    /// </summary>
    /// <param name="str"></param>
    /// <param name="eventArgs"></param>
    public void Broadcast(string eventName, EventArgs args)
    {
        if (eventDic.TryGetValue(eventName, out var handlers))
        {
            // 创建副本（关键！）
            var handlersCopy = handlers.ToList();

            foreach (var handler in handlersCopy) // 遍历副本
            {
                handler.Invoke(args);
            }
        }
    }
}
