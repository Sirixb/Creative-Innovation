using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

/// <summary>
/// Gestor de datos del jugador en PlayFab.
/// Permite almacenar, recuperar y actualizar datos asociados al perfil del jugador.
/// </summary>
/// <remarks>
/// PlayFab ofrece dos tipos de datos de usuario:
/// - Datos de usuario (UserData): Visibles y editables por el cliente
/// - Datos de solo lectura (ReadOnlyData): Solo modificables desde el servidor
/// - Datos internos (InternalData): Datos privados no accesibles desde el cliente
/// </remarks>
public class PlayerDataManager : MonoBehaviour
{
    /// <summary>
    /// Referencia al gestor principal de PlayFab
    /// </summary>
    private PlayFabManager _playFabManager;

    /// <summary>
    /// Inicializa el gestor de datos del jugador.
    /// </summary>
    private void Awake()
    {
        _playFabManager = PlayFabManager.Instancia;
        
        if (_playFabManager == null)
        {
            Debug.LogError("[PlayerDataManager] PlayFabManager no encontrado. Asegúrese de que existe en la escena.");
            return;
        }

        // Suscribirse al evento de login exitoso para cargar datos iniciales
        _playFabManager.OnLoginExitoso += OnLoginExitoso;
    }

    /// <summary>
    /// Callback cuando el login es exitoso. Carga los datos del jugador.
    /// </summary>
    private void OnLoginExitoso()
    {
        Debug.Log("[PlayerDataManager] Login exitoso, listo para sincronizar datos.");
    }

    /// <summary>
    /// Guarda datos del jugador en PlayFab.
    /// </summary>
    /// <param name="datos">Diccionario con clave-valor a guardar</param>
    /// <param name="visibilidad">Permisos de lectura (Publico, Privado o Interno)</param>
    /// <param name="onSuccess">Callback cuando la operación es exitosa</param>
    /// <param name="onError">Callback cuando ocurre un error</param>
    public void GuardarDatos(Dictionary<string, string> datos,
        //UserDataVisibility visibility = UserDataVisibility.Private,      //Comentado para evitar error
        UserDataPermission visibility = UserDataPermission.Private, 
        // Action onSuccess = null,
        Action<string> onSuccess = null, //creado para Me evitar error
        Action<string> onError = null)
    {
        if (!VerificarAutenticacion(onError))
            return;

        var solicitud = new UpdateUserDataRequest
        {
            Data = datos,
            Permission = visibility    //Comentado para evitar error
        };

        PlayFabClientAPI.UpdateUserData(solicitud, 
            resultado => 
            {
                Debug.Log("[PlayerDataManager] Datos guardados exitosamente.");
                onSuccess?.Invoke(null);//cree null para que funcionara
            }, 
            error => 
            {
                string mensaje = $"Error al guardar datos: {error.GenerateErrorReport()}";
                Debug.LogError($"[PlayerDataManager] {mensaje}");
                onError?.Invoke(mensaje);
            });
    }

    /// <summary>
    /// Guarda un dato individual del jugador.
    /// </summary>
    /// <param name="clave">Clave del dato</param>
    /// <param name="valor">Valor del dato</param>
    /// <param name="visibilidad">Permisos de lectura</param>
    /// <param name="callback">Callback con el valor obtenido</param>
    /// <param name="onError">Callback de error</param>
    public void GuardarDato(
        string clave, 
        string valor,
        //UserDataVisibility visibilidad = UserDataVisibility.Private,    //Comentado para evitar error
        UserDataPermission visibilidad = UserDataPermission.Private,    //Reemplazo
        Action<string> callback = null,//Comentado para evitar error
        // Action callback = null,
        Action<string> onError = null)
    {
        GuardarDatos(new Dictionary<string, string> { { clave, valor } }, visibilidad, callback, onError);  //Comentado para evitar error
    }

    /// <summary>
    /// Obtiene los datos del jugador desde PlayFab.
    /// </summary>
    /// <param name="claves">Lista de claves a obtener. Null para obtener todas.</param>
    /// <param name="callback">Callback con el diccionario de datos obtenido</param>
    /// <param name="onError">Callback de error</param>
    public void ObtenerDatos(
        List<string> claves = null,
        Action<Dictionary<string, UserDataRecord>> callback = null,
        Action<string> onError = null)
    {
        if (!VerificarAutenticacion(onError))
            return;

        var solicitud = new GetUserDataRequest
        {
            PlayFabId = _playFabManager.PlayFabId,
            Keys = claves
        };

        PlayFabClientAPI.GetUserData(solicitud, 
            resultado => 
            {
                Debug.Log($"[PlayerDataManager] Datos obtenidos. Cantidad: {resultado.Data?.Count ?? 0}");
                callback?.Invoke(resultado.Data);
            }, 
            error => 
            {
                string mensaje = $"Error al obtener datos: {error.GenerateErrorReport()}";
                Debug.LogError($"[PlayerDataManager] {mensaje}");
                onError?.Invoke(mensaje);
            });
    }

    /// <summary>
    /// Obtiene un dato específico del jugador.
    /// </summary>
    /// <param name="clave">Clave del dato a obtener</param>
    /// <param name="callback">Callback con el valor obtenido</param>
    /// <param name="onError">Callback de error</param>
    public void ObtenerDato(
        string clave,
        Action<string> callback,
        Action<string> onError = null)
    {
        ObtenerDatos(new List<string> { clave }, 
            datos => 
            {
                if (datos != null && datos.ContainsKey(clave))
                {
                    callback?.Invoke(datos[clave].Value);
                }
                else
                {
                    Debug.LogWarning($"[PlayerDataManager] La clave '{clave}' no existe en los datos del jugador.");
                    callback?.Invoke(null);
                }
            }, 
            onError);
    }

    /// <summary>
    /// Actualiza un dato numérico específico del jugador (como monedas o puntuación).
    /// Usa operación atómica para evitar condiciones de carrera.
    /// </summary>
    /// <param name="clave">Clave del dato numérico</param>
    /// <param name="incremento">Cantidad a incrementar (puede ser negativa)</param>
    /// <param name="callback">Callback con el nuevo valor</param>
    /// <param name="onError">Callback de error</param>
    public void IncrementarDato(
        string clave,
        int incremento,
        Action<int> callback = null,
        Action<string> onError = null)
    {
        if (!VerificarAutenticacion(onError))
            return;

        var solicitud = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = clave,
                    Value = incremento
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(solicitud,
            resultado =>
            {
                Debug.Log($"[PlayerDataManager] Estadística '{clave}' incrementada en {incremento}");
                // Para obtener el valor actualizado, usar ObtenerEstadisticas
                callback?.Invoke(incremento);
            },
            error =>
            {
                string mensaje = $"Error al incrementar dato: {error.GenerateErrorReport()}";
                Debug.LogError($"[PlayerDataManager] {mensaje}");
                onError?.Invoke(mensaje);
            });
    }

    /// <summary>
    /// Obtiene las estadísticas del jugador desde PlayFab.
    /// </summary>
    /// <param name="callback">Callback con el diccionario de estadísticas</param>
    /// <param name="onError">Callback de error</param>
    public void ObtenerEstadisticas(
        Action<Dictionary<string, int>> callback,
        Action<string> onError = null)
    {
        if (!VerificarAutenticacion(onError))
            return;

        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(),
            resultado =>
            {
                var estadisticas = new Dictionary<string, int>();
                foreach (var stat in resultado.Statistics)
                {
                    estadisticas[stat.StatisticName] = stat.Value;
                }
                Debug.Log("[PlayerDataManager] Estadísticas obtenidas.");
                callback?.Invoke(estadisticas);
            },
            error =>
            {
                string mensaje = $"Error al obtener estadísticas: {error.GenerateErrorReport()}";
                Debug.LogError($"[PlayerDataManager] {mensaje}");
                onError?.Invoke(mensaje);
            });
    }

    /// <summary>
    /// Obtiene una estadística específica del jugador.
    /// </summary>
    /// <param name="nombre">Nombre de la estadística</param>
    /// <param name="callback">Callback con el valor de la estadística</param>
    /// <param name="onError">Callback de error</param>
    public void ObtenerEstadistica(
        string nombre,
        Action<int> callback,
        Action<string> onError = null)
    {
        ObtenerEstadisticas(
            estadisticas => 
            {
                if (estadisticas.ContainsKey(nombre))
                {
                    callback?.Invoke(estadisticas[nombre]);
                }
                else
                {
                    callback?.Invoke(0);
                }
            }, 
            onError);
    }

    /// <summary>
    /// Verifica que el jugador esté autenticado antes de hacer operaciones.
    /// </summary>
    private bool VerificarAutenticacion(Action<string> callbackError)
    {
        if (_playFabManager == null)
        {
            callbackError?.Invoke("PlayFabManager no encontrado");
            return false;
        }

        if (!_playFabManager.EstaAutenticado)
        {
            string mensaje = "Jugador no autenticado. Inicie sesión primero.";
            Debug.LogWarning($"[PlayerDataManager] {mensaje}");
            callbackError?.Invoke(mensaje);
            return false;
        }

        return true;
    }

    private void OnDestroy()
    {
        // Desuscribirse de eventos cuando se destruye el objeto
        if (_playFabManager != null)
        {
            _playFabManager.OnLoginExitoso -= OnLoginExitoso;
        }
    }
}
