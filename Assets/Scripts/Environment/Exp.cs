using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Exp : MonoBehaviour
{
    [Header("ATRIBUTO INDIVIDUAL")]
    [SerializeField] private int expAmount = 1; // Lượng kinh nghiệm khi ăn Exp

    [SerializeField] AudioSource audioSourceSfx;
    [SerializeField] AudioClip coinSfx;

    private void PickUp()
    {
        // Cập nhật số EXP khi nhặt
        GameManager.instance.AddExp(expAmount);

        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(this.gameObject, 0.3f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PickUp();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Bonus);
    }
}
