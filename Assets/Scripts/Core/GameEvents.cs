using System;
using UnityEngine;
using UnityEngine.UI;

public class GameEvents : MonoBehaviour
{
    public static Action<int> OnGenerateCoin;

    public static Action<ItemRails> OnBuyedRails;

    public static Action<int, Sprite> OnSelectedRails;

    public static Action<int> OnDontHaveMany;

    public static Action<Vector3> OnDestrouBridgeElement;

    public static Action<int, Enum> OnTimeRewardedComplited;

    public static Action OnSplineStarted;
  


}
