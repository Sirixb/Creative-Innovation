using System;

public interface IPlayerDataManager
{
    void Initialize(IDataService playFabService);
    bool DatosCargados { get; }
    event Action<PlayerData> OnDatosCargados;
    PlayerData PlayerData { get; }
    void GuardarDatosDelJugador();
}