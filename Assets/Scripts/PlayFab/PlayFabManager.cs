using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

/// <summary>
/// Gestor principal de PlayFab para el juego Retro Dungeon.
/// Esta clase maneja la conexión, autenticación y configuración inicial con los servidores de PlayFab.
/// </summary>
/// <remarks>
/// Instrucciones de instalación:
/// 1. Instalar el paquete PlayFab SDK desde el Package Manager de Unity
/// 2. Configurar el TitleId en el panel de PlayFab en el Editor
/// 3. Agregar este prefab a la escena principal
/// </remarks>
public class PlayFabManager : MonoBehaviour
{
    /// <summary>
    /// Instancia singleton del gestor de PlayFab
    /// </summary>
    public static PlayFabManager Instancia { get; private set; }

    /// <summary>
    /// Indica si el jugador está actualmente autenticado en PlayFab
    /// </summary>
    public bool EstaAutenticado { get; private set; }

    /// <summary>
    /// ID único del jugador en PlayFab (disponible tras login exitoso)
    /// </summary>
    public string PlayFabId { get; private set; }

    /// <summary>
    /// ID de la entidad del jugador (para operaciones con Entity API)
    /// </summary>
    public string EntityId { get; private set; }

    /// <summary>
    /// Tipo de entidad del jugador (generalmente "title_player_account")
    /// </summary>
    public string EntityType { get; private set; }

    /// <summary>
    /// Evento que se dispara cuando el login es exitoso
    /// </summary>
    public event Action OnLoginExitoso;

    /// <summary>
    /// Evento que se dispara cuando el login falla
    /// </summary>
    public event Action<string> OnLoginFallido;

    private void Awake()
    {
        // Implementación del patrón Singleton para garantizar una única instancia
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instancia = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Iniciar sesión automáticamente al cargar el juego
        IniciarSesion();
    }

    /// <summary>
    /// Inicia sesión en PlayFab usando el identificador único del dispositivo.
    /// Crea una nueva cuenta si no existe una para este dispositivo.
    /// </summary>
    /// <remarks>
    /// Este método usa LoginWithCustomID que es ideal para juegos donde no se requiere
    /// registro/login tradicional. El dispositivo唯一 identificador se usa como ID personalizado.
    /// </remarks>
    public void IniciarSesion()
    {
        // Verificar que el TitleId esté configurado
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            Debug.LogError("PlayFab TitleId no configurado. Configure en PlayFab > Editor Extensions o en el código.");
            OnLoginFallido?.Invoke("TitleId no configurado");
            return;
        }

        // Crear solicitud de login con ID personalizado del dispositivo
        var solicitud = new LoginWithCustomIDRequest
        {
            // Usamos el identificador único del dispositivo como ID personalizado
            // Esto permite que el jugador mantenga su progreso en el mismo dispositivo
            CustomId = SystemInfo.deviceUniqueIdentifier,
            
            // Crear cuenta automáticamente si no existe
            CreateAccount = true
        };

        // Llamar a la API de PlayFab para autenticar
        PlayFabClientAPI.LoginWithCustomID(solicitud, OnLoginExitosoCallback, OnErrorCallback);
    }

    /// <summary>
    /// Callback llamado cuando el login en PlayFab es exitoso.
    /// Guarda los datos del jugador y dispara el evento de login exitoso.
    /// </summary>
    /// <param name="result">Resultado del login contieneniendo tokens y datos del jugador</param>
    private void OnLoginExitosoCallback(LoginResult result)
    {
        PlayFabId = result.PlayFabId;
        EntityId = result.EntityToken.Entity.Id;
        EntityType = result.EntityToken.Entity.Type;
        EstaAutenticado = true;

        Debug.Log($"[PlayFab] Login exitoso. PlayFabId: {PlayFabId}");
        
        OnLoginExitoso?.Invoke();
    }

    /// <summary>
    /// Callback llamado cuando ocurre un error en la comunicación con PlayFab.
    /// Registra el error y dispara el evento de login fallido.
    /// </summary>
    /// <param name="error">Objeto contendo los detalles del error</param>
    private void OnErrorCallback(PlayFabError error)
    {
        string mensajeError = error.GenerateErrorReport();
        Debug.LogError($"[PlayFab] Error de autenticación: {mensajeError}");
        
        EstaAutenticado = false;
        OnLoginFallido?.Invoke(mensajeError);
    }

    /// <summary>
    /// Cierra la sesión del jugador en PlayFab.
    /// </summary>
    public void CerrarSesion()
    {
        if (!EstaAutenticado)
        {
            Debug.LogWarning("[PlayFab] No hay sesión activa para cerrar.");
            return;
        }

        PlayFabClientAPI.UnlinkCustomID(new UnlinkCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier
        }, _ => 
        {
            EstaAutenticado = false;
            PlayFabId = null;
            EntityId = null;
            EntityType = null;
            Debug.Log("[PlayFab] Sesión cerrada exitosamente.");
        }, error => 
        {
            Debug.LogError($"[PlayFab] Error al cerrar sesión: {error.GenerateErrorReport()}");
        });
    }

    /// <summary>
    /// Configura el TitleId de PlayFab programáticamente.
    /// </summary>
    /// <param name="titleId">El ID del título en PlayFab Game Manager</param>
    public void ConfigurarTitleId(string titleId)
    {
        PlayFabSettings.staticSettings.TitleId = titleId;
        Debug.Log($"[PlayFab] TitleId configurado: {titleId}");
    }

    private void OnDestroy()
    {
        // Limpiar la instancia estática cuando el objeto se destruye
        if (Instancia == this)
        {
            Instancia = null;
        }
    }
}
