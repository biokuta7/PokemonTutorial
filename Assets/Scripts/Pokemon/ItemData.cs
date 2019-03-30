using UnityEngine;

[System.Serializable]
public class ItemData : ScriptableObject {

    public enum ItemType
    {
        NONE,                       // 0 - None
        ITEM,                       // 1 - Items
        MEDICINE,                   // 2 - Medicine
        POKEBALL,                   // 3 - Poké Balls
        TM,                         // 4 - TMs & HMs
        BERRIES,                    // 5 - Berries
        MAIL,                       // 6 - Mail
        BATTLE,                     // 7 - Battle Items
        KEY                         // 8 - Key Items
    }

    public enum OverworldType
    {
        NONE,                       // 0 - The item cannot be used outside of battle.
        ONPOKEMON_SINGLEUSE,        // 1 - The item can be used on a Pokémon, and disappears after use(e.g.Potions, Elixirs).
                                    //     The party screen will appear when using this item, allowing you to choose 
                                    //     the Pokémon to use it on. Not for TMs and HMs, though.
        NOTONPOKEMON,               // 2 - The item can be used out of battle, but it isn't used on a Pokémon (e.g. Repel, Escape Rope, usable Key Items).
        TM,                         // 3 - The item is a TM. It teaches a move to a Pokémon, and disappears after use (unless TMs are set to infinite use).
        HM,                         // 4 - The item is a HM. It teaches a move to a Pokémon, but does not disappear after use.
        ONPOKEMON_PERM              // 5 - The item can be used on a Pokémon (like 1), but it does not disappear after use (e.g. Poké Flute).
    }

    public enum BattleType
    {
        NONE,                       // 0 - The item cannot be used in battle.
        ONPOKEMON_SINGLEUSE,        // 1 - The item can be used on one of your party Pokémon, and disappears after use(e.g.Potions, Elixirs). The party screen will appear when using this item, allowing you to choose the Pokémon to use it on.
        DIRECT_SINGLEUSE,           // 2 - The item is a Poké Ball, is used on the active Pokémon you are choosing a command for (e.g.X Accuracy), or is used directly (e.g.Poké Doll).
        ONPOKEMON_PERM,             // 3 - The item can be used on a Pokémon (like 1), but does not disappear after use (e.g. Poké Flute).
        DIRECT_PERMANENT            // 4 - The item can be used directly, but does not disappear after use.
    }

    public enum SpecialItemType
    {
        NONE,                       // 0 - The item is none of the items below.
        MAIL,                       // 1 - The item is a Mail item.
        MAIL_SPECIAL,               // 2 - The item is a Mail item, and the images of the holder and two other party Pokémon appear on the Mail.
        SNAG,                       // 3 - The item is a Snag Ball(i.e.it can capture enemy trainers' Shadow Pokémon).
        POKEBALL,                   // 4 - The item is a Poké Ball item.
        PLANTABLE_BERRY,            // 5 - The item is a berry that can be planted.
        KEY_ITEM,                   // 6 - The item is a Key Item.
        EVOLUTION_STONE,            // 7 - The item is an evolution stone.
        FOSSIL,                     // 8 - The item is a fossil that can be revived.
        APRICORN,                   // 9 - The item is an Apricorn that can be converted into a Poké Ball.
        ELEMENTAL_GEM,              // 10 - The item is an elemental power-raising Gem.
        MULCH,                      // 11 - The item is mulch that can be spread on berry patches.
        MEGA_STONE                  // 12 - The item is a Mega Stone.This does NOT include the Red/Blue Orbs.
    }

    public int ID;
    public new string name;
    public string internalName;
    public string description;

    public Sprite sprite;
    
    public int price;

    public ItemType itemType;
    public OverworldType overworldUsabilityID;
    public BattleType battleUsabilityID;
    public SpecialItemType specialItemID;

    /// <summary>
    /// Only for HMs and TMs.
    /// </summary>
    public MoveData TMmoveToLearn;

}
