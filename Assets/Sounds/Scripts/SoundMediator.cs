using Gameplay.Garages;
using Gameplay.TimerGates;

public class SoundMediator
{
    private SoundController _soundController;
    private TimerGate[] _gates;
    private Garage[] _garages;

    public SoundMediator(SoundController soundController, TimerGate[] gates, Garage[] garages)
    {
        _soundController = soundController;
        _gates = gates;
        _garages = garages;
        
        foreach(TimerGate gate in gates)
        {
            //gate.passed += _soundController.PlaySFXGate();
        }
    }

}
