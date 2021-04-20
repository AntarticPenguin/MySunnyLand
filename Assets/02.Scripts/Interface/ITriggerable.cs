using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interactable를 통해서만 발동되는 오브젝트에만 사용할 것
/// 예: 스위치를 Interaction -> target 오브젝트 발동(Trigger)
/// </summary>
public interface ITriggerable
{
    void Trigger();
}
