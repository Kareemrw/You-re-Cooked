-> main

=== main ===
Pet Translator: “New cook? Naura has gone on and on about you. Sssaid you cooked their meal to perfection. Ready for my meal? I may not be as nice about my review. I have a bit more of a ssspecific diet that can not contain vegetation.”
    +[I am just winging it]
        -> IAJWI
    +[I follow the recipes to perfection]
        -> IFTRTP
        
=== IAJWI ===
//I am just winging it Option
Protagonist: “I would appreciate some information on what your meal consists of. Truth be told, I was winging it the entire time. I could appreciate some pointers, but I don’t want to push you.”

Pet Translator: “ Let’s make thisss quick. Make ssssure to char the exotic protein. I dissslike the texture of the outer gristles. Those are my persssonal preferencesss.”
    +[I will take anything you have to offer]
        -> IWTAYHTO
    +[Understood]
        -> U

=== IWTAYHTO ===
// I will take anything you have to offer Option
Protagonist: “Is there any way I could have any more hints? I will take anything you have to offer. I know I am coming off strong, but any information will help.”

Pet Translator: “I hope I do not regret thissss. I do not like consssuming brainsss because the texture is too ssquissshy for my liking. I prefer my meat tender enough to ssslide off the bone. That isss pretty much the entire recipe. If you fail, I will enjoy cooking and eating you.”
-> END

=== U ===
//Understood Option
Protagonist: “Understood. Thanks for the tips. Anything else you may have been holding back from me?”

Pet Translator: “ I am not as nice as the othersss. I am very picky about the way my meat isss cooked. My preferences have always leaned toward the tender side. I wonder if your flesh cooksss well, too. I guess we will find out sssoon.”
-> END

=== IFTRTP ===
//I follow the recipe to perfection Option
Protagonist: “I mean, have you seen my previous meal? I followed that recipe to perfection. I could use some hints to lead me in the right direction. If you know what I mean.”

Pet Translator: “ You’re doing too much. Not everyone will help you with that attitude. I will warn you to make ssssure to char the protein properly. The outer gristle upsssetss my ssstomach.”

Protagonist: “I will take any help I can get. Please, lend me a….. paw?”

Pet Translator: “I am doing charity work at thissss point. Jussst do not feed me brainsss. I do not appreciate the sssquishy texture. It’ss not the bessst advice, but jussst enough to give you a fighting chance. ”
-> END


-> END