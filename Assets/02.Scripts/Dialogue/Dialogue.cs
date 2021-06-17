using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Dialogue
{
    public string _name;

    [TextArea(3, 10)]
    public string[] _senetences;
}
