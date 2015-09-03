using UnityEngine;
using System.Collections;

public class HitInspector : MonoBehaviour {

    private static PlayerController m_PlayerController;
    public GameObject hitEffect;

    void Awake()
    {
        if (m_PlayerController == null) {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                m_PlayerController = player.GetComponent<PlayerController>();
            }
            else Debug.Log("GameObject(Player) not found in HitInspector.cs:start()!");
        }
    }

    void OnCollisionEnter2D(Collision2D collision2d) {
        if (collision2d.gameObject.tag == "EnemyAttack")
        {
            if (m_PlayerController != null)
            {
                if (!m_PlayerController.isDamage())
                {
                    m_PlayerController.setDamage(collision2d.gameObject.GetComponent<AttackWidget>().m_transform.localScale.x);
                    GameObject tmp_hitEffect = (GameObject)Instantiate(hitEffect, collision2d.contacts[0].point, hitEffect.transform.rotation);
                    tmp_hitEffect.GetComponent<HiteEffect>().playHitEffect(4);
                }
            }
            else Debug.Log("PlayerController not found in HitInspector.cs:OnTriggerEnter2D");
        }
    }
}
