﻿using Photon.Pun;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionSetupController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating Player");
        PhotonNetwork.Instantiate(Path.Combine("MultiplayerPrefabs", "PhotonPlayer"), Vector3.zero, Quaternion.identity);
    }
    
}
