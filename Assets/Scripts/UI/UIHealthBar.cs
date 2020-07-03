using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField]
    private Image foreGroundImage;

    private IDie currentCharacter;

    private void Awake()
    {
        var player = GetComponentInParent<Player>();
        player.OnCharacterChanged += Player_OnCharacterChanged;

        gameObject.SetActive(false);
    }

    private void Player_OnCharacterChanged(IDie character)
    {
        currentCharacter = character;
        currentCharacter.OnHealthChanged += HandleHealthChanged;
        currentCharacter.OnDie += CurrentCharacter_OnDie;
        gameObject.SetActive(true);
    }

    private void CurrentCharacter_OnDie(IDie character)
    {
        currentCharacter.OnHealthChanged -= HandleHealthChanged;
        currentCharacter.OnDie -= CurrentCharacter_OnDie;
        gameObject.SetActive(false);
        currentCharacter = null;
    }

    private void HandleHealthChanged(int currentHealth, int maxHealth)
    {
        float pct = (float)currentHealth / (float)maxHealth;
        foreGroundImage.fillAmount = pct;
    }
}