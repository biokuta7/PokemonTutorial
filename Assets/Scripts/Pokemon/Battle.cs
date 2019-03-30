using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Battle : MonoBehaviour {

    public static Color[] HPSliderBarColors = new Color[3]{
        new Color32(0,192,0,255),
        new Color32(192,192,0,255),
        new Color32(255,64,64,255)
    };

    public Text dialogueBoxText;

    [Space(2)]

    public Sprite[] genderSprites;
    public Sprite[] statusSprites;


    [Space(2)]

    public Pokemon allyPokemon;
    public Pokemon foePokemon;

    public Text foeNameText;
    public Text foeLevelText;
    public Image foeSprite;
    public Image foeGenderSprite;
    public Image foeCaughtSprite;
    public Image foeStatusSprite;
    public Slider foeHPSlider;

    private Image foeHPSliderBar;

    [Space(2)]

    public Text allyNameText;
    public Text allyLevelText;
    public Text allyHealthText;
    public Image allySprite;
    public Image allyGenderSprite;
    public Image allyStatusSprite;
    public Slider allyHPSlider;
    public Slider allyXPSlider;

    private Image allyHPSliderBar;

    [Space(2)]


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
        allyPokemon.InitPokemon();
        foePokemon.InitPokemon();
        UpdateUI();
        StartBattle();
    }

    private void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        //FOE

        float foeHPRatio = foePokemon.GetHPRatio();
        foeHPSlider.value = foeHPRatio;

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

        float allyHPRatio = allyPokemon.GetHPRatio();
        allyHPSlider.value = allyHPRatio;

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

            allyXPSlider.value = allyPokemon.GetXPRatio();
            
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

        yield return StartCoroutine(WaitForCaughtUpText());

        if (move.accuracy > 0 && accuracyCheck > move.accuracy)
        {
            yield return StartCoroutine(WaitForCaughtUpTextAndInput());
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
            yield return StartCoroutine(WaitForCaughtUpTextAndInput());
            dialogueManager.AddDialogue("It didn't effect " + defender.name + "...");
            yield break;
        }


        yield return StartCoroutine(defender.ModHPCoroutine(-damage));

        if (criticalHappened) { yield return StartCoroutine(WaitForCaughtUpTextAndInput()); dialogueManager.AddDialogue("Critical hit!"); }
        if (typeEffectiveness > 1.0f) { yield return StartCoroutine(WaitForCaughtUpTextAndInput()); dialogueManager.AddDialogue("It was super effective!"); }
        if (typeEffectiveness < 1.0f) { yield return StartCoroutine(WaitForCaughtUpTextAndInput()); dialogueManager.AddDialogue("It wasn't very effective..."); }


        dialogueManager.DisplayNextSentence();


    }

    public void StartBattle()
    {
        StartCoroutine(BattleHandler());
    }



    IEnumerator BattleHandler()
    {
        dialogueManager.ClearDialogue();
        dialogueManager.AddDialogue("Foe " + foePokemon.GetName() + " wants to battle!");
        dialogueManager.DisplayNextSentence();
        
        while (!allyPokemon.CheckForDeath() && !foePokemon.CheckForDeath())
        {
            int foeRand = Random.Range(0, 4);
            int allyRand = Random.Range(0, 4);

            Pokemon fasterPokemon;
            Pokemon slowerPokemon;

            if(allyPokemon.speed > foePokemon.speed)
            {
                fasterPokemon = allyPokemon;
                slowerPokemon = foePokemon;
            } else
            {
                fasterPokemon = foePokemon;
                slowerPokemon = allyPokemon;
            }

            yield return StartCoroutine(SingleAttack(fasterPokemon, slowerPokemon, foeRand));

            Debug.Log("First Check! " + CheckIfEitherPokemonDied());

            if (CheckIfEitherPokemonDied())
            {
                
                break;
            }

            yield return StartCoroutine(SingleAttack(slowerPokemon, fasterPokemon, allyRand));

            Debug.Log("Second Check! " + CheckIfEitherPokemonDied());

            if (CheckIfEitherPokemonDied())
            {
                
                break;
            }

        }

        dialogueManager.AddDialogue("BATTLE OVER");

    }

    private bool CheckIfEitherPokemonDied()
    {
        if(foePokemon.CheckForDeath())
        {
            dialogueManager.AddDialogue(foePokemon.GetName() + " fainted!");
            return true;
        }

        if (allyPokemon.CheckForDeath())
        {
            dialogueManager.AddDialogue(allyPokemon.GetName() + " fainted!");
            return true;
        }

        return false;

    }

    private IEnumerator WaitForCaughtUpText()
    {
        while (!dialogueManager.AllCaughtUp())
            yield return null;
    }

    private IEnumerator WaitForCaughtUpTextAndInput()
    {
        while (!Input.GetButtonDown("Fire1") || !dialogueManager.AllCaughtUp())
            yield return null;
    }

}
