using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonGameManager : MonoBehaviour
{

    public enum GameState
    {
        OVERWORLD,
        BATTLE,
        MENU,
        CUTSCENE
    }

    public Camera pixelPerfectCamera;
    public Camera battleCamera;
    public GameObject overworld;

    Battle battle;
    public static PokemonGameManager instance;

    private Material transitionMaterial;

    public GameState gameState;
    private Color defaultTransitionColor;

    private PlayerController player;
    private DialogueManager dialogueManager;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        battle = Battle.instance;
        player = PlayerController.instance;
        dialogueManager = DialogueManager.instance;
        transitionMaterial = pixelPerfectCamera.GetComponent<RenderTextureToScreen>().TransitionMaterial;
        transitionMaterial.SetFloat("_Cutoff", 0f);
        defaultTransitionColor = transitionMaterial.color;
    }

    public void TraversePortal(Portal start)
    {

        Portal destination = start.target;

        StartCoroutine(TraversePortalCoroutine(start, destination));
    }

    private IEnumerator TraversePortalCoroutine(Portal start, Portal destination)
    {
        gameState = GameState.CUTSCENE;
        start.OnEntered();
        
        yield return StartCoroutine(Fade(true));
        player.SetRunning(false);
        player.transform.position = destination.transform.position;
        player.FaceDirection(destination.facingDirectionExit);
        yield return StartCoroutine(Fade(false));

        player.MoveInDirection(destination.facingDirectionExit);

        destination.OnExited();

        yield return new WaitForSeconds(.1f);

        gameState = GameState.OVERWORLD;
    }

    public void StartWildBattle(PokemonData pokemon, int level)
    {
        StartCoroutine(StartWildBattleCoroutine(pokemon, level));
    }

    private IEnumerator StartWildBattleCoroutine(PokemonData pokemon, int level)
    {
        if(gameState != GameState.OVERWORLD)
        { yield break; }

        gameState = GameState.BATTLE;

        yield return StartCoroutine(Fade(true, true));


        battleCamera.enabled = true;
        //overworld.SetActive(false);
        battle.foePokemon.pokemonData = pokemon;
        battle.foePokemon.level = level;
        battle.InitWildBattle();

        yield return StartCoroutine(Fade(false));
    }

    IEnumerator Fade(bool fadeOut, bool flashes = false)
    {

        if(flashes)
        {

            yield return new WaitForSeconds(.3f);

            transitionMaterial.SetFloat("_Fade", 1f);
            transitionMaterial.SetFloat("_Cutoff", 1f);
            transitionMaterial.color = Color.white;

            for(int i = 0; i < 30; i++)
            {
                transitionMaterial.SetFloat("_Fade", Mathf.Clamp01(Mathf.Sin(i/3f)));
                yield return new WaitForEndOfFrame();
            }
            transitionMaterial.color = defaultTransitionColor;
            transitionMaterial.SetFloat("_Fade", 1f);
            transitionMaterial.SetFloat("_Cutoff", 0f);

        }

        yield return new WaitForSeconds(.2f);

        int smoothness = 30;
        
        for (int i = 0; i < smoothness; i++)
        {

            float cutoff = fadeOut ? i / (1.0f * smoothness) : (1 - i / (1.0f * smoothness));

            transitionMaterial.SetFloat("_Cutoff", cutoff);
            yield return new WaitForEndOfFrame();
        }

        transitionMaterial.SetFloat("_Cutoff", fadeOut? 1f : 0f);

    }

    public void EndBattle()
    {
        StartCoroutine(EndBattleCoroutine());
    }

    IEnumerator EndBattleCoroutine()
    {
        yield return StartCoroutine(Fade(true));

        battleCamera.enabled = false;
        //overworld.SetActive(true);
        dialogueManager.SetDisplay(false);

        player.StartMovement();

        yield return StartCoroutine(Fade(false));

        gameState = GameState.OVERWORLD;
    }

    public void StartDialogue(Dialogue d, string colorString = "")
    {
        StartCoroutine(DialogueCoroutine(d, colorString));
    }

    IEnumerator DialogueCoroutine(Dialogue d, string colorString = "")
    {
        dialogueManager.SetColor(colorString);
        dialogueManager.ClearDialogue();
        dialogueManager.AddDialogue(d);
        
        gameState = GameState.CUTSCENE;



        dialogueManager.DisplayNextSentence();

        yield return dialogueManager.WaitForCaughtUpTextAndInput();

        gameState = GameState.OVERWORLD;

        dialogueManager.SetDisplay(false);

    }

}
