using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Battle : MonoBehaviour {

    public static Color[] HPSliderBarColors = new Color[3]{
        new Color32(0,192,0,255),
        new Color32(192,192,0,255),
        new Color32(255,64,64,255)
    };
    
    [Space(20)]

    public Sprite[] genderSprites;
    public Sprite[] statusSprites;


    [Space(20)]

    public Pokemon allyPokemon;
    public Pokemon foePokemon;

    [Space(20)]

    public GameObject foeParent;
    public GameObject foePokemonParent;
    public Text foeNameText;
    public Text foeLevelText;
    public Image foeSprite;
    public Image foeGenderSprite;
    public Image foeCaughtSprite;
    public Image foeStatusSprite;
    public Slider foeHPSlider;

    private Image foeHPSliderBar;

    [Space(20)]

    public GameObject allyParent;
    public GameObject allyPokemonParent;
    public Text allyNameText;
    public Text allyLevelText;
    public Text allyHealthText;
    public Image allySprite;
    public Image allyGenderSprite;
    public Image allyStatusSprite;
    public Slider allyHPSlider;
    public Slider allyXPSlider;

    private Image allyHPSliderBar;

    [Space(20)]


    public static Battle instance;

    DialogueManager dialogueManager;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        dialogueManager = DialogueManager.instance;
        foeHPSliderBar = foeHPSlider.GetComponentInChildren<Image>();
        allyHPSliderBar = allyHPSlider.GetComponentInChildren<Image>();
    }

    private void Update()
    {
        UpdateUI();
    }

    public void InitWildBattle()
    {
        foePokemon.InitPokemon();
        UpdateUI();
        StartBattle();
    }

    public void UpdateUI()
    {
        //FOE

        
        foeHPSlider.minValue = 0;
        foeHPSlider.maxValue = foePokemon.HP;
        //foeHPSlider.value = foePokemon.currentHP;

        if(foeHPSlider.value != foePokemon.currentHP)
        {
            foeHPSlider.value = Mathf.RoundToInt(foeHPSlider.value + Mathf.Sign(foePokemon.currentHP - foeHPSlider.value));

            if (Mathf.Abs(foePokemon.currentHP - foeHPSlider.value) < 3)
            {
                foeHPSlider.value = foePokemon.currentHP;
            }

        }

        float foeHPRatio = foeHPSlider.value / foePokemon.HP;

        if (foeHPRatio > .5f)
        {
            foeHPSliderBar.color = HPSliderBarColors[0];
        }
        else if (foeHPRatio > .2f)
        {
            foeHPSliderBar.color = HPSliderBarColors[1];
        }
        else
        {
            foeHPSliderBar.color = HPSliderBarColors[2];
        }

        if (!foePokemon.CheckForDeath())
        {

            foeNameText.text = foePokemon.GetName();
            foeLevelText.text = "Lv" + foePokemon.level;
            foeSprite.sprite = foePokemon.GetFrontSprite();

            if (!foePokemon.gender.Equals(Gender.NONE))
            { foeGenderSprite.enabled = true; foeGenderSprite.sprite = genderSprites[(int)foePokemon.gender]; }
            else { foeGenderSprite.enabled = false; }

            if (!foePokemon.status.Equals(Status.NONE))
            { foeStatusSprite.enabled = true; foeStatusSprite.sprite = statusSprites[(int)foePokemon.status]; }
            else { foeStatusSprite.enabled = false; }
            
        }
        
        //ALLY

        allyHealthText.text = allyPokemon.currentHP + "/" + allyPokemon.HP;

        
        allyHPSlider.minValue = 0;
        allyHPSlider.maxValue = allyPokemon.HP;
        //allyHPSlider.value = allyPokemon.currentHP;

        if (allyHPSlider.value != allyPokemon.currentHP)
        {
            allyHPSlider.value = allyHPSlider.value + Mathf.Sign(allyPokemon.currentHP - allyHPSlider.value);

            if(Mathf.Abs(allyPokemon.currentHP - allyHPSlider.value) < 3)
            {
                allyHPSlider.value = allyPokemon.currentHP;
            }

        }

        float allyHPRatio = allyHPSlider.value / allyPokemon.HP;

        if (allyHPRatio > .5f)
        {
            allyHPSliderBar.color = HPSliderBarColors[0];
        }
        else if (allyHPRatio > .2f)
        {
            allyHPSliderBar.color = HPSliderBarColors[1];
        }
        else
        {
            allyHPSliderBar.color = HPSliderBarColors[2];
        }

        if (!allyPokemon.CheckForDeath())
        {
            allyNameText.text = allyPokemon.GetName();
            allyLevelText.text = "Lv" + allyPokemon.level;
            
            allySprite.sprite = allyPokemon.GetBackSprite();

            if (!allyPokemon.gender.Equals(Gender.NONE))
            { allyGenderSprite.enabled = true; allyGenderSprite.sprite = genderSprites[(int)allyPokemon.gender]; }
            else { allyGenderSprite.enabled = false; }

            if (!allyPokemon.status.Equals(Status.NONE))
            { allyStatusSprite.enabled = true; allyStatusSprite.sprite = statusSprites[(int)allyPokemon.status]; }
            else { allyStatusSprite.enabled = false; }

            allyXPSlider.minValue = allyPokemon.GetXPMin();
            allyXPSlider.maxValue = allyPokemon.GetXPMax();
            allyXPSlider.value = allyPokemon.XP;
            
        }

    }
    
    public IEnumerator SingleAttack(Pokemon attacker, Pokemon defender, int moveIndex)
    {
        MoveInSet moveInSet = attacker.moveset[moveIndex];
        
        yield return StartCoroutine(SingleAttack(attacker, defender, moveInSet));
    }

    public IEnumerator SingleAttack(Pokemon attacker, Pokemon defender, MoveInSet moveInSet)
    {

        float accuracyCheck = Random.Range(0,100f);

        MoveData move = moveInSet.move;

        dialogueManager.AddDialogue(attacker.GetName() + " used " + move.name + "!");

        yield return StartCoroutine(dialogueManager.WaitForCaughtUpText());

        if (move.accuracy > 0 && accuracyCheck > move.accuracy)
        {
            yield return StartCoroutine(dialogueManager.WaitForCaughtUpTextAndInput());
            dialogueManager.AddDialogue("But it missed!");
            yield break;
        }

        float level = attacker.level;
        float power = move.basePower;

        float ratingMultiplier;
        float burn;

        if(move.isSpecial)
        {
            burn = 1.0f;
            ratingMultiplier = (float)attacker.specialAttack / defender.specialDefense;
        } else
        {
            burn = attacker.status.Equals(Status.BURN) ? .5f : 1.0f;
            ratingMultiplier = (float)attacker.attack / defender.defense;
        }

        float targets = 1.0f;
        float weather = 1.0f;
        float badge = 1.0f;

        float criticalChance = 1f / 16.0f;

        bool criticalHappened = Random.Range(0f, 1f) < criticalChance;

        float critical = (criticalHappened? 2.0f : 1.0f);

        float randomVariation = Random.Range(.85f, 1.0f);

        float STAB = (attacker.IsType(move.type) ? 1.5f : 1.0f);

        float typeEffectiveness = PokemonTypeData.Effectiveness(move.type, defender);

        float other = 1.0f;

        float modifier = targets * weather * badge * critical * randomVariation * STAB * typeEffectiveness * burn * other;

        int damage = Mathf.CeilToInt((2f + ((2f + (2f * level) / 5f) * power * ratingMultiplier)/50f) * modifier);

        if(damage <= 0)
        {
            yield return StartCoroutine(dialogueManager.WaitForCaughtUpTextAndInput());
            dialogueManager.AddDialogue("It didn't effect " + defender.GetName() + "...");
            yield break;
        }


        defender.ModHP(-damage);
        yield return StartCoroutine(WaitForBarsToLoad());

        if (criticalHappened) { yield return StartCoroutine(dialogueManager.WaitForCaughtUpTextAndInput()); dialogueManager.AddDialogue("Critical hit!"); }
        if (typeEffectiveness > 1.0f) { yield return StartCoroutine(dialogueManager.WaitForCaughtUpTextAndInput()); dialogueManager.AddDialogue("It was super effective!"); }
        if (typeEffectiveness < 1.0f) { yield return StartCoroutine(dialogueManager.WaitForCaughtUpTextAndInput()); dialogueManager.AddDialogue("It wasn't very effective..."); }


        dialogueManager.DisplayNextSentence();


    }
    
    private IEnumerator WaitForBarsToLoad()
    {

        while(
            Mathf.Abs(allyHPSlider.value - allyPokemon.currentHP) > 1 ||
            Mathf.Abs(allyXPSlider.value - allyPokemon.XP) > 1 ||
            Mathf.Abs(foeHPSlider.value - foePokemon.currentHP) > 1)
        {

            Debug.Log("HP@" + allyHPSlider.value + " " + allyPokemon.currentHP);
            Debug.Log("XP@" + allyXPSlider.value + " " + allyPokemon.XP);
            Debug.Log("ENEMY HP@" + foeHPSlider.value + " " + foePokemon.currentHP);

            yield return null;
        }

    }

    public void StartBattle()
    {
        StartCoroutine(BattleHandler());
    }

    private int ExperienceCalculation(Pokemon victor, Pokemon loser, bool trainerBattle = false)
    {
        float a = trainerBattle ? 1.5f : 1.0f; //Trainer battle or not
        float b = loser.pokemonData.baseEXP; //Base
        float e = (victor.heldItem != null && victor.heldItem.internalName.Equals("LUCKYEGG")) ? 1.5f : 1.0f; //Lucky egg held?
        float l = loser.level; //Opponent level
        float s = 1f; //Participants

        float exp = (a * b * e * l) / (7 * s);

        return Mathf.RoundToInt(exp);
        
    }

    IEnumerator BattleHandler()
    {
        
        allyPokemon = Player.instance.party.GetFirstNonFaintedPokemon();
        allyHPSlider.value = allyPokemon.HP;
        dialogueManager.ClearDialogue();
        
        dialogueManager.AddDialogue("Foe " + foePokemon.GetName() + " wants to battle!");

        allyParent.transform.localPosition = new Vector3(600, -32, 0);

        dialogueManager.DisplayNextSentence();

        foePokemonParent.transform.localPosition = Vector3.zero;

        for (int i = 600; i >= 0; i-=10) {

            foeParent.transform.localPosition = new Vector3(-i, 96, 0);
            yield return null;
        }

        bool partyDead = false;

        while (Player.instance.party.HasUsablePokemon() && !foePokemon.CheckForDeath())
        {

            bool allyDied = false;
            bool foeDied = false;

            allyParent.transform.localPosition = new Vector3(600, -32, 0);
            allyPokemon = Player.instance.party.GetFirstNonFaintedPokemon();
            dialogueManager.AddDialogue("Go, " + allyPokemon.GetName() + "!");

            yield return dialogueManager.WaitForCaughtUpText();

            allyPokemonParent.transform.localPosition = Vector3.zero;

            for (int i = 500; i >= 0; i -= 10)
            {
                allyParent.transform.localPosition = new Vector3(i, -32, 0);
                yield return null;
            }

            while (!allyPokemon.CheckForDeath() && !foePokemon.CheckForDeath())
            {
                int foeRand = Random.Range(0, foePokemon.GetNumberOfMoves());
                int allyRand = Random.Range(0, allyPokemon.GetNumberOfMoves());

                Pokemon fasterPokemon;
                Pokemon slowerPokemon;

                if (allyPokemon.speed > foePokemon.speed)
                {
                    fasterPokemon = allyPokemon;
                    slowerPokemon = foePokemon;
                }
                else
                {
                    fasterPokemon = foePokemon;
                    slowerPokemon = allyPokemon;
                }

                yield return StartCoroutine(SingleAttack(fasterPokemon, slowerPokemon, foeRand));

                

                int check = CheckIfEitherPokemonDied();

                if (check != 0)
                {
                    allyDied = (check == 1);
                    foeDied = (check == 2);
                    break;
                }

                yield return StartCoroutine(SingleAttack(slowerPokemon, fasterPokemon, allyRand));

                check = CheckIfEitherPokemonDied();

                if (check != 0)
                {
                    allyDied = (check == 1);
                    foeDied = (check == 2);
                    break;
                }

            }

            if (foeDied)
            {
                int exp = ExperienceCalculation(allyPokemon, foePokemon);

                yield return StartCoroutine(dialogueManager.WaitForCaughtUpTextAndInput());

                for (int i = 0; i < 150; i += 10)
                {
                    foePokemonParent.transform.localPosition = new Vector3(0, -i, 0);
                    yield return null;
                }

                dialogueManager.AddDialogue(foePokemon.GetName() + " fainted!");
                yield return StartCoroutine(dialogueManager.WaitForCaughtUpTextAndInput());
                dialogueManager.AddDialogue(allyPokemon.GetName() + " gained " + exp + " experience points!");
                yield return StartCoroutine(dialogueManager.WaitForCaughtUpTextAndInput());
                allyPokemon.ModXP(exp);
                yield return StartCoroutine(WaitForBarsToLoad());
                break;
            }

            if (allyDied)
            {

                yield return StartCoroutine(dialogueManager.WaitForCaughtUpTextAndInput());

                for (int i = 0; i < 150; i += 10)
                {
                    allyPokemonParent.transform.localPosition = new Vector3(0, -i, 0);
                    yield return null;
                }

                dialogueManager.AddDialogue(allyPokemon.GetName() + " fainted!");
                yield return StartCoroutine(dialogueManager.WaitForCaughtUpTextAndInput());
                if(!Player.instance.party.HasUsablePokemon())
                {
                    partyDead = true;
                }
            }

        }

        if(partyDead)
        {
            dialogueManager.AddDialogue(Player.instance.name + " is out of usable Pokemon!");
            dialogueManager.AddDialogue(Player.instance.name + " blacked out!");
            yield return StartCoroutine(dialogueManager.WaitForCaughtUpTextAndInput());
        }

        //dialogueManager.AddDialogue("BATTLE OVER");
        
        PokemonGameManager.instance.EndBattle();
    }
    
    /// <summary>
    /// Returns 2 for foe death, 1 for player death, 0 for neither
    /// </summary>
    /// <returns></returns>
    private int CheckIfEitherPokemonDied()
    {
        if(foePokemon.CheckForDeath())
        {
            return 2;
        }

        if (allyPokemon.CheckForDeath())
        {
            return 1;
        }

        return 0;

    }

    

}
