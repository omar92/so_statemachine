﻿using System;
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
        //public UnityEvent OnPause;
        //public UnityEvent OnUnPause;
        public UnityEvent OnExit;
        [TextArea]
        [Tooltip("What does this object do in this GameState")]
        public string GameStateBehaviour;
        internal GameStateListenerSM source;
    }

    public class GameStateListenerSM : MonoBehaviour
    {
        //static Dictionary<int, GameStateListenerSM> inistances = null;

        [SerializeField] private UnityEvent Init;
        [SerializeField] private List<gameStateListener> StatesListeners = new List<gameStateListener>();

        private bool isInitialized;

        //GameStateListenerSM()
        //{
        //    if (inistances == null) inistances = new Dictionary<int, GameStateListenerSM>();
        //}

        //private void Awake()
        //{
        //    if (Application.isPlaying)
        //        inistances.Add(this.GetInstanceID(), this);
        //    else
        //        inistances = null;
        //}

        private void OnEnable()
        {
            Z.InvokeEndOfFrame(() =>
            {
                if (!isInitialized) { Init.Invoke(); isInitialized = true; }
                for (int i = 0; i < StatesListeners.Count; i++)
                {
                    StatesListeners[i].source = this;
                    StatesListeners[i].GameState.RegisterListener(StatesListeners[i]);

                    foreach (var stateListener in (GetComponentsInChildren<IStateListener>(true)))
                    {
                        if (this == ((MonoBehaviour)stateListener).gameObject.GetComponentInParent<GameStateListenerSM>())
                            StatesListeners[i].GameState.RegisterIListener(stateListener);
                    }
                }
            });
        }
        private void OnDestroy()
        {
            for (int i = 0; i < StatesListeners.Count; i++)
            {
                UnRigester(i);
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < StatesListeners.Count; i++)
            {
                if (StatesListeners[i].listenWhenDisabled == false)
                    UnRigester(i);
            }
        }

        private void UnRigester(int i)
        {
            try
            {
                StatesListeners[i].GameState.UnregisterListener(StatesListeners[i]);
                StatesListeners[i].GameState.UnregisterIListeners(GetComponentsInChildren<IStateListener>());
            }
            catch (Exception)
            {
            }
        }
    }

    public interface IStateListener
    {
        void OnEnter(GameStateSM gameState);
        void OnExit(GameStateSM gameState);
    }
}