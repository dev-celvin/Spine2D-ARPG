using UnityEngine;
using System.Collections;

public class HiteEffect : MonoBehaviour {

    public SkeletonAnimation m_SkeletonAnimation;
    public MeshRenderer m_MeshRenderer;

    void Update() {
        if (m_SkeletonAnimation.AnimationName != null) {
            m_MeshRenderer.enabled = true;
            if (m_SkeletonAnimation.state.GetCurrent(0).time >= m_SkeletonAnimation.state.GetCurrent(0).endTime) {
                GameObject.Destroy(gameObject);
            }
        }
    }

    public void playHitEffect(int type) {
        string animName;
        switch (type) {
            case 1:
                animName = "defence_hit";
                break;
            case 2:
                animName = "hit_1";
                break;
            case 3:
                animName = "hit_2";
                break;
            case 4:
                animName = "hit_3";
                break;
            default:
                animName = null;
                break;
        }
        m_SkeletonAnimation.AnimationName = animName;
    }
    
}
