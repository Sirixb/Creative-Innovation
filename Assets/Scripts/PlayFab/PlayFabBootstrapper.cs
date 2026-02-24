using System;
using UnityEngine;

//BootStrapper = Iniciador (Installer)
public class PlayFabBootstrapper : MonoBehaviour
{
    [SerializeField] private PlayFabService playFabService;
    [SerializeField] private PlayerDataManager playerDataManager;

    void Awake()
    {
        playerDataManager.Config(playFabService);
    }
}