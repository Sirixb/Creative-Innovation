using System;
using UnityEngine;

//BootStrapper = Iniciador (Installer)
public class PlayFabBootstrapper : MonoBehaviour
{
    [SerializeField] private PlayFabDataService playFabDataService;
    [SerializeField] private PlayerDataManager playerDataManager;

    void Awake()
    {
        playerDataManager.Initialize(playFabDataService);
    }
}