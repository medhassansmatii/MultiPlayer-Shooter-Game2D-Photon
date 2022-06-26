using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class UIHandler : MonoBehaviourPunCallbacks
{

    public TMP_InputField CreateRoomTF;
    public TMP_InputField JoinRoomTF;

    public void OnClick_JoinRoom()
    {
        PhotonNetwork.JoinRoom(JoinRoomTF.text, null);
    }
    public void OnClick_CreateRoom()
    {
        PhotonNetwork.CreateRoom(CreateRoomTF.text, new RoomOptions { MaxPlayers = 4 }, null); 
    }
   

    #region CallBacks

    /// <summary>
    /// When We JointLobby Message will print
    /// </summary>
    public override void OnJoinedRoom()
    {
        print("room Joint Sucess");
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("Join room failed " + returnCode + "message" + message);
    }




    

    #endregion
}
