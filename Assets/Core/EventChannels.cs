using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using UnityEngine;

namespace Core
{
    //Action info
    public class ActionInfo
    {
        public List<Action> actions;
        public bool isRunning;

    }

    public class ActionInfo<T1>
    {
        public List<Action<T1>> actions;
        public bool isRunning;

    }

    public class ActionInfo<T1, T2>
    {
        public List<Action<T1, T2>> actions;
        public bool isRunning;

    }

    public class ActionInfo<T1, T2, T3>
    {
        public List<Action<T1, T2, T3>> actions;
        public bool isRunning;

    }

    public class ActionInfo<T1, T2, T3, T4>
    {
        public List<Action<T1, T2, T3, T4>> actions;
        public bool isRunning;

    }

    /// <summary>
    /// provides subscribing for some event 
    /// </summary>
    public class SimpleActionProvider
    {

        private Dictionary<string, ActionInfo> channels = new ();

        private Dictionary<string, ActionInfo> actionsToAdd = new();

        private Dictionary<string, ActionInfo> actionsToRemove = new();

        public void AddAction(string topic, Action callback)
        {

            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            if (!channels.ContainsKey(topic))
            {
                var info = new ActionInfo
                {
                    actions = new List<Action>() { callback },
                    isRunning = false,
                };
                channels.Add(topic, info);
            }
            else
            {
                if(!channels[topic].isRunning)
                {
                    channels[topic].actions.Add(callback);
                    return;
                }

                if(!actionsToAdd.ContainsKey(topic))
                {
                    var info = new ActionInfo
                    {
                        actions = new List<Action>() { callback },
                        isRunning = false,
                    };

                    actionsToAdd.Add(topic, info);  
                }
            }            
        }

        public void RemoveAction(string topic, Action callback)
        {

            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            if (!channels.ContainsKey(topic))
            {
                Debug.LogWarning("There is not action \" " + topic + "\"");
                return;
            }

            if (!channels[topic].isRunning)
            {
                channels[topic].actions.Remove(callback);

                if(channels[topic].actions.Count == 0)
                {
                    channels.Remove(topic);
                }
                return;
            }

            if (actionsToRemove.ContainsKey(topic))
            {
                actionsToRemove[topic].actions.Add(callback);
            }
            else
            {
                var info = new ActionInfo
                {
                    actions = new List<Action>() { callback },
                    isRunning = false,
                };
                actionsToRemove.Add(topic, info);
            }
        }

        public void DoAction(string topic)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }

            foreach(var action in actionsToAdd)
            {
                if(!channels.ContainsKey(action.Key))
                {
                    channels.Add(action.Key, action.Value);
                }
                else
                {
                    channels[action.Key].actions.AddRange(action.Value.actions); 
                }
            }

            actionsToAdd.Clear();

            foreach (var action in actionsToRemove)
            {
                if (channels.ContainsKey(action.Key))
                {
                    foreach (var item in action.Value.actions)
                    {
                        if (channels[action.Key].actions.Contains(item))
                        {
                            channels[action.Key].actions.Remove(item);
                        }
                    }

                    if (channels[action.Key].actions.Count == 0)
                    {
                        channels.Remove(action.Key);
                    }
                }
            }

            actionsToRemove.Clear();

            if (!channels.ContainsKey(topic))
            {
                Debug.LogWarning("There is not action \" " + topic + "\"");
                return;
            }

            channels[topic].isRunning = true;

            try
            {
                foreach (var action in channels[topic].actions)
                {
                    action.Invoke();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            channels[topic].isRunning = false;
        }
    }



    /// <summary>
    /// provides subscribing for some event only for objects wich implement IListener
    /// </summary>
    public class EventChannels<T> where T : IListener
    {
        private Dictionary<string, List<T>> channels = new();

        public void AddListener(string channel, T listener)
        {
            if (!channels.ContainsKey(channel))
            {
                channels[channel] = new List<T>();
                
            }
            channels[channel].Add(listener);
        }

        public void RemoveListener(string channel, T listener)
        {
            if (!channels.ContainsKey(channel))
            {
                Debug.LogWarning("There is not event channel \" " + channel + "\"");
                return;
            }
            if(!channels[channel].Contains(listener))
            {
                Debug.LogWarning("There is not listener \" " + channel + "\"");
                return;
            }

            channels[channel].Remove(listener);

            if(channels[channel].Count == 0)
            {
                RemoveChannel(channel);
            }
        }

        public void RemoveChannel(string channel)
        {
            if (!channels.ContainsKey(channel))
            {
                Debug.LogWarning("There is not event channel \" " + channel + "\"");
                return;
            }
            channels.Remove(channel);
        }

        public void Broadcast(string channel)
        {
            if (!channels.ContainsKey(channel))
            {
                Debug.LogWarning("There is not event channel \" " + channel + "\"");
                return;
            }

            var cachedChannel = new List<T>(channels[channel]);

            foreach (T listener in cachedChannel) 
            {
                listener.UpdateState();
            }           
        }
    }
}
