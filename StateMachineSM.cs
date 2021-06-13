using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
using UnityEngine.Events;

namespace SO.SMachine
{
    //[CreateAssetMenu(fileName = "GameState", menuName = "SM/Machine")]
    public class StateMachineSM : MonoBehaviour
    {
        public GameStateSMSO CurrentGameState;
        public BoolSO IsGamePused;
        public GameStateSM startState;
        private GameStateSM RuningGameState = null;
        public UnityEvent onSwitchState;
        private bool _IsGamePaused;
        private void Awake()
        {
            CurrentGameState.Subscripe(OnStateChange);
            IsGamePused.Subscripe(OnGamePauseChange);
            _IsGamePaused = IsGamePused.Value;
        }

        private void Start()
        {
            if (startState != null)
                CurrentGameState.Value = startState;
        }

        private void OnDestroy()
        {
            CurrentGameState.UnSubscripe(OnStateChange);
            IsGamePused.UnSubscripe(OnGamePauseChange);
        }

        private void OnStateChange(object sender, EventArgs e)
        {
            Z.InvokeEndOfFrame(() =>
            {
                if (RuningGameState != null) RuningGameState.OnExit();
                RuningGameState = null;
                Z.InvokeEndOfFrame(() =>
                {
                    RuningGameState = CurrentGameState.Value;
                    if (RuningGameState != null) RuningGameState.OnEnter(); else Debuger.LogWarning("Open Null state");
                    onSwitchState.Invoke();
                });
            });
        }
        private void OnGamePauseChange(object sender, EventArgs e)
        {
            if (_IsGamePaused != IsGamePused.Value)//check that Value is inverted
            {
                _IsGamePaused = IsGamePused.Value;
                if (_IsGamePaused)
                {
                    RuningGameState.OnPause();
                }
                else
                {
                    RuningGameState.OnUnPause();
                }
            }

        }
    }
}