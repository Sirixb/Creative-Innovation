using System;
using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine;

public class LocalDataService : MonoBehaviour, IDataService
{
    // TO DO...persistente local necesitarías además serializar el SO a disco, con PlayerPrefs
    //o un archivo JSON en Application.persistentDataPath
    public void GuardarDato(string clave, string valor, UserDataPermission visibility = UserDataPermission.Private,
        Action<string> onSuccess = null, Action<string> onError = null)
    {
        throw new NotImplementedException();
    }

    public void GuardarDatos(Dictionary<string, string> datos, UserDataPermission visibility = UserDataPermission.Private, Action<string> onSuccess = null,
        Action<string> onError = null)
    {
        throw new NotImplementedException();
    }

    public void ObtenerDato(string clave, Action<string> callback, Action<string> onError = null)
    {
        throw new NotImplementedException();
    }
}