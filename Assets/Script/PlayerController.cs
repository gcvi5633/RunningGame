using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayerController : MonoBehaviour {

    static int fallSpeedHash = Animator.StringToHash("FallSpeed");
    static int isGroundHash = Animator.StringToHash("IsGround");
    static int dieHash = Animator.StringToHash("Die");

    private Animator m_anima;
    Rigidbody2D m_rig2D;
    Button m_btn;

    public bool isDie = false;
    public bool isGround;
    public float jumpPower = 10f;
    public float jumpingMultiple = 2f;
    public float dashPower = 10f;
    public float jumpTime = 1f;
    float lineY = 1.5f;
    float origGravity;
    public float fallGravity;

    void Awake() {

    }

    void OnEnable() {
        Debug.Log("Enable Script");
        isDie = false;
    }

    void Start() {
        m_anima = GetComponent<Animator>();
        m_rig2D = GetComponent<Rigidbody2D>();
        origGravity = m_rig2D.gravityScale;
    }

    void Update() {
        m_anima.SetBool(isGroundHash,isGround);
        m_anima.SetFloat(fallSpeedHash,m_rig2D.velocity.y);
        PlayerJump();
        PlayerDash();
        SetGravity();
        PlsyerDie();
    }

    void PlayerDash() {
        dashPower = Mathf.Clamp(dashPower + Time.deltaTime / 20,15,25);
        m_rig2D.velocity = new Vector2(dashPower,m_rig2D.velocity.y);
        GameManager.Instance.SetScore((int)(dashPower / 5));
    }

    void PlayerJump() {
        //開啟 Ground 這一層 LayerMask
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Ground");
        //判斷是否是 Ground
        Vector2 thisPos = transform.position;
        Vector2 posA = new Vector2(thisPos.x + 0.45f,thisPos.y - lineY);
        Vector2 posB = new Vector2(thisPos.x - 0.4f,thisPos.y - lineY);
        isGround = Physics2D.OverlapArea(posA,posB,layerMask);

        if((Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(0)) && isGround) {
            Debug.Log("Jump");
            m_rig2D.velocity = new Vector2(m_rig2D.velocity.x,jumpPower);
            StartCoroutine("Jumping");
        }
    }

    void PlsyerDie() {
        if(!isGround&&m_rig2D.velocity.y == 0f) {
            isDie = true;
        }
        if(isDie) {
            m_rig2D.velocity = Vector2.zero;
            m_anima.SetTrigger(dieHash);
            GameManager.Instance.GameOver();
            this.enabled = false;
        }
    }

    void SetGravity() {
        m_rig2D.gravityScale = (isGround && m_rig2D.velocity.y >= 0) ? origGravity : fallGravity;
    }

    IEnumerator Jumping() {

        float timer = 0;

        while(Input.GetMouseButton(0) && timer < jumpTime) {
            float proportionCompleted = timer / jumpTime;

            Vector2 thieFrameJumpVector = Vector2.Lerp(new Vector2(m_rig2D.velocity.x,jumpPower * jumpingMultiple),
                new Vector2(m_rig2D.velocity.x,0f),
                proportionCompleted);

            m_rig2D.AddForce(thieFrameJumpVector);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator SpeedUp(float time) {
        dashPower += 5f;
        yield return new WaitForSeconds(time);
        dashPower -= 5f;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "TriggerZone") {
            collision.gameObject.SetActive(false);
            GameManager.Instance.SetStage(collision.transform.position.x + 36f);
            
        }
        if(collision.tag == "DamageObject" && !isDie) {
            //m_state = State.Die;
            isDie = true;
        }
        if(collision.tag == "Item") {
            collision.gameObject.SetActive(false);
            StartCoroutine("SpeedUp",5f);
        }
    }
}
