using System;
using UnityEngine;

//BootStrapper = Iniciador (Installer)
public class PlayFabBootstrapper : MonoBehaviour
{
    [SerializeField] private PlayFabDataService playFabDataService;
    // [SerializeField] private LocalDataService _localDataService;// To config
    
    [SerializeField] private PlayerDataManager playerDataManager;
    [SerializeField] private PlayerDataManagerJSON playerDataManagerJson;

    void Awake()
    {
        //Registra dato por dato
        // ServiceLocator.Register<IPlayerDataManager>(playerDataManager);
        // playerDataManager.Initialize(playFabDataService);
        
        //Registra un JSON con todos los datos
        ServiceLocator.Register<IPlayerDataManager>(playerDataManagerJson);
        playerDataManagerJson.Initialize(playFabDataService);
    }
}