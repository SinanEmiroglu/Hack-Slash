﻿using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int playerNumber;

    private UIPlayerText uiPlayerText;

    public event Action<Character> OnCharacterChanged = delegate { };

    public int PlayerNumber => playerNumber;
    public bool HasController { get { return Controller != null; } }
    public Controller Controller { get; private set; }
    public Character CharacterPrefab { get; set; }

    private void Awake()
    {
        uiPlayerText = GetComponentInChildren<UIPlayerText>();
    }

    public void InitializePlayer(Controller controller)
    {
        Controller = controller;

        gameObject.name = string.Format("Player {0} - {1}", playerNumber, controller.gameObject.name);

        uiPlayerText.HandlePlayerInitialized();
    }

    public void SpawnCharacter()
    {
        var character = CharacterPrefab.Get<Character>(Vector3.zero, Quaternion.identity);
        character.SetController(Controller);
        OnCharacterChanged(character);

        character.OnDie += Character_OnDie;
    }

    private void Character_OnDie(IDie character)
    {
        character.OnDie -= Character_OnDie;
        Destroy(character.gameObject);
        StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(4);
        SpawnCharacter();
    }
}