using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SO.SMachine
{
    [CreateAssetMenu(fileName = "GameState", menuName = "SM/GameState")]
    public class GameStateSM : ScriptableObject
    {
        public List<gameStateListener> listeners = new List<gameStateListener>();
        
        [TextArea]
        [Tooltip("What does this GameState do")]
        public string GameStateDescription = "[What does this GameState do]";

        public void RegisterListener(gameStateListener listener)
        {
            if (!listeners.Contains(listener))
            {
                Debug.Log("register: " + listener.source.gameObject.name + " on " + name);
                listeners.Add(listener);
            }
        }
        public void UnregisterListener(gameStateListener listener)
        {
            if (listeners.Contains(listener))
            {
                Debug.Log("unregister: " + listener.source.gameObject.name + " on " + name);
                listeners.Remove(listener);
            }
        }

        internal void OnEnter()
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].OnEnter.Invoke();
            }
        }

        internal void OnExit()
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].OnExit.Invoke();
            }
        }
        internal void OnPause()
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].OnPause.Invoke();
            }
        }

        internal void OnUnPause()
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].OnUnPause.Invoke();
            }
        }
    }
}