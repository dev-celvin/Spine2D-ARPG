using UnityEngine;
using System.Collections;

public class AttackEffect : MonoBehaviour {

	//扩展技能时需要更新此处
	public static string[] attack_effects = {"atk_1", "atk_2"};
	public static string[] skill_effects = {"skill_1", "skill_2", "skill_3", "skill_4", "skill_5", "skill_6"};
    /*---------------------------------------------------*/

    public static PlayerController m_PlayerController;
	public SkeletonAnimation m_SkeletonAnimation;
    private float direction;
    private bool playCheck = false;

    void Start() {
        if (m_PlayerController == null) {
            m_PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
    }

	// Update is called once per frame
	void Update () {
        if (m_SkeletonAnimation.AnimationName != null)
        {
            if (m_SkeletonAnimation.state.GetCurrent(0).time >= m_SkeletonAnimation.state.GetCurrent(0).endTime)
            {
                GameObject.Destroy(gameObject);
            }
            else
            {
                GetComponent<MeshRenderer>().enabled = true;
                if (!playCheck && m_PlayerController.checkState("Damage")) {
                    playCheck = true;
                    switch (m_SkeletonAnimation.AnimationName) {
                        case "skill_2":
                            if(m_SkeletonAnimation.state.GetCurrent(0).time < 0.25f) GameObject.Destroy(gameObject);
                            break;
                        case "skill_5":
                            if (m_SkeletonAnimation.state.GetCurrent(0).time < 0.8f) GameObject.Destroy(gameObject);
                            break;
                        default:
                            GameObject.Destroy(gameObject);
                            break;
                    }
                }
                if (m_SkeletonAnimation.state.GetCurrent(0).time >= 0.1f)transform.Find("SkeletonUtility-Root/root/effect/bone/[BoundingBox]atk_check").GetComponent<PolygonCollider2D>().enabled = true;
                if (m_SkeletonAnimation.AnimationName == "skill_6")
                    transform.Translate(Time.deltaTime * GlobalValue.skill_6_moveSpeed * direction * Vector3.right);
            }
        }
    }

    //type为1即普通攻击特效类型，type为2即技能攻击特效类型
    public void setEffect(int index, int type, float direction) {
        m_SkeletonAnimation.timeScale = 1;
        if (type == 1)
        {
            m_SkeletonAnimation.AnimationName = attack_effects[index];
        }
        else
        {
            m_SkeletonAnimation.AnimationName = skill_effects[index];
            if (index == 5) m_SkeletonAnimation.timeScale = 1.5f;
;        }
        this.direction = direction;
        transform.localScale = new Vector3(direction, 1, 1);
    }

}
