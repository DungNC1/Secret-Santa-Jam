using UnityEngine;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public List<Skill> availableSkills = new List<Skill>();
    private int selectedSkillIndex = 0;

    void Start()
    {
        PlayerMana playerMana = GetComponent<PlayerMana>();

        // Initialize each skill with the player's mana component
        foreach (Skill skill in availableSkills)
        {
            skill.Initialize(playerMana);
        }

        if (availableSkills.Count > 0)
        {
            SelectSkill(0);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseSelectedSkill();
        }

        // Scroll through skills with number keys (1, 2, 3, etc.)
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SelectSkill(0); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SelectSkill(1); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { SelectSkill(2); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { SelectSkill(3); }
    }

    public void SelectSkill(int index)
    {
        if (index >= 0 && index < availableSkills.Count)
        {
            selectedSkillIndex = index;
            Debug.Log("Selected skill: " + availableSkills[selectedSkillIndex].skillName);
        }
    }

    public void UseSelectedSkill()
    {
        if (availableSkills.Count > 0)
        {
            Skill skillToUse = availableSkills[selectedSkillIndex];
            skillToUse.UseSkill();
        }
    }
}
