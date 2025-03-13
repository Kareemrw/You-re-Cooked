//Naura: Balut and MC

-> main

=== main ===
Pet Translator: “ So you are the cook for the day? I wonder if I will get to enjoy your food or yourself. Can’t say I dislike either option.”
    +[Explain your dish to me]
        ->EYDTM
    +[It is an honor to cook for you]
        -> END

=== EYDTM ===
// Explain your dish to me Option
Protagonist: “Uuumm…. Can you explain your dish to me in human terms at all? It is the least you can do.”

Pet Translator: “The least I can do? You will regret that. Go ahead, amaze me. Watching you fail will bring me joy. Just make sure it’s something boiled.”
    +[Start Cooking]
        -> END
    +[Apologize for your attitude]
        -> AFYA

=== AFYA ===
//Apologize for your attitude Option
Protagonist: “Wait. I am sincerely sorry. Can you spare any information at all? ”

Pet Translator: “Pathetic apology. Just remember, it should be soft and chewy. You’ll figure it out.”
-> END

=== IIAHTCFY ===

-> END
-> END