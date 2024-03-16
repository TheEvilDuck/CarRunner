using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TimerGate : MonoBehaviour
{
    public event Action<float> passed;
    private bool _passed;
    private float _time;
    public void Init(float time)
    {
        _time = time;
    }
    private void OnTriggerEnter(Collider other) 
    {
        //TODO здесь нужна дополнительная проверка на наличие компонента авто

        if (_passed)
            return;

        passed?.Invoke(_time);
        _passed = true;
    }
}
