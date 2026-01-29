-> main
VAR correct = "To break the ice"
VAR chosen = ""

=== main ===
Savage Camel : Which idiom means to say or do something to make people feel more relaxed in a social situation?
    + To break the ice
        ~ chosen = "To break the ice"
        #CORRECT_CHOICE
        -> check_choice
        
    + A lone wolf
        ~ chosen = "A lone wolf"
         #WRONG_CHOICE
        -> check_choice
        
    + To be in the same boat
        ~ chosen = "To be in the same boat"
        #WRONG_CHOICE
        -> check_choice
        

=== check_choice ===
Barbas : {chosen == correct: Brrr, it's getting warmer in here! Correct! | No, that's too cold. Think about frozen water.}
-> END