using TMPro.Examples;
using UnityEngine;

public class Throw_Stone : MonoBehaviour
{
    public static Throw_Stone Instance;

    void Awake()
    {
        Instance = this;
    }

    public Transform Stone_TS;
    public GameObject Stone;
    public float Stone_Speed;

    public void Create_Stone() 
    {
        GameObject obj = Instantiate (Stone, Stone_TS.position, Stone_TS.rotation);
        obj.GetComponent<Rigidbody>().velocity = obj.transform.forward * Stone_Speed;
        Destroy(obj, 5);
    }

}
