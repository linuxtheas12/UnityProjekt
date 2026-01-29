-> main
VAR correct = "A dark horse"
VAR chosen = ""

=== main ===
Savage Camel : How do you call a person with a secret ability who surprises everyone?
    + To let the cat out of the bag
        ~ chosen = "To let the cat out of the bag"
        #WRONG_CHOICE
        -> check_choice
        
    + A dark horse
        ~ chosen = "A dark horse"
        #CORRECT_CHOICE
        -> check_choice
        
    + To bark up the wrong tree
        ~ chosen = "To bark up the wrong tree"
        #WRONG_CHOICE
        -> check_choice
        

=== check_choice ===
Barbas : {chosen == correct: You surprised me! Correct! | No, think about a racing animal with a secret.}
-> END