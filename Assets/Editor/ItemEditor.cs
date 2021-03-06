using System.Collections;
using UnityEngine;
using UnityEditor;


[CustomEditor (typeof(ItemControl))]
public class ItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ItemControl obj = target as ItemControl;

        //共通
        obj.item = (ItemControl.Item)EditorGUILayout.EnumPopup("Item", obj.item);

        EditorGUI.indentLevel++;    //インデントを入れる

        switch (obj.item)
        {
            case ItemControl.Item.Area:

                obj.itemArea.IsCreate = EditorGUILayout.Toggle("IsCreate", obj.itemArea.IsCreate);

                break;

            case ItemControl.Item.Drop:

                break;
        }

        EditorGUI.indentLevel --;

        EditorUtility.SetDirty(target);
    }
}
