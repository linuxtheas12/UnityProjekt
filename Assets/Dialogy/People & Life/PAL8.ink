-> main
VAR correct = "To bark up the wrong tree"
VAR chosen = ""

=== main ===
Mythical Wise Tree : Which idiom means to have a wrong idea about who is responsible?
    + To break the ice
        ~ chosen = "To break the ice"
        #WRONG_CHOICE
        -> check_choice
        
    + To bark up the wrong tree
        ~ chosen = "To bark up the wrong tree"
        #CORRECT_CHOICE
        -> check_choice
        
    + A lone wolf
        ~ chosen = "A lone wolf"
        #WRONG_CHOICE
        -> check_choice
        

=== check_choice ===
Barbas : {chosen == correct: Woof! You are looking at the right tree now! | No, the cat is in a different tree! Try again.}
-> END