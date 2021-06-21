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
        public SMachine.GameStateSMSO statemachine;


        [TextArea]
        [Tooltip("What does this GameState do")]
        public string GameStateDescription = "[What does this GameState do]";


        public void OnAfterDeserialize() { listeners = new List<gameStateListener>(); }
        public void OnBeforeSerialize() { listeners = new List<gameStateListener>(); }
        //  private void OnDestroy() { listeners = new List<gameStateListener>(); }

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

        public void Switch()
        {
            _Switch(false);
        }
        public void ForceSwitch()
        {
            _Switch(true);
        }

        private void _Switch(bool isForced)
        {
            if (statemachine != null)
            {
                if (!isForced) statemachine.SetState(this);
                else statemachine.ForceSetState(this);
            }
            else
                Debuger.LogError("please set parent statemachine first for " + this.name);
        }
    }
}