using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opcjonalne_ruchKamery : MonoBehaviour
{

    public Transform gracz;
    private void LateUpdate()
    {


        if (gameObject.name == "Main Camera2")
        {
            transform.position = new Vector3(GameManager.transformP2.position.x, GameManager.transformP2.position.y, -10);
        }
        else if (gameObject.name == "Main Cameraa")
        {
            transform.position = new Vector3(GameManager.transformP1.position.x, GameManager.transformP1.position.y, -10);
        }

        if (gameObject.name == "testCamera")
        {
            transform.position = new Vector3(gracz.position.x, gracz.position.y, -10);
        }
    }
}
