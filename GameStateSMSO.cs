using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SO;

namespace SO.SMachine
{
    [CreateAssetMenu(fileName = "GameStateSO", menuName = "SM/Variables/GameState")]
    public class GameStateSMSO : VariableSO<GameStateSM>
    {
        public override void SetValue(string value)
        {
            IVariableSO parsedVal;
            if (IVariableSO.TryParse(value, out parsedVal))
            {
                try
                {
                    SetValue((GameStateSMSO) parsedVal);
                }
                catch (Exception) { }
            }
        }
        public  void SetState(GameStateSM value)
        {
            SetValue(value);
        }
        public void ForceSetState(GameStateSM value)
        {
            SetValue(value,true);
        }
        public override string ToString(string format, IFormatProvider formatProvider = null)
        {
            if (String.IsNullOrEmpty(format)) format = "N";
            if (Value != null)
            {
                switch (format)
                {
                    case "N":
                    case "Name":
                        return Value.name;
                    case "ID":
                    case "InstanceID":
                        return Value.GetInstanceID().ToString();

                    default:
                        throw new FormatException(String.Format("The {0} format string is not supported.", format));
                }
            }
            else
            {
                return "Null";
            }
        }
    }
}