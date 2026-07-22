using Content.Server.Power.Components;
using Content.Shared.UserInterface;
using Content.Server.Advertise;
using Content.Server.Advertise.Components;
using Content.Shared.Power;
using static Content.Shared.Arcade.SharedSpaceVillainArcadeComponent;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Random;

namespace Content.Server.Arcade.BlackJack;

public sealed partial class BlackJackGameSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedAudioSystem _audioSystem = default!;
    [Dependency] private readonly UserInterfaceSystem _uiSystem = default!;
    [Dependency] private readonly SpeakOnUIClosedSystem _speakOnUIClosed = default!;

    public override void Initialize()
    {
        base.Initialize();
    }

    
}
