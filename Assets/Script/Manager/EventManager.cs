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
                DontDestroyOnLoad(instance); // ȷ���ڳ����л�ʱ������
            }
            return instance;
        }
    }
    // ˽�л����캯��
    private EventManager()
    {
        eventDic = new Dictionary<string, List<Action<EventArgs>>>();
    }
    public void Init()
    {

    }
    /// <summary>
    /// �¼�ע��
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
                Debug.Log("����ظ��ĺ���" + str);
            }
        }
    }
    /// <summary>
    /// �¼�ע��
    /// </summary>
    /// <param name="str"></param>
    /// <param name="actions"></param>
    public void RemoveEvent(string str, params Action<EventArgs>[] actions)
    {
        if (!eventDic.ContainsKey(str))
        {
            Debug.LogError("û�и��¼���ע��ʧ��" + str);
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
    /// �¼��㲥
    /// </summary>
    /// <param name="str"></param>
    /// <param name="eventArgs"></param>
    public void Broadcast(string eventName, EventArgs args)
    {
        if (eventDic.TryGetValue(eventName, out var handlers))
        {
            // �����������ؼ�����
            var handlersCopy = handlers.ToList();

            foreach (var handler in handlersCopy) // ��������
            {
                handler.Invoke(args);
            }
        }
    }
}
