
using System;
using System.Collections.Generic;
using PlayFab.ClientModels;

public interface IDataService
{
    // void GuardarDatos(PlayerSaveData data);
    // void CargarDatos(System.Action<PlayerSaveData> onCompletado);
    // void GuardarDato(string clave, string valor);
    void GuardarDato(string clave, string valor, UserDataPermission visibility = UserDataPermission.Private,
        Action<string> onSuccess = null, Action<string> onError = null);

    void GuardarDatos(Dictionary<string, string> datos, UserDataPermission visibility = UserDataPermission.Private,
        Action<string> onSuccess = null, Action<string> onError = null);
    void ObtenerDato(string clave, Action<string> callback, Action<string> onError = null);
    
}