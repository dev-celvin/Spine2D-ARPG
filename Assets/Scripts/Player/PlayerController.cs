using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    //状态数组，用来检测不同状态切换不同碰撞体
    public string[] states;
    /*----------------------------------------------------------------------------*/
    public GameObject attack_effect;
    public Animator m_animator;
    public float direction = 1;
	public float move_spped = 5;
	public float jump_speed = 10;
    public float damage_moveSpeed = 10;
	public float sprint_speed = 10;//shift速度
	private ArrayList attackActions;//普通攻击序列
	private ArrayList skillActions;//技能攻击序列
	private int normalAttack_index = 0;
	private int skill_index = 0;
	private bool attack_combo = false;
	private bool toAttack = true;//是否首次进入攻击状态
	private AnimatorStateInfo current_stateInfo;
	private string last_state = "Idle";//默认碰撞体为Idle
    
	
	// Use this for initialization
	void Start () {
		
	}
	
	void Awake() {
		attackActions = new ArrayList();
		skillActions = new ArrayList();
        //初始普通攻击顺序
        int[] temp = new int[]{1,2};
		foreach(int sequence in temp) {
			attackActions.Add(sequence);
		}
		//初始化技能序列
		int[] temp1 = new int[]{1,2,3,4,5,6};
		foreach(int sequence in temp1)
			skillActions.Add(sequence);
	}
	
	// Update is called once per frame
	void Update () {
		//得到水平方向上的控制
	    float moveRate, horizontal = KeyManager.instance.GetMoveDis();
		current_stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        if (current_stateInfo.IsName("Damage") || m_animator.GetBool("hitted")) {
            if(current_stateInfo.IsName("Damage")) m_animator.SetBool("hitted", false);
            transform.Translate(-1*damage_moveSpeed * Time.deltaTime * Vector3.right);
        }
        else {
            if (current_stateInfo.IsTag("NormalAttack") || current_stateInfo.IsTag("SkillAttack"))
            {
                if (current_stateInfo.IsTag("SkillAttack") && (current_stateInfo.IsName("Skill_6"))) transform.Translate(Time.deltaTime * GlobalValue.skill_6_moveSpeed * direction * Vector3.right);
                //判断是否刚进入攻击状态
                if (toAttack)
                {
                    if (current_stateInfo.IsTag("NormalAttack"))
                        playEffect((int)attackActions[(normalAttack_index + attackActions.Count - 1) % attackActions.Count] - 1, 1);
                    else playEffect((int)skillActions[(skill_index + skillActions.Count - 1) % skillActions.Count] - 1, 2);
                    m_animator.SetInteger("attackAction", 0);
                    m_animator.SetInteger("skillAction", 0);
                    toAttack = false;
                }
                if (!attack_combo)
                {
                    if (KeyManager.instance.GetKeyMessage(KeyManager.KeyCode.Combo))
                    {
                        normalAttack_index = 0;
                        m_animator.SetInteger("attackAction", 0);
                        m_animator.SetInteger("skillAction", (int)skillActions[skill_index]);
                        skill_index = (skill_index + 1) % skillActions.Count;
                        attack_combo = true;
                    }
                    else if (KeyManager.instance.GetKeyMessage(KeyManager.KeyCode.Attack))
                    {
                        m_animator.SetInteger("skillAction", 0);
                        m_animator.SetInteger("attackAction", (int)attackActions[normalAttack_index]);
                        normalAttack_index = (normalAttack_index + 1) % attackActions.Count;
                        attack_combo = true;
                    }
                }
            }
            else if (current_stateInfo.IsName("Shift"))
            {
                m_animator.SetBool("shift", false);
                transform.Translate(direction * sprint_speed * Time.deltaTime * Vector3.right);
            }
            else if (current_stateInfo.IsName("IdleStandby")) { }
            else
            {
                toAttack = true;
                attack_combo = false;
                if (current_stateInfo.IsName("Idle"))
                {
                    m_animator.SetBool("normalState", true);
                }
                else if (current_stateInfo.IsTag("NextToAttack"))
                {
                    if (KeyManager.instance.GetKeyMessage(KeyManager.KeyCode.Combo))
                    {
                        m_animator.SetInteger("skillAction", (int)skillActions[skill_index]);
                        skill_index = (skill_index + 1) % skillActions.Count;
                    }
                    else if (KeyManager.instance.GetKeyMessage(KeyManager.KeyCode.Attack))
                    {
                        normalAttack_index = 0;
                        m_animator.SetInteger("attackAction", (int)attackActions[normalAttack_index]);
                        normalAttack_index = (normalAttack_index + 1) % attackActions.Count;
                    }
                    else if (KeyManager.instance.GetKeyMessage(KeyManager.KeyCode.Evade))
                    {
                        m_animator.SetBool("shift", true);
                    }
                }
                //暂时设置更改正常至战斗状态的按键为T
                if (Input.GetKeyDown(KeyCode.T))
                {
                    m_animator.SetBool("normalState", !m_animator.GetBool("normalState"));
                }
                //  if(Input.GetButtonDown("Jump")) {
                //  	m_animator.SetBool("jumping", true);
                //  	
                //  }
                moveRate = Mathf.Abs(horizontal);
                m_animator.SetFloat("moveRate", moveRate);
                if (horizontal > 0)
                {
                    direction = 1;
                    transform.localScale = new Vector3(direction, 1, 1);
                }
                else if (horizontal < 0)
                {
                    direction = -1;
                    transform.localScale = new Vector3(direction, 1, 1);
                }
                transform.Translate(direction * moveRate * move_spped * Time.deltaTime * Vector3.right);
                //transform.Translate(Time.deltaTime*jump_speed*Vector3.up);
            }
        }
    }

    void LateUpdate() {
        current_stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        checkCollider();
    }

    void checkCollider() {
        //碰撞体的切换
        if (!current_stateInfo.IsName(last_state))
        {
            foreach (string state in states)
            {
                if (current_stateInfo.IsName(state))
                {
                    setCollider(state);
                    break;
                }
            }
        }
    }

	//设置当前碰撞体
	void setCollider(string state) {
        if (last_state != "IdleStandby" && last_state != "Shift" && last_state != "Damage") {
            string temp = last_state;
            if (last_state == "Skill_1" || last_state == "Skill_3" || last_state == "Skill_6") temp = "Skill_general";
            GameObject.Find("KarateGirl/Character/Collider/" + temp).SetActive(false);
        }
        last_state = state;
        if (state != "IdleStandby" && state != "Shift" && state!="Damage")
        {
            if (state == "Skill_1" || state == "Skill_3" || state == "Skill_6") state = "Skill_general";
            GameObject.Find("KarateGirl/Character/Collider/" + state).SetActive(true);
        }
	}

    void playEffect(int index, int type) {
        GameObject atkEffect = (GameObject)GameObject.Instantiate(attack_effect, transform.position, attack_effect.transform.rotation);
        atkEffect.GetComponent<AttackEffect>().setEffect(index, type, transform.localScale.x);
        atkEffect.transform.parent = transform.parent;
    }

	//设置技能队列
	public void setComboQueue(int[] queue) {
		skillActions.Clear();
        normalAttack_index = 0;
        skill_index = 0;
		foreach(int sequence in queue) {
			skillActions.Add(sequence);
		}
	}

    public bool isDamage() {
        return m_animator.GetBool("hitted");
    }

    public void setDamage(float direction) {
        transform.localScale = new Vector3(-direction, 1, 1);
        this.direction = -direction;
        m_animator.SetBool("hitted", true);
    }

    public bool checkState(string state) {
        return current_stateInfo.IsName(state);
    }
}
