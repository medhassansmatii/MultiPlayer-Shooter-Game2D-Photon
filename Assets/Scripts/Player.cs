using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using TMPro;

public class Player : MonoBehaviourPun, IPunObservable
{

    public PhotonView pView;

    public float moveSpeed = 10;
    public float JumpForce = 800;

    private Vector3 smoothMove;

    private  GameObject sceneCamera;
    public GameObject PlayerCamera;

    public SpriteRenderer spriteRenderer;

    public Rigidbody2D rigidbody;
    public bool isGrounded;

    public TMP_Text nameText;

    public GameObject BulletePrefab;
    public Transform RbulleteSpownPoint;
    public Transform LbulleteSpownPoint;


    void Start()
    {
        PhotonNetwork.SendRate = 20;
        PhotonNetwork.SerializationRate = 15;

        //disable sceneCam an enable PlayerCam
        if(photonView.IsMine)
        {
            nameText.text = PhotonNetwork.NickName; 

            rigidbody = GetComponent<Rigidbody2D>();
            PlayerCamera = GameObject.Find("Main Camera");
           // sceneCamera.SetActive(false);
            PlayerCamera.SetActive(true); 
        }
        else
        {
            nameText.text = pView.Owner.NickName;
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

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            spriteRenderer.flipX = false;
            photonView.RPC("OnDirectionChange_RIGHT", RpcTarget.Others);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            spriteRenderer.flipX = true;
            photonView.RPC("OnDirectionChange_LEFT", RpcTarget.Others);

        }

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        if(Input.GetKeyDown(KeyCode.PageDown))
        {
            shoot(); 
        }
    }

    public void shoot()
    {
       // GameObject bullete = PhotonNetwork.Instantiate(BulletePrefab.name,bulleteSpownPoint.position,Quaternion.identity );
        if(spriteRenderer.flipX == false)
        {
            GameObject bullete = PhotonNetwork.Instantiate(BulletePrefab.name, RbulleteSpownPoint.position, Quaternion.identity);

            //bullete.GetComponent<PhotonView>().RPC("changeDirection", RpcTarget.AllBuffered);

        }else
        {
            GameObject bullete01 = PhotonNetwork.Instantiate(BulletePrefab.name, LbulleteSpownPoint.position, Quaternion.identity);

            bullete01.GetComponent<PhotonView>().RPC("changeDirection", RpcTarget.AllBuffered);
        }
         
    }


    [PunRPC]
    void OnDirectionChange_LEFT()
    {
        spriteRenderer.flipX = true;
    }

    [PunRPC]
    void OnDirectionChange_RIGHT()
    {
        spriteRenderer.flipX = false;

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(photonView.IsMine)
        {
            if (col.gameObject.tag == "Ground")
            {
                isGrounded = true;
            }
        }
      
    }
    void OnCollisionExit2D(Collision2D col)
    { 
        if(photonView.IsMine)
        {
            if (col.gameObject.tag == "Ground")
            {
                isGrounded = false;
            }
        }
        
    }

    void Jump()
    {
        rigidbody.AddForce(Vector2.up * JumpForce);
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
