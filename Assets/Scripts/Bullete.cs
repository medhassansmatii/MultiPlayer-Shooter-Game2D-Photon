using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullete : MonoBehaviourPun
{
    public float speed = 10f;
    public float destroyTime = 2f;
        public bool shootLeft = false;

    private void Start()
    {
        StartCoroutine("DestrouBullete");
    }
    IEnumerator DestrouBullete()
    {
        yield return new WaitForSeconds(destroyTime);
        GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);
    }

    

    // Update is called once per frame
    void Update()
    {
        if (!shootLeft)
            transform.Translate(Vector2.right * Time.deltaTime * speed);
        else
        {
            transform.Translate(Vector2.left * Time.deltaTime * speed);
        }
    }
    [PunRPC]
    public void Destroy()
    {
        Destroy(gameObject);
    }
    [PunRPC]
    public void changeDirection()
    {
        shootLeft = true ;
    }
}
