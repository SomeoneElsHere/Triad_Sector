using Content.Shared.Medical.SuitSensor;
using Robust.Shared.Audio;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Medical.CrewMonitoring;

[RegisterComponent]
[Access(typeof(CrewMonitoringConsoleSystem))]
public sealed partial class CrewMonitoringConsoleComponent : Component
{
    /// <summary>
    ///     List of all currently connected sensors to this console.
    /// </summary>
    public Dictionary<string, SuitSensorStatus> ConnectedSensors = new();
    /// <summary>
    ///     After what time sensor consider to be lost.
    /// </summary>
    [DataField("sensorTimeout"), ViewVariables(VVAccess.ReadWrite)]
    public float SensorTimeout = 10f;
    /// <summary>
    ///     Used for the time for the next sound to play.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan? NextSound;
    /// <summary>
    ///     Used as the warning sound for the console.
    /// </summary>
    [DataField]
    public SoundSpecifier? WarningSound;
}
