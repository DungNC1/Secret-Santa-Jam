using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public string skillName;
    public int manaCost;
    public float cooldown;
    public Sprite icon;

    protected PlayerMana playerMana;

    public void Initialize(PlayerMana mana)
    {
        playerMana = mana;
    }

    public abstract void UseSkill();
}
