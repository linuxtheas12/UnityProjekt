-> main
VAR correct = "Cutting edge"
VAR chosen = ""

=== main ===
Savage Camel : How do you describe the newest and most advanced technology?
    + To throw a spanner in the works
        ~ chosen = "To throw a spanner in the works"
        #WRONG_CHOICE
        -> check_choice
        
    + A well-oiled machine
        ~ chosen = "A well-oiled machine"
        #WRONG_CHOICE
        -> check_choice
        
    + Cutting edge
        ~ chosen = "Cutting edge"
        #CORRECT_CHOICE
        -> check_choice
        

=== check_choice ===
Barbas : {chosen == correct: Sharp and modern! Correct! | No, that's old school. Think about a sharp blade!}
-> END