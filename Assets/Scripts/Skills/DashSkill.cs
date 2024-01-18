using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    [Header("Dash")]
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;
    public bool dashUnlocked { get; private set; }

    [Header("Clone on dash")]
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockButton;
    public bool cloneOnDashUnlocked { get; private set; }

    [Header("Clone on arrival")]
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockButton;
    public bool cloneOnArrivalUnlocked { get; private set; }

    [SerializeField]
    private GameObject _dashEffect;

    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Start()
    {
        base.Start();

        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }

    protected override void CheckUnlock()
    {
        UnlockDash();
        UnlockCloneOnDash();
        UnlockCloneOnArrival();
    }

    public void UnlockDash()
    {

        if(dashUnlockButton.unlocked)
            dashUnlocked = true;
    }

    public void UnlockCloneOnDash()
    {
        if(cloneOnDashUnlockButton.unlocked)
            cloneOnDashUnlocked = true;
    }

    public void UnlockCloneOnArrival()
    {
        if(cloneOnArrivalUnlockButton.unlocked)
            cloneOnArrivalUnlocked = true;
    }


    public void CloneOnDash(Vector3 position, Vector3 rotation)
    {
        if (cloneOnDashUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
            InstantiateDashEffect(position, rotation);
        }
    }

    public void CloneOnArrival()
    {
        if(cloneOnArrivalUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }

    /// <summary>
    /// ダッシュ時のエフェクトを生成する
    /// </summary>
    private void InstantiateDashEffect(Vector3 position, Vector3 rotation)
    {
        Vector3 reverseRotation = new Vector3(rotation.x, rotation.y + 180, rotation.z);
        GameObject effect = Instantiate(_dashEffect, position, Quaternion.Euler(reverseRotation));

        ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();

        if(particleSystem != null)
        {
            Destroy(effect, particleSystem.main.duration);
        }
    }
}
