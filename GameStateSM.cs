using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
namespace SO.SMachine
{
    [CreateAssetMenu(fileName = "GameState", menuName = "SM/GameState")]
    public class GameStateSM : ScriptableObject
    {


        public List<gameStateListener> listeners = new List<gameStateListener>();
        public SMachine.GameStateSMSO statemachine;
        List<IStateListener> iStateListner =new List<IStateListener>();

        [TextArea]
        [Tooltip("What does this GameState do")]
        public string GameStateDescription = "[What does this GameState do]";


        private void Awake()
        {
            listeners = new List<gameStateListener>();
            iStateListner = new List<IStateListener>();
        }

        private void OnEnable()
        {
            listeners = new List<gameStateListener>();
            iStateListner = new List<IStateListener>();
        }

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
            Z.InvokeEndOfFrame(() =>
            {
                for (int i = 0; i < iStateListner.Count; i++)
                {
                    iStateListner[i].OnEnter(this);
                }
            });
        }

        internal void OnExit()
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].OnExit.Invoke();
            }
            for (int i = 0; i < iStateListner.Count; i++)
            {
                iStateListner[i].OnExit(this);
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

        [Button("Switch")]
        public void Switch()
        {
            _Switch(false);
        }

        internal void RegisterIListeners(IStateListener[] stateListeners)
        {
            for (int i = 0; i < stateListeners.Length; i++)
            {
                if (iStateListner.Contains(stateListeners[i])) continue;
                iStateListner.Add(stateListeners[i]);
            }
        }
        internal void UnregisterIListeners(IStateListener[] stateListeners)
        {
            for (int i = 0; i < stateListeners.Length; i++)
            {
                if (!iStateListner.Contains(stateListeners[i])) continue;
                iStateListner.Remove(stateListeners[i]);
            }
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