using StarterAssets;
using System;
using UnityEngine;

public class PetsController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float offSetY;
    [SerializeField] float distanceToMove;
    [SerializeField] Animator animator;
    [SerializeField] GameObject camTarget;
    public bool isGetedPet = false;

    [SerializeField]  ThirdPersonController playerController;


    private Vector3 lastPosition;
    RotatorObj rotator;

    PetID petId;

    private void Awake()
    {
        petId = GetComponent<PetID>();
        rotator = GetComponent<RotatorObj>();


    }
    private void Start()
    {
        GameEvents.OnTimeRewardedComplited += OnTimeRewarded;
    }

    private void OnDisable()
    {
        GameEvents.OnTimeRewardedComplited -= OnTimeRewarded;
    }

    private void OnTimeRewarded(int ID, Enum type)
    {
        switch (type)
        {
            case TypeNagrada.Pet:

                if(petId.ID == ID)
                {
                    camTarget.SetActive(false);
                     isGetedPet = true;
                    rotator.enabled = false;
                }
                    
                break;
        }
    }

    private void Update()
    {
        if (!isGetedPet) return;

        Vector3 toPlayer = playerController.transform.position - transform.position;
        float distance = toPlayer.magnitude;

        if (distance > distanceToMove)
        {
            // 1️⃣ Повернуть к игроку (только по горизонтали)
            Vector3 lookDir = toPlayer;
            lookDir.y = 0;
            transform.rotation = Quaternion.LookRotation(lookDir);

            // 2️⃣ Двигаться вперед к игроку
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            // 3️⃣ Поднять на нужное смещение по Y
            Vector3 pos = transform.position;
            pos.y = playerController.transform.position.y + offSetY;
            transform.position = pos;
        }

        float currentSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;

        // ---- отправляем параметр в Animator ----
        animator.SetFloat("Speed", currentSpeed);

        lastPosition = transform.position;
    }
}
