-> main
VAR correct = "To throw a spanner in the works"
VAR chosen = ""

=== main ===
Jimmy the Joker : Which idiom means to do something that prevents a plan from succeeding?
    + To throw a spanner in the works
        ~ chosen = "To throw a spanner in the works"
         #CORRECT_CHOICE
        -> check_choice
        
    + Light years ahead
        ~ chosen = "Light years ahead"
        #WRONG_CHOICE
        -> check_choice
        
    + Cog in the machine
        ~ chosen = "Cog in the machine"
        #WRONG_CHOICE
        -> check_choice
        

=== check_choice ===
Barbas : {chosen == correct: Correct! You really fixed that problem! | Oh no, the plan failed! Try again.}
-> END