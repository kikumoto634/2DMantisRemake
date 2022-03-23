using UnityEngine;
using UnityEngine.UI;

public class RadarControl : MonoBehaviour
{
    private GameObject Player = null;
    private GameObject[] Enemys = null;
    private GameObject[] _target = null;

    [SerializeField] private float r = 10.5f;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        _target = GameObject.FindGameObjectsWithTag("RadarTarget");
    }

    private void LateUpdate()
    {
        Debug.Log("ëçêî(ìG)ÅF"+Enemys.Length);
        Debug.Log("ëçêî(çÙ)ÅF"+_target.Length);

        for(int i = 0; i < Enemys.Length; i++)
        {
            if(Enemys[i].transform.position.magnitude <= 30f){
                _target[i].transform.localPosition = (Enemys[i].transform.position - Player.transform.position) * r;
            }
            else
            {
                _target[i].transform.localPosition = new Vector2(500, 500);
            }
        }
    }
}
