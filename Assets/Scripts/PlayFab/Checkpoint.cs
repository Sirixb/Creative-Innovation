using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Checkpoint que maneja la sincronización de datos del jugador con PlayFab.
/// Este script se encarga de:
/// - Cargar datos del jugador al iniciar sesión
/// - Guardar datos cuando el jugador toca el checkpoint
/// </summary>
/// <remarks>
/// Para usar este script:
/// 1. Agregar este componente a un GameObject en la escena (puede ser un objeto vacío o el player)
/// 2. Referenciar los componentes del jugador (PlayerHealth, Dash)
/// 3. Crear un Collider2D con trigger para detectar cuando el jugador toca el checkpoint
/// </remarks>
public class Checkpoint : MonoBehaviour
{
    [Header("Referencias del Jugador")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Dash dash;
    [SerializeField] private TMP_Text coinText;

    [Header("Configuración")]
    [SerializeField] private bool cargarAlIniciar = true;
    [SerializeField] private bool guardarEnCheckpoint = true;

    [SerializeField] private PlayerDataManager _playerDataManager;
    [SerializeField] private bool _datosCargados = false;

    private void Awake()
    {
        // Obtener referencia al PlayerDataManager
        _playerDataManager = FindObjectOfType<PlayerDataManager>();

        if (!_playerDataManager)
        {
            Debug.LogError("[Checkpoint] PlayerDataManager no encontrado en la escena.");
            return;
        }
    }

    private void Start()
    {
        // Suscribirse al evento de login exitoso
        if (PlayFabManager.Instancia)
        {
            PlayFabManager.Instancia.OnLoginExitoso += OnLoginExitoso;
        }
        
        if (!playerHealth)
            playerHealth = FindObjectOfType<PlayerHealth>();

        if (!dash)
            dash = FindObjectOfType<Dash>();
        
    }

    /// <summary>
    /// Callback cuando el login en PlayFab es exitoso.
    /// Carga todos los datos del jugador.
    /// </summary>
    private void OnLoginExitoso()
    {
        if (cargarAlIniciar && !_datosCargados)
        {
            CargarDatosDelJugador();
            _datosCargados = true;
        }
    }

    /// <summary>
    /// Carga todos los datos del jugador desde PlayFab.
    /// </summary>
    public void CargarDatosDelJugador()
    {
        if (!_playerDataManager)
        {
            Debug.LogError("[Checkpoint] PlayerDataManager no disponible.");
            return;
        }

        Debug.Log("[Checkpoint] Cargando datos del jugador...");

        // Cargar salud
        _playerDataManager.ObtenerDato("salud", valor =>
        {
            if (playerHealth && int.TryParse(valor, out int salud) && salud > 0)
            {
                playerHealth.SetHealthByPlayFab(salud);
                Debug.Log($"[Checkpoint] Salud cargada: {salud}");
            }
            else
            {
                Debug.Log("[Checkpoint] No hay salud guardada o es inválida.");
            }
        });

        // Cargar monedas
        _playerDataManager.ObtenerDato/*ObtenerEstadistica*/("monedas", valor =>
        {
            if (playerHealth/*coinText*/&& int.TryParse(valor, out int currency))
            {
                // coinText.text = valor.ToString("D3");
                //Refactor to separate currency off health
                playerHealth.SetCurrencyByPlayFab(currency);
                Debug.Log($"[Checkpoint] Monedas cargadas: {currency}");
            }
        });

        // Cargar estado del dash
        _playerDataManager.ObtenerDato("tieneDash", valor =>
        {
            if (dash)
            {
                bool tieneDash = valor == "true";
                dash.enabled = tieneDash;
                Debug.Log($"[Checkpoint] Dash cargado: {tieneDash}");
            }
        });
    }
    
    /// <summary>
    /// Guarda todos los datos del jugador en PlayFab.
    /// </summary>
    public void GuardarDatosDelJugador()
    {
        if (_playerDataManager == null)
        {
            Debug.LogError("[Checkpoint] PlayerDataManager no disponible.");
            return;
        }

        Debug.Log("[Checkpoint] Guardando datos del jugador...");

        // Guardar salud (necesitamos obtener el valor actual)
        int saludActual = ObtenerSaludActual();
        _playerDataManager.GuardarDato("salud", saludActual.ToString());

        // Guardar monedas (usar estadísticas)
        int monedasActuales = ObtenerMonedasActuales();
        _playerDataManager.GuardarDatos(new Dictionary<string, string>
        {
            { "monedas", monedasActuales.ToString() }
        });

        // Guardar estado del dash
        bool tieneDash = dash != null && dash.enabled;
        _playerDataManager.GuardarDato("tieneDash", tieneDash ? "true" : "false");

        Debug.Log($"[Checkpoint] Datos guardados - Salud: {saludActual}, Monedas: {monedasActuales}, Dash: {tieneDash}");
    }

    /// <summary>
    /// Obtiene la salud actual del jugador.
    /// </summary>
    private int ObtenerSaludActual()
    {
        if (!playerHealth) return 100;

        // Obtener la salud a través de la reflexión o método público
        var healthField = typeof(PlayerHealth).GetField("currentHealth", 
            System.Reflection.BindingFlags.NonPublic | 
            System.Reflection.BindingFlags.Instance);

        if (healthField != null)
        {
            return (int)healthField.GetValue(playerHealth);
        }

        return 100; // Valor por defecto
    }

    /// <summary>
    /// Obtiene las monedas actuales del jugador.
    /// </summary>
    private int ObtenerMonedasActuales()
    {
        if (playerHealth == null) return 0;

        var goldField = typeof(PlayerHealth).GetField("currentGold", 
            System.Reflection.BindingFlags.NonPublic | 
            System.Reflection.BindingFlags.Instance);

        if (goldField != null)
        {
            return (int)goldField.GetValue(playerHealth);
        }

        return 0;
    }

    /// <summary>
    /// Detecta cuando el jugador toca el checkpoint.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("[Checkpoint] Player collision");
        if (guardarEnCheckpoint)
        {
            GuardarDatosDelJugador();
            Debug.Log("[Checkpoint] Checkpoint alcanzado - Datos guardados.");
        }
    }

    private void OnDestroy()
    {
        // Desuscribirse del evento
        if (PlayFabManager.Instancia != null)
        {
            PlayFabManager.Instancia.OnLoginExitoso -= OnLoginExitoso;
        }
    }
}
