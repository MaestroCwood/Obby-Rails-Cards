using System;
using UnityEngine;
using UnityEngine.UI;

public class GameEvents : MonoBehaviour
{
    public static Action<int> OnGenerateCoin;


    public static Action<int> OnDontHaveMany;


    public static Action<int, Enum> OnTimeRewardedComplited;

    public static Action<bool> OnPickUpBrainRot;

    public static Action OnDamageToPlayer;

    public static Action OnAddBrainrot;

}
