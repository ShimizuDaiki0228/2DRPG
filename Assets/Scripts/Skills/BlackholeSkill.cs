using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackholeSkill : Skill
{
    [SerializeField] private UI_SkillTreeSlot blackHoleUnlockButton;
    public bool blackHoleUnloced { get; private set; }
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneCooldown;
    [SerializeField] private float blackholeDuration;
    [Space]
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;


    BlackholeSkillController currentBlackHole;

    /// <summary>
    /// ブラックホール周りに波動のようなエフェクトを出す
    /// </summary>
    [SerializeField]
    private GameObject _blackHoleEffect;

    /// <summary>
    /// ブラックホールが消えた時に生成するエフェクト
    /// </summary>
    [SerializeField]
    private GameObject _blackHoleEndEffect;

    private void UnlockBlackHole()
    {
        if (blackHoleUnlockButton.unlocked)
        {
            blackHoleUnloced = true;
        }
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override async void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);
        GameObject blackHoleAura = Instantiate(_blackHoleEffect, newBlackHole.transform.position, Quaternion.identity);
        EffectDestroy(blackHoleAura);

        Vector3 blackHolePosition = newBlackHole.transform.position;
        //blackHolePosition.y = 0;

        currentBlackHole = newBlackHole.GetComponent<BlackholeSkillController>();

        currentBlackHole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneCooldown, blackholeDuration);

        AudioManager.instance.PlaySFX(18, player.transform);
        AudioManager.instance.PlaySFX(19, player.transform);

        await UniTask.WaitForSeconds(blackholeDuration+0.3f);

        GameObject endEffect = Instantiate(_blackHoleEndEffect, blackHolePosition, Quaternion.identity);
        EffectDestroy(endEffect);
    }

    /// <summary>
    /// エフェクトをライフタイム終わりに破棄する
    /// </summary>
    private void EffectDestroy(GameObject effect)
    {
        ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            Destroy(effect, particleSystem.main.duration);
        }
    }

    protected override void Start()
    {
        base.Start();

        blackHoleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackHole);
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool SkillComplete()
    {
        if (!currentBlackHole)
            return false;

        if(currentBlackHole.playerCanExitState)
        {
            currentBlackHole = null;
            return true;
        }
        return false;
    }

    public float GetBlackHoleRadius()
    {
        return maxSize / 2;
    }

    protected override void CheckUnlock()
    {
        base.CheckUnlock();

        UnlockBlackHole();
    }
}
