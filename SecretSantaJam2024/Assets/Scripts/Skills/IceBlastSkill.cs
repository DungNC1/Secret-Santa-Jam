using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Ice Blast")]
public class IceBlastSkill : Skill
{
    public GameObject iceBlastEffectPrefab;
    public float freezeDuration = 3f;
    public float laserRange = 5f;
    public LayerMask enemyLayers;

    public override void UseSkill()
    {
        if (playerMana.UseMana(manaCost))
        {
            GameObject newLaser = Instantiate(iceBlastEffectPrefab, playerMana.transform.position + new Vector3(0.7f, 0.1f), Quaternion.identity);
            newLaser.GetComponent<IceBlastEffect>().UpdateLaserRange(laserRange);
        }
        else
        {
            Debug.Log("Not enough mana to cast Ice Blast!");
        }
    }
}

