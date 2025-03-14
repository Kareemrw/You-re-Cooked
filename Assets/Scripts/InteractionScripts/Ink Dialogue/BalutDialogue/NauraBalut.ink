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
// It is an honor to cook for you Option
Protagonist: “Yes, I am the cook for the day. It is an honor to cook you my first meal on this ship. I am unsure if I would taste as good, but let’s hope we don’t find out. Hehe. I would appreciate any tips or advice if you would be kind.”

Pet Translator: “You know what? Why the flip not? You have been rather pleasant. You must know how to boil small protein-packed ingredients.”
    +[I am grateful for the help]
        -> IAGFTH
    +[Thanks, I guess...]
        -> TIG

=== IAGFTH ===
//I am grateful for the help Option
Protagonist: “Well, um, thank you. I am nothing but grateful for the help you have given me.”

Pet Translator: “I guess I have time to spare, so listen up. The texture should be soft and chewy. Also, the addition of a traditional spicy condiment should give it the kick I like. Get cooking.”
-> END

=== TIG ===
//Thank, I guess.. Option
Protagonist: “Uh- you could have specified a bit more. But, thanks, I guess...”

Pet Translator: “Just start cooking. I can not hold your hand if I want to taste something out of this world… ship. Either you make my meal, or you will become my meal. I have no preference. ”
-> END