using UnityEngine;

public class AnimateShowman : MonoBehaviour
{
    [SerializeField] GameObject targetObjAnim;
    [SerializeField] float rangeScale;
    [SerializeField] float speedScale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetObjAnim.LeanScaleY(rangeScale, speedScale).setLoopPingPong(-1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
