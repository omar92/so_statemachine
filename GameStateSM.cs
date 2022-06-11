using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SO.SMachine
{
    [CreateAssetMenu(fileName = "GameState", menuName = "SM/GameState")]
    public class GameStateSM : ScriptableObject
    {


        public List<gameStateListener> listeners = new List<gameStateListener>();
        public SMachine.GameStateSMSO statemachine;
        List<IStateListener> iStateListner = new List<IStateListener>();
        bool isEntered = false;
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
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            listeners = new List<gameStateListener>();
            iStateListner = new List<IStateListener>();
            isEntered = false;
        }



        //  private void OnDestroy() { listeners = new List<gameStateListener>(); }

        public void RegisterListener(gameStateListener listener)
        {
            if (!listeners.Contains(listener))
            {
              //  Debug.Log("register: " + listener.source.gameObject.name + " on " + name);
                listeners.Add(listener);
            }
        }
        public void UnregisterListener(gameStateListener listener)
        {
            if (listeners.Contains(listener))
            {
              //  Debug.Log("unregister: " + listener.source.gameObject.name + " on " + name);
                listeners.Remove(listener);
            }
        }

        internal void OnEnter()
        {
            isEntered = true;

            for (int i = 0; i < listeners.Count; i++)
            {
                try
                {
                    listeners[i].OnEnter.Invoke();
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
               
            }
            Z.InvokeEndOfFrame(() =>
            {
                for (int i = 0; i < iStateListner.Count; i++)
                {
                    try
                    {
                        iStateListner[i].OnEnter(this);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogException(ex);
                    }
                    
                }
            });
        }

        internal void OnExit()
        {
            if (!isEntered) return;
            for (int i = 0; i < listeners.Count; i++)
            {
                try
                {
                    listeners[i].OnExit.Invoke();
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
                
            }
            for (int i = 0; i < iStateListner.Count; i++)
            {
                try
                {
                    iStateListner[i].OnExit(this);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
           
            }
        }

        //internal void OnPause()
        //{
        //    for (int i = 0; i < listeners.Count; i++)
        //    {
        //        try
        //        {
        //            listeners[i].OnPause.Invoke();
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.LogException(ex);
        //        }
               
        //    }

        //}

        //internal void OnUnPause()
        //{
        //    for (int i = 0; i < listeners.Count; i++)
        //    {
        //        try
        //        {
        //            listeners[i].OnUnPause.Invoke();
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.LogException(ex);
        //        }
                
        //    }
        //}

        public void Switch()
        {
            _Switch(false);
        }

        internal void RegisterIListeners(IStateListener[] stateListeners)
        {
            for (int i = 0; i < stateListeners.Length; i++)
            {
                RegisterIListener(stateListeners[i]);
            }
        }
        internal void UnregisterIListeners(IStateListener[] stateListeners)
        {
            for (int i = 0; i < stateListeners.Length; i++)
            {
                UnregisterIListener(stateListeners[i]);
            }
        }

        internal void RegisterIListener(IStateListener stateListener)
        {
            if (iStateListner.Contains(stateListener)) return;
            iStateListner.Add(stateListener);
        }
        internal void UnregisterIListener(IStateListener stateListener)
        {
            if (!iStateListner.Contains(stateListener)) return;
            iStateListner.Remove(stateListener);
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