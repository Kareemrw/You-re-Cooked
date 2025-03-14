-> main

=== main ===
Which pokemon do you choose?
    + [Charmander]
        -> choice2("Charmander")
    + [Bulbasaur]
        -> choice2("Bulbasaur")
    + [Squirtle]
        -> choice2("Squirtle")
        
=== chosen(pokemon) ===
Congratulations! You chose {pokemon}!
    ->END
        
=== choice2 (pokemon) ===
You chose {pokemon}!
Do you like this pokemon?
    +Yes
    -> chosen(pokemon)
    +No, I want to choose again
    -> main

-> END