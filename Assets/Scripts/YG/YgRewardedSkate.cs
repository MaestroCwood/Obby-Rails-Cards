using UnityEngine;
using YG;

public class YgRewardedSkate : MonoBehaviour
{
    public enum SkateBoardReward
    {
        Skate1, Skate2, Skate3, Skate4, Skate5, Skate6, Skate7, Skate8, Skate9, Skate10,
    }

    public SkateBoardReward Skate;
    [SerializeField] GameObject triggerMesh;
    [SerializeField] SpriteRenderer icoSpriteRenderer;
    bool isActiveRewarded  = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (isActiveRewarded)
            {
                GameEvents.OnActivateSkate?.Invoke(Skate);
            }
            else
            {
                isActiveRewarded = true;
               // triggerMesh.SetActive(false);
                icoSpriteRenderer.enabled = false;
                switch (Skate)
                {
                    case SkateBoardReward.Skate1:
                        YG2.RewardedAdvShow("Skate1");
                        break;
                    case SkateBoardReward.Skate2:
                        YG2.RewardedAdvShow("Skate2");
                        break;
                    case SkateBoardReward.Skate3:
                        YG2.RewardedAdvShow("Skate3");
                        break;
                    case SkateBoardReward.Skate4:
                        YG2.RewardedAdvShow("Skate4");
                        break;
                    case SkateBoardReward.Skate5:
                        YG2.RewardedAdvShow("Skate5");
                        break;
                    case SkateBoardReward.Skate6:
                        YG2.RewardedAdvShow("Skate6");
                        break;
                    case SkateBoardReward.Skate7:
                        YG2.RewardedAdvShow("Skate7");
                        break;
                    case SkateBoardReward.Skate8:
                        YG2.RewardedAdvShow("Skate8");
                        break;
                    case SkateBoardReward.Skate9:
                        YG2.RewardedAdvShow("Skate9");
                        break;
                    case SkateBoardReward.Skate10:
                        YG2.RewardedAdvShow("Skate10");
                        break;
                }                   
                
            }
            
        }

        Debug.Log("OnRewarded state " + Skate);

    }
}
