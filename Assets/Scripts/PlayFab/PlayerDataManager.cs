using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private bool cargarAlIniciar = true;
    [SerializeField] private bool datosCargados = false;
    [SerializeField] public bool guardarEnCheckpoint = true;

    [Header("Data Saved")]
    [SerializeField] private PlayerData playerData;
    public PlayerData PlayerData => playerData;

    [Header("Data Service")]
    [SerializeField] private IDataService _iDataService;

    public event Action OnDatosCargados;

    public void Initialize(IDataService playFabService)
    {
        this._iDataService = playFabService;
    }

    private void Start()
    {
        // Suscribirse al evento de login exitoso
        if (PlayFabLogin.Instancia)
        {
            PlayFabLogin.Instancia.OnLoginExitoso += OnLoginExitoso;
        }
    }

    /// <summary>
    /// Callback cuando el login en PlayFab es exitoso.
    /// Carga todos los datos del jugador.
    /// </summary>
    private void OnLoginExitoso()
    {
        if (cargarAlIniciar && !datosCargados)
        {
            CargarDatosDelJugador();
            datosCargados = true;
        }
    }

    /// <summary>
    /// Carga todos los datos del jugador desde PlayFab.
    /// </summary>
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

        // Cargar posición del PlayerDataManager
        _iDataService.ObtenerDato("checkpointX", valorX =>
        {
            _iDataService.ObtenerDato("checkpointY", valorY =>
            {
                if (float.TryParse(valorX, out float x) && float.TryParse(valorY, out float y))
                {
                    playerData.checkPointPosition = new Vector2(x, y);
                    Debug.Log($"[PlayerDataManager] Posición cargada: ({x}, {y})");
                }
            });
        });

        // Cargar llave
        _iDataService.ObtenerDato("tieneLlave", valor =>
        {
            bool tieneLlave = valor == "true";
            playerData.hasKey = tieneLlave;
            Debug.Log($"[PlayerDataManager] Llave cargada: {tieneLlave}");
        });

        // Cargar salud
        _iDataService.ObtenerDato("salud", valor =>
        {
            if (int.TryParse(valor, out int salud) && salud > 0)
            {
                playerData.currentHealth = salud;
                Debug.Log($"[PlayerDataManager] Salud cargada: {salud}");
            }
            else
            {
                Debug.Log("[PlayerDataManager] No hay salud guardada o es inválida.");
            }
        });

        // Cargar monedas
        _iDataService.ObtenerDato("monedas", valor =>
        {
            if (int.TryParse(valor, out int currency))
            {
                playerData.currentGold = currency;
                Debug.Log($"[PlayerDataManager] Monedas cargadas: {currency}");
            }
        });

        // Cargar estado del dash
        _iDataService.ObtenerDato("tieneDash", valor =>
        {
            bool tieneDash = valor == "true";
            playerData.dashPower = tieneDash;
            Debug.Log($"[PlayerDataManager] Dash cargado: {tieneDash}");
        });

        Debug.Log("[PlayerDataManager] Datos Cargados...");
        OnDatosCargados?.Invoke();
    }


    /// <summary>
    /// Guarda todos los datos del jugador en PlayFab.
    /// </summary>
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

        // Guardar posición actual del checkpoint
        Vector2 checkPointPosition = playerData.checkPointPosition;
        _iDataService.GuardarDato("checkpointX", checkPointPosition.x.ToString());
        _iDataService.GuardarDato("checkpointY", (1.5f + checkPointPosition.y).ToString());

        // Guardar llave
        bool tieneLlave = playerData.hasKey;
        _iDataService.GuardarDato("tieneLlave", tieneLlave ? "true" : "false");

        // Guardar salud
        int saludActual = playerData.currentHealth;
        _iDataService.GuardarDato("salud", saludActual.ToString());

        // Guardar monedas
        int monedasActuales = /*ObtenerMonedasActuales();*/ playerData.currentGold;
        ;
        _iDataService.GuardarDatos(new Dictionary<string, string>
        {
            { "monedas", monedasActuales.ToString() }
        });

        // Guardar estado del dash
        bool tieneDash = playerData.dashPower;
        _iDataService.GuardarDato("tieneDash", tieneDash ? "true" : "false");

        Debug.Log($"[PlayerDataManager] Datos guardados - Salud: {saludActual}, " +
                  $"Monedas: {monedasActuales}, Dash: {tieneDash}, Llave: {tieneLlave}," +
                  $" Checkpoint: {checkPointPosition}");
    }

    // /// <summary>
    // /// Obtiene la salud actual del jugador.
    // /// </summary>
    // private int ObtenerSaludActual()
    // {
    //     if (!playerHealth) return 100;
    //
    //     // Obtener la salud a través de la reflexión o método público
    //     var healthField = typeof(PlayerHealth).GetField("currentHealth",
    //         System.Reflection.BindingFlags.NonPublic |
    //         System.Reflection.BindingFlags.Instance);
    //
    //     if (healthField != null)
    //     {
    //         return (int)healthField.GetValue(playerHealth);
    //     }
    //
    //     return 100; // Valor por defecto
    // }

    // /// <summary>
    // /// Obtiene las monedas actuales del jugador.
    // /// </summary>
    // private int ObtenerMonedasActuales()
    // {
    //     if (playerHealth == null) return 0;
    //
    //     var goldField = typeof(PlayerHealth).GetField("currentGold",
    //         System.Reflection.BindingFlags.NonPublic |
    //         System.Reflection.BindingFlags.Instance);
    //
    //     if (goldField != null)
    //     {
    //         return (int)goldField.GetValue(playerHealth);
    //     }
    //
    //     return 0;
    // }

    private void OnDestroy()
    {
        // Desuscribirse del evento
        if (PlayFabLogin.Instancia != null)
        {
            PlayFabLogin.Instancia.OnLoginExitoso -= OnLoginExitoso;
        }
    }
}