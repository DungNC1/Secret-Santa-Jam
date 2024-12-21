using UnityEngine;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public List<Skill> availableSkills = new List<Skill>();
    public List<InventorySlot> inventorySlots = new List<InventorySlot>(); // References to inventory slots in the scene

    private int selectedSkillIndex = 0;
    private PlayerMana playerMana;

    void Start()
    {
        playerMana = GetComponent<PlayerMana>();

        // Initialize each skill with the player's mana component
        foreach (Skill skill in availableSkills)
        {
            skill.Initialize(playerMana);
        }

        // Assign available skills to inventory slots
        for (int i = 0; i < availableSkills.Count && i < inventorySlots.Count; i++)
        {
            inventorySlots[i].AddSkill(availableSkills[i]);
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
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectSkillFromSlot(i);
            }
        }
    }

    public void SelectSkillFromSlot(int index)
    {
        if (index >= 0 && index < inventorySlots.Count)
        {
            InventorySlot slot = inventorySlots[index];
            if (slot.skill != null)
            {
                // Highlight the selected slot and unhighlight others
                HighlightSelectedSlot(index);
                selectedSkillIndex = index;
                Debug.Log("Selected skill from slot: " + slot.skill.skillName);
            }
        }
    }

    public void SelectSkill(int index)
    {
        if (index >= 0 && index < availableSkills.Count)
        {
            HighlightSelectedSlot(index);
            selectedSkillIndex = index;
            Debug.Log("Selected skill: " + availableSkills[selectedSkillIndex].skillName);
        }
    }

    public void UseSelectedSkill()
    {
        if (selectedSkillIndex >= 0 && selectedSkillIndex < inventorySlots.Count)
        {
            Skill skillToUse = inventorySlots[selectedSkillIndex].skill;
            if (skillToUse != null)
            {
                skillToUse.UseSkill();
            }
        }
    }

    private void HighlightSelectedSlot(int index)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].Highlight(i == index);
        }
    }
}
