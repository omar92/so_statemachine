using System;
using UnityEngine;
using UnityEngine.Events;

namespace SO.SMachine
{
    //[CreateAssetMenu(fileName = "GameState", menuName = "SM/Machine")]
    public class StateMachineSM : MonoBehaviour
    {
        [SerializeField] private GameStateSMSO CurrentGameState;
        //[SerializeField] BoolSO IsGamePused;
        [SerializeField] private GameStateSM startState;
        [SerializeField] private UnityEvent onSwitchState;

        private GameStateSM RuningGameState = null;
        //private bool _IsGamePaused;
        private void Awake()
        {
            CurrentGameState.Value = null;
            RuningGameState = null;
            CurrentGameState.Subscripe(OnStateChange);
        }

        private void OnEnable()
        {
            if (startState != null)
                CurrentGameState.Value = startState;
        }

        private void OnDestroy()
        {
            CurrentGameState.UnSubscripe(OnStateChange);
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
                    if (RuningGameState != null) { onSwitchState.Invoke(); RuningGameState.OnEnter(); } else Debuger.LogWarning("Open Null state");
                });
            });
        }
    }
}