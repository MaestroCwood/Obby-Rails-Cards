using UnityEngine;

public class RandSkinBot : MonoBehaviour
{
    [SerializeField] GameObject[] skinMeshBot;
   

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }



    private void Start()
    {
        SetSkin();
    }
    void SetSkin()
    {
        for (int i = 0; i < skinMeshBot.Length; i++)
        {
            skinMeshBot[i].SetActive(false);
        }

        int index = RandIndex();
        skinMeshBot[index].SetActive(true);
        var avatarBtot = skinMeshBot[index].GetComponent<AvatarBot>();

        // var avatar = skinMeshBot[index].GetComponent<AvatarBot>();
        animator.avatar = avatarBtot.avatarBot;

    }

    int RandIndex()
    {
        int rand = Random.Range(0, skinMeshBot.Length);
        return rand;
    }
}
