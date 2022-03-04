using System.Collections;
using UnityEngine;
using UnityEditor;


[CustomEditor (typeof(ItemControl))]
public class ItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ItemControl obj = target as ItemControl;

        //‹¤’Ê
        obj.item = (ItemControl.Item)EditorGUILayout.EnumPopup("Item", obj.item);

        EditorGUI.indentLevel++;    //ƒCƒ“ƒfƒ“ƒg‚ð“ü‚ê‚é

        switch (obj.item)
        {
            case ItemControl.Item.Area:

                obj.itemArea._rangeA = (Transform)EditorGUILayout.ObjectField("RangeA", obj.itemArea._rangeA, typeof(Transform), true);
                obj.itemArea._rangeB = (Transform)EditorGUILayout.ObjectField("RangeB", obj.itemArea._rangeB, typeof(Transform), true);

                obj.itemArea.IsCreate = EditorGUILayout.Toggle("IsCreate", obj.itemArea.IsCreate);

                break;

            case ItemControl.Item.Drop:

                break;
        }

        EditorGUI.indentLevel --;

        EditorUtility.SetDirty(target);
    }
}
