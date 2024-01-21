using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : Entity
{
    [Header("Move area")]
    [SerializeField] private AreaMove areaMove;
    [SerializeField] private Sprite centerToOutGradationImage;
    [SerializeField] private Sprite LeftToRightGradationImage;
    [SerializeField] private Sprite RightToLeftGradationImage;

    /// <summary>
    /// 別のスクリプトから画面をフェードアウトさせたい場合などに使用
    /// </summary>
    [Header("UI")]
    [SerializeField]
    private UI ui;

    [Header("Image")]
    public SpriteRenderer playerRenderer;

    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = 0.2f;
    
    public bool isBusy { get; private set; }
    [Header("Move info")]
    public float moveSpeed = 12f;
    public float jumpForce = 12;
    public float swordReturnImpact;
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    private float defaultDashSpeed;

    public float dashDir { get; private set; }
    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }

    public PlayerFX fx { get; private set; }



    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }

    public PlayerMoveState moveState { get; private set; }

    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }

    public PlayerDashState dashState { get; private set; }

    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; }

    public PlayerAimSwordState aimSword { get; private set; }
    public PlayerCatchSwordState catchSword { get; private set; }

    public PlayerBlackholeState blackHole { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    public PlayerStageMoveState stageMoveState { get; private set; }

    /// <summary>
    /// ゲートの入る方
    /// </summary>
    [SerializeField]
    private GameObject[] _admissionGate;

    private Dictionary<GameObject, int> _admissionGateIndexMap = new Dictionary<GameObject, int>();

    /// <summary>
    /// ゲートの出るほう
    /// </summary>
    [SerializeField]
    private GameObject[] _exitGate;

    public int gateNumber;

    protected override void Awake()
    {
        base.Awake();

        fx = GetComponent<PlayerFX>();

        for(int i = 0; i < _admissionGate.Length; i++)
        {
            _admissionGateIndexMap[_admissionGate[i]] = i;
        }

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");

        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");

        aimSword = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSword = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackHole = new PlayerBlackholeState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Die");

        stageMoveState = new PlayerStageMoveState(this, stateMachine, "Jump", _exitGate);

        Bind();
    }

    /// <summary>
    /// バインド
    /// </summary>
    private void Bind()
    {
        stageMoveState.OnFadeObservable
            .Subscribe(fade =>
            {
                if(fade == "fadeOut")
                    ui.GradationFadeOut(centerToOutGradationImage);
                else
                    ui.GradationFadeIn(centerToOutGradationImage);
            })
            .AddTo(this);
    }

    protected override void Start()
    {
        base.Start();

        skill = SkillManager.instance;

        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;

    }

    protected override void Update()
    {
        if (Time.timeScale == 0)
            return;

        base.Update();

        stateMachine.currentState.Update();

        CheckForDashInput();

        if (Input.GetKeyDown(KeyCode.F) && skill.crystal.crystalUnlocked)
            skill.crystal.CanUseSkill();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Inventory.instance.UseFlask();
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage); ;
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
    }


    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;

        if (skill.dash.dashUnlocked == false)
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);

        }
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }


    protected override void SetupZeroKnockbackPower()
    {
        knockbackPower = new Vector2(0, 0);
    }

    private async void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Gate")
        {
            if(transform.position.x > collision.transform.position.x - 0.1f
                && collision.transform.position.x + 0.1f > transform.position.x)
            {
                if (_admissionGateIndexMap.TryGetValue(collision.gameObject, out int _index))
                {
                    gateNumber = _index;
                }
                else
                    return;

                rb.velocity = Vector2.zero;
                stateMachine.ChangeState(stageMoveState);
            }
        }
        
        else if(collision.tag == "RightArea")
        {
            await ui.GradationFadeOut(LeftToRightGradationImage);
            await UniTask.WaitForSeconds(2);
            transform.position = areaMove.CollisionRightArea(collision.gameObject).Value;

            ui.GradationFadeIn(RightToLeftGradationImage).Forget();
        }
    }
}
