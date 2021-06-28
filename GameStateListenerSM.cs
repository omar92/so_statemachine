using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SO.SMachine
{
    [System.Serializable]
    public class gameStateListener
    {
        public GameStateSM GameState;
        [Tooltip("must be enabled at first")]
        public bool listenWhenDisabled;
        public UnityEvent OnEnter;
        public UnityEvent OnPause;
        public UnityEvent OnUnPause;
        public UnityEvent OnExit;
        [TextArea]
        [Tooltip("What does this object do in this GameState")]
        public string GameStateBehaviour;
        internal GameStateListenerSM source;
    }

    public class GameStateListenerSM : MonoBehaviour
    {

        static Dictionary<int, GameStateListenerSM> inistances = null;

        public List<gameStateListener> StatesListeners = new List<gameStateListener>();

        GameStateListenerSM()
        {
            if (inistances == null) inistances = new Dictionary<int, GameStateListenerSM>();
            try
            {
                if (Application.isPlaying)
                    inistances.Add(this.GetInstanceID(), this);
                else
                    inistances = null;
            }
            catch (Exception)
            {
            }
        }

        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        //static void OnLoad()
        //{
        //    Z.InvokeEndOfFrame(() =>
        //    {
        //        foreach (var inistance in inistances.Values)
        //        {
        //            for (int i = 0; i < inistance.StatesListeners.Count; i++)
        //            {
        //                try
        //                {
        //                    if (inistance.StatesListeners[i].listenWhenDisabled == true)
        //                    {
        //                        inistance.StatesListeners[i].source = inistance;
        //                        inistance.StatesListeners[i].GameState.RegisterListener(inistance.StatesListeners[i]);
        //                    }
        //                }
        //                catch (Exception e)
        //                {
        //                    Debuger.LogError(e.Message);
        //                }
        //            }
        //        }
        //    });
        //}

        private void OnEnable()
        {
            for (int i = 0; i < StatesListeners.Count; i++)
            {
                StatesListeners[i].source = this;
                StatesListeners[i].GameState.RegisterListener(StatesListeners[i]);
            }
        }
        private void OnDestroy()
        {
            for (int i = 0; i < StatesListeners.Count; i++)
            {
                StatesListeners[i].GameState.UnregisterListener(StatesListeners[i]);
            }
        }
        private void OnDisable()
        {
            for (int i = 0; i < StatesListeners.Count; i++)
            {
                if (StatesListeners[i].listenWhenDisabled == false)
                    StatesListeners[i].GameState.UnregisterListener(StatesListeners[i]);
            }
        }

        //public void OnEnter(SMGameState GameState)
        //{
        //    for (int i = 0; i < StatesListeners.Count; i++)
        //    {
        //        if (StatesListeners[i].GameState == GameState)
        //            StatesListeners[i].OnEnter.Invoke();
        //    }
        //}

        //public void OnPause(SMGameState GameState)
        //{
        //    for (int i = 0; i < StatesListeners.Count; i++)
        //    {
        //        if (StatesListeners[i].GameState == GameState)
        //            StatesListeners[i].OnPause.Invoke();
        //    }
        //}

        //public void OnUnPause(SMGameState GameState)
        //{
        //    for (int i = 0; i < StatesListeners.Count; i++)
        //    {
        //        if (StatesListeners[i].GameState == GameState)
        //            StatesListeners[i].OnUnPause.Invoke();
        //    }
        //}

        //public void OnExit(SMGameState GameState)
        //{
        //    for (int i = 0; i < StatesListeners.Count; i++)
        //    {
        //        if (StatesListeners[i].GameState == GameState)
        //            StatesListeners[i].OnExit.Invoke();
        //    }
        //}

    }
}