using System;
using UnityEngine;

public class RangedInt : MonoBehaviour {

    [SerializeField] int Min = 0;
    [SerializeField] int Max = 100;

    [SerializeField] string Name = "";

    [SerializeField] bool ModulateOverflow = false;
    [SerializeField] int _Current = 0;
    public int Current {
        get {
            FixRange();
            if (_Current < Min) {
                _Current = ModulateOverflow ? Modulate(_Current, true) : Min;
            }
            if (_Current > Max) {
                _Current = ModulateOverflow ? Modulate(_Current, false) : Max;
            }
            return _Current;
        }
        set {
            FixRange();
            if (value < Min) { 
                value = ModulateOverflow ? Modulate(value, true) : Min; 
            }
            if (value > Max) { 
                value = ModulateOverflow ? Modulate(value, false) : Max; 
            }
            _Current = value;
        }
    }

    private int Modulate(int value, bool modulateDown) {
        int range = Max - Min;
        if (range == 0) { return Max; }
        int modded = (modulateDown ? Min - value : value - Min) % range;
        return modulateDown ? Max - modded : Min + modded;
    }

    private void FixRange() {
        if (Max < Min) {
            int temp = Max;
            Max = Min;
            Min = temp;
        }
    }

    public void SetRange(int min, int max) {
        Max = max;
        Min = min;
        FixRange();
    }

    public void SetMax(int max) {
        Max = max;
        FixRange();
    }

    public int GetMax() {
        FixRange();
        return Max;
    }

    public void SetMin(int min) {
        Min = min;
        FixRange();
    }

    public int GetMin() {
        FixRange();
        return Min;
    }

    public string GetName() {
        return Name;
    }

    public void SetName(string name) { 
        Name = name;
    }
}
