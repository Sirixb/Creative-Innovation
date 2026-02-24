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
public class PlayFabService : MonoBehaviour, IDataService
{
    private PlayFabLogin _playFabLogin;

    private void Awake()
    {
        _playFabLogin = PlayFabLogin.Instancia;
        
        if (_playFabLogin == null)
        {
            Debug.LogError("[PlayFabService] PlayFabLogin no encontrado. Asegúrese de que existe en la escena.");
            return;
        }

        _playFabLogin.OnLoginExitoso += OnLoginExitoso;
    }

    private void OnLoginExitoso()
    {
        Debug.Log("[PlayFabService] Login exitoso, listo para sincronizar datos.");
    }

    /// <summary>
    /// Guarda datos del jugador en PlayFab.
    /// </summary>
    public void GuardarDatos(Dictionary<string, string> datos, UserDataPermission visibility = UserDataPermission.Private, Action<string> onSuccess = null, Action<string> onError = null)
    {
        if (!VerificarAutenticacion(onError))
            return;

        var solicitud = new UpdateUserDataRequest
        {
            Data = datos,
            Permission = visibility
        };

        PlayFabClientAPI.UpdateUserData(solicitud, 
            resultado => 
            {
                Debug.Log("[PlayFabService] Datos guardados exitosamente.");
                onSuccess?.Invoke("Datos guardados");
            }, 
            error => 
            {
                string mensaje = $"Error al guardar datos: {error.GenerateErrorReport()}";
                Debug.LogError($"[PlayFabService] {mensaje}");
                onError?.Invoke(mensaje);
            });
    }

    /// <summary>
    /// Guarda un dato individual del jugador.
    /// </summary>
    public void GuardarDato(
        string clave, 
        string valor,
        UserDataPermission visibility = UserDataPermission.Private,
        Action<string> onSuccess = null,
        Action<string> onError = null)
    {
        GuardarDatos(new Dictionary<string, string> { { clave, valor } }, visibility, onSuccess, onError);
    }

    /// <summary>
    /// Obtiene los datos del jugador desde PlayFab.
    /// </summary>
    public void ObtenerDatos(
        List<string> claves = null,
        Action<Dictionary<string, UserDataRecord>> callback = null,
        Action<string> onError = null)
    {
        if (!VerificarAutenticacion(onError))
            return;

        var solicitud = new GetUserDataRequest
        {
            PlayFabId = _playFabLogin.PlayFabId,
            Keys = claves
        };

        PlayFabClientAPI.GetUserData(solicitud, 
            resultado => 
            {
                Debug.Log($"[PlayFabService] Datos obtenidos. Cantidad: {resultado.Data?.Count ?? 0}");
                callback?.Invoke(resultado.Data);
            }, 
            error => 
            {
                string mensaje = $"Error al obtener datos: {error.GenerateErrorReport()}";
                Debug.LogError($"[PlayFabService] {mensaje}");
                onError?.Invoke(mensaje);
            });
    }

    // public void GuardarDato(string clave, string valor)
    // {
    //     throw new NotImplementedException();
    // }

    /// <summary>
    /// Obtiene un dato específico del jugador.
    /// </summary>
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
                    Debug.LogWarning($"[PlayFabService] La clave '{clave}' no existe en los datos del jugador.");
                    callback?.Invoke(null);
                }
            }, 
            onError);
    }

    /// <summary>
    /// Incrementa una estadística del jugador (suma al valor actual).
    /// Primero obtiene el valor actual, luego suma el incremento y guarda.
    /// </summary>
    public void IncrementarDato(
        string clave,
        int incremento,
        Action<int> callback = null,
        Action<string> onError = null)
    {
        if (!VerificarAutenticacion(onError))
            return;

        ObtenerEstadisticas(
            estadisticas =>
            {
                int valorActual = estadisticas.ContainsKey(clave) ? estadisticas[clave] : 0;
                int nuevoValor = valorActual + incremento;

                var solicitud = new UpdatePlayerStatisticsRequest
                {
                    Statistics = new List<StatisticUpdate>
                    {
                        new StatisticUpdate
                        {
                            StatisticName = clave,
                            Value = nuevoValor
                        }
                    }
                };

                PlayFabClientAPI.UpdatePlayerStatistics(solicitud,
                    resultado =>
                    {
                        Debug.Log($"[PlayFabService] Estadística '{clave}' actualizada: {valorActual} + {incremento} = {nuevoValor}");
                        callback?.Invoke(nuevoValor);
                    },
                    error =>
                    {
                        string mensaje = $"Error al incrementar dato: {error.GenerateErrorReport()}";
                        Debug.LogError($"[PlayFabService] {mensaje}");
                        onError?.Invoke(mensaje);
                    });
            },
            onError);
    }

    /// <summary>
    /// Establece el valor de una estadística directamente (reemplaza el valor).
    /// </summary>
    public void EstablecerEstadistica(
        string clave,
        int valor,
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
                    Value = valor
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(solicitud,
            resultado =>
            {
                Debug.Log($"[PlayFabService] Estadística '{clave}' establecida a {valor}");
                callback?.Invoke(valor);
            },
            error =>
            {
                string mensaje = $"Error al establecer estadística: {error.GenerateErrorReport()}";
                Debug.LogError($"[PlayFabService] {mensaje}");
                onError?.Invoke(mensaje);
            });
    }

    /// <summary>
    /// Obtiene las estadísticas del jugador desde PlayFab.
    /// </summary>
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
                Debug.Log("[PlayFabService] Estadísticas obtenidas.");
                callback?.Invoke(estadisticas);
            },
            error =>
            {
                string mensaje = $"Error al obtener estadísticas: {error.GenerateErrorReport()}";
                Debug.LogError($"[PlayFabService] {mensaje}");
                onError?.Invoke(mensaje);
            });
    }

    /// <summary>
    /// Obtiene una estadística específica del jugador.
    /// </summary>
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

    private bool VerificarAutenticacion(Action<string> callbackError)
    {
        if (_playFabLogin == null)
        {
            callbackError?.Invoke("PlayFabLogin no encontrado");
            return false;
        }

        if (!_playFabLogin.EstaAutenticado)
        {
            string mensaje = "Jugador no autenticado. Inicie sesión primero.";
            Debug.LogWarning($"[PlayFabService] {mensaje}");
            callbackError?.Invoke(mensaje);
            return false;
        }

        return true;
    }

    private void OnDestroy()
    {
        if (_playFabLogin != null)
        {
            _playFabLogin.OnLoginExitoso -= OnLoginExitoso;
        }
    }
}
