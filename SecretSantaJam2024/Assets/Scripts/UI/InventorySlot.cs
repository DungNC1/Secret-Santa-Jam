using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Skill skill; // The skill assigned to this slot
    public Image icon; // Icon to represent the skill
    public Image background; // Background image for highlighting

    public void AddSkill(Skill newSkill)
    {
        skill = newSkill;
        icon.sprite = skill.icon; // Assuming Skill class has an icon property
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        skill = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void Highlight(bool highlight)
    {
        if (highlight)
        {
            // Make the background darker to highlight the slot
            background.color = new Color(0.5f, 0.5f, 0.5f, 1f); // Adjust as needed
        }
        else
        {
            // Reset the background color
            background.color = Color.white;
        }
    }
}
