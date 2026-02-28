using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour, IPlayerDataManager
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

    public void Initialize(IDataService playFabService) => this._iDataService = playFabService;

    private void Start()
    {
        if (PlayFabLogin.Instancia)
        {
            PlayFabLogin.Instancia.OnLoginExitoso += OnLoginExitoso;
        }
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
            Debug.LogError("[PlayerDataManager] PlayFabDataService no disponible.");
            return;
        }
        else if (!playerData)
        {
            Debug.LogError("[PlayerDataManager] PlayerData no disponible.");
            return;
        }

        Debug.Log("[PlayerDataManager] Cargando datos del jugador...");
        
        //Cargas
        int cargasPendientes = 5;
        Action OnTodasCargasCompletadas = () =>
        {
            cargasPendientes--;
            if (cargasPendientes <= 0)
            {
                Debug.Log("[PlayerDataManager] Todos los datos cargados.");
                datosCargados = true;
                OnDatosCargados?.Invoke(playerData);
            }
        };

        _iDataService.ObtenerDato("checkpointX", valorX =>
        {
            _iDataService.ObtenerDato("checkpointY", valorY =>
            {
                if (float.TryParse(valorX, out float x) && float.TryParse(valorY, out float y))
                {
                    playerData.CheckPointPosition = new Vector2(x, y + 1.5f);
                    Debug.Log($"[PlayerDataManager] Posición cargada: ({x}, {y})");
                }

                OnTodasCargasCompletadas();
            });
        });

        _iDataService.ObtenerDato("tieneLlave", valor =>
        {
            playerData.HasKey = valor == "true";
            Debug.Log($"[PlayerDataManager] Llave cargada: {playerData.HasKey}");
            OnTodasCargasCompletadas();
        });

        _iDataService.ObtenerDato("salud", valor =>
        {
            if (int.TryParse(valor, out int salud) && salud > 0)
            {
                playerData.CurrentHealth = salud;
                Debug.Log($"[PlayerDataManager] Salud cargada: {salud}");
            }

            OnTodasCargasCompletadas();
        });

        _iDataService.ObtenerDato("monedas", valor =>
        {
            if (int.TryParse(valor, out int currency))
            {
                playerData.CurrentGold = currency;
                Debug.Log($"[PlayerDataManager] Monedas cargadas: {currency}");
            }

            OnTodasCargasCompletadas();
        });

        _iDataService.ObtenerDato("tieneDash", valor =>
        {
            playerData.DashPower = valor == "true";
            Debug.Log($"[PlayerDataManager] Dash cargado: {playerData.DashPower}");
            OnTodasCargasCompletadas();
        });
    }

    public void GuardarDatosDelJugador()
    {
        if (_iDataService == null)
        {
            Debug.LogError("[PlayerDataManager] PlayFabDataService no disponible.");
            return;
        }
        else if (playerData == null)
        {
            Debug.LogError("[PlayerDataManager] PlayerData no disponible.");
            return;
        }

        Debug.Log("[PlayerDataManager] Guardando datos del jugador...");

        Vector2 checkPointPosition = playerData.CheckPointPosition;
        _iDataService.GuardarDato("checkpointX", checkPointPosition.x.ToString());
        _iDataService.GuardarDato("checkpointY",  checkPointPosition.y.ToString());

        bool tieneLlave = playerData.HasKey;
        _iDataService.GuardarDato("tieneLlave", tieneLlave ? "true" : "false");

        int saludActual = playerData.CurrentHealth;
        _iDataService.GuardarDato("salud", saludActual.ToString());

        int monedasActuales = playerData.CurrentGold;
        _iDataService.GuardarDatos(new Dictionary<string, string>
        {
            { "monedas", monedasActuales.ToString() }
        });

        bool tieneDash = playerData.DashPower;
        _iDataService.GuardarDato("tieneDash", tieneDash ? "true" : "false");

        Debug.Log(
            $"[PlayerDataManager] Datos guardados - Salud: {saludActual}, Monedas: {monedasActuales}, Dash: {tieneDash}, Llave: {tieneLlave}");
    }

    private void OnDestroy()
    {
        if (PlayFabLogin.Instancia != null)
        {
            PlayFabLogin.Instancia.OnLoginExitoso -= OnLoginExitoso;
        }
    }
}