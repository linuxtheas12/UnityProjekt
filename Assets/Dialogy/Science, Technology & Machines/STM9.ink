-> main
VAR correct = "Cog in the machine"
VAR chosen = ""

=== main ===
Jimmy the Joker : Which idiom describes a person who feels like an unimportant part of a large system?
    + Well-oiled machine
        ~ chosen = "Well-oiled machine"
         #WRONG_CHOICE
        -> check_choice
        
    + To pull the plug
        ~ chosen = "To pull the plug"
        #WRONG_CHOICE
        -> check_choice
        
    + Cog in the machine
        ~ chosen = "Cog in the machine"
         #CORRECT_CHOICE
        -> check_choice
        

=== check_choice ===
Barbas : {chosen == correct: You might be small, but you are important to me! Correct! | Incorrect. Think about a small gear.}
-> END