-> main
VAR correct = "The elephant in the room"
VAR chosen = ""

=== main ===
Jimmy the Joker : How do we describe an obvious problem that everyone is avoiding?
    + The elephant in the room
        ~ chosen = "The elephant in the room"
         #CORRECT_CHOICE
        -> check_choice
        
    + To bark up the wrong tree
        ~ chosen = "To bark up the wrong tree"
        #WRONG_CHOICE
        -> check_choice
        
    + To beat around the bush
        ~ chosen = "To beat around the bush"
        #WRONG_CHOICE
        -> check_choice
        

=== check_choice ===
Barbas : {chosen == correct: Finally, someone said it! Correct! | No, that's avoiding the issue.}
-> END