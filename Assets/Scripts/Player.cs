using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class Player : MonoBehaviourPun, IPunObservable
{

    public PhotonView pView;

    public float moveSpeed = 10;
    public float JumpForce = 800;

    private Vector3 smoothMove;

    private  GameObject sceneCamera;
    public GameObject PlayerCamera;

    void Start()
    {    
        //disable sceneCam an enable PlayerCam
        if(photonView.IsMine)
        {
            PlayerCamera = GameObject.Find("Main Camera");
            sceneCamera.SetActive(false);
            PlayerCamera.SetActive(true); 
        }
       
    }

    void Update()
    {
        //if we are local player processtheInputs && update position with keyBoard
        if(photonView.IsMine)
        {
            ProcessInputs();
        }
        //if we are not the local player so update movements
        else
        {
            SmoothMovement();
        }
    }

    private void SmoothMovement()
    {
        transform.position = Vector3.Lerp(transform.position,smoothMove,Time.deltaTime * 10);
    }

    private void ProcessInputs()
    {
        var move = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    //this method only  work if player moving
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else if(stream.IsReading)
        {
            smoothMove = (Vector3) stream.ReceiveNext();
        }
    }
}
