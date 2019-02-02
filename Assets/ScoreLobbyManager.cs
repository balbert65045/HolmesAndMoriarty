using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreLobbyManager : Photon.PunBehaviour
{
    public void RemakeMatch()
    {
        FindObjectOfType<PhotonLauncher>().MoveToBacktoLobby();
      //  photonView.RPC("RpcRemakeMatch", PhotonTargets.AllViaServer);
    }

   // [PunRPC]
   // void RpcRemakeMatch()
    //{
   //     FindObjectOfType<PhotonLauncher>().MoveToBacktoLobby();
   // }

}
