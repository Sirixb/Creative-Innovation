using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManagerJSON : MonoBehaviour, IPlayerDataManager
{
    [Header("Configuración")]
    [SerializeField] private bool cargarAlIniciar = true;
    [SerializeField] private bool datosCargados = false;
    public bool DatosCargados => datosCargados;


    [Header("Data Saved")]
    [SerializeField] private PlayerData playerData;
    public PlayerData PlayerData => playerData;
    
    private IDataService _iDataService;
    public event Action<PlayerData> OnDatosCargados;
        
    [TextArea(5,20)]
    public string textJson;
    private const string PLAYER_DATA_KEY = "PlayerData";

    public void Initialize(IDataService playFabService) => this._iDataService = playFabService;

    private void Start()
    {
        PlayFabLogin.Instancia.OnLoginExitoso += OnLoginExitoso;
    }
    
    private void OnLoginExitoso()
    {
        if (cargarAlIniciar && !datosCargados)
        {
            CargarDatosDelJugador();
        }
    }

    public void CargarDatosDelJugador()
    {
        if (_iDataService == null)
        {
            Debug.LogError("[PlayerDataManagerJSON] PlayFabDataService no disponible.");
            return;
        }
        else if (!playerData)
        {
            Debug.LogError("[PlayerDataManagerJSON] PlayerData no disponible.");
            return;
        }
        Debug.Log("[PlayerDataManagerJSON] Cargando datos del jugador...");
        
        _iDataService.ObtenerDato(PLAYER_DATA_KEY, json =>
        {
            if (!string.IsNullOrEmpty(json))
            {
                PlayerDataDTO dto = JsonUtility.FromJson<PlayerDataDTO>(json);
                AplicarDTO(dto);
                Debug.Log("[PlayerDataManagerJSON] Todos los datos cargados.");
                textJson = json;
                
                datosCargados = true;
                OnDatosCargados?.Invoke(playerData);
            }
            else
            {
                Debug.Log("No hay datos guardados. Usando valores por defecto.");
                playerData.ResetData();
            }
        });
    }

    public void GuardarDatosDelJugador()
    {
        if (_iDataService == null)
        {
            Debug.LogError("[PlayerDataManagerJSON] PlayFabDataService no disponible.");
            return;
        }
        else if (!playerData)
        {
            Debug.LogError("[PlayerDataManagerJSON] PlayerData no disponible.");
            return;
        }
        
        Debug.Log("[PlayerDataManagerJSON] Guardando datos del jugador...");

        PlayerDataDTO dto = CrearDTO();
        string json = JsonUtility.ToJson(dto);
        textJson = json;
        _iDataService.GuardarDato(PLAYER_DATA_KEY, json.ToString());
        
        Debug.Log($"[PlayerDataManagerJSON] Datos guardados : {json}");
    }

    private PlayerDataDTO CrearDTO()
    {
        return new PlayerDataDTO
        {
            checkpointX = playerData.CheckPointPosition.x,
            checkpointY = playerData.CheckPointPosition.y,
            currentHealth = playerData.CurrentHealth,
            currentGold = playerData.CurrentGold,
            dashPower = playerData.DashPower,
            hasKey = playerData.HasKey
        };
    }

    private void AplicarDTO(PlayerDataDTO dto)
    {
        playerData.CheckPointPosition = new Vector2(dto.checkpointX, dto.checkpointY + 1.5f);
        playerData.CurrentHealth = dto.currentHealth;
        playerData.CurrentGold = dto.currentGold;
        playerData.DashPower = dto.dashPower;
        playerData.HasKey = dto.hasKey;
    }
    
    private void OnDestroy()
    {
        if (PlayFabLogin.Instancia != null)
        {
            PlayFabLogin.Instancia.OnLoginExitoso -= OnLoginExitoso;
        }
    }
}

[Serializable]
public class PlayerDataDTO
{
    public float checkpointX;
    public float checkpointY;
    public  int currentHealth;
    public int currentGold; 
    public bool dashPower; 
    public bool hasKey;
}
