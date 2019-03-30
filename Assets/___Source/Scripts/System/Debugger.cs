using UnityEngine;
using UnityEngine.UI;

public class Debugger : MonoBehaviour {

	public InputField field;

	public static Debugger instance;

    private Battle battle;

	private bool active = false;

	public bool IsActive() {
		return active;
	}

	private void Awake() {
		instance = this;
	}

    public void Start()
    {
        battle = Battle.instance;
    }

    private void Update() {

		if (Input.GetKeyDown (KeyCode.BackQuote)) {
			active = !active;
		}

		if (active) {
			field.gameObject.SetActive (true);
			field.ActivateInputField ();
		} else {
			field.text = "";
			field.gameObject.SetActive (false);
		}

	}

	public void TerminalInput() {

		string[] plots = field.text.ToLower().Split (new char[] { ' ', '.' }, System.StringSplitOptions.None);


        int x = 0;

        if (plots.Length > 0)
        {
            if (plots.Length > 2)
            {
                if (!System.Int32.TryParse(plots[2], out x)) { }
            }

            switch (plots[0])
            {

                default:
                    Debug.Log("Not a command! Use 'help basic' for commands.");
                    break;
                case "":
                    break;
                case "startbattle":
                    battle.StartBattle();
                    break;
                case "ally":
                    switch (plots[1])
                    {
                        case "modhp":

                            battle.allyPokemon.ModHP(x);
                            break;
                        case "modxp":
                            Debug.Log("mod xp " + battle.allyPokemon.XP);
                            battle.allyPokemon.ModXP(x);
                            break;
                        case "modstatus":
                            battle.allyPokemon.AfflictStatus(plots[2]);
                            break;
                        case "userandommove":
                            Debug.Log(battle.SingleAttack(battle.allyPokemon, battle.foePokemon, Random.Range(0, 4)));
                            break;
                        case "usemove":
                            Debug.Log(battle.SingleAttack(battle.allyPokemon, battle.foePokemon, x));
                            break;
                    }
                    break;
                case "foe":
                    switch (plots[1])
                    {
                        case "modhp":
                            battle.foePokemon.ModHP(x);
                            break;
                        case "modxp":
                            battle.foePokemon.ModXP(x);
                            break;
                        case "modstatus":
                            battle.foePokemon.AfflictStatus(plots[2]);
                            break;
                        case "userandommove":
                            Debug.Log(battle.SingleAttack(battle.foePokemon, battle.allyPokemon, Random.Range(0, 4)));
                            break;
                        case "usemove":
                            Debug.Log(battle.SingleAttack(battle.foePokemon, battle.allyPokemon, x));
                            break;
                    }
                    break;
            }
        }
	}

}