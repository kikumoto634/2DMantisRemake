using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(EnemyControl))]
public class EnemyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnemyControl obj = target as EnemyControl;

        //ã§í 
        obj.type = (EnemyControl.EnemyType)EditorGUILayout.EnumPopup("Type", obj.type);
        obj.WaitTime = EditorGUILayout.FloatField("WaitTime", obj.WaitTime);
        obj._rb = (Rigidbody2D)EditorGUILayout.ObjectField("Rigidbody", obj._rb, typeof(Rigidbody2D), true);


        EditorGUI.indentLevel++;

        //äeéÌInspector
        switch(obj.type)
        {
            case EnemyControl.EnemyType.Wandering:

                obj.wandering._normalSpeed = EditorGUILayout.FloatField("NormalSpeed", obj.wandering._normalSpeed);

                break;

            case EnemyControl.EnemyType.Tracking:

                obj.tracking._normalSpeed = EditorGUILayout.FloatField("NormalSpeed", obj.tracking._normalSpeed);
                obj.tracking._trackingSpeed = EditorGUILayout.FloatField("TrackingSpeed", obj.tracking._trackingSpeed);
                obj.tracking._trackingRange = EditorGUILayout.FloatField("TrackingRange", obj.tracking._trackingRange);

                obj.tracking._player = (GameObject)EditorGUILayout.ObjectField("Player", obj.tracking._player, typeof(GameObject), true);

                break;

            case EnemyControl.EnemyType.Firing:

                obj.firing._normalSpeed = EditorGUILayout.FloatField("NormalSpeed", obj.firing._normalSpeed);
                obj.firing._serachRange = EditorGUILayout.FloatField("TrackingRange", obj.firing._serachRange);

                obj.firing._player = (GameObject)EditorGUILayout.ObjectField("Player", obj.firing._player, typeof(GameObject), true);

                break;
        }

        EditorGUI.indentLevel --;

        EditorUtility.SetDirty(target);
    }
}
