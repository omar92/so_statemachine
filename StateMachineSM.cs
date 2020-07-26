using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
namespace SO.SMachine
{
    //[CreateAssetMenu(fileName = "GameState", menuName = "SM/Machine")]
    public class StateMachineSM : MonoBehaviour
    {
        public GameStateSMSO CurrentGameState;
        public boolSO IsGamePused;
        private GameStateSM RuningGameState = null;
        private bool _IsGamePaused;
        private void Awake()
        {
            CurrentGameState.Subscripe(OnStateChange);
            IsGamePused.Subscripe(OnGamePauseChange);
            _IsGamePaused = IsGamePused.Value;
        }


        private void OnDestroy()
        {
            CurrentGameState.UnSubscripe(OnStateChange);
            IsGamePused.UnSubscripe(OnGamePauseChange);
        }

        private void OnStateChange(object sender, EventArgs e)
        {
            if (RuningGameState != null) RuningGameState.OnExit();
            RuningGameState = CurrentGameState.Value;
            RuningGameState.OnEnter();
        }
        private void OnGamePauseChange(object sender, EventArgs e)
        {
           if(_IsGamePaused != IsGamePused.Value)//check that Value is inverted
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