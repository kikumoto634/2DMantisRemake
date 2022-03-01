using UnityEngine;

public class ItemControl : MonoBehaviour
{
    private GameObject Player = null;


    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
}
