using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SO.SMachine
{
    [CustomPropertyDrawer(typeof(GameStateSMSO), true)]
    public class GameStateSMSODrawer : IVariableSODrawer
    {
        protected override void VraiableField(Rect position, GUIContent label, IVariableSO variableSO)
        {
            var varRefrence = (GameStateSMSO)variableSO;
            varRefrence.Value = (GameStateSM)EditorGUI.ObjectField(position, label, varRefrence.Value, typeof(GameStateSM), true);
        }
    }
}